using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly DiscountContext _discountContext;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(
            DiscountContext discountContext,
            ILogger<DiscountService> logger)
        {
            _discountContext = discountContext;
            _logger = logger;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountContext
                .Coupons
                .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            coupon ??= new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));
            
            _discountContext.Coupons.Add(coupon);
            await _discountContext.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            _discountContext.Coupons.Update(coupon);
            await _discountContext.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountContext
                .Coupons
                .FirstOrDefaultAsync(x => x.ProductName == request.ProductName) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            
            _discountContext.Coupons.Remove(coupon);
            await _discountContext.SaveChangesAsync();

            _logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", request.ProductName);

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
