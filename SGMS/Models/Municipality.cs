using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGMS.Models
{
    public class Municipality
    {
        [Key]
        public Guid MunicipalityId { get; set; }

        [ForeignKey("District")]
        public Guid DistrictId { get; set; }

        [Display(Name = "Municipality")]
        public string MunicipalityName { get; set; }
    }
}
