using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class BookModel : MaterialModel
    {
        [DisplayName("Авторы")]
        [DataType(DataType.MultilineText)]
        [Required]
        [StringLength(500)]
        public string BookAuthors { get; set; }

        [DisplayName("Количество страниц")]
        [DataType(DataType.Text)]
        [Required]
        public int BookPages { get; set; }

        [DisplayName("Формат")]
        [DataType(DataType.Text)]
        [Required]
        [StringLength(50)]
        public string BookFormat { get; set; }

        [DisplayName("Год выпуска")]
        [DataType(DataType.Text)]
        [RegularExpression("^[1-2][0-9]{3}$", ErrorMessage = "Год должен состоять из 4-х символов")]
        public int BookYear { get; set; }
    }
}