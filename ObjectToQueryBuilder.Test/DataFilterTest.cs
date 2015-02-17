using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectToQueryBuilder.Test
{
    [TestClass]
    public class DataFilterTest
    {
        #region EqualTo

        [TestMethod]
        public void EqualTo_Single_String()
        {
            const string columnName = "ColName";
            const string strValue = "Value";

            var f = new DataFilter();
            f.EqualTo(columnName, strValue);

            var expected = string.Format("where {0}='{1}'", columnName, strValue);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void EqualTo_Multiple_Strings()
        {
            string columnName1 = "ColName1";
            string strValue1 = "Value1";
            string columnName2 = "ColName2";
            string strValue2 = "Value2";

            var f = new DataFilter();
            f.EqualTo(columnName1, strValue1);
            f.EqualTo(columnName2, strValue2);

            string expected = string.Format("where {0}='{1}' And {2}='{3}'", columnName1, strValue1, columnName2, strValue2);

            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void EqualTo_Single_DateTime()
        {
            string columnName = "ColName";
            DateTime dateValue = DateTime.Now;

            var f = new DataFilter();
            f.EqualTo(columnName, dateValue);

            string expected = string.Format("where {0}='{1}'", columnName, dateValue.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void EqualTo_Single_Int()
        {
            string columnName = "ColName";
            int intValue = 1234;

            var f = new DataFilter();
            f.EqualTo(columnName, intValue);

            string expected = string.Format("where {0}={1}", columnName, intValue);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void EqualTo_Chained()
        {
            var f = new DataFilter();
            // equals method should return itself...
            Assert.AreEqual(f, f.EqualTo("foo", "bar"));

            // ...so calls can be chained
            f.EqualTo("foo", "bar").EqualTo("hinkle", "pinkle");
        }

        #endregion

        #region NotEqualTo

        [TestMethod]
        public void NotEqualTo_Single_String()
        {
            string columnName = "ColName";
            string strValue = "Value";

            var f = new DataFilter();
            f.NotEqualTo(columnName, strValue);

            string expected = string.Format("where {0}<>'{1}'", columnName, strValue);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void NotEqualTo_Single_DateTime()
        {
            string columnName = "ColName";
            DateTime dateValue = DateTime.Now;

            var f = new DataFilter();
            f.NotEqualTo(columnName, dateValue);

            string expected = string.Format("where {0}<>'{1}'", columnName, dateValue.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void NotEqualTo_Single_Int()
        {
            string columnName = "ColName";
            int intValue = 1234;

            var f = new DataFilter();
            f.NotEqualTo(columnName, intValue);

            string expected = string.Format("where {0}<>{1}", columnName, intValue);
            Assert.AreEqual(expected, f.ToString());
        }

        #endregion

        #region Greater than

        [TestMethod]
        public void GreaterThan_Single_DateTime()
        {
            string columnName = "ColName";
            DateTime dateValue = DateTime.Now;

            var f = new DataFilter();
            f.GreaterThan(columnName, dateValue);

            string expected = string.Format("where {0}>'{1}'", columnName, dateValue.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void GreaterThan_Single_Int()
        {
            string columnName = "ColName";
            int intValue = 1234;

            var f = new DataFilter();
            f.GreaterThan(columnName, intValue);

            string expected = string.Format("where {0}>{1}", columnName, intValue);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void GreaterThan_Chained()
        {
            var f = new DataFilter();
            // equals method should return itself...
            Assert.AreEqual(f, f.GreaterThan("foo", "bar"));

            // ...so calls can be chained
            f.GreaterThan("foo", "bar").GreaterThan("hinkle", "pinkle");
        }

        #endregion

        #region Greater than or Equal to

        [TestMethod]
        public void GreaterThanOrEqualTo_Single_Int()
        {
            string columnName = "ColName";
            int intValue = 1234;

            var f = new DataFilter();
            f.GreaterThanOrEqualTo(columnName, intValue);

            string expected = string.Format("where {0}>={1}", columnName, intValue);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void GreaterThanOrEqualTo_Single_DateTime()
        {
            string columnName = "ColName";
            DateTime date = DateTime.Now;

            var f = new DataFilter();
            f.GreaterThanOrEqualTo(columnName, date);

            string expected = string.Format("where {0}>='{1}'", columnName, date.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expected, f.ToString());
        }

        #endregion

        #region Less than

        [TestMethod]
        public void LessThan_Single_DateTime()
        {
            string columnName = "ColName";
            DateTime dateValue = DateTime.Now;

            var f = new DataFilter();
            f.LessThan(columnName, dateValue);

            string expected = string.Format("where {0}<'{1}'", columnName, dateValue.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void LessThan_Single_Int()
        {
            string columnName = "ColName";
            int intValue = 1234;

            var f = new DataFilter();
            f.LessThan(columnName, intValue);

            string expected = string.Format("where {0}<{1}", columnName, intValue);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void LessThan_Chained()
        {
            var f = new DataFilter();
            // equals method should return itself...
            Assert.AreEqual(f, f.LessThan("foo", "bar"));

            // ...so calls can be chained
            f.LessThan("foo", "bar").GreaterThan("hinkle", "pinkle");
        }

        #endregion

        #region Less than or Equal to

        [TestMethod]
        public void LessThanOrEqualTo_Single_Int()
        {
            string columnName = "ColName";
            int intValue = 1234;

            var f = new DataFilter();
            f.LessThanOrEqualTo(columnName, intValue);

            string expected = string.Format("where {0}<={1}", columnName, intValue);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void LessThanOrEqualTo_Single_DateTime()
        {
            string columnName = "ColName";
            DateTime date = DateTime.Now;

            var f = new DataFilter();
            f.LessThanOrEqualTo(columnName, date);

            string expected = string.Format("where {0}<='{1}'", columnName, date.ToString("dd MMM yyyy HH:mm:ss"));
            Assert.AreEqual(expected, f.ToString());
        }

        #endregion

        #region Or... Tests

        [TestMethod]
        public void OrEqualTo()
        {
            string col = "ColName";
            var f = new DataFilter().EqualTo(col, 1).OrEqualTo(col, 2);
            string expected = string.Format("{0}=1 Or {0}=2", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrNotEqualTo()
        {
            string col = "ColName";
            var f = new DataFilter().NotEqualTo(col, 1).OrNotEqualTo(col, 2);
            string expected = string.Format("{0}<>1 Or {0}<>2", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrGreaterThan()
        {
            string col = "ColName";
            var f = new DataFilter().GreaterThan(col, 1).OrGreaterThan(col, 2);
            string expected = string.Format("{0}>1 Or {0}>2", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrGreaterThanOrEqualTo()
        {
            string col = "ColName";
            var f = new DataFilter().GreaterThanOrEqualTo(col, 1).OrGreaterThanOrEqualTo(col, 2);
            string expected = string.Format("{0}>=1 Or {0}>=2", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrLessThan()
        {
            string col = "ColName";
            var f = new DataFilter().LessThan(col, 1).OrLessThan(col, 2);
            string expected = string.Format("{0}<1 Or {0}<2", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrLessThanOrEqualTo()
        {
            string col = "ColName";
            var f = new DataFilter().LessThanOrEqualTo(col, 1).OrLessThanOrEqualTo(col, 2);
            string expected = string.Format("{0}<=1 Or {0}<=2", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }
        [TestMethod]
        public void OrLike()
        {
            string col = "ColName";
            var f = new DataFilter().Like(col, "1%").OrLike(col, "%2");
            string expected = string.Format("{0} Like '1%' Or {0} Like '%2'", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrContainsString()
        {
            string col = "ColName";
            var f = new DataFilter().EqualTo(col, 1).OrContainsString(col, "2");
            string expected = string.Format("{0}=1 Or {0} Like '%2%'", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrStartsWithString()
        {
            string col = "ColName";
            var f = new DataFilter().EqualTo(col, 1).OrStartsWithString(col, "2");
            string expected = string.Format("{0}=1 Or {0} Like '2%'", col);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrContainsStringWithinCommaSeparatedValues()
        {
            string col = "ColName";
            var f = new DataFilter().EqualTo(col, 1).EqualTo(col, 1).OrContainsStringWithinCommaSeparatedValues(col, "2");
            string expected = string.Format("({0}={1} And {0}={1}) Or ({0}='{2}' Or {0} Like '{2},%' Or {0} Like '%,{2}' Or {0} Like '%,{2},%')", col, 1, 2);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrIsNotNull()
        {
            string colName = "ColName";
            var f = new DataFilter().EqualTo(colName, 1).OrIsNotNull(colName);
            string expected = string.Format("{0}={1} Or {0} Is Not Null", colName, 1);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrIsNull()
        {
            string colName = "ColName";
            var f = new DataFilter().EqualTo(colName, 1).OrIsNull(colName);
            string expected = string.Format("{0}={1} Or {0} Is Null", colName, 1);
            Assert.AreEqual(expected, f.GetExpressionString());
        }

        [TestMethod]
        public void OrEquals_OnlyCondition()
        {
            var f = new DataFilter().OrEqualTo("Col", "Val");
            Assert.AreEqual("Col='Val'", f.GetExpressionString());
        }

        #endregion

        #region Like (for strings)

        [TestMethod]
        public void Like()
        {
            string colName = "ColName";
            string likePattern = "A%";

            var filter = new DataFilter().Like(colName, likePattern);

            string expected = string.Format("where {0} Like '{1}'", colName, likePattern);
            Assert.AreEqual(expected, filter.ToString());
        }

        [TestMethod]
        public void ContainsString()
        {
            string colName = "ColName";
            string subString = "SubString";

            var filter = new DataFilter().ContainsString(colName, subString);

            string expected = string.Format("where {0} Like '%{1}%'", colName, subString);
            Assert.AreEqual(expected, filter.ToString());
        }

        [TestMethod]
        public void StartsWithString()
        {
            string colName = "ColName";
            string prefix = "Prefix";

            var filter = new DataFilter().StartsWithString(colName, prefix);

            string expected = string.Format("where {0} Like '{1}%'", colName, prefix);
            Assert.AreEqual(expected, filter.ToString());
        }

        [TestMethod]
        public void ContainsStringWithinCommaSeparatedValues()
        {
            string colName = "ColName";
            string val = "CsvValue";

            var filter = new DataFilter().ContainsStringWithinCommaSeparatedValues(colName, val);

            // "({0} Like '{1}' OR {0} Like '{1},%' OR {0} Like '%,{1}' OR {0} Like '%,{1},%')"

            string expected = string.Format("where {0}='{1}' Or {0} Like '{1},%' Or {0} Like '%,{1}' Or {0} Like '%,{1},%'", colName, val);
            Assert.AreEqual(expected, filter.ToString());
        }

        [TestMethod]
        public void QueryWithTwoOrBlocksOnly()
        {
            var filter = new DataFilter().EqualTo("A", "B").Or(new DataFilter().EqualTo("C", "D"));
            string expected = "where A='B' Or C='D'";
            Assert.AreEqual(expected, filter.ToString());
        }

        #endregion

        #region NotNull

        [TestMethod]
        public void IsNotNull()
        {
            string colName = "ColName";
            var f = new DataFilter().IsNotNull(colName);
            string expected = string.Format("where {0} Is Not Null", colName);
            Assert.AreEqual(expected, f.ToString());
        }


        #endregion

        #region IsNull

        [TestMethod]
        public void IsNull()
        {
            string colName = "ColName";
            var f = new DataFilter().IsNull(colName);
            string expected = string.Format("where {0} Is Null", colName);
            Assert.AreEqual(expected, f.ToString());
        }


        #endregion

        #region Empty

        [TestMethod]
        public void CreateEmpty()
        {
            var filter = DataFilter.CreateEmpty();
            Assert.AreEqual("", filter.ToString());
        }

        [TestMethod]
        public void CreateEmpty_Modification()
        {
            var filter = DataFilter.CreateEmpty();
            filter.EqualTo("Name", "Bob");
            Assert.AreEqual("", DataFilter.CreateEmpty().ToString());
            Assert.AreEqual("", DataFilter.CreateEmpty().GetExpressionString());
        }


        [TestMethod]
        public void AddEmptyQuery()
        {
            var filter = new DataFilter();
            string expressionString = filter.GetExpressionString();

            filter.And(DataFilter.CreateEmpty());
            filter.And(new DataFilter());
            filter.Or(DataFilter.CreateEmpty());
            filter.Or(new DataFilter());

            Assert.AreEqual(expressionString, filter.GetExpressionString(), "expression string unchanged");
        }

        #endregion

        #region Or

        [TestMethod]
        public void Or()
        {
            var colName = "ColName";
            var val1 = "Val1";
            var val2 = "Val2";

            var innerQuery = new DataFilter().EqualTo(colName, val2);
            var f = new DataFilter().EqualTo(colName, val1).Or(innerQuery);

            string expected = string.Format("where {0}='{1}' Or {0}='{2}'", colName, val1, val2);
            Assert.AreEqual(expected, f.ToString());
        }

        [TestMethod]
        public void Or_TwoOperators()
        {
            var colName = "ColName";
            var val1 = "Val1";
            var val2 = "Val2";
            var val3 = "Val3";

            var innerQuery = new DataFilter().EqualTo(colName, val2).EqualTo(colName, val3);
            var f = new DataFilter().EqualTo(colName, val1).Or(innerQuery);

            string expected = string.Format("where {0}='{1}' Or ({0}='{2}' And {0}='{3}')", colName, val1, val2, val3);
            Assert.AreEqual(expected, f.ToString());
        }


        #endregion

        #region HasExpression

        [TestMethod]
        public void HasExpression()
        {
            var f = new DataFilter();
            Assert.IsFalse(f.HasExpression);
            f.EqualTo("foo", "bar");
            Assert.IsTrue(f.HasExpression);
        }

        #endregion

        #region Literal string escaping

        [TestMethod]
        public void EscapeLiteral()
        {
            var f = new DataFilter().EqualTo("Col1", "ab'cd");
            Assert.AreEqual("Col1='ab''cd'", f.GetExpressionString());
        }

        #endregion

        #region Equals & GetHashCode

        [TestMethod]
        public void Equals_True()
        {
            var q1 = new DataFilter();
            var q2 = new DataFilter();

            Assert.IsTrue(q1.Equals(q2));
            Assert.IsTrue(q2.Equals(q1));

            q1.EqualTo("foo", 2);
            q2.EqualTo("foo", 2);

            Assert.IsTrue(q1.Equals(q2));
            Assert.IsTrue(q2.Equals(q1));
        }

        [TestMethod]
        public void Equals_False()
        {
            var q1 = new DataFilter();
            var q2 = new DataFilter();

            q1.EqualTo("foo", 1);
            q2.EqualTo("bar", 2);

            Assert.IsFalse(q1.Equals(q2));
            Assert.IsFalse(q2.Equals(q1));
        }

        [TestMethod]
        public void GetHashCode_UsingHashMap()
        {
            var q1 = new DataFilter();
            var q2 = new DataFilter();

            Assert.AreEqual(q1.GetHashCode(), q2.GetHashCode());

            q1.EqualTo("foo", 1);
            q2.EqualTo("foo", 1);

            Assert.AreEqual(q1.GetHashCode(), q2.GetHashCode());

            q2.EqualTo("different", 123);

            Assert.IsTrue(q1.GetHashCode() != q2.GetHashCode());
        }


        #endregion
    }
}
