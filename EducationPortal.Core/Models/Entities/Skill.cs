using EducationPortal.Core.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Core.Models.Entities
{
    public class Skill : BaseEntity
    {
        public Skill()
        {
            this.Material = new HashSet<Material>();
        }

        [Key]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SkillName { get; set; }

        [Required]
        public int SkillScore { get; set; }
        public virtual ICollection<Material> Material { get; set; }
    }
}
