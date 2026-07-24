using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Cart;
using StackExchange.Redis;

namespace Presistence.Repository
{
    public class CartRepository(IConnectionMultiplexer _connection) : ICartRepository
    {

        private readonly IDatabase _database = _connection.GetDatabase();


        public async Task<Cart?> GetCartAsync(string CartId)
        {
            var res = await _database.StringGetAsync(CartId);

            if(res.IsNullOrEmpty) return null;

            var cart = JsonSerializer.Deserialize<Cart>(res.ToString());
            if(cart is null) return null;
            
            return cart;
        }
        public async Task<Cart?> AddCartAsync(Cart cart, TimeSpan ExistanceDuration)
        {
            var SerializedCart = JsonSerializer.Serialize(cart);
            var isAdded = await _database.StringSetAsync(cart.Id, SerializedCart, ExistanceDuration);

            if(!isAdded) return null;

            return cart;
        }
        public async Task<bool> DeleteCart(string CartId) => await _database.KeyDeleteAsync(CartId);
        

    }
}