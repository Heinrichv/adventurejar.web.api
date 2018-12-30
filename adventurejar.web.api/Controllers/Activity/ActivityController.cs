using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdventureJar.Web.Api.Controllers.Activity
{
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        
        public ActivityController(IAmazonDynamoDB dynamoDb)
        {
            this._amazonDynamoDb = dynamoDb;
        }

        [HttpGet("api/activity/all")]
        public async Task<IActionResult> GetAllActivities()
        {
            ScanResponse res = await this._amazonDynamoDb.ScanAsync(new ScanRequest("Activity"));

            if (res.HttpStatusCode == HttpStatusCode.OK)
            {
                return this.Ok(res.Items);
            }

            return this.BadRequest(res);
        }

        [HttpGet("api/activity")]
        public async Task<IActionResult> AddActivity()
        {
            PutItemResponse res = await this._amazonDynamoDb.PutItemAsync(new PutItemRequest
            {
                TableName = "Activity",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "ActivityCodeName", new AttributeValue("Running") },
                    { "ActivityName", new AttributeValue("Lets Go Run") },
                    { "ActivityDescription", new AttributeValue("Running Description") }
                }
            });

            if (res.HttpStatusCode == HttpStatusCode.OK)
            {
                return this.Ok(res);
            }

            return this.BadRequest(res);
        }
    }
}