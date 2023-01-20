using SGMS.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGMS.Models
{
    public class Match : Base
    {
        [Key]
        public Guid MatchId { get; set; }

        [Display(Name = "Team One")]
        public Guid FirstTeamId { get; set; }

        [Display(Name = "Team Two")]
        public Guid SecondTeamId { get; set; }

        [Display(Name = "Referee")]
        public Guid RefereeId { get; set; }

        [Display(Name = "Man Of The Match")]
        public Guid? PlayerId { get; set; }
        public int? Score1  { get; set; }
        public int? Score2 { get; set; }
        public eMatchStatus? Status { get; set; }

        [Display(Name = "Tournament Name")]
        public Guid TournamentId { get; set; }

        [Display(Name = "Game Day & Time")]
        public DateTime GameDay { get; set; }

        [Display(Name = "Match Venue")]
        public string? Venue { get; set; }

        [Display(Name = "Add Post Game Info?")]
        public bool AddPostGameInfo { get; set; }
    }
}
