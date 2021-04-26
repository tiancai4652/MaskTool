# MaskTool
Two ppp.window Show like in one 
# 任意程序上的蒙版画笔实现



**根据**

[SetWindowPos function (winuser.h) - Win32 apps | Microsoft Docs](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos)

[SetWindowDisplayAffinity function (winuser.h) - Win32 apps | Microsoft Docs](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowdisplayaffinity)

**总效果：**

![总效果](https://github.com/tiancai4652/MaskTool/tree/master/MaskToolSample/Image/总效果.gif)

*上面是一个透明画的笔应用和一个阅读的应用，我们可以控制画笔在上层或者下层达到一个蒙版的效果。*

### 核心:

##### 1 一个窗口盖住另一个效果

```C#
        public static void Active(IntPtr handleToActive, IntPtr handleToPutToSecondFloor)
        {
            SetWindowPos(handleToActive, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOSIZE | SWP_NOMOVE);
            SetWindowPos(handleToPutToSecondFloor, new IntPtr(HWND_NOTOPMOST), 0, 0, 0, 0, SWP_NOACTIVATE | SWP_SHOWWINDOW | SWP_NOSIZE | SWP_NOMOVE);
            SetForegroundWindow(handleToActive);
        }

```

##### 2 去除窗体辩题边框等

```c#
  public static void RemoveWindowBorder(IntPtr handle)
        {
            SetWindowLongA(handle, GWL_STYLE, WS_VISIBLE);
        }
```

##### 3 移动窗体改变大小

```c#
  public static void MoveAndResizeWindow(IntPtr handle, int x, int y, int cx, int cy, bool repaint)
        {
            MoveWindow(handle, x, y, cx, cy, repaint);
        }
```

### 一 透明的IncCavans画板

对于蒙版画笔程序的窗体，我们需要设置透明

```c#
WindowState="Maximized" WindowStyle="None"  Background="Transparent" AllowsTransparency="True" Topmost="True" IsHitTestVisible="True"
```

对于IncCavans，我们需要设置其背景透明，但是纯透明的话就无法显示画笔了，所以要留一点透明度

```c#
 <InkCanvas Name="inkCanvas" >
            <InkCanvas.Background>
                <SolidColorBrush Color="Gray" Opacity="0.05" ></SolidColorBrush>
            </InkCanvas.Background>
        </InkCanvas>
```

### 二 在启动被蒙版的应用

我们通过Process启动该程序，需要注意的是，AppProcess.WaitForInputIdle();并不能判断窗体已经加载完成，可以通过判断AppProcess.MainWindowHandle == IntPtr.Zero来判断窗体是否创建完成

```c#
  try
            {

                AppProcess = new System.Diagnostics.Process();
                var procInfo = new System.Diagnostics.ProcessStartInfo(AppPath);
                procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(AppPath);
                //procInfo.UseShellExecute = false;
                //procInfo.CreateNoWindow = true;
                //procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                AppProcess.StartInfo = procInfo;
                AppProcess.Start();
                AppProcess.WaitForInputIdle();
                while (AppProcess.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(10);
                }
                AppWindowHandle = AppProcess.MainWindowHandle;
            }
            catch (Exception ex)
            {
                //Debug.Print(ex.Message + "Error");
                MessageBox.Show(ex.Message + "Error");
            }
```

### 三 将被蒙版的应用放在合适的位置

```c#
   var window = Window.GetWindow(this);
            var helper = new WindowInteropHelper(Window.GetWindow(window));
            MaskToolModel = new MaskToolModel(AppPath, helper.Handle);
            var loacation = this.TransformToAncestor(window).Transform(new Point(0, 0));
            MaskToolModel.LoadApplicationWindow((int)loacation.X, (int)loacation.Y, (int)this.ActualWidth, (int)this.ActualHeight);
```

### 四 关于透射的一些注意事项

比如

****![投射](https://github.com/tiancai4652/MaskTool/tree/master/MaskToolSample/Image/投射.gif)

因为上层窗体透明，所以当鼠标划过，下层的Button.MouseOver事件也会触发，此时，我们需要给他一个Contaniner，并设置IsHitTestVisible="True"

```c#
  <Grid Grid.Row="0" Name="Container" IsHitTestVisible="True">
            <mask:MaskTool x:Name="maskControl" AppPath="{Binding U3DAppPath}" IsAppWindowActive="{Binding IsU3DWindowActive}" ></mask:MaskTool>
        </Grid>
```



#### 五 个人封装了一个自定义控件来简单实现此效果

```c#
<Grid Grid.Row="0" Name="Container" IsHitTestVisible="True">
            <mask:MaskTool x:Name="maskControl" AppPath="{Binding U3DAppPath}" IsAppWindowActive="{Binding IsU3DWindowActive}" ></mask:MaskTool>
        </Grid>
```

**AppPath**:被蒙版层应用路径

**IsAppWindowActive**:True蒙版层应用显示在上层

核心代码：

在Userload时加载应用，也不用担心Dispose下层应用，在window.closed时自动Dispose:

```c#
   private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.Closed += Window_Closed;
            var helper = new WindowInteropHelper(Window.GetWindow(window));
            MaskToolModel = new MaskToolModel(AppPath, helper.Handle);
            var loacation = this.TransformToAncestor(window).Transform(new Point(0, 0));
            MaskToolModel.LoadApplicationWindow((int)loacation.X, (int)loacation.Y, (int)this.ActualWidth, (int)this.ActualHeight);
        }
```



*Github*:[tiancai4652/MaskTool: Two ppp.window Show like in one (github.com)](https://github.com/tiancai4652/MaskTool)

