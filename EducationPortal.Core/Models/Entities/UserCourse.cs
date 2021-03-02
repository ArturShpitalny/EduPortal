using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class UserCourse : BaseEntity
    {
        [Key]
        [Column("UserId", Order = 0)]
        public override int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public int CourseId { get; set; }

        public bool IsComplete { get; set; }
        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
