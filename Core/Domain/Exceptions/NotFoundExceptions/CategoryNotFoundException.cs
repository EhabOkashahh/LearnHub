using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions.NotFoundExceptions
{
    public class CategoryNotFoundException(Guid Id) : NotFoundException($"the category with id: {Id}, was not found")
    {
        
    }
}