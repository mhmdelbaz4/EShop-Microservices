using Discount.gRPC.Data;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Services;
public class DiscountService(DiscountDbContext discountDbContext): DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon =await discountDbContext.Coupons
                                    .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        if(coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

        var couponModel = coupon.Adapt<CouponModel>();
        return new GetDiscountResponse() { Coupon = couponModel };
    }
    public override async Task<CreateDiscountResponse> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Models.Coupon>();
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Coupon is null."));
        
        discountDbContext.Coupons.Add(coupon);
        await discountDbContext.SaveChangesAsync();

        return new CreateDiscountResponse() {Coupon = coupon.Adapt<CouponModel>() };
    }

    public override async Task<UpdateDiscountResponse> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon =await discountDbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.Coupon.ProductName);
        if(coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.Coupon.ProductName} is not found."));

        request.Coupon.Adapt(coupon);
        discountDbContext.Coupons.Update(coupon);
        await discountDbContext.SaveChangesAsync();

        return new UpdateDiscountResponse() { Coupon = coupon.Adapt<CouponModel>() };

    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountDbContext.Coupons
                                    .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        if (coupon == null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

        discountDbContext.Coupons.Remove(coupon);
        await discountDbContext.SaveChangesAsync();
        return new DeleteDiscountResponse() { Success = true };
    }
}
