using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_Create_Update_Delete.Shared.DTO
{
    public class AgentTourPackagesViewDTO
    {
        public int TravelAgentId { get; set; }
        [Required]
        public string PackageName { get; set; } = default!;
        [Required]
        public decimal CostPerPerson { get; set; }
    }
}
