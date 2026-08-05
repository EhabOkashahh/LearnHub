using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Enums;

namespace Shared.DTOS.Courses
{
    public class CourseQueryParams
    {
        // [FromQuery] CourseLevel? Level , Guid? CategpryId , string? sort , string? search ,CancellationToken ct, int? PageIndex, int? PageSize = 5 
        public CourseLevel? Level { get; set; }
        public Guid? CategpryId { get; set; }
        public string? sort { get; set; }
        public string? search { get; set; }
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 5;
    }
}