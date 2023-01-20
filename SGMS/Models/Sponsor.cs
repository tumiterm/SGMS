using SGMS.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGMS.Models
{
    public class Sponsor
    {
        [Key]
        public Guid SponsorId { get; set; }
        public eTitle Title { get; set; }

        [ForeignKey("Tournament")]
        public Guid TournamentId { get; set; }
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(10)]
        [MinLength(10)]
        [StringLength(10)]
        public string Phone { get; set; }

        [Display(Name = "Organization Name")]
        public string? OrganizationName { get; set; }


    }
}
