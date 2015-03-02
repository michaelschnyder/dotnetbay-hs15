using System.Windows;

using DotNetBay.Core;
using DotNetBay.WPF.ViewModel;

namespace DotNetBay.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var app = Application.Current as App;

            var memberService = new SimpleMemberService(app.MainRepository);
            var auctionService = new AuctionService(app.MainRepository, memberService);

            this.DataContext = new MainViewModel(app.AuctionRunner.Auctioneer, auctionService);
        }
    }
}
