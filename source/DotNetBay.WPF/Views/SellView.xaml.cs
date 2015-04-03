using System.Windows;

using DotNetBay.Core;
using DotNetBay.WPF.ViewModel;

namespace DotNetBay.WPF.Views
{
    /// <summary>
    /// Interaction logic for SellView.xaml
    /// </summary>
    public partial class SellView : Window
    {
        public SellView()
        {
            this.InitializeComponent();

            var app = Application.Current as App;

            var memberService = new SimpleMemberService(app.MainRepository);
            var auctionService = new AuctionService(app.MainRepository, memberService);
            
            this.DataContext = new SellViewModel(memberService, auctionService);
        }
    }
}
