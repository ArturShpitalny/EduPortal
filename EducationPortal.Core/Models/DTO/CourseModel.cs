using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Core.Models.DTO
{
    public class CourseModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CourseImagePath { get; set; }
        public bool IsComplete { get; set; }
    }
}
