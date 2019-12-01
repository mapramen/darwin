namespace Darwin.WebApi.Controllers
{
    using Autofac.Features.AttributeFilters;
    using AutoMapper;
    using Darwin.Data.Models;
    using Darwin.Data.Models.RequestContext;
    using Darwin.Data.Stores;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Linq;
    using System.Threading.Tasks;
    using Api = Darwin.WebApi.Models;

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private CloudTable usersTable { get; }

        private IMapper mapper { get; }

        public UserController([KeyFilter(StoreConstants.TableNames.Users)] CloudTable usersTable, IMapper mapper)
        {
            this.usersTable = usersTable;
            this.mapper = mapper;
        }

        [HttpGet]
        public string Get()
        {
            return "Booyah!!!";
        }

        [HttpGet]
        public async Task<Api.User> Me()
        {
            var userContext = this.HttpContext.GetUserContext();

            TableQuery<User> usersTableQuery = new TableQuery<User>()
                .Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userContext.UserId)
                );

            var user = (await usersTable.ExecuteQuerySegmentedAsync(usersTableQuery, null)).SingleOrDefault();

            return this.mapper.Map<Api.User>(user);
        }
    }
}