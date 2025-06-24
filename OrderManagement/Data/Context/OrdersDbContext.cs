
using Microsoft.EntityFrameworkCore;
using OrderManagement.Data.Entity;

namespace OrderManagement.Data.Context;

public class OrdersDbContext:DbContext
{
   
       protected override void OnConfiguring(DbContextOptionsBuilder options)
       {
              if (!options.IsConfigured)
              {
                     options.UseSqlite("Data Source=orders.db");
              }
       }
    
       public DbSet<Order> orders {  get; set; }
  
}