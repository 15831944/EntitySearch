using EntitySearch.StoreAPI.Core.Application.Interfaces;
using EntitySearch.StoreAPI.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntitySearch.StoreAPI.Core.Infrastructures.Data.Contexts
{
    public class StoreContext : DbContext, IStoreContext
    {
        protected StoreContext()
        {
            base.Database.EnsureCreated();
        }

        public StoreContext(DbContextOptions options) : base(options)
        {
            base.Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
    }
}