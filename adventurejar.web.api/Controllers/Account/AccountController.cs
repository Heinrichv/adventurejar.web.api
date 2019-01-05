using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureJar.Web.Contracts.Request;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AdventureJar.Web.Api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Table _table;
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly IAmazonS3 _amazonS3;
        private readonly IConfiguration _Configuration;

        public AccountController(IAmazonDynamoDB dynamoDb, IAmazonS3 amazonS3, IConfiguration configuration)
        {
            this._dynamoDbContext = new DynamoDBContext(dynamoDb);
            this._table = Table.LoadTable(dynamoDb, "Account");
            this._amazonS3 = amazonS3;
            _Configuration = configuration;
        }

        [HttpGet("api/account/{id}")]
        public IActionResult GetAccountById(string id)
        {
            return this.Ok();
        }

        
        [HttpGet("api/account/{search}/search")]
        public IActionResult GetAccountsBySearch(string search)
        {
            return this.Ok();
        }
        
        [HttpPost("api/account")]
        public IActionResult CreateAccount([FromBody] CreateAccountRequest request)
        {
            return this.Ok();
        }

        [HttpPost("api/account/link")]
        public IActionResult LinkAccount([FromBody] LinkAccountRequest request)
        {
            return this.Ok();
        }
    }
}