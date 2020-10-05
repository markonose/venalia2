using System;
using System.Collections.Generic;
using WebApi.Shared;

namespace WebApi.Responses.Installers
{
	public class ListInstallersResponse
	{
		public List<ListInstallersResponseData> Data { get; set; }
		public List<Error> Errors { get; set; }
		public Pagination Pagination { get; set; }
	}

	public class ListInstallersResponseData
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
