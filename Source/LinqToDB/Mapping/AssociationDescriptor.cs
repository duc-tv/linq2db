﻿using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToDB.Mapping
{
	using Common;
	using Extensions;
	using Linq.Builder;

	/// <summary>
	/// Stores association descriptor.
	/// </summary>
	public class AssociationDescriptor
	{
		/// <summary>
		/// Creates descriptor instance.
		/// </summary>
		/// <param name="type">From (this) side entity mapping type.</param>
		/// <param name="memberInfo">Association member (field, property or method).</param>
		/// <param name="thisKey">List of names of from (this) key members.</param>
		/// <param name="otherKey">List of names of to (other) key members.</param>
		/// <param name="expressionPredicate">Optional predicate expression source property or method.</param>
		/// <param name="predicate">Optional predicate expression.</param>
		/// <param name="expressionQueryMethod">Optional name of query method.</param>
		/// <param name="expressionQuery">Optional query expression.</param>
		/// <param name="storage">Optional association value storage field or property name.</param>
		/// <param name="canBeNull">If <c>true</c>, association will generate outer join, otherwise - inner join.</param>
		/// <param name="aliasName">Optional alias for representation in SQL.</param>
		public AssociationDescriptor(
			Type        type,
			MemberInfo  memberInfo,
			string[]    thisKey,
			string[]    otherKey,
			string?     expressionPredicate,
			Expression? predicate,
			string?     expressionQueryMethod,
			Expression? expressionQuery,
			string?     storage,
			bool?       canBeNull,
			string?     aliasName)
		{
			if (memberInfo == null) ThrowHelper.ThrowArgumentNullException(nameof(memberInfo));
			if (thisKey    == null) ThrowHelper.ThrowArgumentNullException(nameof(thisKey));
			if (otherKey   == null) ThrowHelper.ThrowArgumentNullException(nameof(otherKey));

			if (thisKey.Length == 0 && string.IsNullOrEmpty(expressionPredicate) && predicate == null && string.IsNullOrEmpty(expressionQueryMethod) && expressionQuery == null)
				ThrowHelper.ThrowArgumentOutOfRangeException(
					nameof(thisKey),
					$"Association '{type.Name}.{memberInfo.Name}' does not define keys.");

			if (thisKey.Length != otherKey.Length)
				ThrowHelper.ThrowArgumentException(
					$"Association '{type.Name}.{memberInfo.Name}' has different number of keys for parent and child objects.");

			MemberInfo            = memberInfo;
			ThisKey               = thisKey;
			OtherKey              = otherKey;
			ExpressionPredicate   = expressionPredicate;
			Predicate             = predicate;
			ExpressionQueryMethod = expressionQueryMethod;
			ExpressionQuery       = expressionQuery;
			Storage               = storage;
			CanBeNull             = canBeNull ?? AnalyzeCanBeNull();
			AliasName             = aliasName;
		}

		/// <summary>
		/// Gets or sets association member (field, property or method).
		/// </summary>
		public MemberInfo  MemberInfo          { get; set; }
		/// <summary>
		/// Gets or sets list of names of from (this) key members. Could be empty, if association has predicate expression.
		/// </summary>
		public string[]    ThisKey             { get; set; }
		/// <summary>
		/// Gets or sets list of names of to (other) key members. Could be empty, if association has predicate expression.
		/// </summary>
		public string[]    OtherKey            { get; set; }
		/// <summary>
		/// Gets or sets optional predicate expression source property or method.
		/// </summary>
		public string?     ExpressionPredicate { get; set; }
		/// <summary>
		/// Gets or sets optional query method source property or method.
		/// </summary>
		public string?     ExpressionQueryMethod { get; set; }
		/// <summary>
		/// Gets or sets optional query expression.
		/// </summary>
		public Expression? ExpressionQuery     { get; set; }
		/// <summary>
		/// Gets or sets optional predicate expression.
		/// </summary>
		public Expression? Predicate           { get; set; }
		/// <summary>
		/// Gets or sets optional association value storage field or property name. Used with LoadWith.
		/// </summary>
		public string?     Storage             { get; set; }
		/// <summary>
		/// Gets or sets join type, generated for current association.
		/// If <c>true</c>, association will generate outer join, otherwise - inner join.
		/// </summary>
		public bool        CanBeNull           { get; set; }
		/// <summary>
		/// Gets or sets alias for association. Used in SQL generation process.
		/// </summary>
		public string?     AliasName           { get; set; }

		/// <summary>
		/// Parse comma-separated list of association key column members into string array.
		/// </summary>
		/// <param name="keys">Comma-separated (spaces allowed) list of association key column members.</param>
		/// <returns>Returns array with names of association key column members.</returns>
		public static string[] ParseKeys(string? keys)
		{
			return keys?.Replace(" ", "").Split(',') ?? Array<string>.Empty;
		}

		/// <summary>
		/// Generates table alias for association.
		/// </summary>
		/// <returns>Generated alias.</returns>
		public string GenerateAlias()
		{
			if (!string.IsNullOrEmpty(AliasName))
				return AliasName!;

			if (!string.IsNullOrEmpty(Configuration.Sql.AssociationAlias))
				return string.Format(Configuration.Sql.AssociationAlias, MemberInfo.Name);

			return string.Empty;
		}

		public bool IsList
		{
			get
			{
				var type = MemberInfo.GetMemberType();
				return typeof(IEnumerable).IsSameOrParentOf(type);
			}
		}

		public Type GetElementType(MappingSchema mappingSchema)
		{
			var type = MemberInfo.GetMemberType();
			return EagerLoading.GetEnumerableElementType(type, mappingSchema);
		}

		public Type GetParentElementType()
		{
			if (MemberInfo.MemberType == MemberTypes.Method)
			{
				var methodInfo = (MethodInfo)MemberInfo;
				if (methodInfo.IsStatic)
				{
					var pms = methodInfo.GetParameters();
					if (pms.Length > 0)
					{
						return pms[0].ParameterType;
					}
				}
				else
				{
					return methodInfo.DeclaringType!;
				}

				ThrowHelper.ThrowLinqToDBException($"Can not retrieve declaring type form member {methodInfo}");
			}

			return MemberInfo.DeclaringType!;
		}


		/// <summary>
		/// Loads predicate expression from <see cref="ExpressionPredicate"/> member.
		/// </summary>
		/// <param name="parentType">Type of object that declares association</param>
		/// <param name="objectType">Type of object associated with expression predicate</param>
		/// <returns><c>null</c> of association has no custom predicate expression or predicate expression, specified
		/// by <see cref="ExpressionPredicate"/> member.</returns>
		public LambdaExpression? GetPredicate(Type parentType, Type objectType)
		{
			if (Predicate == null && string.IsNullOrEmpty(ExpressionPredicate))
				return null;

			Expression? predicate = null;

			var type = MemberInfo.DeclaringType;

			if (type == null)
				ThrowHelper.ThrowArgumentException($"Member '{MemberInfo.Name}' has no declaring type");

			if (!string.IsNullOrEmpty(ExpressionPredicate))
			{
				var members = type.GetStaticMembersEx(ExpressionPredicate!);

				if (members.Length == 0)
					ThrowHelper.ThrowLinqToDBException($"Static member '{ExpressionPredicate}' for type '{type.Name}' not found");

				if (members.Length > 1)
					ThrowHelper.ThrowLinqToDBException($"Ambiguous members '{ExpressionPredicate}' for type '{type.Name}' has been found");

				var propInfo = members[0] as PropertyInfo;

				if (propInfo != null)
				{
					var value = propInfo.GetValue(null, null);
					if (value == null)
						return null;

					predicate = value as Expression;
					if (predicate == null)
						ThrowHelper.ThrowLinqToDBException($"Property '{ExpressionPredicate}' for type '{type.Name}' should return expression");
				}
				else
				{
					var method = members[0] as MethodInfo;
					if (method != null)
					{
						if (method.GetParameters().Length > 0)
							ThrowHelper.ThrowLinqToDBException($"Method '{ExpressionPredicate}' for type '{type.Name}' should have no parameters");
						var value = method.Invoke(null, Array<object>.Empty);
						if (value == null)
							return null;

						predicate = value as Expression;
						if (predicate == null)
							ThrowHelper.ThrowLinqToDBException($"Method '{ExpressionPredicate}' for type '{type.Name}' should return expression");
					}
				}
				if (predicate == null)
					ThrowHelper.ThrowLinqToDBException(
						$"Member '{ExpressionPredicate}' for type '{type.Name}' should be static property or method");
			}
			else
				predicate = Predicate;

			var lambda = predicate as LambdaExpression;
			if (lambda == null || lambda.Parameters.Count != 2)
				if (!string.IsNullOrEmpty(ExpressionPredicate))
					ThrowHelper.ThrowLinqToDBException(
						$"Invalid predicate expression in {type.Name}.{ExpressionPredicate}. Expected: Expression<Func<{parentType.Name}, {objectType.Name}, bool>>");
				else
					ThrowHelper.ThrowLinqToDBException(
						$"Invalid predicate expression in {type.Name}. Expected: Expression<Func<{parentType.Name}, {objectType.Name}, bool>>");

			var firstParameter = lambda.Parameters[0];
			if (!firstParameter.Type.IsSameOrParentOf(parentType) && !parentType.IsSameOrParentOf(firstParameter.Type))
			{
				ThrowHelper.ThrowLinqToDBException($"First parameter of expression predicate should be '{parentType.Name}'");
			}

			if (lambda.Parameters[1].Type != objectType)
				ThrowHelper.ThrowLinqToDBException($"Second parameter of expression predicate should be '{objectType.Name}'");

			if (lambda.ReturnType != typeof(bool))
				ThrowHelper.ThrowLinqToDBException("Result type of expression predicate should be 'bool'");

			return lambda;
		}


		public bool HasQueryMethod()
		{
			return ExpressionQuery != null || !string.IsNullOrEmpty(ExpressionQueryMethod);
		}

		/// <summary>
		/// Loads query method expression from <see cref="ExpressionQueryMethod"/> member.
		/// </summary>
		/// <param name="parentType">Type of object that declares association</param>
		/// <param name="objectType">Type of object associated with query method expression</param>
		/// <returns><c>null</c> of association has no custom query method expression or query method expression, specified
		/// by <see cref="ExpressionQueryMethod"/> member.</returns>
		public LambdaExpression? GetQueryMethod(Type parentType, Type objectType)
		{
			if (!HasQueryMethod())
				return null;

			Expression queryExpression;

			var type = MemberInfo.DeclaringType;

			if (type == null)
				ThrowHelper.ThrowArgumentException($"Member '{MemberInfo.Name}' has no declaring type");

			if (!string.IsNullOrEmpty(ExpressionQueryMethod))
				queryExpression = type.GetExpressionFromExpressionMember<Expression>(ExpressionQueryMethod!);
			else
				queryExpression = ExpressionQuery!;

			var lambda = queryExpression as LambdaExpression;
			if (lambda == null || lambda.Parameters.Count < 1)
				if (!string.IsNullOrEmpty(ExpressionQueryMethod))
					ThrowHelper.ThrowLinqToDBException(
						$"Invalid predicate expression in {type.Name}.{ExpressionQueryMethod}. Expected: Expression<Func<{parentType.Name}, IDataContext, IQueryable<{objectType.Name}>>>");
				else
					ThrowHelper.ThrowLinqToDBException(
						$"Invalid predicate expression in {type.Name}. Expected: Expression<Func<{parentType.Name}, IDataContext, IQueryable<{objectType.Name}>>>");

			if (!lambda.Parameters[0].Type.IsSameOrParentOf(parentType))
				ThrowHelper.ThrowLinqToDBException($"First parameter of expression predicate should be '{parentType.Name}'");

			if (!(typeof(IQueryable<>).IsSameOrParentOf(lambda.ReturnType) &&
			      lambda.ReturnType.GetGenericArguments()[0].IsSameOrParentOf(objectType)))
				ThrowHelper.ThrowLinqToDBException("Result type of expression predicate should be 'IQueryable<{objectType.Name}>'");

			return lambda;
		}

		private bool AnalyzeCanBeNull()
		{
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
			// Note that nullability of Collections can't be determined from types.
			// OUTER JOIN are usually materialized in non-nullable, but empty, collections.
			// For example, `IList<Product> Products` might well require an OUTER JOIN.
			// Neither `IList<Product>?` nor `IList<Product?>` would be correct.
			if (Configuration.UseNullableTypesMetadata && !IsList)
			{
				// Extract info from C# Nullable Reference Types if available.
				// Note that this should also handle Nullable Value Types.
				var context = new NullabilityInfoContext();
				var nullability = MemberInfo switch 
				{
					PropertyInfo p => context.Create(p).ReadState,
					FieldInfo    f => context.Create(f).ReadState,
					MethodInfo   m => context.Create(m.ReturnParameter).ReadState,
								 _ => NullabilityState.Unknown,
				};
				
				if (nullability != NullabilityState.Unknown)
					return nullability == NullabilityState.Nullable;				
			}
#endif
			return true;
		}
	}
}
