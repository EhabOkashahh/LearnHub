using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Cart;
using Domain.Entities.Courses;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Services.Specifications.CoursesSpecifications;
using ServicesAbstraction.Cart;
using Shared.DTOS.Cart;
using RedLockNet;

namespace Services
{
    public class CartServices(
        ICartRepository _cartRepository,
        IUnitOfWork _uof,
        IMapper _mapper,
        IDistributedLockFactory _LockFactory) : ICartServices
    {
        private static readonly TimeSpan _cartTtl = TimeSpan.FromDays(10);
        public async Task<CartResponse> GetCartAsync(string userId, CancellationToken ct)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart is null || cart.Items.Count == 0)
                return new CartResponse { Id = userId, Items = [] };

            var courseIds = cart.Items.Select(i => i.CourseId).ToList();
            var spec = new CoursesByIdsSpec(courseIds);
            var courses = await _uof.GetRepository<Guid, Course>().GetAllAsync(spec, ct);

            var items = _mapper.Map<IEnumerable<CartItemResponse>>(courses);

            return new CartResponse { Id = userId, Items = items };
        }

        public async Task AddItemAsync(string userId, Guid courseId, CancellationToken ct)
        {
            
            await using var redlock = await _LockFactory.CreateLockAsync(
                userId,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromMilliseconds(200)
            );

            if(!redlock.IsAcquired) throw new BadRequestException("Cart is busy, try again");

            var cart = await _cartRepository.GetCartAsync(userId)
                ?? new Cart { Id = userId, Items = [] };

            if (cart.Items.Any(i => i.CourseId == courseId))
                throw new BadRequestException("This course is already in your cart");

            var courseSpec = new CoursesSpec(courseId);
            var course = await _uof.GetRepository<Guid, Course>().GetAsync(courseSpec, ct);
            if (course is null)
                throw new CourseNotFoundException(courseId);

            if (course.Status != CourseStatus.Published)
                throw new BadRequestException("This course is not available");

            if (course.InstructorId == userId)
                throw new BadRequestException("You cannot add your own course to the cart");

            var enrollmentSpec = new EnrollmentsSpec(userId, courseId);
            if (await _uof.GetRepository<Guid, Enrollment>().Exists(enrollmentSpec))
                throw new BadRequestException("You are already enrolled in this course");

            cart.Items.Add(new CartItem { CourseId = courseId });
            await _cartRepository.AddCartAsync(cart, _cartTtl);
        }

        public async Task RemoveItemAsync(string userId, Guid courseId, CancellationToken ct)
        {
            var cart = await _cartRepository.GetCartAsync(userId);
            if (cart is null) return;

            cart.Items.RemoveAll(i => i.CourseId == courseId);
            await _cartRepository.AddCartAsync(cart, _cartTtl);
        }

        public async Task ClearCartAsync(string userId, CancellationToken ct)
        {
            await _cartRepository.DeleteCart(userId);
        }

    }
}
