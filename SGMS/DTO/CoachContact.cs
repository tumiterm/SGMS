using SGMS.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMS.DTO
{
    public class CoachContact
    {
        public Guid CoachId { get; set; }
        public eTitle Title { get; set; }
      
        public string Name { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "ID Number")]
        [MaxLength(13)]
        public string IDNumber { get; set; }

        [Display(Name = "RSA ID Copy")]
        public string? RSAIDCopy { get; set; }

         public Guid ContactId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [StringLength(10)]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [StringLength(10)]

        [Display(Name = "Alternative Cellphone Number")]
        public string? Alternative { get; set; }

        [NotMapped]

        [Display(Name = "RSA ID Copy File")]
        public IFormFile RSAIDCopyFile { get; set; }

    }
}
