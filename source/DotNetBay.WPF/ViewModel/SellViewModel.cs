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
        private readonly IMemberService memberService;

        private readonly IAuctionService auctionService;

        private string filePathToImage;

        public SellViewModel(IMemberService memberService, IAuctionService auctionService)
        {
            this.memberService = memberService;
            this.auctionService = auctionService;

            this.SelectImageFileCommand = new RelayCommand(this.SelectFolderAction);
            this.CloseDialogCommand = new RelayCommand<Window>(this.CloseAction);
            this.AddAuctionAndCloseCommand = new RelayCommand<Window>(this.AddActionAndClose);

            // Default Values
            this.StartDateTimeUtc = DateTime.UtcNow.AddMinutes(1);
            this.EndDateTimeUtc = DateTime.UtcNow.AddDays(7);
        }

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


        private void AddActionAndClose(Window window)
        {
            var newAuction = new Auction()
            {
                Title = this.Title,
                Description = this.Description,
                StartPrice = this.StartPrice,
                StartDateTimeUtc = this.StartDateTimeUtc,
                EndDateTimeUtc = this.EndDateTimeUtc,
                Seller = this.memberService.GetCurrentMember(),
            };

            if (!string.IsNullOrEmpty(this.FilePathToImage))
            {
                newAuction.Image = File.ReadAllBytes(this.FilePathToImage);
            }

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