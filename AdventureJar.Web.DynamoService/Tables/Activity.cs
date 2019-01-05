using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureJar.Web.Contracts.Models;
using AdventureJar.Web.DynamoService.Contracts;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Microsoft.Extensions.Caching.Memory;

namespace AdventureJar.Web.DynamoService.Tables
{
    public class Activity : IActivity
    {
        private readonly Table _table;
        private readonly DynamoDBContext _dynamoDbContext;
        private readonly IMemoryCache _memoryCache;

        public Activity(IAmazonDynamoDB dynamoDb, IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            this._dynamoDbContext = new DynamoDBContext(dynamoDb);
            this._table = Table.LoadTable(dynamoDb, "Activity");
        }

        public async Task<List<ActivityModel>> FindAll()
        {
            return await this._memoryCache.GetOrCreateAsync("AdventureJar.Web.DynamoService.Tables.FindAll", async o =>
            {
                o.AbsoluteExpirationRelativeToNow = new TimeSpan(0, 5, 0);

                Search search = this._table.Scan(new Expression());

                List<Document> documentList;
                do
                {
                    documentList = await search.GetNextSetAsync();
                } while (!search.IsDone);

                return documentList.Select(doc => this._dynamoDbContext.FromDocument<ActivityModel>(doc)).ToList();
            });
        }

        public async Task<ActivityModel> AddActivity(ActivityModel model)
        {
            model.ImageUrl = $"https://d2am33tdkempau.cloudfront.net/{model.Id}";

            Document document = Document.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(model));
            await this._table.PutItemAsync(document);

            return model;
        }

        public async Task<ActivityModel> UpdateActivity(ActivityModel model)
        {
            Document document = Document.FromJson(Newtonsoft.Json.JsonConvert.SerializeObject(model));
            await this._table.UpdateItemAsync(document);

            return model;
        }

        public async Task<ActivityModel> SelectRandom()
        {
            List<ActivityModel> activities = await this.FindAll();
            int index = new Random(10).Next(0, activities.Count - 1);

            return activities[index];
        }
    }
}
