using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Cart;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using ServicesAbstraction.Cart;
using Shared.DTOS.Cart;

namespace Services
{
    public class CartServices(ICartRepository _cartRepository, IMapper _mapper) : ICartServices
    {
        public async Task<CartDto> GetCartAsync(string cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            if(cart is null) throw new CartNotFoundException(cartId);

            return _mapper.Map<CartDto>(cart);
        }
        public async Task<CartDto> CreateCartAsync(CartDto cartdto, TimeSpan ExistanceDuration)
        {
            var cart = _mapper.Map<Cart>(cartdto);
            var res = await _cartRepository.AddCartAsync(cart,ExistanceDuration);

            if(res is null) throw new BadRequestException("Failed to create or update cart - something went wrong while saving to Redis ");

            return _mapper.Map<CartDto>(res);
        }

        public async Task<bool> DeleteCartAsync(string cartId)
        {

            await GetCartAsync(cartId);

            var res = await _cartRepository.DeleteCart(cartId);

            if(!res) throw new BadRequestException("Failed to delete cart — something went wrong while deleting from Redis");

            return res;
        }

    }
}