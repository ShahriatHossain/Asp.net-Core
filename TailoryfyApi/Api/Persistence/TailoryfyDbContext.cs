using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Persistence
{
    public class TailoryfyDbContext : DbContext
    {
        public TailoryfyDbContext(DbContextOptions<TailoryfyDbContext> options)
            : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemStep> ItemSteps { get; set; }
        public DbSet<ItemStepAttachment> ItemStepAttachments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemStep> OrderItemSteps { get; set; }
        public DbSet<OrderReceiver> OrderReceivers { get; set; }
    }
}
