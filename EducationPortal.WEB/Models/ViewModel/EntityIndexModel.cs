using EducationPortal.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationPortal.WEB.Models.ViewModel
{
    public class EntityIndexModel<TEntity>
    {
        public TEntity Entity { get; set; }
        public int Index { get; set; }
    }
}