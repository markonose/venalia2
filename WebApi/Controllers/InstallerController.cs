using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Requests.Installers;
using WebApi.Responses.Installers;
using WebApi.Services;
using WebApi.Shared;

namespace WebApi.Apis
{
    [Route("api/installers")]
    public class InstallerController : ApiBase
    {
        private readonly InstallerService _installerService;
        private readonly IMapper _mapper;

        public InstallerController(InstallerService installerService, IMapper mapper)
        {
            _installerService = installerService;
            _mapper = mapper;
        }
    }
}
