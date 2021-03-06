﻿using EducationPortal.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationPortal.Core.Models.DTO
{
    public class CourseInformationModel
    {
        public Course Course { get; set; }
        public IEnumerable<Material> Materials { get; set; }
    }
}
