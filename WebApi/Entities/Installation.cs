using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Enums;
using WebApi.Shared;

namespace WebApi.Entities
{
	public class Installation : EntityBase
	{
		[Required]
		public Guid BusinessId { get; set; }
		public Guid? InstallerId { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string PostCode { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string Region { get; set; }
		[Required]
		public string CustomerFirstName { get; set; }
		[Required]
		public string CustomerLastName { get; set; }
		[Required]
		public string CustomerEmail { get; set; }
		[Required]
		public string CustomerPhoneNumber { get; set; }
		[Required]
		public string AcUnitBrand { get; set; }
		[Required]
		public decimal AcUnitPower { get; set; }
		public decimal? AcIndoorUnitHeight { get; set; }
		public decimal? AcOutdoorUnitHeight { get; set; }
		public bool IsExpress { get; set; }
		public string Remark { get; set; }
		[Required]
		public DateTime PreferedDate { get; set; }
		[Required]
		public InstallationStatus Status { get; set; }
		public bool IsDeleted { get; set; }

		public virtual User Business { get; set; }
		public virtual User Installer { get; set; }
	}
}
