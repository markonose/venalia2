using System;
using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Responses.Installation
{
	public class DeleteInstallationResponse
    {
		public DeleteInstallationResponseData Data { get; set; }
		public List<Error> Errors { get; set; }
    }

    public class DeleteInstallationResponseData
	{
		public Guid Id { get; set; }
		public Guid BusinessId { get; set; }
		public string Address { get; set; }
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
		public int Quantity { get; set; }
		public InstallationStatus Status { get; set; }

		public class InstallationDetails
		{
			public string AcUnitBrand { get; set; }
			public decimal AcUnitPower { get; set; }
			public decimal? AcIndoorUnitHeight { get; set; }
			public decimal? AcOutdoorUnitHeight { get; set; }
		}
	}
}
