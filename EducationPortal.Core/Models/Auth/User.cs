using EducationPortal.Core.Models.Entities;
using EducationPortal.Core.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Core.Models.Auth
{
    public class User : BaseEntity
    {
        public User()
        {
            this.UserCourse = new HashSet<UserCourse>();
            this.CompletedUserMaterials = new HashSet<CompletedUserMaterial>();
        }

        [Key]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(20)]
        public string UserPassword { get; set; }

        [Required]
        public DateTime UserBirthdate { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserCourse> UserCourse { get; set; }
        public virtual UserSkill UserSkill { get; set; }
        public ICollection<CompletedUserMaterial> CompletedUserMaterials { get; set; }
    }
}
