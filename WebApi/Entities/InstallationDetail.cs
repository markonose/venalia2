using System;
using WebApi.Shared;

namespace WebApi.Entities
{
    public class InstallationDetail : EntityBase
    {
        public Guid InstallationId { get; set; }
        public string AcUnitBrand { get; set; }
        public decimal AcUnitPower { get; set; }
        public decimal? AcIndoorUnitHeight { get; set; }
        public decimal? AcOutdoorUnitHeight { get; set; }
    }
}
