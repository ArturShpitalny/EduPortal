using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Core.Models.DTO
{
    public class AboutUserModel
    {
        public UserModel UserModel { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
        public UserSkill TotalUserSkill { get; set; }
    }
}
