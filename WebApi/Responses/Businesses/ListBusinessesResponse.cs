using System;
using System.Collections.Generic;
using WebApi.Shared;

namespace WebApi.Responses.Businesses
{
	public class ListBusinessesResponse
	{
		public List<ListBusinessResponseData> Data { get; set; }
		public List<Error> Errors { get; set; }
		public Pagination Pagination { get; set; }
	}

	public class ListBusinessResponseData
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}
}
