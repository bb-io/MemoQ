﻿using MQS.ServerProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memoq.Models.ServerProjects.Responses
{
    public class ListAllProjectsResponse
    {
        public IEnumerable<ServerProjectInfo> ServerProjects { get; set; }
    }
}
