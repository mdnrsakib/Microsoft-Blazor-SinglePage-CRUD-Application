using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor_Create_Update_Delete.Shared.DTO
{
    public class AgentTourPackagesDTO
    {
        [ForeignKey("TravelAgent")]
        public int TravelAgentId { get; set; }
        [ForeignKey("TourPackage")]
        public int TourPackageId { get; set; }
        
    }
}
