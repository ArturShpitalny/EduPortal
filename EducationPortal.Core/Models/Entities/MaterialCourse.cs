using EducationPortal.Core.Models.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class MaterialCourse : BaseEntity
    {
        [Key]
        [Column("MaterialId", Order = 0)]
        public override int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public int CourseId { get; set; }

        public int Position { get; set; }

        public Course Course { get; set; }
        public Material Material { get; set; }        
    }
}
