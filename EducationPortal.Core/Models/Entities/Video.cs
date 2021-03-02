using System;
using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Core.Models.Entities
{
    public class Video : Material
    {
        [Required]
        public int VideoLength { get; set; }

        [Required]
        [StringLength(10)]
        public string VideoResolution { get; set; }

        public override string[] GetAdditionalInformation()
        {
            return new string[] { $"Length: {this.VideoLength} min.", $"Resolution: {this.VideoResolution}" };
        }
    }
}
