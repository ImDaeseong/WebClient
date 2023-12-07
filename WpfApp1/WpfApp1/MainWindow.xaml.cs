using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private clsWebClient webClient;

        public MainWindow()
        {
            InitializeComponent();

            webClient = new clsWebClient();
            DataContext = webClient;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string strUrl = "http://images.freeimages.com/images/previews/0e6/war-soldier-military-arms-hand-legsnt-1633631.jpg";
            await Dispatcher.InvokeAsync(() => webClient.GetWebClient(strUrl));
        }
    }
}
