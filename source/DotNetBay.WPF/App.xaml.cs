using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using DotNetBay.Data.FileStorage;
using DotNetBay.Interfaces;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.MainRepository = new FileSystemMainRepository("appdata.json");
            this.MainRepository.SaveChanges();
        }

        public IMainRepository MainRepository { get; private set; }
    }
}
