using EducationPortal.Core.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class Course : BaseEntity
    {
        public Course()
        {
            this.UserCourse = new HashSet<UserCourse>();
            this.MaterialCourses = new HashSet<MaterialCourse>();
        }

        [Key]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string CourseName { get; set; }

        [Required]
        [StringLength(500)]
        public string CourseDescription { get; set; }

        [StringLength(150)]
        public string CourseImagePath { get; set; }

        public virtual ICollection<UserCourse> UserCourse { get; set; }
        public ICollection<MaterialCourse> MaterialCourses { get; set; }
    }
}
