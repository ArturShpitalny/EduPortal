using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class VideoModel : MaterialModel
    {
        [DisplayName("Продолжительность мин.")]
        [DataType(DataType.Text)]
        [Required]
        public int VideoLength { get; set; }

        [DisplayName("Разрешение px.")]
        [DataType(DataType.Text)]
        [Required]
        [StringLength(10)]
        public string VideoResolution { get; set; }
    }
}