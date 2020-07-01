using Microsoft.AspNet.Identity.EntityFramework;
using Model.Entities;
using System.Data.Entity;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
       
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductionDetail> ProductionDetails { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<DueTransaction> DueTransactions { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<SalesStatement> SalesStatements { get; set; }
        public DbSet<ProductionStatement> ProductionStatements { get; set; }
        public DbSet<CompanySetup> CompanySetups { get; set; }
        public DbSet<DailySalesStatement> DailySalesStatements { get; set; }
        public DbSet<VatEntry> VatEntrys { get; set; }
        public DbSet<Tbl_UserRole> Tbl_UserRole { get; set; }
        public DbSet<Tbl_User> Tbl_User { get; set; }
        public DbSet<Tbl_Menu> Tbl_Menu { get; set; }
        public DbSet<Tbl_View> Tbl_View { get; set; }
        public DbSet<Tbl_UserPermission> Tbl_UserPermission { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
