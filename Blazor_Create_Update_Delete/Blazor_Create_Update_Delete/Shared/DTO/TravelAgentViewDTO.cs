using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blazor_Create_Update_Delete.Shared.DTO
{
    public class TravelAgentViewDTO
    {
        [Key]
        public int TravelAgentId { get; set; }
        [Required, StringLength(50), Display(Name = "Agent Name")]
        public string AgentName { get; set; } = default!;
        [Required, StringLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [Required, StringLength(70), Display(Name = "Agent Address")]
        public string AgentAddress { get; set; } = default!;

    }
}
