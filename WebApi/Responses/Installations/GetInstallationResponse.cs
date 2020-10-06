using System;
using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Responses.Installation
{
	public class GetInstallationResponse
    {
		public GetInstallationResponseData Data { get; set; }
		public List<Error> Errors { get; set; }
    }

    public class GetInstallationResponseData
    {
		public Guid Id { get; set; }
		public Guid BusinessId { get; set; }
		public Guid? InstallerId { get; set; }
		public string Address { get; set; }
		public string StreetNumber { get; set; }
		public string PostCode { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string CustomerFirstName { get; set; }
		public string CustomerLastName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerPhoneNumber { get; set; }
		InstallationDetails[] Installations { get; set; }
		public bool IsExpress { get; set; }
		public string Remark { get; set; }
		public DateTime Deadline { get; set; }
		public InstallationStatus Status { get; set; }
		public DateTime Created { get; set; }
		public int Quantity { get; set; }
		public bool IsNew => (Created - DateTime.Now).Days <= 5;
		public bool IsDeleted { get; set; }
		public List<WebApi.Entities.File> Files { get; set; }

		public class InstallationDetails
		{
			public string AcUnitBrand { get; set; }
			public decimal AcUnitPower { get; set; }
			public decimal? AcIndoorUnitHeight { get; set; }
			public decimal? AcOutdoorUnitHeight { get; set; }
		}
	}
}
