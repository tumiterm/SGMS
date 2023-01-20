using SGMS.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMS.Models
{
    public class Coach : Base
    {
        [Key]
        public Guid CoachId { get; set; }
        public eTitle Title { get; set; }
        public string Name { get; set; }

        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [Display(Name ="ID Number")]
        [MaxLength(13)]
        public string IDNumber { get; set; }

        [Display(Name ="RSA ID Copy")]
        public string? RSAIDCopy { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        public Contact Contact { get; set; }

        [NotMapped]
        [Display(Name ="RSA ID Copy File")]
        public IFormFile RSAIDCopyFile { get; set; }

    }
}
