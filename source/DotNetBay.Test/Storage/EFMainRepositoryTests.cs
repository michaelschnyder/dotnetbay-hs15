using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;

using DotNetBay.Data.EF;

using DotNetBay.Interfaces;

namespace DotNetBay.Test.Storage
{
    public class EFMainRepositoryTests : MainRepositoryTestBase
    {
        public EFMainRepositoryTests()
        {
            // ROLA - This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
            // As it is installed in the GAC, Copy Local does not work. It is required for probing.
            // Fixed "Provider not loaded" error
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        protected override IRepositoryFactory CreateFactory()
        {
            return new EFMainRepositoryFactory();
        }
    }

    public class EFMainRepositoryFactory : IRepositoryFactory
    {
        private List<EFMainRepository> repos = new List<EFMainRepository>();

        public void Dispose()
        {
            foreach (var repo in this.repos)
            {
                repo.Database.Delete();
            }
        }

        public IMainRepository CreateMainRepository()
        {
            var repo = new EFMainRepository();

            if (!this.repos.Any())
            {
                repo.Database.Delete();
            }
            
            this.repos.Add(repo);

            return repo;
        }
    }
}
