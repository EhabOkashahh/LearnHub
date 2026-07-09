using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions.NotFoundExceptions
{
    public class CartNotFoundException(string cartId) : NotFoundException($"the cart with id:{cartId} was not found")
    {
        
    }
}