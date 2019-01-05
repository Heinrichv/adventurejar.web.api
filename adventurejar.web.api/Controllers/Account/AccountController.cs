using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly Table _activityTable;
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly IAmazonS3 _amazonS3;
        private readonly IConfiguration _Configuration;

        public AccountController(IAmazonDynamoDB dynamoDb, IAmazonS3 amazonS3, IConfiguration configuration)
        {
            this._dynamoDbContext = new DynamoDBContext(dynamoDb);
            this._activityTable = Table.LoadTable(dynamoDb, "Account");
            this._amazonS3 = amazonS3;
            _Configuration = configuration;
        }

        [HttpGet("api/account/random")]
        public IActionResult GetAccount()
        {

        }
    }
}