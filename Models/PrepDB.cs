using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using commander.Data;

namespace commander.Models
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CommanderContext>());
            }
            
        }

        public static void SeedData(CommanderContext context)
        {
            System.Console.WriteLine("Applying Migrations...");

            context.Database.Migrate();

            if(!context.Commands.Any())
            {
                System.Console.WriteLine("Adding data - seeding...");
                context.Commands.AddRange(
                    new Command() {HowTo="foobar", Line="foobar1", Platform="foobar2"}

                );
                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Already have data - not seeding");
            }
        }
    }
}