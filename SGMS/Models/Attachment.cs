using SGMS.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMS.Models
{
    public class Attachment : Base
    {
        [Key]
        public Guid AttachmentId { get; set; }
        public Guid AssociativeKey { get; set; }
        public eAttachmentType Type { get; set; }

        [Display(Name = "Qualification Status")]
        public QualStatus? Status { get; set; }

        [Display(Name = "Qualification Name")]
        public string? QualificationName { get; set; }
        public string File { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "File")]
        public IFormFile AttachmentFile { get; set; }


    }
}
