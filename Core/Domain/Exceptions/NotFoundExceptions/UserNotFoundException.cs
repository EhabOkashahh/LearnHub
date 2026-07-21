using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions.NotFoundExceptions
{
    public class UserNotFoundException(string claim) : NotFoundException($"User with this {claim} was not found")
    {
        
    }
}