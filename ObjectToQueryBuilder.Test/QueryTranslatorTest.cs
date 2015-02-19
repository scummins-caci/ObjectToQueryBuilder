using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectToQueryBuilder.Models;
using ODataToQuery;

namespace ObjectToQueryBuilder.Test
{
    [TestClass]
    public class QueryTranslatorTest
    {
        private ODataQueryContext context;
        private IDictionary<string, string> columnMapping;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<SampleModel>("Samples");
            context = new ODataQueryContext(builder.GetEdmModel(), typeof(SampleModel));

            columnMapping = new Dictionary<string, string>
            {
                {"Name", "NAME_COLUMN"},
                {"Description", "DESCRIPTION_COLUMN"},
                {"Count", "COUNT_COLUMN"},
                {"CurrentDate", "CURRENT_DATE"},
                {"IsReady", "IS_READY"}
            };

        }

        [TestCleanup]
        public void Cleanup()
        {
            context = null;
        }

        [TestMethod]
        public void ExpressionToWhereClause_EqualTo_SingleString_Test()
        {
            // setup
            Expression<Func<SampleModel, bool>> expression = x => x.Name == "Test";
            const string expectedSql = "Name = 'Test'";

            // execute
            var translator = new QueryTranslator();
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_EqualTo_SingleStringNoMapping_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=Name eq 'Test'");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "Name = 'Test'";

            // execute
            var translator = new QueryTranslator();
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        #region Where

        #region Equal To

        [TestMethod]
        public void ODataUri_EqualTo_SingleString_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=Name eq 'Test'");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "NAME_COLUMN = 'Test'";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_EqualTo_SingleBooleanTrue_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=IsReady eq true");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "IS_READY = 1";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_EqualTo_SingleBooleanFalse_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=IsReady eq false");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "IS_READY = 0";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        #endregion

        #region Greater than

        [TestMethod]
        public void ODataUri_GreaterThan_SingleDateTime_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=CurrentDate gt DATETIME'2015-02-19'");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "CURRENT_DATE > TO_DATE('2/19/2015 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM')";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_GreaterThanEqualTo_SingleDateTime_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=CurrentDate ge DATETIME'2015-02-19'");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "CURRENT_DATE >= TO_DATE('2/19/2015 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM')";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_GreaterThan_SingleInteger_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=Count gt 20");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "COUNT_COLUMN > 20";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_GreaterThanEqualTo_SingleInteger_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=Count ge 50");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "COUNT_COLUMN >= 50";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        #endregion

        #region Less than

        [TestMethod]
        public void ODataUri_LessThan_SingleDateTime_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=CurrentDate lt DATETIME'2015-02-19'");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "CURRENT_DATE < TO_DATE('2/19/2015 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM')";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_LessThanEqualTo_SingleDateTime_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=CurrentDate le DATETIME'2015-02-19'");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "CURRENT_DATE <= TO_DATE('2/19/2015 12:00:00 AM', 'MM/DD/YYYY HH:MI:SS AM')";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_LessThan_SingleInteger_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=Count lt 20");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "COUNT_COLUMN < 20";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_LessThanEqualTo_SingleInteger_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=Count le 50");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "COUNT_COLUMN <= 50";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        #endregion

        #region Contains

        [TestMethod]
        public void ODataUri_StartsWith_SingleString_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get,
                "http://www.example.com/?$filter=startswith(Description, 'start of')");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "DESCRIPTION_COLUMN like 'start of%'";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        [TestMethod]
        public void ODataUri_EndsWith_SingleString_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=endswith(Description, 'end of')");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "DESCRIPTION_COLUMN like '%end of'";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }


        [TestMethod]
        public void ODataUri_Contains_SingleString_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$filter=substringof('contains this', Description)");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.Filter.ToExpression<SampleModel>();
            const string expectedSql = "DESCRIPTION_COLUMN like '%contains this%'";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var whereClause = translator.WhereClause;

            // assert
            Assert.AreEqual(expectedSql, whereClause);
        }

        #endregion

        #endregion

        #region Order by

        [TestMethod]
        public void ODataUri_OrderBy_Single_Asc_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$orderby=Name asc");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.OrderBy.ToExpression<SampleModel>();
            const string expectedSql = "NAME_COLUMN ASC";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var orderBy = translator.OrderBy;

            // assert
            Assert.AreEqual(expectedSql, orderBy);
        }

        [TestMethod]
        public void ODataUri_OrderBy_Single_Desc_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$orderby=Description desc");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.OrderBy.ToExpression<SampleModel>();
            const string expectedSql = "DESCRIPTION_COLUMN DESC";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var orderBy = translator.OrderBy;

            // assert
            Assert.AreEqual(expectedSql, orderBy);
        }

        [TestMethod]
        public void ODataUri_OrderBy_Multiple_Desc_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$orderby=Description desc, Name desc, Count desc");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.OrderBy.ToExpression<SampleModel>();
            const string expectedSql = "COUNT_COLUMN DESC, NAME_COLUMN DESC, DESCRIPTION_COLUMN DESC";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var orderBy = translator.OrderBy;

            // assert
            Assert.AreEqual(expectedSql, orderBy);
        }

        [TestMethod]
        public void ODataUri_OrderBy_Multiple_Asc_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$orderby=Description, Name, Count asc");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.OrderBy.ToExpression<SampleModel>();
            const string expectedSql = "COUNT_COLUMN ASC, NAME_COLUMN ASC, DESCRIPTION_COLUMN ASC";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var orderBy = translator.OrderBy;

            // assert
            Assert.AreEqual(expectedSql, orderBy);
        }

        [TestMethod]
        public void ODataUri_OrderBy_Multiple_Mixed_Test()
        {
            // setup
            var request = new HttpRequestMessage(HttpMethod.Get, "http://www.example.com/?$orderby=Description desc, Name");
            var queryOptions = new ODataQueryOptions<SampleModel>(context, request);

            var expression = queryOptions.OrderBy.ToExpression<SampleModel>();
            const string expectedSql = "NAME_COLUMN ASC, DESCRIPTION_COLUMN DESC";

            // execute
            var translator = new QueryTranslator(columnMapping);
            translator.Translate(expression);
            var orderBy = translator.OrderBy;

            // assert
            Assert.AreEqual(expectedSql, orderBy);
        }

        #endregion
    }
}
