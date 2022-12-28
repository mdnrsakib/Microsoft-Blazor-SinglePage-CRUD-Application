using Blazor_Create_Update_Delete.Shared.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blazor_Create_Update_Delete.Shared.DTO
{
    public class TourPackageDTO
    {
        public int TourPackageId { get; set; }
        [EnumDataType(typeof(PakageCategory)), Display(Name = "Pakage Category")]
        public PakageCategory PackageCategory { get; set; }
        [Required, StringLength(50), Display(Name = "Package Name")]
        public string PackageName { get; set; } = default!;
        [Required, Column(TypeName = "money"), Display(Name = "Cost Per Person"), DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal CostPerPerson { get; set; }
        [Required, Display(Name = "Tour Time")]
        public int TourTime { get; set; }
        public bool CanDelete { get; set; }
    }
}
