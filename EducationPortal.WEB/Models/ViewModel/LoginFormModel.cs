using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class LoginFormModel
    {
        [DisplayName("Email")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string Email { get; set; }

        [DisplayName("Пароль")]
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^.{5,}$", ErrorMessage = "Пароль должен быть не меньше 5 символов")]
        [StringLength(20)]
        public string Password { get; set; }
    }
}