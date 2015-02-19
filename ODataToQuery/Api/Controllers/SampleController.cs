using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.OData.Query;
using ObjectToQueryBuilder;
using ObjectToQueryBuilder.Models;

namespace ODataToQuery.Api.Controllers
{
    public class SampleController : ApiController
    {
        [HttpGet]
        public IEnumerable<SampleModel> Get(ODataQueryOptions<SampleModel> queryOptions)
        {
            // grab the OData filter expression
            var filterExpression = queryOptions.Filter.ToExpression<SampleModel>();

            //TODO:  need to implement the order by portion
            var orderBy = queryOptions.OrderBy;

            return new List<SampleModel>();
        }
    }
}
