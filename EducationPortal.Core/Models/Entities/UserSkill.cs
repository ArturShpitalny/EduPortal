using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class UserSkill : BaseEntity
    {
        [Key]
        [Column("UserId")]
        public override int Id { get; set; }
        public int Rating { get; set; }
        public virtual User User { get; set; }
    }
}
