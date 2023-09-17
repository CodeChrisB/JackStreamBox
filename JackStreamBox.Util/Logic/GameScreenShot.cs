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
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out Rectangle rect);

        public static MemoryStream? CaptureScreenshotAsStream()
        {
            try
            {
                IntPtr hwnd = WindowNavigator.GameProcess.Handle;
                GetWindowRect(hwnd, out Rectangle bounds);

                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                    MemoryStream stream = new MemoryStream();
                    bitmap.Save(stream, ImageFormat.Png);
                    stream.Position = 0;
                    return stream;
                }
            }
            catch {
                return null;
            
            }
        }
    }
}
