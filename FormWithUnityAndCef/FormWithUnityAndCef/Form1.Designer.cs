namespace FormWithUnityAndCef
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.bowserPanel = new System.Windows.Forms.Panel();
            this.unityPanel = new System.Windows.Forms.Panel();
            this.unityParentPanel = new System.Windows.Forms.Panel();
            this.bowserPanel.SuspendLayout();
            this.unityParentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bowserPanel
            // 
            this.bowserPanel.Controls.Add(this.unityParentPanel);
            this.bowserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bowserPanel.Location = new System.Drawing.Point(0, 0);
            this.bowserPanel.Name = "bowserPanel";
            this.bowserPanel.Size = new System.Drawing.Size(800, 450);
            this.bowserPanel.TabIndex = 0;
            // 
            // unityPanel
            // 
            this.unityPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unityPanel.Location = new System.Drawing.Point(0, 0);
            this.unityPanel.Name = "unityPanel";
            this.unityPanel.Size = new System.Drawing.Size(200, 100);
            this.unityPanel.TabIndex = 0;
            // 
            // unityParentPanel
            // 
            this.unityParentPanel.Controls.Add(this.unityPanel);
            this.unityParentPanel.Location = new System.Drawing.Point(287, 183);
            this.unityParentPanel.Name = "unityParentPanel";
            this.unityParentPanel.Size = new System.Drawing.Size(200, 100);
            this.unityParentPanel.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.unityParentPanel);
            this.Controls.Add(this.bowserPanel);
            
            this.Name = "Form1";
            this.Text = "Form1";
            this.bowserPanel.ResumeLayout(false);
            this.unityParentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bowserPanel;
        private System.Windows.Forms.Panel unityPanel;
        private System.Windows.Forms.Panel unityParentPanel;
    }
}

