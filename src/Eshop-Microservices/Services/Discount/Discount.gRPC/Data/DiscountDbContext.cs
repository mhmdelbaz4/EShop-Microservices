using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Data
{
    public class DiscountDbContext: DbContext
    {
        public DiscountDbContext(DbContextOptions<DiscountDbContext> options)
                                : base(options)
        { }

        public DbSet<Models.Coupon> Coupons { get; set; }
    }
}
