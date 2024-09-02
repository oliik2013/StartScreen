using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using ModernWpf.Media.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for AllApps.xaml
    /// </summary>
    public partial class AllApps : Page
    {
        public AllApps()
        {
            InitializeComponent();
            listBox.ItemsSource = MainWindow.Instance.appListNameFriendly;
            listBox.SelectedIndex = 0;

            GoUp_Button.Style = Assets.Styles.circleButtonStyle;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.Instance.content.GoBack();
        }

        private void AppButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.info("Starting selected app");
            var selectedItem = (sender as ListBox).SelectedItem as TileBackend.tileData;
            Process.Start("explorer.exe", $@"shell:appsFolder\{selectedItem.name}");
            Home.closeAppAnim();
        }

        private void sortByAlphabet()
        {
            listBox.Items.Clear();
        }

        // by alphabet
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            sortByAlphabet();
        }

        private void sortByZphabet()
        {
            var newl = MainWindow.Instance.appListNameFriendly;
            newl.Reverse();
            listBox.Items.Clear();
            foreach (var item in newl)
            {
                listBox.Items.Add(item);
            }
        }

        // by zphabet
        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            sortByZphabet();
        }

        // by categories
        private void ComboBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {

        }

        private void Call_AppWiz(object sender, RoutedEventArgs e)
        {
            Process.Start("appwiz.cpl");
            Environment.Exit(0);
        }
    }

    public enum SortGroup
    {
        Symbol = 0,
        A, B, C, D, E, F, G, H, I, J, K,
        L, M, N, O, P, Q, R, S, T, U, V,
        W, X, Y, Z
    }

}
