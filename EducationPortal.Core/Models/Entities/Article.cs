using EducationPortal.Core.Models.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class Article : Material
    {
        public DateTime ArticlePublicationDate { get; set; }

        public override string[] GetAdditionalInformation()
        {
            return new string[] { $"Publication date: {this.ArticlePublicationDate.ToShortDateString()}" };
        }
    }
}
