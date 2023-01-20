using SGMS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGMS.Models
{
    public class PlayerSelection : Base
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Match")]
        public Guid MatchId { get; set; }

        [Display(Name = "Team")]
        public Guid TeamId { get; set; }

        [Display(Name = "Player")]
        public Guid PLayerId { get; set; }
        public ePosition Position { get; set; }

        [Display(Name = "Jersey Number")]
        public int Number { get; set; }

        [Display(Name = "Player Status?")]
        public eChoice IsSubstitute { get; set; }
        public Guid FirstTeamId { get; set; }
        public Guid SecondTeamId { get; set; }

    }

}

