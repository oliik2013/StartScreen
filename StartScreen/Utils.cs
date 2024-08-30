using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using static StartScreen.pinvoke;
using System.Windows.Media;
using System.Windows;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace StartScreen
{
    public class Utils
    {
        [DllImport("shell32.dll", EntryPoint = "#261",
               CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetUserTilePath(
            string username, UInt32 whatever, // 0x80000000
            StringBuilder picpath, int maxLength
        );

        public static String getWallpaperPath()
        {
            Logger.info("[Utils] Using Fallback Wallpaper Method!");
            Logger.info("[Utils] GetWallpaperPath() Called");
            var wallpaperFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Themes\\CachedFiles";
            var transcodedWallpaper = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Themes";
            if (Directory.Exists(wallpaperFolder))
            {
                foreach (String file in Directory.GetFiles(wallpaperFolder))
                {
                    if (file.Contains("Cached"))
                    {
                        return file;
                    }
                    else
                    {
                        return transcodedWallpaper + "\\TranscodedWallpaper";
                    }
                }
            }
            return transcodedWallpaper + "\\TranscodedWallpaper";
        }
        public static BitmapImage GetUserimage()
        {
            Logger.info("[Utils] GetUserImage() Called");

            // Does not work on my machine (Windows 10 22H2)
            //if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + Environment.UserName + ".bmp"))
            //{
            //    return new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp\" + Environment.UserName + ".bmp"));
            //}
            //else
            //{
            //    return null;
            //}

            var sb = new StringBuilder(1000);
            GetUserTilePath(null, 0x80000000, sb, sb.Capacity);
            return new BitmapImage(new Uri(sb.ToString(), UriKind.Absolute));
        }
        public static Image CaptureScreen()
        {
            Logger.info("[P/Invoke] CaptureScreen() Called");
            return CaptureWindow(User32.GetDesktopWindow());
        }

        public static Image CaptureWindow(IntPtr handle)
        {
            Logger.info("[P/Invoke] CaptureWindow() Called");
            IntPtr hdcSrc = User32.GetWindowDC(handle);

            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, pinvoke.SRCCOPY);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            Image image = Bitmap.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);

            return image;
        }
        public static Image DrawToImage(IntPtr window)
        {
            Logger.info("[P/Invoke] DrawToImage() Called");
            return Utils.CaptureWindow(window);
        }
        public static Image getDesktopWallpaper()
        {
            Logger.info("[P/Invoke] GetDesktopWallpaper() Called");
            return DrawToImage(User32.FindWindow("Progman",null));
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            if (source == null)
                return new BitmapImage(source);

            using (var fs = new FileStream(source.LocalPath, FileMode.Open))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = fs;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }
    }
}
