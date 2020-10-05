using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Requests.Businesses;
using WebApi.Responses.Businesses;
using WebApi.Services;
using WebApi.Shared;

namespace WebApi.Apis
{
    [Route("api/businesses")]
    public class BusinessController : ApiBase
    {
        private readonly BusinessService _businessService;
        private readonly IMapper _mapper;

        public BusinessController(BusinessService businessService, IMapper mapper)
        {
            _businessService = businessService;
            _mapper = mapper;
        }

    }
}
