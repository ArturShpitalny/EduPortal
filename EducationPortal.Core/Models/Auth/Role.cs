using EducationPortal.Core.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Core.Models.Auth
{
    public class Role : BaseEntity
    {
        public Role()
        {
            this.User = new HashSet<User>();
        }

        [Key]
        public override int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
