using System;
using System.IO;
using System.Windows;

using DotNetBay.Core;
using DotNetBay.Model;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using Microsoft.Win32;

namespace DotNetBay.WPF.ViewModel
{
    public class SellViewModel : ViewModelBase
    {
        private readonly SimpleMemberService memberService;

        private AuctionService auctionService;

        private string filePathToImage;

        public RelayCommand<Window> CloseDialogCommand { get; set; }

        public RelayCommand<Window> AddAuctionAndCloseCommand { get; set; }

        public RelayCommand SelectImageFileCommand { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public long StartPrice { get; set; }
        public DateTime StartDateTimeUtc { get; set; }
        public DateTime EndDateTimeUtc { get; set; }

        public string FilePathToImage
        {
            get { return this.filePathToImage; }
            set { this.Set(() => this.FilePathToImage, ref this.filePathToImage, value); }
        }

        public SellViewModel()
        {
            var app = Application.Current as App;

            this.memberService = new SimpleMemberService(app.MainRepository);
            this.auctionService = new AuctionService(app.MainRepository, this.memberService);

            this.SelectImageFileCommand = new RelayCommand(this.SelectFolderAction);
            this.CloseDialogCommand = new RelayCommand<Window>(this.CloseAction);
            this.AddAuctionAndCloseCommand = new RelayCommand<Window>(this.AddActionAndClose);

            // Default Values
            this.StartDateTimeUtc = DateTime.UtcNow.AddMinutes(1);
            this.EndDateTimeUtc = DateTime.UtcNow.AddDays(7);
        }

        private void AddActionAndClose(Window window)
        {
            var newAuction = new Auction()
            {
                Title = this.Title,
                Description = this.Description,
                StartPrice = this.StartPrice,
                StartDateTimeUtc = this.StartDateTimeUtc,
                EndDateTimeUtc = this.EndDateTimeUtc,
                Image = File.ReadAllBytes(this.FilePathToImage),
                Seller = this.memberService.GetCurrentMember(),
            };

            this.auctionService.Save(newAuction);

            window.Close();
        }

        private void SelectFolderAction()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true && File.Exists(openFileDialog.FileName))
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);
                if (fileInfo.Extension.EndsWith("jpg"))
                {
                    this.FilePathToImage = openFileDialog.FileName;
                }
            }
        }

        private void CloseAction(Window window)
        {
            window.Close();
        }
    }
}