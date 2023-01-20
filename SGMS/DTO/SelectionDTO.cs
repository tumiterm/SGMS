using SGMS.Enums;
using System.ComponentModel.DataAnnotations;

namespace SGMS.DTO
{
    public class SelectionDTO
    {
        public string Match { get; set; }
        public string FirstTeam { get; set; }
        public string SecondTeam { get; set; }
        public string Tournament { get; set; }
        public DateTime GameDay { get; set; }
        public string Venue { get; set; }



    }
}
