using SGMS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGMS.DTO
{
    public class TournamentSponsor
    {
        public Guid TournamentId { get; set; }

        [Display(Name = "Tournament Name")]
        public string TournamentName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Commences On?")]
        public DateTime From { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Finishes On?")]
        public DateTime Till { get; set; }

        [Display(Name = "Last Day of Registration")]
        public DateTime? CutOffDate { get; set; }

        [Display(Name = "Tournament Status")]
        public eTournamentStatus TournamentStatus { get; set; }
        public eTournamentCycle? Cycle { get; set; }

        [Display(Name = "Tournament Type")]
        public eTournamentType? Type { get; set; }

        [Display(Name = "Has Sponsor?")]
        public bool HasSponsor { get; set; }
        public eTitle Title { get; set; }
        public Guid SponsorId { get; set; }
        public Guid TournamentSponsorId { get; set; }
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

        [Display(Name = "Has Affiliation Fee?")]
        public bool HasAffiliationFee { get; set; }

        [Display(Name = "Affiliation Fee")]
        public decimal? AffiliationFee { get; set; }

        [Display(Name= "Has Winner Price ?")]
        public bool HasPrice { get; set; }

        [Display(Name = "First Price")]
        public decimal? Price1 { get; set; }

        [Display(Name = "Second Price")]
        public decimal? Price2 { get; set; }
    }
}
