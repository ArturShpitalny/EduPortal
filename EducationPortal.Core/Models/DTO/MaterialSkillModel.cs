using EducationPortal.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Core.Models.DTO
{
    public class MaterialSkillModel<TMaterial>
    {
        public TMaterial Material { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }
}
