﻿using System;

namespace LinqToDB.Mapping
{
	/// <summary>
	/// Marks target column as identity column with value, generated on database side during insert operations.
	/// Identity columns will be ignored for insert and update operations with implicit column list like
	/// <see cref="DataExtensions.Insert{T}(IDataContext, T, string?, string?, string?, string?, TableOptions)"/> or
	/// <see cref="DataExtensions.Update{T}(IDataContext, T, string?, string?, string?, string?, TableOptions)"/> methods.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
	public class IdentityAttribute : MappingAttribute
	{
		/// <summary>
		/// Creates attribute instance.
		/// </summary>
		public IdentityAttribute()
		{
		}

		/// <summary>
		/// Creates attribute instance.
		/// </summary>
		/// <param name="configuration">Mapping schema configuration name. See <see cref="Configuration"/>.</param>
		public IdentityAttribute(string? configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// Gets or sets mapping schema configuration name, for which this attribute should be taken into account.
		/// <see cref="ProviderName"/> for standard names.
		/// Attributes with <c>null</c> or empty string <see cref="Configuration"/> value applied to all configurations (if no attribute found for current configuration).
		/// </summary>
		public string? Configuration { get; set; }

		public override string GetObjectID() => Configuration ?? string.Empty;
	}
}
