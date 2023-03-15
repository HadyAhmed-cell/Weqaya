using System.Text.Json;
using VirtualClinic.Entities;

namespace VirtualClinic.Data
{
    public class DataContextSeed
    {
        public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if ( !context.testsAndRisks.Any() )
                {
                    var testsData = File.ReadAllText("../VirtualClinic/Data/SeedData/tests.json");
                    var tests = JsonSerializer.Deserialize<List<TestsAndRisks>>(testsData);
                    foreach ( var test in tests )
                    {
                        context.testsAndRisks.Add(test);
                    }
                    await context.SaveChangesAsync();
                }
                if ( !context.SocialStatus.Any() )
                {
                    var socialStatusData = File.ReadAllText("../VirtualClinic/Data/SeedData/socialstatus.json");
                    var socialStatus = JsonSerializer.Deserialize<List<SocialStatusType>>(socialStatusData);
                    foreach ( var socialstatus in socialStatus )
                    {
                        context.SocialStatus.Add(socialstatus);
                    }
                    await context.SaveChangesAsync();
                }
                if ( !context.Syndicates.Any() )
                {
                    var syndicatesData = File.ReadAllText("../VirtualClinic/Data/SeedData/syndicates.json");
                    var syndicates = JsonSerializer.Deserialize<List<SyndicatesTypes>>(syndicatesData);
                    foreach ( var syndicate in syndicates )
                    {
                        context.Syndicates.Add(syndicate);
                    }
                    await context.SaveChangesAsync();
                }
                if ( !context.Addresses.Any() )
                {
                    var addressData = File.ReadAllText("../VirtualClinic/Data/SeedData/address.json");
                    var addresses = JsonSerializer.Deserialize<List<Address>>(addressData);
                    foreach ( var address in addresses )
                    {
                        context.Addresses.Add(address);
                    }
                    await context.SaveChangesAsync();
                }
            }
            catch ( Exception ex )
            {
                var logger = loggerFactory.CreateLogger<DataContext>();
                logger.LogError(ex.Message);
            }
        }
    }
}