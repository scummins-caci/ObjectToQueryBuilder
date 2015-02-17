using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ObjectToQueryBuilder
{
    /// <summary>
    /// Builds a sql 'where' statement to DataFilter down repository data based on properties and their values added
    /// </summary>
    [Serializable]
    public class DataFilter
    {
        #region CreateEmpty

		/// <summary>
		/// Gets an empty query.
		/// </summary>
		public static DataFilter CreateEmpty()
		{
			return new DataFilter();
		}


        #endregion

        ConditionNode _topNode;

		/// <summary>
		/// Constructs a new, empty, query.
		/// </summary>
		public DataFilter()
		{
			_topNode = null;
		}


		#region Equals

		/// <summary>
		/// Extends the Query (using the AND operator) such that only rows with values equal to
		/// that specified in the named column should be included.
		/// </summary>
		/// <param name="columnName">The column to DataFilter within.</param>
        /// <param name="equalTo">The value that remaining rows must be equal to.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter EqualTo(string columnName, object equalTo)
		{
			return And(CreateEqualToCondition(columnName, equalTo));
		}

		public DataFilter OrEqualTo(string columnName, object equalTo)
		{
			return Or(CreateEqualToCondition(columnName, equalTo));
		}

		private static ConditionNode CreateEqualToCondition(string columnName, object equalTo)
		{
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(equalTo), "=");
			return condition;
		}


		#endregion

		#region NotEqualTo

		/// <summary>
		/// Extends the Query (using the AND operator) such that only rows with values not equal to
		/// that specified in the named column should be included.
		/// </summary>
		/// <param name="columnName">The column to DataFilter within.</param>
		/// <param name="notEqualTo">The value that remaining rows must not be equal to.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter NotEqualTo(string columnName, object notEqualTo)
		{
			return And(CreateNotEqualToCondition(columnName, notEqualTo));

		}

		public DataFilter OrNotEqualTo(string columnName, object notEqualTo)
		{
			return Or(CreateNotEqualToCondition(columnName, notEqualTo));
		}

		private ConditionNode CreateNotEqualToCondition(string columnName, object notEqualTo)
		{
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(notEqualTo), "<>");
			return condition;
		}

		#endregion

		#region GreaterThan

		/// <summary>
		/// Extends the Query (using the AND operator) such that only rows with values greater than
		/// that specified in the named column should be included.
		/// </summary>
		/// <param name="columnName">The column to DataFilter within.</param>
		/// <param name="greaterThan">The value that remaining rows must be greater than.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter GreaterThan(string columnName, object greaterThan)
		{
			return And(CreateGreaterThanCondition(columnName, greaterThan));
		}

		public DataFilter OrGreaterThan(string columnName, object greaterThan)
		{
			return Or(CreateGreaterThanCondition(columnName, greaterThan));
		}

		private ConditionNode CreateGreaterThanCondition(string columnName, object greaterThan)
		{
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(greaterThan), ">");
			return condition;
		}

		#endregion

		#region GreaterThanOrEqualTo

		/// <summary>
		/// Extends the Query (using the AND operator) such that only rows with values greater than or equal to
		/// that specified in the named column should be included.
		/// </summary>
		/// <param name="columnName">The column to DataFilter within.</param>
        /// <param name="greaterThanOrEqualTo">The value that remaining rows must be greater than or equal to.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter GreaterThanOrEqualTo(string columnName, object greaterThanOrEqualTo)
		{
			return And(CreateGreaterThanOrEqualToCondition(columnName, greaterThanOrEqualTo));
		}

		public DataFilter OrGreaterThanOrEqualTo(string columnName, object greaterThanOrEqualTo)
		{
			return Or(CreateGreaterThanOrEqualToCondition(columnName, greaterThanOrEqualTo));
		}

		private ConditionNode CreateGreaterThanOrEqualToCondition(string columnName, object greaterThanOrEqualTo)
		{
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(greaterThanOrEqualTo), ">=");
			return condition;
		}


		#endregion

		#region LessThan

		/// <summary>
		/// Extends the Query (using the AND operator) such that only rows with values less than
		/// that specified in the named column should be included.
		/// </summary>
		/// <param name="columnName">The column to DataFilter within.</param>
		/// <param name="lessThan">The value that remaining rows must be less than.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter LessThan(string columnName, object lessThan)
		{
			return And(CreateLessThanCondition(columnName, lessThan));
		}

		public DataFilter OrLessThan(string columnName, object lessThan)
		{
			return Or(CreateLessThanCondition(columnName, lessThan));
		}

		private ConditionNode CreateLessThanCondition(string columnName, object lessThan)
		{
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(lessThan), "<");
			return condition;
		}

		#endregion

		#region LessThanOrEqualTo

		/// <summary>
		/// Extends the Query (using the AND operator) such that only rows with values less than or equal to
		/// that specified in the named column should be included.
		/// </summary>
		/// <param name="columnName">The column to DataFilter within.</param>
		/// <param name="lessThanOrEqualTo">The value that remaining rows must be less than or equal to.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter LessThanOrEqualTo(string columnName, object lessThanOrEqualTo)
		{
			return And(CreateLessThanOrEqualToCondition(columnName, lessThanOrEqualTo));
		}

		public DataFilter OrLessThanOrEqualTo(string columnName, object lessThanOrEqualTo)
		{
			return Or(CreateLessThanOrEqualToCondition(columnName, lessThanOrEqualTo));
		}

		private static ConditionNode CreateLessThanOrEqualToCondition(string columnName, object lessThanOrEqualTo)
		{
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(lessThanOrEqualTo), "<=");
			return condition;
		}


		#endregion

		#region Like

		/// <summary>
		/// Performs wildcard string matching upon the specified column.
		/// </summary>
		/// <param name="columnName">The column to match within.</param>
		/// <param name="likePattern">The wildcard expression.  Patterns commonly use the % symbol as a wildcard.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter Like(string columnName, string likePattern)
		{
			return And(CreateLikeCondition(columnName, likePattern));
		}

		public DataFilter OrLike(string columnName, string likePattern)
		{
			return Or(CreateLikeCondition(columnName, likePattern));
		}

		private ConditionNode CreateLikeCondition(string columnName, string likePattern)
		{
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(likePattern), " Like ");
			return condition;
		}

		#endregion

		#region ContainsString

		public DataFilter ContainsString(string columnName, string subString)
		{
			return And(CreateContainsStringCondition(subString, columnName));
		}

		public DataFilter OrContainsString(string columnName, string subString)
		{
			return Or(CreateContainsStringCondition(subString, columnName));
		}

		private ConditionNode CreateContainsStringCondition(string subString, string columnName)
		{
			var likePattern = string.Format("%{0}%", subString);
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(likePattern), " Like ");
			return condition;
		}

		#endregion

		#region StartsWithString

		public DataFilter StartsWithString(string columnName, string prefix)
		{
			return And(CreateStartsWithStringCondition(prefix, columnName));
		}

		public DataFilter OrStartsWithString(string columnName, string prefix)
		{
			return Or(CreateStartsWithStringCondition(prefix, columnName));
		}

		private static ConditionNode CreateStartsWithStringCondition(string prefix, string columnName)
		{
			var likePattern = string.Format("{0}%", prefix);
			ConditionNode condition 
				= new OperatorNode(new ColumnNameNode(columnName), new ValueNode(likePattern), " Like ");
			return condition;
		}

		#endregion

		#region ContainsStringWithinCommaSeparatedValues

		public DataFilter ContainsStringWithinCommaSeparatedValues(string columnName, string csvValue)
		{
			return And(CreateContainsStringWithinCommaSeparatedValuesCondition(columnName, csvValue));
		}

		public DataFilter OrContainsStringWithinCommaSeparatedValues(string columnName, string csvValue)
		{
			return Or(CreateContainsStringWithinCommaSeparatedValuesCondition(columnName, csvValue));
		}

		private ConditionNode CreateContainsStringWithinCommaSeparatedValuesCondition(string columnName, string csvValue)
		{
			var likeFilter = new DataFilter();
			likeFilter.EqualTo(columnName, csvValue);
			likeFilter.OrLike(columnName, string.Format("{0},%", csvValue));
			likeFilter.OrLike(columnName, string.Format("%,{0}", csvValue));
			likeFilter.OrLike(columnName, string.Format("%,{0},%", csvValue));
			var condition = likeFilter._topNode;
			return condition;
		}

		#endregion

		#region IsNotNull

		public DataFilter IsNotNull(string columnName)
		{
			return And(CreateIsNotNullCondition(columnName));
		}

		public DataFilter OrIsNotNull(string columnName)
		{
			return Or(CreateIsNotNullCondition(columnName));
		}

		private ConditionNode CreateIsNotNullCondition(string columnName)
		{
			ConditionNode condition 
				= new NullCheckingNode(new ColumnNameNode(columnName), false);
			return condition;
		}

		#endregion

		#region IsNull

		public DataFilter IsNull(string columnName)
		{
			return And(CreateIsNullCondition(columnName));
		}

		public DataFilter OrIsNull(string columnName)
		{
			return Or(CreateIsNullCondition(columnName));
		}

		private ConditionNode CreateIsNullCondition(string columnName)
		{
			ConditionNode condition 
				= new NullCheckingNode(new ColumnNameNode(columnName), true);
			return condition;
		}

		#endregion

		#region ToString and GetExpressionString

		/// <summary>
		/// Returns the query's expression as a SQL string, prefixed with the 'Where' command.
		/// For example, <code>Where Column1='Value1'</code>.
		/// </summary>
		public override string ToString()
		{
			if (!HasExpression)
				return string.Empty;

			var sb = new StringBuilder();
			sb.Append("where ");
			sb.Append(GetExpressionString());
			return sb.ToString();
		}


		/// <summary>
		/// Returns the where clause without the leading 'Where' command.  To access the statement
		/// with a where command, use <see cref="ToString()"/>.
		/// </summary>
		/// <returns>The where clause without the leading 'Where' command.</returns>
		public string GetExpressionString()
		{
		    return !HasExpression ? string.Empty : _topNode.GetExpressionString();
		}

        #endregion

		#region And & Or operator support

		/// <summary>
		/// Adds the specified sub-query using the OR logical operator.
		/// </summary>
        /// <param name="innerQueries">The sub-query to be added via the OR operator.</param>
		/// <returns>This Query instance, such that multiple methods may be chained together for convenience.</returns>
		public DataFilter Or(params DataFilter[] innerQueries)
		{
		    foreach (var innerQuery in innerQueries.Where(innerQuery => innerQuery._topNode!=null))
		    {
		        Or(innerQuery._topNode);
		    }

		    return this;
		}

        DataFilter Or(ConditionNode newCondition)
		{
			if (_topNode==null)
			{
				_topNode = newCondition;
			}
			else
			{
				MergeConditionIntoTree(LogicalOperator.OR, newCondition);
			}
			return this;
		}		

		public DataFilter And(params DataFilter[] innerQueries)
		{
		    foreach (var innerQuery in innerQueries.Where(innerQuery => innerQuery._topNode!=null))
		    {
		        And(innerQuery._topNode);
		    }

		    return this;
		}

        DataFilter And(ConditionNode newCondition)
		{
			if (newCondition==null)
				throw new ArgumentNullException("newCondition", "condition may not be null");

			if (_topNode==null)
			{
				_topNode = newCondition;
			}
			else
			{
				MergeConditionIntoTree(LogicalOperator.AND, newCondition);
			}
			return this;
		}

		void MergeConditionIntoTree(LogicalOperator desiredLogicalOperator, ConditionNode newCondition)
		{
			if (newCondition==null)
				throw new ArgumentNullException("newCondition", "condition may not be null");

		    var node = _topNode as LogicalNode;
		    if (node != null && node.Operator==desiredLogicalOperator)
			{
				// top node is already of the desired type, so just add into it
				var logicalNode = node;
				logicalNode.AddCondition(newCondition);
			}
			else
			{
				// push existing top node down to be beneath this node...
				var newTopNode = new LogicalNode(desiredLogicalOperator);
				newTopNode.AddCondition(_topNode);
				newTopNode.AddCondition(newCondition);
				_topNode = newTopNode;
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets whether the Query instance has any conditions set, and hence whether
		/// would return a non-zero length string.
		/// </summary>
		public bool HasExpression
		{
			get
			{
				return _topNode!=null;
			}
		}

		#endregion

		#region Node classes

		/// <summary>
		/// Private enumeration of all supported logical operators for combination of
		/// condition nodes.
		/// </summary>
		enum LogicalOperator
		{
			AND,
			OR
		}

		abstract class ConditionNode
		{
			public abstract string GetExpressionString();
		}

		class LogicalNode : ConditionNode
		{
		    readonly ArrayList _childNodes; // may contain only ConditionNode entries...
		    readonly LogicalOperator _operator; // eg. AND or OR

			public LogicalNode(LogicalOperator logicalOperator)
			{
				_operator = logicalOperator;
				_childNodes = new ArrayList();
			}

			public override string GetExpressionString()
			{
				var sb = new StringBuilder();
				for (var i=0; i<_childNodes.Count; i++)
				{
					if (i>0)
					{
						sb.AppendFormat(" {0} ", OperatorName);
					}
					var child = (ConditionNode)_childNodes[i];

					string formatString;
					if (child is LogicalNode)
						formatString = "({0})";
					else
						formatString = "{0}";

					sb.AppendFormat(formatString, child.GetExpressionString());
				}
				return sb.ToString();
			}

			public void AddCondition(ConditionNode condition)
			{
				if (condition==null)
					throw new ArgumentNullException("condition", "Cannot add a null condition.");

				_childNodes.Add(condition);
			}

			public LogicalOperator Operator
			{
				get
				{
					return _operator;
				}
			}

			string OperatorName
			{
				get
				{
				    switch (_operator)
				    {
				        case LogicalOperator.AND:
				            return "And";
				        case LogicalOperator.OR:
				            return "Or";
				        default:
				            throw new Exception("Unsupported logical operator: " + _operator);
				    }
				}
			}
		}

		class OperatorNode : ConditionNode
		{
		    readonly string _operator;
		    readonly LiteralNode _left;
		    readonly LiteralNode _right;

			public OperatorNode(LiteralNode left, LiteralNode right, string operatorString)
			{
				_left = left;
				_right = right;
				_operator = operatorString;
			}

			public override string GetExpressionString()
			{
				return string.Format("{0}{1}{2}", _left, _operator, _right);
			}
		}

		class NullCheckingNode : ConditionNode
		{
		    readonly bool _mustBeNull;
		    readonly ColumnNameNode _column;
			public NullCheckingNode(ColumnNameNode column, bool desireNull)
			{
				_column = column;
				_mustBeNull = desireNull;
			}
			public override string GetExpressionString()
			{
			    return string.Format(_mustBeNull ? "{0} Is Null" : "{0} Is Not Null", _column);
			}
		}


		abstract class LiteralNode
		{
		}

		class ValueNode : LiteralNode
		{
			/// <summary>
			/// The default date format string used for SQL commands.
			/// </summary>
			const string DateFormat = "dd MMM yyyy HH:mm:ss";

		    readonly object _value;
			public ValueNode(object val)
			{
				_value = val;
			}

			public override string ToString()
			{
				return FormatValue(_value);
			}

		    static string FormatValue(object val)
			{
				if (val is DateTime)
				{
					return string.Format("'{0}'", ((DateTime)val).ToString(DateFormat));
				}
				
                if (val is int)
				{
					return string.Format("{0}", val);
				}

				EscapeString(ref val);
				return string.Format("'{0}'", val);
			}

		    static void EscapeString(ref object val)
			{
				val = val.ToString().Replace("'", "''");
			}
		}

		class ColumnNameNode : LiteralNode
		{
		    readonly string _columnName;
			public ColumnNameNode(string columnName)
			{
				_columnName = columnName;
			}
			public override string ToString()
			{
				return string.Format("{0}", _columnName);
			}
		}


		#endregion

		#region Equals & GetHashCode

		public override bool Equals(object obj)
		{
            var that = obj as DataFilter;
			
			return that != null && GetExpressionString().Equals(that.GetExpressionString());
		}

		public override int GetHashCode()
		{
			return GetExpressionString().GetHashCode();
		}



		#endregion
    }
}
