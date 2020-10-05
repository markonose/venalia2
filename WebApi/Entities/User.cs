using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Enums;
using WebApi.Shared;

namespace WebApi.Entities
{
    public class User : EntityBase
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public UserType Type { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BusinessName { get; set; }

        public string Address { get; set; }

        public string PostCode { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public bool IsTaxablePerson { get; set; }

        public string VATNumber { get; set; }

        public string IBAN { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Demo;
        public bool IsConfirmed { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [InverseProperty("Business")]
        public virtual ICollection<Installation> BusinessInstallations { get; set; }

        [InverseProperty("Installer")]
        public virtual ICollection<Installation> InstallerInstallations { get; set; }
    }
}
