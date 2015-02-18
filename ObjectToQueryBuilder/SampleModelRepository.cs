using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ObjectToQueryBuilder.Models;

namespace ObjectToQueryBuilder
{
    public class SampleModelRepository
    {

        public IEnumerable<SampleModel> GetItems(Expression<Func<SampleModel, bool>> filter)
        {
            // convert expression to sql
            var sqlTranslator = new QueryTranslator();
            var whereClause = sqlTranslator.Translate(filter);


            return new List<SampleModel>();
        }

    }
}
