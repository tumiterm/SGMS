using SGMS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGMS.Models
{
    public class Tournament : Base
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
        public eTournamentType? Type { get; set; }
        public bool HasSponsor { get; set; }
        public eSponsorType SponsorType { get; set; }
        public Sponsor? Sponsor { get; set; }

        [Display(Name = "Has Affiliation Fee?")]
        public bool HasAffiliationFee { get; set; }

        [Display(Name = "Affiliation Fee")]
        public decimal? AffiliationFee { get; set; }
        public bool HasPrice { get; set; }
        public decimal? Price1 { get; set; }
        public decimal? Price2 { get; set; }

    }
}
