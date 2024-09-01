using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using ModernWpf.Media.Animation;
using System.Windows.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
using System.Diagnostics.Metrics;
using System.Windows.Threading;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Security.RightsManagement;
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        public DispatcherTimer counter2 = new();
        DispatcherTimer counter = new();

        /// Is the window initialized?
        private bool initialized = false;

        /// <summary>
        /// Sets whether to use the user desktop wallpaper or not
        /// (before this will be used to draw the entire desktop with no windows)
        /// </summary>
        bool userBackgroundEnabled = true;

        public AllApps allApps;
        public Home homeScreen;

        public MainWindow()
        {
            alreadyShowing = true;
            
            InitializeComponent();
            Logger.info("Object initialized!");

            Instance = this;
            Logger.info("Instance is set to \"This\"!");

            Opacity = 0;
            Logger.info("Opacity has been set to 0");
            
            Topmost = true;
            Logger.info("Window has been set to Top-Most");
            
            ShowInTaskbar = false;
            Logger.info("Successfully hidden from taskbar");
            
            if (!initialized)
            {
                counter.Tick += new EventHandler(windowAnim);
                counter.Interval = new TimeSpan(0, 0, 0, 0, 2);
                counter.Start();
                Logger.info("windowAnim timer has initialized and started");
                
                DispatcherTimer checkLauncher = new DispatcherTimer();
                
                checkLauncher.Tick += new EventHandler(launcherTick);
                checkLauncher.Interval = new TimeSpan(0, 0, 0, 0, 1);
                checkLauncher.Start();
                Logger.info("Launcher Checker has been started");
                
                Loaded += MainWindow_Loaded;
                homeScreen = new Home();
                content.Navigate(homeScreen, new EntranceNavigationTransitionInfo());
                
                new Thread(() =>
                {
                    if (userBackgroundEnabled)
                    {
                        Logger.info("Getting user's desktop wallpaper");
                        imageBackground.Dispatcher.Invoke(() =>
                            imageBackground.Source = Utils.BitmapFromUri(new Uri(Utils.getWallpaperPath()))
                        );
                        Logger.info("Done getting user's desktop wallpaper");
                    }

                }).Start();

                imageBackground.Stretch = Stretch.UniformToFill;
                Logger.info("Background Opacity has been set to 1");

                counter2.Tick += new EventHandler(MainWindow.Instance.windowAnim2);
                counter2.Interval = new TimeSpan(0, 0, 0, 0, 2);
                ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}"));
                Logger.info("Listing All Apps");
                foreach (var app in (IKnownFolder)appsFolder)
                {
                    Logger.info("Name : " + app.Name + " | ID : " + app.ParsingName);

                    // The friendly app name
                    string name = app.Name;
                    
                    // The ParsingName property is the AppUserModelID
                    string appUserModelID = app.ParsingName; // or app.Properties.System.AppUserModel.ID
                                                             // You can even get the Jumbo icon in one shot
                    BitmapSource icon = app.Thumbnail.SmallBitmapSource;
                    //appList.SortDescriptions.Add(new System.ComponentModel.SortDescription("NAME", System.ComponentModel.ListSortDirection.Ascending));

                    appList.Add(new AppsIcons
                    {
                        Icon = icon,
                        Name = name + "[" + appUserModelID
                    });

                    appListNameFriendly.Add(new AppsIcons
                    {
                        Icon = icon,
                        Name = name
                    });

                    Logger.info("Added Successfully");
                }
                initialized = true;
                allApps = new AllApps();
            }
        }
        public List<AppsIcons> appList = new List<AppsIcons>();
        public List<AppsIcons> appListNameFriendly = new List<AppsIcons>();
        public bool alreadyShowing = false;
        private void launcherTick(object? sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("ScreenLaunch");
            if (pname.Length >= 1)
            {
                if (alreadyShowing)
                {
                    Home.closeAppAnim();
                    foreach (Process p in pname)
                    {
                        p.Kill();
                    }
                    return;
                }
                this.Opacity = 0;
                this.Show();
                if (content.CanGoBack)
                    content.GoBack();
                //Home.beginTilesInit();
                counter.Start();
                foreach (Process p in pname)
                {
                    p.Kill();
                }
                new Thread(() =>
                {
                    if (userBackgroundEnabled)
                    {
                        Logger.info("Getting user background");
                        imageBackground.Dispatcher.Invoke(() =>
                            imageBackground.Source = Utils.BitmapFromUri(new Uri(Utils.getWallpaperPath()))
                        );
                    }

                }).Start();
                homeScreen.beginTilesInit();
                startPressed = false;
                alreadyShowing = true;
            }
        }

        public void windowAnim2(object? sender, EventArgs e)
        {
            if (mainWindow.Opacity > 0.1)
            {
                mainWindow.Opacity -= 0.1;
            }
            else
            {
                counter2.Stop();
                //MainWindow.Instance.closeAnimDone = true;
                MainWindow.Instance.Hide();
                GC.Collect();
            }
        }

        private void windowAnim(object? sender, EventArgs e)
        {
            if (mainWindow.Opacity < 1)
                mainWindow.Opacity += 0.1;
            else
                counter.Stop();
        }

        public bool hidden = false;
        private HwndSource hwndSource;
        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            return IntPtr.Zero;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            hwndSource = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            Logger.info("Adding HWND Hook for receiving messages");
            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }
        bool startPressed = false;
        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LWin || e.Key == Key.RWin)
            {
                startPressed = true;
                Home.closeAppAnim();
            }
        }
        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void mainWindow_Deactivated(object sender, EventArgs e)
        {
            if (startPressed) return;
            Home.closeAppAnim();
        }
        public void HideWindow()
        {
            counter2.Start();
        }
    }
}
