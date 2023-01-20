using SGMS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGMS.Models
{
    public class Referee : Base
    {
        [Key]
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
        [Display(Name ="Last Name")]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(13)]
        [MinLength(13)]
        [StringLength(13)]
       [Display(Name ="ID Number")]
        public string IDNumber { get; set; } = string.Empty;

        [MaxLength(10)]
        [MinLength(10)]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        public Contact? Contact { get; set; }
        public string? Photo { get; set; } = string.Empty;

        [Display(Name ="Referee Type")]
        public eRefereeType RefereeType { get; set; }

        [Display(Name ="Referee Level")]
        public eRefereeLevel RefereeLevel { get; set; }

        [Display(Name ="Is a member of LFA?")]
        public eChoice HasJoinedLFA { get; set; }
        public eLevelofExperience Experience { get; set; }


    }
}
