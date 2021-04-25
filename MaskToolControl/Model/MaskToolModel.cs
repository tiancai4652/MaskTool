
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace MaskToolControl.Model
{
    public class MaskToolModel: IDisposable
    {
        public IntPtr MainWindowHandle;

        public string AppPath { get; set; }

        public Process AppProcess { get; set; }

        public IntPtr AppWindowHandle { get; set; }


        public MaskToolModel(string otherApplicationPath, IntPtr mainWindowHandle)
        {
            MainWindowHandle = mainWindowHandle;
            AppPath = otherApplicationPath;
        }

        public void LoadApplicationWindow(int x, int y, int width, int height)
        {

            AppWindowHandle = IntPtr.Zero;

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

            WindowSwitchHelper.RemoveWindowBorder(AppWindowHandle);
            WindowSwitchHelper.MoveAndResizeWindow(AppWindowHandle, x, y, width, height, true);

        }

        public void ActiveOtherApp()
        {
            WindowSwitchHelper.Active(AppWindowHandle, MainWindowHandle);
        }

        public void ActiveMainWindow()
        {
            WindowSwitchHelper.Active(MainWindowHandle, AppWindowHandle);
        }

        bool _isdisposed=false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_isdisposed)
            {
                if (disposing)
                {
                    if (AppWindowHandle != IntPtr.Zero && !AppProcess.HasExited)
                    {
                        // Stop the application
                        AppProcess.Kill();

                        // Clear internal handle
                        AppWindowHandle = IntPtr.Zero;
                    }
                }
                _isdisposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}
