using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoList.Context;
using TodoList.Models;

namespace TodoListTests.Integration
{
    [TestFixture]
    public class WebFactoryProgram : WebApplicationFactory<Program>
    {
        private IServiceCollection _service;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(b =>
            {

                var descriptor = b.SingleOrDefault(
               d => d.ServiceType ==
                   typeof(DbContextOptions<TodoAppContext>));

                if (descriptor != null) b.Remove(descriptor);

                var connectionString = "User ID=postgres;Password=Veteranos1;Host=Localhost;Port=5432;Database=TodoApp.Test;";

                b.AddDbContext<TodoAppContext>(s => s.UseNpgsql(connectionString, opt => opt.EnableRetryOnFailure(5)));

                _service = b;
                var sp = b.BuildServiceProvider();


                using var scope = sp.CreateScope();

                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TodoAppContext>();

                db.Database.EnsureCreated();
                db.Todos.Add(new Todo
                {
                    Title = "Teste",
                });
                db.SaveChanges();

            });
        }

        [OneTimeTearDown]
        public void DisposeDb()
        {
            var sp = _service.BuildServiceProvider();

            using var scope = sp.CreateScope();

            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<TodoAppContext>();
            db.Database.EnsureDeleted();
        }
    }
}
