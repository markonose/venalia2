using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Extensions;
using WebApi.Requests.Installations;
using WebApi.Responses.Installation;
using WebApi.Services;
using WebApi.Shared;

namespace WebApi.Apis
{
    [Route("api/installations")]
    public class InstallationController : ApiBase
    {
        private readonly InstallationService _installationService;
        private readonly IMapper _mapper;

        public InstallationController(InstallationService installationService, IMapper mapper)
        {
            _installationService = installationService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("{id:guid}/approve")]
        public ActionResult Approve([FromRoute] ApproveInstallationRequest request)
        {
            _installationService.Approve(request);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("{id:guid}/assign/{installerId}")]
        public ActionResult Assign([FromRoute] AssignInstallationRequest request)
        {
            _installationService.Assign(request);

            return Ok();
        }

        [Authorize(Roles = "Administrator,Business")]
        [HttpPatch("{id:guid}/cancel")]
        public ActionResult Cancel([FromRoute] CancelInstallationRequest request)
        {
            _installationService.Cancel(request);

            return Ok();
        }

        [Authorize(Roles = "Installer")]
        [HttpPatch("{id:guid}/complete")]
        public ActionResult Complete([FromRoute] CompleteInstallationRequest request)
        {
            _installationService.Complete(request);

            return Ok();
        }

        [Authorize(Roles = "Administrator,Business")]
        [HttpPost]
        public CreateInstallationResponse Create([FromBody] CreateInstallationRequest request)
        {
            Installation installation = _mapper.Map<Installation>(request);
            _installationService.Create(installation);

            return new CreateInstallationResponse()
            {
                Data = _mapper.Map<CreateInstallationResponseData>(installation)
            };
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:guid}")]
        public ActionResult Delete([FromRoute] DeleteInstallationRequest request)
        {
            _installationService.Delete(request);

            return Ok();
        }

        [HttpGet("{id:guid}")]
        public GetInstallationResponse Get([FromRoute] GetInstallationRequest request)
        {
            var installation = _installationService.GetById(request);

            return new GetInstallationResponse()
            {
                Data = installation
            };
        }

        [HttpGet]
        public ActionResult<ListInstallationsResponse> List([FromRoute] ListInstallationsRequest request)
        {
            var (installations, pagination) = _installationService.List(request);

            return new ListInstallationsResponse()
            {
                Data = installations,
                Pagination = pagination
            };
        }

        [Authorize(Roles = "Installer")]
        [HttpPatch("{id:guid}/start")]
        public ActionResult Start([FromRoute] StartInstallationRequest request)
        {
            _installationService.Start(request);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("{id:guid}/unassign")]
        public ActionResult Unassign([FromRoute] UnassignInstallationRequest request)
        {
            _installationService.Unassign(request);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch("{id:guid}/undelete")]
        public ActionResult Undelete([FromRoute] UndeleteInstallationRequest request)
        {
            _installationService.Undelete(request);

            return Ok();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:guid}")]
        public UpdateInstallationResponse Update(Guid id, [FromBody]UpdateInstallationRequest request)
        {
            Installation installation = _mapper.Map<Installation>(request);
            installation.Id = id;
            installation.ApplyUpdatedFields(HttpContext);

            _installationService.Update(installation);

            return new UpdateInstallationResponse()
            {
                Data = _mapper.Map<UpdateInstallationResponseData>(installation)
            };
        }
    }
}
