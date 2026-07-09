using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Cart;

namespace Domain.Contracts
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartAsync(string CartId);
        Task<Cart?> AddCartAsync(Cart cart, TimeSpan ExistanceDuration);
        Task<bool> DeleteCart(string CartId);
    }
}