using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AdventureJar.Web.Api.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyPng;
using TinyPng.Responses;

namespace AdventureJar.Web.Api.Controllers.Activity
{
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly Table _activityTable;
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly IAmazonS3 _amazonS3;
        private readonly string _bucket = "adventure-jar-bucket";
        
        public ActivityController(IAmazonDynamoDB dynamoDb, IAmazonS3 amazonS3)
        {
            this._dynamoDbContext = new DynamoDBContext(dynamoDb);
            this._activityTable = Table.LoadTable(dynamoDb, "Activity");
            this._amazonS3 = amazonS3;
        }

        [HttpGet("api/activity/all")]
        public async Task<IActionResult> GetAllActivities()
        {
            Search search = this._activityTable.Scan(new Expression());

            List<Document> documentList;
            do
            {
                documentList = await search.GetNextSetAsync();
            } while (!search.IsDone);
            
            List<ActivityModel> activities = documentList.Select(doc => this._dynamoDbContext.FromDocument<ActivityModel>(doc)).ToList();
            
            return this.Ok(activities);
        }

        [HttpPost("api/activity")]
        public async Task<IActionResult> AddActivity([FromBody] ActivityModel request)
        {
            request.ImageUrl = $"https://{_bucket}.s3.{Amazon.RegionEndpoint.EUWest1.SystemName}.amazonaws.com/{request.Id}";

            Document document = Document.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            await this._activityTable.PutItemAsync(document);
            return this.Ok(request);
        }

        [HttpPost("api/activity/image/{id}")]
        public async Task<IActionResult> AddActivityImage([FromForm] IFormFile file, string id)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (TransferUtility tranUtility = new TransferUtility(this._amazonS3))
                {
                    await tranUtility.UploadAsync(memoryStream, _bucket, id);
                }
            }

            return this.Ok(new AddActivityImageResponse
            {
                Url = $"https://{_bucket}.s3.{Amazon.RegionEndpoint.EUWest1.SystemName}.amazonaws.com/{id}"
            });
        }
        
        [HttpPut("api/activity")]
        public async Task<IActionResult> UpdateActivity([FromBody] ActivityModel request)
        {
            Document document = Document.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            await this._activityTable.UpdateItemAsync(document);
            return this.Ok();
        }
        
        [HttpGet("api/activity/random")]
        public async Task<IActionResult> GetRandomActivity()
        {
            Search search = this._activityTable.Scan(new Expression());

            List<Document> documentList;
            do
            {
                documentList = await search.GetNextSetAsync();
            } while (!search.IsDone);

            List<ActivityModel> activities = documentList.Select(doc => this._dynamoDbContext.FromDocument<ActivityModel>(doc)).ToList();
            
            int index = new Random(10).Next(0, activities.Count - 1);
            return this.Ok(activities[index]);
        }
    }
}