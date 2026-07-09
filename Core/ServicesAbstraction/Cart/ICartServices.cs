using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOS.Cart;

namespace ServicesAbstraction.Cart
{
    public interface ICartServices
    {
        Task<CartDto> GetCartAsync(string cartId);
        Task<CartDto> CreateCartAsync(CartDto cartdto, TimeSpan ExistanceDuration);
        Task<bool> DeleteCartAsync(string cartId);
    }
}