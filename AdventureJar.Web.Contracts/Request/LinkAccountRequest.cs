using System;
using System.Collections.Generic;
using System.Text;

namespace AdventureJar.Web.Contracts.Request
{
    public class LinkAccountRequest
    {
        public string AccountId { get; set; }
        
        public string LinkAccountId { get; set; }
    }
}
