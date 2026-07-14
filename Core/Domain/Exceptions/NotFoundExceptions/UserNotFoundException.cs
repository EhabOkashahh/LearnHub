using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions.NotFoundExceptions
{
    public class UserNotFoundException(string email) : NotFoundException($"User with this email: {email} was not found")
    {
        
    }
}