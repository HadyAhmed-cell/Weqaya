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
            }
            catch ( Exception ex )
            {
                var logger = loggerFactory.CreateLogger<DataContext>();
                logger.LogError(ex.Message);
            }
        }
    }
}