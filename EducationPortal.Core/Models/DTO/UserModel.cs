using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Core.Models.DTO
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public int RoleId { get; set; }
    }
}
