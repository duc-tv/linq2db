﻿using System;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB.Extensions;

namespace LinqToDB.Linq.Builder
{
	using LinqToDB.Expressions;

	class ContextRefBuilder : ISequenceBuilder
	{
		public int BuildCounter { get; set; }

		public bool CanBuild(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			if (builder.IsAssociation(buildInfo.Expression))
			{
				var association = builder.MakeAssociation(buildInfo.Expression, out var rootContext);

				if (rootContext == null)
					return false;

				return builder.IsSequence(new BuildInfo(buildInfo, association));
			}

			return buildInfo.Expression is ContextRefExpression;
		}

		public IBuildContext BuildSequence(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			if (builder.IsAssociation(buildInfo.Expression))
			{
				var association = builder.MakeAssociation(buildInfo.Expression, out var rootContext);

				return builder.BuildSequence(new BuildInfo(buildInfo, association));
			}


			var context = ((ContextRefExpression)buildInfo.Expression).BuildContext;

			if (buildInfo.IsSubQuery)
			{
				var elementContext = context.GetContext(buildInfo.Expression, 0, buildInfo);
				if (elementContext != null)
					return elementContext;
			}

			return context;
		}

		public SequenceConvertInfo? Convert(ExpressionBuilder builder, BuildInfo buildInfo, ParameterExpression? param)
		{
			return null;
		}

		public bool IsSequence(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			if (builder.IsAssociation(buildInfo.Expression))
			{
				var association = builder.MakeAssociation(buildInfo.Expression, out var rootContext);

				if (rootContext == null)
					return false;

				return builder.IsSequence(new BuildInfo(buildInfo, association));
			}

			if (buildInfo.Expression is not ContextRefExpression contextRef)
				return false;

			if (buildInfo.InAggregation)
				return contextRef.BuildContext is GroupByBuilder.GroupByContext;

			return true;
		}
	}
}
