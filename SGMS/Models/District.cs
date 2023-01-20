using System.ComponentModel.DataAnnotations;

namespace SGMS.Models
{
    public class District
    {
        [Key]
        public Guid DistrictId { get; set; }

        [Display(Name = "District Name")]
        public string DistrictName { get; set; } = string.Empty;
    }
}
