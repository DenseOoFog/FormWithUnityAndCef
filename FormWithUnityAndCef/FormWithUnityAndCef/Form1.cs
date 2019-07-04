using System.Windows.Forms;
using System.IO;
using System.Reflection;
using CefSharp;
using CefSharp.WinForms;
using MessageLibrary;
using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FormWithUnityAndCef
{
    public partial class Form1 : Form
    {
        private readonly string _configPath = "..\\config.ini";

        #region config字段

        private string[] keys = new string[] { "width", "height",
            "unityWidth", "unityHeight",
            "inCommFileSize", "outCommFileSize",
            "unityPath","browserUrl", "formBorder" };

        public int width;
        public int height;
        public int unityWidth;
        public int unityHeight;
        public int inCommFileSize;
        public int outCommFileSize;
        public string unityPath = string.Empty;
        public string browserUrl = string.Empty;
        public bool formBorder;

        #endregion

        #region unity进程相关
        //当前unity进程
        private Process _process;
        //当前unity窗口句柄
        private IntPtr unityHWND = IntPtr.Zero;
        private const int WM_ACTIVATE = 0x0006;
        private readonly IntPtr WA_ACTIVE = new IntPtr(1);
        private readonly IntPtr WA_INACTIVE = new IntPtr(0);
        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        #endregion


        //cef相关
        private ChromiumWebBrowser _browser;
        //sharedMemory相关
        private string _inSharedCommFile = "inSharedCommFile";
        private string _outSharedCommFile = "outSharedCommFile";
        private SharedCommServer _inServer;
        private SharedCommServer _outServer;
        
        public Form1()
        {
            InitializeComponent();
            ReadConfig();
            InitForm();
            InitBrowser();
            InitUnity();
            InitComm();
        }
       
        private void InitForm()
        {
            //设置窗口大小
            this.ClientSize = new System.Drawing.Size(width, height);
            //设置Unity窗口大小
            this.unityParentPanel.Size = new System.Drawing.Size(unityWidth, unityHeight);
            //设置边框大小
            if (!formBorder) this.FormBorderStyle = FormBorderStyle.None;

        }

        private void InitUnity()
        {
            try
            {
                _process = new Process();
                _process.StartInfo.FileName = unityPath;
                _process.StartInfo.Arguments = "-parentHWND " + unityPanel.Handle.ToInt32() + " -screen-width " +
                    unityPanel.Width + " -screen-height " + unityPanel.Height + " " + Environment.CommandLine;


                _process.StartInfo.UseShellExecute = true;
                //process.StartInfo.CreateNoWindow = true;

                _process.Start();

                _process.WaitForInputIdle();
                // Doesn't work for some reason ?!
                //unityHWND = process.MainWindowHandle;
                EnumChildWindows(this.unityPanel.Handle, WindowEnum, IntPtr.Zero);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ".\nCheck if Container.exe is placed next to Child.exe.");
            }


        }

        private void InitBrowser()
        {
            Cef.Initialize(new CefSettings());

            Cef.EnableHighDPISupport();
            _browser = new ChromiumWebBrowser(browserUrl)
            {
                Dock = DockStyle.Fill,

            };

            this.bowserPanel.Controls.Add(_browser);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            _browser.RegisterJsObject("unityBridge", new UnityBridgeBound(this));
        }

        private void InitComm()
        {
            //Read
            _inServer = new SharedCommServer(false);
            //Write
            _outServer = new SharedCommServer(true);

            _inServer.InitComm(inCommFileSize, _inSharedCommFile);
            _outServer.InitComm(outCommFileSize, _outSharedCommFile);

            //开启读取线程
            Action serverRead = () =>
            {
                EventPacket ep;
                while (true)
                {
                    ep = _inServer.GetMessage();
                    if (ep != null)
                    {
                        //把获取到的信息直接发送到浏览器中
                        Console.WriteLine("Message From Unity:" + ep.eventValue);
                        _browser.ExecuteScriptAsync("DisplayValue", ep.eventValue);
                    }

                    //Console.WriteLine("serverRead!!");
                    
                    //没那么频繁、可以调节
                    Thread.Sleep(200);

                }
            };


            // ThreadPool.QueueUserWorkItem((o) => { serverWrite(); });
            ThreadPool.QueueUserWorkItem((o) => { serverRead(); });

           
        }


        private void ReadConfig()
        {
            if (File.Exists(_configPath))
            {
                string ini = File.ReadAllText(_configPath).Trim();
                string[] keyAndValues = ini.Split(',');
                FieldInfo info = null;

                PropertyInfo[] infoes = this.GetType().GetProperties();
                FieldInfo[] fieldInfo = this.GetType().GetFields();

                for (int i = 0; i < keyAndValues.Length; i++)
                {
                    for (int j = 0; j < keys.Length; j++)
                    {
                        if (keyAndValues[i].IndexOf(keys[j]) != -1)
                        {
                            info = this.GetType().GetField(keys[j]);

                            if (info != null)
                            {
                                if (info.GetValue(this) is int)
                                {
                                    int value = int.Parse(keyAndValues[i].Substring(keyAndValues[i].IndexOf('=') + 1));
                                    info.SetValue(this, value);
                                }
                                else if (info.GetValue(this) is bool)
                                {
                                    bool value = bool.Parse(keyAndValues[i].Substring(keyAndValues[i].IndexOf('=') + 1));
                                    info.SetValue(this, value);
                                }
                                else if (info.GetValue(this) is string)
                                {
                                    string value = keyAndValues[i].Substring(keyAndValues[i].IndexOf('=') + 1);
                                    info.SetValue(this, value);
                                }

                            }
                        }

                    }
                }
                System.Console.WriteLine("Read Config Completed");
            }
        }

        public void WriteToUnity(string message)
        {
            EventPacket ep = new EventPacket
            {
                eventId = 1,
                eventValue = message
            };

            _outServer.WriteMessage(ep);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int x = (this.Width - this.unityParentPanel.Width) / 2;
            int y = (this.Height - this.unityParentPanel.Height) / 2;

            this.unityParentPanel.Location = new System.Drawing.Point(x, y);
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            //ActivateUnityWindow();
            return 0;
        }

        private void ActivateUnityWindow()
        {
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            _process.Kill();
            _inServer.Dispose();
            _outServer.Dispose();
        }
    }

    public class UnityBridgeBound
    {

        private Form1 _form1;
        public UnityBridgeBound(Form1 form)
        {
            this._form1 = form;
        }
        public void GoTo(string xx)
        {
            //MessageBox.Show("这个一个测试:" + xx);
            _form1.WriteToUnity(xx);
        }
    }

}
