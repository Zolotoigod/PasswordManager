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

namespace WPFPassManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindowLoad;
        }
        private void ToLoder_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            ToMenu.Visibility = Visibility.Visible;
            PassRead.Visibility = Visibility.Visible;
        }
        private void ToGenerator_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            PassGenerator.Visibility = Visibility.Visible;
            ToMenu.Visibility = Visibility.Visible;
        }

        private void ToCustom_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            PassCustom.Visibility = Visibility.Visible;
            ToMenu.Visibility = Visibility.Visible;
        }

        private void ToMenu_Click(object sender, RoutedEventArgs e)
        {
            ToMenu.Visibility = Visibility.Hidden;
            PassGenerator.Visibility = Visibility.Hidden;
            PassCustom.Visibility = Visibility.Hidden;
            PassRead.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindowLoad(object sender, RoutedEventArgs e)
        {
            ToMenu.Visibility = Visibility.Hidden;
            PassGenerator.Visibility = Visibility.Hidden;
            PassCustom.Visibility = Visibility.Hidden;
            PassRead.Visibility = Visibility.Hidden;
        }
    }
}
