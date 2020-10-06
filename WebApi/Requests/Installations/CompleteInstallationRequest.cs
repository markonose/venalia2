﻿using System;
using System.Collections.Generic;

namespace WebApi.Requests.Installations
{
    public class CompleteInstallationRequest
    {
        public Guid Id { get; set; }
        public List<Guid> Files { get; set; }
    }
}