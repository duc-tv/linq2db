﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB.SqlQuery;

namespace LinqToDB.Linq.Builder
{
	using LinqToDB.Expressions;

	static class SequenceHelper
	{
		public static Expression PrepareBody(LambdaExpression lambda, params IBuildContext[] sequences)
		{
			var body = lambda.GetBody(sequences
				.Select((s, idx) => (Expression)new ContextRefExpression(lambda.Parameters[idx].Type, s)).ToArray());

			return body;
		}

		public static bool IsSameContext(Expression? expression, IBuildContext context)
		{
			return expression == null || expression is ContextRefExpression contextRef && contextRef.BuildContext == context;
		}

		[return: NotNullIfNotNull("expression")]
		public static Expression? CorrectExpression(Expression? expression, IBuildContext current, IBuildContext underlying)
		{
			if (expression != null)
			{
				var root = current.Builder.GetRootObject(expression);
				if (root is ContextRefExpression refExpression)
				{
					if (refExpression.BuildContext == current)
					{
						expression = expression.Replace(root, new ContextRefExpression(root.Type, underlying), EqualityComparer<Expression>.Default);
					}
				}
			}

			return expression;
		}

		public static TableBuilder.TableContext? GetTableContext(IBuildContext context)
		{
			var contextRef = new ContextRefExpression(typeof(object), context);

			var rootContext = context.Builder.MakeExpression(contextRef, ProjectFlags.Root) as ContextRefExpression;

			var tableContext = rootContext?.BuildContext as TableBuilder.TableContext;

			return tableContext;
		}

		public static IBuildContext UnwrapSubqueryContext(IBuildContext context)
		{
			while (context is SubQueryContext sc)
			{
				context = sc.SubQuery;
			}

			return context;
		}

		public static bool IsDefaultIfEmpty(IBuildContext context)
		{
			return UnwrapSubqueryContext(context) is DefaultIfEmptyBuilder.DefaultIfEmptyContext;
		}

		public static Expression RequireSqlExpression(this IBuildContext context, Expression? path)
		{
			var sql = context.Builder.MakeExpression(path, ProjectFlags.SQL);
			if (sql == null)
				throw new LinqException("'{0}' cannot be converted to SQL.", path);

			return sql;
		}
	}
}
