using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class Material : BaseEntity
    {
        public Material()
        {
            this.CompletedUserMaterials = new HashSet<CompletedUserMaterial>();
            this.MaterialCourses = new HashSet<MaterialCourse>();
        }

        [Key]
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string MaterialName { get; set; }

        [Required]
        [StringLength(500)]
        public string MaterialDescription { get; set; }        

        [Required]
        [StringLength(500)]
        public string MaterialResource { get; set; }

        public int SkillId { get; set; }
        public virtual Skill Skill { get; set; }
        public ICollection<CompletedUserMaterial> CompletedUserMaterials { get; set; }
        public ICollection<MaterialCourse> MaterialCourses { get; set; }

        public virtual string[] GetAdditionalInformation()
        {
            return new string[] { nameof(Material) };
        }
    }
}
