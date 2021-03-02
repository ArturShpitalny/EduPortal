using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class UserRegistrationFormModel
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

        [DisplayName("Повторите пароль")]
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [RegularExpression("^.{5,}$", ErrorMessage = "Пароль должен быть не меньше 5 символов")]
        [StringLength(20)]
        public string PasswordConfirm { get; set; }

        [DisplayName("Дата рождения")]
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; } = DateTime.Now.AddDays(-1);
    }
}