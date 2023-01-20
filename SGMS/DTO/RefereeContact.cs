using SGMS.Enums;
using SGMS.Models;
using System.ComponentModel.DataAnnotations;

namespace SGMS.DTO
{
    public class RefereeContact
    {
        public Guid RefereeId { get; set; }
        public eTitle Title { get; set; }
        public eGender Gender { get; set; }

        [MaxLength(15)]
        [MinLength(3)]
        [StringLength(15)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(15)]
        [MinLength(3)]
        [StringLength(15)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(13)]
        [MinLength(13)]
        [StringLength(13)]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; } = string.Empty;

        [MaxLength(10)]
        [MinLength(10)]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        public Contact? Contact { get; set; }
        public string? Photo { get; set; } = string.Empty;

        [Display(Name = "Referee Type")]
        public eRefereeType RefereeType { get; set; }

        [Display(Name = "Referee Level")]
        public eRefereeLevel RefereeLevel { get; set; }

        [Display(Name = "Has Joined LFA?")]
        public eChoice HasJoinedLFA { get; set; }

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
        public eLevelofExperience Experience { get; set; }

    }
}
