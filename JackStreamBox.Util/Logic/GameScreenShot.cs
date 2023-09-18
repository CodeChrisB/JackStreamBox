using JackStreamBox.Util.logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JackStreamBox.Util.Logic
{
    public class GameScreenShot
    {
        public static MemoryStream CaptureWindowScreenshot()
        {
            if (WindowNavigator.GameProcess == null) return null;


            IntPtr hwnd = WindowNavigator.GameProcess.MainWindowHandle;

            if (hwnd == IntPtr.Zero)
            {
                throw new ArgumentException("The specified process does not have a main window.");
            }

            RECT bounds;
            GetClientRect(hwnd, out bounds);

            if (bounds.Right > bounds.Left && bounds.Bottom > bounds.Top)
            {
                using (Bitmap bitmap = new Bitmap(bounds.Right - bounds.Left, bounds.Bottom - bounds.Top))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        IntPtr hdcBitmap = g.GetHdc();

                        PrintWindow(hwnd, hdcBitmap, 0);

                        g.ReleaseHdc(hdcBitmap);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            bitmap.Save(stream, ImageFormat.Png);
                            stream.Position = 0;
                            return stream;
                        }
                    }
                }
            }
            else
            {
                return null;
            }
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
