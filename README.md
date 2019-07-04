# FormWithUnityAndCef
## 目标:本方案可以解决B/S与C/S并用以及通信问题
启发是unity命令行启动的一个例子EmbeddedWindow.zip
### 客户端使用工具:Unity
### 内嵌浏览器工具:Cef的CSharp版本
[CefSharp](]https://github.com/cefsharp/)
### 其他版本:
[查看全部版本](https://bitbucket.org/chromiumembedded/cef/src/master/)
### 通信工具:SharedMemory
[前往Github](https://github.com/spazzarama/SharedMemory)
### 工程示例
[前往Github](https://github.com/DenseOoFog/FormWithUnityAndCef)

### build工程
https://pan.baidu.com/s/1kg9pTtLfMFAoNM4Ih2ubKA 密码hs4y

看到这里大神已经可以自己去玩耍了,如果大神还有兴趣继续往下看也请轻轻的吐槽.....(无法整理说出来的东东,说明你没有掌握,也不知道是谁说的。)重新搭一遍的时间需要半小时、但是如果你要下载啥的请根据网速适当延长(也许一小时、也许一天、也许....)

(所以自己记录下)
### 简述搭建流程:
1. 创建窗体应用FormWithUnityAndCef
   - 利用Nuget安装CefSharp.WinForms
   - 引入MessageLibrary
   - 提供了一个可以配置的config.ini目前路径是写死了(在exe目录的上一级)
   - 利用命令行调用Unity并嵌入
2. 创建用于Unity与Form交互的库MessageLibrary
   - 利用Nuget安装SharedMemory
   - 创建EventPacket可序列化的类用于传输数据(不要纠结写的太随意,可以自定义，只包含了一个int值与string)
   - 创建SharedMemServer类对SharedMemory进行基础封装(对字节的读写与连接等)
   - 创建SharedCommServer类对SharedMemServer再次封装(提供对EventPacket的消息读写)
3. 创建Unity场景
   - 导入自己的MessageLibrary.dll、SharedMemory.dll
   - 使用测试(相见代码)
4. 创建html页面(详见html)
5. 观赏效果自恋一翻(可选)

结构图效果图:

![效果图](https://github.com/DenseOoFog/FormWithUnityAndCef/blob/master/%E7%BB%93%E6%9E%84.png?raw=true)

![效果图](https://raw.githubusercontent.com/DenseOoFog/FormWithUnityAndCef/master/%E6%95%88%E6%9E%9C.png)

## FAQ:
1. 如何下载.NET Framework4.x？
https://dotnet.microsoft.com/download/dotnet-framework
https://dotnet.microsoft.com/download/visual-studio-sdks?utm_source=getdotnetsdk&utm_medium=referral
2. 下载了.NET Framework4.x之后为什么我的目标框架里依然不能选择？
你可能下载了一个假的..如果你下载的只有10+M的文件。那么请去重新下载开发包
3. 设置CEF的Cef.EnableHighDPISupport()就发现嵌入Unity的窗体中的分辨率有所偏差，如何解决？
先创建app.manifest
https://blog.csdn.net/sjt223857130/article/details/80699685
4. 调用共享内存的时候报错说xxx不支持
请在Unity中确认 Api Compatibility Level 应为.NET 4.x

```
<application xmlns="urn:schemas-microsoft-com:asm.v3">
    <windowsSettings>
      <dpiAware xmlns="http://schemas.microsoft.com/SMI/2005/WindowsSettings">true</dpiAware>
    </windowsSettings>
  </application>
```
### 后续可以继续深究内容：
1. CefSharp的深入应用
2. SharedMemory的使用原理
   - MemoryMappedFile .NET Framework4.0以上
   - 多读写线程安全问题
3. C#窗体应用
   - form的应用---最老的(可以直接拿到窗口句柄,所以就直接用这个了)
   - wpf的应用---比form新一点(没怎么用过)
   - uwp的应用---新的(微软为了打造各平台体系所出的东东，没有怎么搞过，xmal这东西一直不知道怎么玩)
4. Unity中的命令启动参数
https://docs.unity3d.com/Manual/CommandLineArguments.html 中可以下载EmbeddedWindow.zip
5. 然后在是反射和Marshal等的了解
