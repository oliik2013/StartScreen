using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartScreen
{
    /// <summary>
    /// Interaction logic for StartGroupDetails.xaml
    /// </summary>
    public partial class StartGroupDetails : Page
    {
        public StartGroupDetails(string groupName, List<TileBackend.tileData> apps)
        {
            InitializeComponent();

            GoBack_Button.Style = Assets.Styles.circleButtonStyle;

            AppsList.ItemsSource = apps;
            GroupEntry.Text = groupName;
        }

        public void GoBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.content.GoBack();
        }
    }
}
