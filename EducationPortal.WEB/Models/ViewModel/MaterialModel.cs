using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class MaterialModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string MaterialName { get; set; }

        [DisplayName("Описание")]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string MaterialDescription { get; set; }

        [DisplayName("Указание ресурса")]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string MaterialResource { get; set; }

        [DisplayName("Умение")]
        [Required]
        public int SkillId { get; set; }
    }
}