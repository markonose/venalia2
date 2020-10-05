using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Entities
{
    public class InstallationDetails
    {
        public string AcUnitBrand { get; set; }
        public decimal AcUnitPower { get; set; }
        public decimal? AcIndoorUnitHeight { get; set; }
        public decimal? AcOutdoorUnitHeight { get; set; }
    }
}
