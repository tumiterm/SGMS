using SGMS.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMS.Models
{
    public class Team : Base
    {
        [Key]
        public Guid TeamId { get; set; }

        [Display(Name = "Team Name")]
        public string TeamName { get; set; } = string.Empty;

        [Display(Name = "Team Logo")]
        public string? TeamLogo { get; set; } = string.Empty;

        [Display(Name = "About Team")]
        public string? Description { get; set; }

        [Display(Name = "District")]
        public eDistrict DistrictId { get; set; }

        [Display(Name = "Municipality")]
        public eMunicipality MunicipalityId { get; set; }

        [Display(Name = "Contact Person")]
        public string? TeamUser { get; set; }

        [Display(Name = "Contact Person Role")]
        public eRole Role { get; set; }

        [NotMapped]
        public IFormFile? TeamLogoFile { get; set; }

    }
}
