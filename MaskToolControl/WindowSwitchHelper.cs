using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MaskToolControl
{
    public static class WindowSwitchHelper
    {
        private const int HWND_BOTTOM = 1;
        private const int HWND_NOTOPMOST = -2;
        private const int HWND_TOP = 0;
        private const int HWND_TOPMOST = -1;

        private const int SWP_NOOWNERZORDER = 0x200;
        private const int SWP_NOREDRAW = 0x8;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_SHOWWINDOW = 0x0040;
        private const int WS_EX_MDICHILD = 0x40;
        private const int SWP_FRAMECHANGED = 0x20;
        private const int SWP_NOACTIVATE = 0x10;
        private const int SWP_ASYNCWINDOWPOS = 0x4000;
        private const int SWP_NOMOVE = 0x2;
        private const int SWP_NOSIZE = 0x1;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_CHILD = 0x40000000;

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        public static extern int SetWindowLongA([System.Runtime.InteropServices.InAttribute()] System.IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);

        public static void Active(IntPtr handleToActive, IntPtr handleToPutToSecondFloor)
        {
            SetWindowPos(handleToActive, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOSIZE | SWP_NOMOVE);
            SetWindowPos(handleToPutToSecondFloor, new IntPtr(HWND_NOTOPMOST), 0, 0, 0, 0, SWP_NOACTIVATE | SWP_SHOWWINDOW | SWP_NOSIZE | SWP_NOMOVE);
            SetForegroundWindow(handleToActive);
        }

        public static void RemoveWindowBorder(IntPtr handle)
        {
            SetWindowLongA(handle, GWL_STYLE, WS_VISIBLE);
        }

        public static void MoveAndResizeWindow(IntPtr handle, int x, int y, int cx, int cy, bool repaint)
        {
            MoveWindow(handle, x, y, cx, cy, repaint);
        }

     
    }
}
