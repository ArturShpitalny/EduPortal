using EducationPortal.Core.Models.States;
using System;
using System.Collections.Generic;

namespace EducationPortal.Core.Models.DTO
{
    public class EntityItemModel<T>
    {
        public IEnumerable<T> Entities{ get; set; }
        public Pagination Pagination { get; set; }
    }
}
