using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions.NotFoundExceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
        
    }
}