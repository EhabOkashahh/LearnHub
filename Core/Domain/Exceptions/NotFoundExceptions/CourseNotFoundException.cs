using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions.NotFoundExceptions
{
    public class CourseNotFoundException(Guid id) : NotFoundException($"this course with id: {id}, was not found")
    {
        
    }
}