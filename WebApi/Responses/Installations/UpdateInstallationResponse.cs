using System;
using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Responses.Installation
{
	public class UpdateInstallationResponse
    {
		public UpdateInstallationResponseData Data { get; set; }
		public List<Error> Errors { get; set; }
    }

    public class UpdateInstallationResponseData
    {
		public Guid Id { get; set; }
		public Guid BusinessId { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string CustomerFirstName { get; set; }
		public string CustomerLastName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerPhoneNumber { get; set; }
        public string AcUnitBrand { get; set; }
        public decimal AcUnitPower { get; set; }
		public decimal? AcIndoorUnitHeight { get; set; }
		public decimal? AcOutdoorUnitHeight { get; set; }
		public bool IsExpress { get; set; }
		public string Remark { get; set; }
		public DateTime PreferedDate { get; set; }
		public InstallationStatus Status { get; set; }
	}
}
