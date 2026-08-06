using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Courses;
using Domain.Exceptions.BadRequestExceptions;
using Services.Specifications.CoursesSpecifications;
using ServicesAbstraction;
using ServicesAbstraction.Courses;
using Shared.DTOS;
using Shared.DTOS.Orders;

namespace Services
{
    public class OrderService(ICartRepository _cartRepo , IUnitOfWork _uof, IMapper _mapper) : IOrderService
    {
        public async Task<OrderSummaryResponse> OrderSummaryAsync(string userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepo.GetCartAsync(userId);
            if(cart is null) throw new BadRequestException("Cannot get summary for non existsing cart"); 
            if(cart.Items.Count == 0) throw new BadRequestException("your cart is empty, Try to add some courses");

            var coursesId = cart.Items.Select(i => i.CourseId);

            var CartCourses = await _uof.GetRepository<Guid,Course>().GetAllAsync(new CoursesSpec(coursesId),cancellationToken);
            

            var subtotal = CartCourses.Sum(c => c.Price);
            var DiscountTotal = CartCourses.Sum(c => c.GetEffectivePrice(DateTime.UtcNow));
            
            return new OrderSummaryResponse()
            {
                DiscountAmount = subtotal - DiscountTotal,
                Items = _mapper.Map<ICollection<OrderItemResponse>>(CartCourses),
                Subtotal = subtotal,
                Total = DiscountTotal
            };

        }
        public Task<CheckoutResponse> CreateOrderAsync(string userId, CancellationToken ct)
        {
            
        }

        public Task<PaginatedResponse<OrderResponse>> GetMyOrdersAsync(string userId, OrderQueryParams queryParams, CancellationToken ct)
        {
            
        }

        public Task<OrderResponse> GetOrderByIdAsync(string userId, Guid orderId, CancellationToken ct)
        {
            
        }

    }
}