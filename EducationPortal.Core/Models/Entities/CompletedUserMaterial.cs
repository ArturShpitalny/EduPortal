using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class CompletedUserMaterial : BaseEntity
    {
        [Key]
        [Column("UserId", Order = 0)]
        public override int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MaterialId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CourseId { get; set; }

        public User User { get; set; }
        public Material Material { get; set; }        
    }
}
