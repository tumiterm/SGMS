using System.ComponentModel.DataAnnotations;

namespace SGMS.Models
{
    public class Contact
    {
        [Key]
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
    }
}
