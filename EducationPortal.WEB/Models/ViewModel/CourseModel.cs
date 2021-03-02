using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class CourseModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string CourseName { get; set; }

        [DisplayName("Описание")]
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string CourseDescription { get; set; }
        public string CourseImagePath { get; set; }

        [DisplayName("Выберите изображение")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}