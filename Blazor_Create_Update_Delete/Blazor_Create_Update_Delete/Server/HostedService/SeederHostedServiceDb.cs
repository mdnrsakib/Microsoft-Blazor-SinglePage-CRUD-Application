

using Blazor_Create_Update_Delete.Shared.Models;

namespace Blazor_Create_Update_Delete.HostedService
{
    public class SeederHostedServiceDb : IHostedService
    {
        IServiceProvider serviceProvider;
        public SeederHostedServiceDb(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {

                var db = scope.ServiceProvider.GetRequiredService<TravelTourDbContext>();

                await SeedDbAsync(db);

            }
        }
        public async Task SeedDbAsync(TravelTourDbContext db)
        {
            await db.Database.EnsureCreatedAsync();
            if (!db.TravelAgents.Any())
            {
                var t1 = new TravelAgent { AgentName = "Asib", Email = "asib@gmail.com", AgentAddress = "Mirpur, Dhaka" };
                await db.TravelAgents.AddAsync(t1);
                var t2 = new TravelAgent { AgentName = "Shoyeb", Email = "Shoyeb@gmail.com", AgentAddress = "Gazipur, Dhaka" };
                await db.TravelAgents.AddAsync(t2);
                var p1 = new TourPackage { PackageCategory = PakageCategory.Economy, PackageName = "GhureAsi", CostPerPerson = 2500.00M, TourTime = 5 };
                await db.TourPackages.AddAsync(p1);
                var p2 = new TourPackage { PackageCategory = PakageCategory.Platinum, PackageName = "Honeymoon Highlight", CostPerPerson = 3500.00M, TourTime = 7 };
                await db.TourPackages.AddAsync(p2);
                var h1 = new Tourist { TouristName = "Sakib", BookingDate = new DateTime(2000, 02, 02), TouristOccupation = "Student", TouristPicture = "1.jpg",TourPackages=p1 };
                await db.Tourists.AddAsync(h1);
                var h2 = new Tourist { TouristName = "Hossain", BookingDate = new DateTime(2000, 02, 02), TouristOccupation = "Business Man", TouristPicture = "2.jpg",TourPackages=p2 };
                await db.Tourists.AddAsync(h2);
                var atp1 = new AgentTourPackage { TravelAgents = t1, TourPackages = p1};
                var atp2 = new AgentTourPackage { TravelAgents = t2, TourPackages = p2 };
                await db.AgentTourPackages.AddAsync(atp1);
                await db.AgentTourPackages.AddAsync(atp2);
                await db.SaveChangesAsync();
            }

        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}