﻿using System;
using System.Collections.Generic;
using WebApi.Enums;

namespace WebApi.Responses.Users
{
	public class GetUserResponse
    {
		public GetUserResponseData Data { get; set; }
		public List<Error> Errors { get; set; }
    }

    public class GetUserResponseData
    {
		public Guid Id { get; set; }
        public UserType Type { get; set; }
        public string Email { get; set; }
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
        public UserStatus Status { get; set; }
        public bool IsDeleted { get; set; }
        public List<WebApi.Entities.File> Files { get; set; }
    }
}
