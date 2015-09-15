using HiLToysWebApplication.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HiLToysDataModel;

namespace HiLToysWebApplication.Models
{
    public interface IApplicationDbContext
    {
        IDbSet<Category> Categories { get; set; }
        IDbSet<Product> Products { get; set; }
        IDbSet<Cart> Carts { get; set; }
        IDbSet<HiLToysDataModel.Models.Order> Orders { get; set; }
        IDbSet<HiLToysDataModel.Models.OrderDetail> OrderDetails { get; set; }
        IDbSet<HiLToysDataModel.Models.Customer> Customers { get; set; }

        IDbSet<HiLToysDataModel.Models.Shipper> Shippers { get; set; }

        int SaveChanges();
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            base.OnModelCreating(modelBuilder);
        }

       
    }
    public class HiLToysApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public HiLToysApplicationDbContext()
            : base("HiLToysEMDModelContainer", throwIfV1Schema: false)
        {
            
        }

        public static HiLToysApplicationDbContext Create()
        {
            return new HiLToysApplicationDbContext();
        }
        
        public IDbSet<HiLToysDataModel.Category> Categories { get; set; }
        public IDbSet<HiLToysDataModel.Product> Products { get; set; }
        public IDbSet<HiLToysDataModel.Cart> Carts { get; set; }
        public IDbSet<HiLToysDataModel.Models.Order> Orders { get; set; }
        public IDbSet<HiLToysDataModel.Models.OrderDetail> OrderDetails { get; set; }
        public IDbSet<HiLToysDataModel.Models.Customer> Customers { get; set; }
        public IDbSet<HiLToysDataModel.Models.Shipper> Shippers { get; set; }
    }
    public class FakeApplicationDbContext : IApplicationDbContext
    {
        public IDbSet<HiLToysDataModel.Category> Categories { get; set; }
        public IDbSet<HiLToysDataModel.Product> Products { get; set; }
        public IDbSet<HiLToysDataModel.Cart> Carts { get; set; }
        public IDbSet<HiLToysDataModel.Models.Order> Orders { get; set; }
        public IDbSet<HiLToysDataModel.Models.OrderDetail> OrderDetails { get; set; }
        public IDbSet<HiLToysDataModel.Models.Customer> Customers { get; set; }
        public IDbSet<HiLToysDataModel.Models.Shipper> Shippers { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
    }

}