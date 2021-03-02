using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class SkillModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string SkillName { get; set; }

        [DisplayName("Балл")]
        [Required]
        [DataType(DataType.Text)]
        public int SkillScore { get; set; }
    }
}