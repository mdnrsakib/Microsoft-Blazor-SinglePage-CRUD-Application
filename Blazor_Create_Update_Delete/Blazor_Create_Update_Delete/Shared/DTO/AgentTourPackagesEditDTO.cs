using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_Create_Update_Delete.Shared.DTO
{
    public class AgentTourPackagesEditDTO
    {
        [Key]
        public int TravelAgentId { get; set; }
        [Required]
        public int TourPackageId { get; set; }
    }
}
