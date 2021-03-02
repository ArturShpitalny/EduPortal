using EducationPortal.Core.Models.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Core.Models.Entities
{
    public class Book : Material
    {
        [Required]
        [StringLength(500)]
        public string BookAuthors { get; set; }

        [Required]
        public int BookPages { get; set; }

        [Required]
        [StringLength(50)]
        public string BookFormat { get; set; }

        [Required]
        public int BookYear { get; set; }

        public override string[] GetAdditionalInformation()
        {
            return new string[] { $"Authors: {this.BookAuthors}", $"Number of pages: {this.BookPages}", $"Format: {this.BookFormat}", $"Year of issue: {this.BookYear}" };
        }
    }
}
