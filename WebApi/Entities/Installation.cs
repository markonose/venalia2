using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Enums;
using WebApi.Shared;

namespace WebApi.Entities
{
	public class Installation : EntityBase
	{
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
		public InstallationDetail[] InstallationDetails { get; set; }
		public int Quantity { get; set; }
		public bool IsExpress { get; set; }
		public string Remark { get; set; }
		public DateTime Deadline { get; set; }
		public InstallationStatus Status { get; set; }
		public bool IsDeleted { get; set; }
	}
}
