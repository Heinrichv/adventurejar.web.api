using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventureJar.Web.Contracts.Models;
using AdventureJar.Web.DynamoService.Contracts;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AdventureJar.Web.Api.Controllers.Activity
{
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivity _activity;
        private readonly IAmazonS3 _amazonS3;
        private readonly IConfiguration _configuration;
        
        public ActivityController(IAmazonS3 amazonS3, IActivity activity, IConfiguration configuration)
        {
            this._amazonS3 = amazonS3;
            this._activity = activity;
            this._configuration = configuration;
        }

        [HttpGet("api/activity/all")]
        public async Task<IActionResult> GetAllActivities()
        {
            List<ActivityModel> activities = await this._activity.FindAll();
            return this.Ok(activities);
        }

        [HttpPost("api/activity")]
        public async Task<IActionResult> AddActivity([FromBody] ActivityModel request)
        {
            ActivityModel activity = await this._activity.AddActivity(request);
            return this.Ok(activity);
        }

        [HttpPost("api/activity/image/{id}")]
        public async Task<IActionResult> AddActivityImage([FromForm] IFormFile file, string id)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                using (TransferUtility tranUtility = new TransferUtility(this._amazonS3))
                {
                    await tranUtility.UploadAsync(memoryStream, _configuration.GetSection("S3").ToString(), id);
                }
            }

            return this.Ok();
        }
        
        [HttpPut("api/activity")]
        public async Task<IActionResult> UpdateActivity([FromBody] ActivityModel request)
        {
            ActivityModel activity =  await this._activity.UpdateActivity(request);
            return this.Ok(activity);
        }
       
        [HttpGet("api/activity/random")]
        public async Task<IActionResult> GetRandomActivity()
        {
            ActivityModel activity = await this._activity.SelectRandom();
            return this.Ok(activity);
        }
    }
}