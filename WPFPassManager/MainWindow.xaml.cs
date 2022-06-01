using MenagerCore;
using MenagerCore.DTO;
using MenagerCore.Storage;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace WPFPassManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string StoreDir = "data";
        private const string StorePath = StoreDir + "/storage";
        private const string RestorePath = StoreDir + "/restore";
        private const string SecurePath = StoreDir + "/secure";

        private readonly PasswordService service;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindowLoad;
            this.Closing += ProgramClose;
            if (!Directory.Exists(StoreDir))
            {
                Directory.CreateDirectory(StoreDir);
            }
            this.service = new PasswordService(new Restore(RestorePath), new FileStorage(), new AESKeyService(SecurePath), StorePath);
        }
        private void ToLoder_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            ToMenu.Visibility = Visibility.Visible;
            PassRead.Visibility = Visibility.Visible;
            NameR.Text = string.Empty;
            CommentR.Text = string.Empty;
            PasswordR.Text = string.Empty;
        }
        private void ToGenerator_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            PassGenerator.Visibility = Visibility.Visible;
            ToMenu.Visibility = Visibility.Visible;
            NameG.Text = string.Empty;
            CommentG.Text = string.Empty;
            PasswordG.Text = string.Empty;
        }

        private void ToCustom_Click(object sender, RoutedEventArgs e)
        {
            Menu.Visibility = Visibility.Hidden;
            PassCustom.Visibility = Visibility.Visible;
            ToMenu.Visibility = Visibility.Visible;
            NameC.Text = string.Empty;
            CommentC.Text = string.Empty;
            OpResult.Text = string.Empty;
            PasswordC.Text = string.Empty;
        }

        private void ToMenu_Click(object sender, RoutedEventArgs e)
        {
            ToMenu.Visibility = Visibility.Hidden;
            PassGenerator.Visibility = Visibility.Hidden;
            PassCustom.Visibility = Visibility.Hidden;
            PassRead.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Visible;
        }

        private async void Generate_Click(object sender, RoutedEventArgs e)
        {
            PasswordG.Text = await service.SavePassword(new ExternalData() { Key = NameG.Text, Comment = CommentG.Text });
        }

        private async void Show_Click(object sender, RoutedEventArgs e)
        {
            var data = await service.ReadPassword(NameR.Text);
            PasswordR.Text = data.Password;
            CommentR.Text = data.Comment;
        }

        private void CopyR_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PasswordR.Text);
        }

        private void CopyG_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PasswordG.Text);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string result = await service.SavePassword(new ExternalData() {
                Key = NameC.Text,
                Password = PasswordC.Text,
                Comment = CommentC.Text
            });

            OpResult.Text = result;
            await Task.Delay(2000);
            OpResult.Text = string.Empty;
        }

        private async void MainWindowLoad(object sender, RoutedEventArgs e)
        {
            ToMenu.Visibility = Visibility.Hidden;
            PassGenerator.Visibility = Visibility.Hidden;
            PassCustom.Visibility = Visibility.Hidden;
            PassRead.Visibility = Visibility.Hidden;
            await service.RestorMarks();
        }

        private async void ProgramClose(object? sender, CancelEventArgs e)
        {
            await service.SaveMark();
        }
    }
}
