using EducationPortal.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Core.Models.DTO
{
    public class MaterialCoursePageModel
    {
        public Material Material { get; set; }
        public bool IsComplete { get; set; }
        public int CourseId { get; set; }
        public int NextPage { get; set; }
        public int PageCount { get; set; }
    }
}
