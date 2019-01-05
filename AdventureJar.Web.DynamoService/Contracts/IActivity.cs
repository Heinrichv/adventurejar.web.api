using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AdventureJar.Web.Contracts.Models;

namespace AdventureJar.Web.DynamoService.Contracts
{
    public interface IActivity
    {
        Task<List<ActivityModel>> FindAll();

        Task<ActivityModel> AddActivity(ActivityModel model);

        Task<ActivityModel> UpdateActivity(ActivityModel model);

        Task<ActivityModel> SelectRandom();
    }
}
