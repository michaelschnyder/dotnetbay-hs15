﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using DotNetBay.Core;
using DotNetBay.Core.Execution;
using DotNetBay.Data.EF;
using DotNetBay.Data.EF.Migrations;
using DotNetBay.Data.FileStorage;
using DotNetBay.Interfaces;
using DotNetBay.Model;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            var ensureDLLIsCopied = SqlProviderServices.Instance;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MainDbContext, Configuration>());

            // this.MainRepository = new FileSystemMainRepository("appdata.json");
            this.MainRepository = new EFMainRepository();
            this.MainRepository.SaveChanges();

            var memberService = new SimpleMemberService(this.MainRepository);
            var service = new AuctionService(this.MainRepository, memberService);

            if (!service.GetAll().Any())
            {
                var me = memberService.GetCurrentMember();

                service.Save(new Auction
                        {
                            Title = "My First Auction",
                            StartDateTimeUtc = DateTime.UtcNow.AddSeconds(10),
                            EndDateTimeUtc = DateTime.UtcNow.AddDays(14),
                            StartPrice = 72,
                            Seller = me
                        });
            }
        
            this.AuctionRunner = new AuctionRunner(this.MainRepository);
            this.AuctionRunner.Start();
        }

        public IMainRepository MainRepository { get; private set; }
        
        public IAuctionRunner AuctionRunner { get; private set; }
    }
}
