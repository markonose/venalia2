using FluentValidation;
using System;

namespace WebApi.Requests.Installations
{
    public class AssignInstallationRequest
    {
        public Guid Id { get; set; }
        public Guid InstallerId { get; set; }
    }
}