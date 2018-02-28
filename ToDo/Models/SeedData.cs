using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ToDo.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ToDoContext( serviceProvider.GetRequiredService<DbContextOptions<ToDoContext>>()) )
            {
                if (context.Tasks.Any() || context.Categories.Any() || context.Statuses.Any())
                    return;
                else
                {
                    context.Categories.AddRange
                    (
                        new Category { Name = "Annual goal" },
                        new Category { Name = "EveryDAy" },
                        new Category { Name = "Shopping" }
                    );
                    context.Statuses.AddRange
                    (
                        new Status { Name = "To do" },
                        new Status { Name = "Doing" },
                        new Status { Name = "Done" }
                    );

                    context.Tasks.AddRange
                    (
                        new Task
                        {
                            Name = "Buy bread",
                            Priority = 2,
                            StatusId = 1,
                            CategoryId = 3
                        },
                        new Task
                        {
                            Name = "Find Girlfriend)",
                            Priority = 1,
                            StatusId = 2,
                            CategoryId = 1
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
