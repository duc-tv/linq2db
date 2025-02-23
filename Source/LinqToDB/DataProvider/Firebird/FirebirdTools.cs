﻿using System;
using System.Data.Common;
using System.Reflection;

using JetBrains.Annotations;

namespace LinqToDB.DataProvider.Firebird
{
	using Configuration;
	using Data;

	[PublicAPI]
	public static class FirebirdTools
	{
		static readonly Lazy<IDataProvider> _firebirdDataProvider = DataConnection.CreateDataProvider<FirebirdDataProvider>();

		internal static IDataProvider? ProviderDetector(IConnectionStringSettings css, string connectionString)
		{
			if (css.ProviderName is ProviderName.Firebird or FirebirdProviderAdapter.ClientNamespace ||
			    css.Name.Contains("Firebird"))
				return _firebirdDataProvider.Value;

			return null;
		}

		public static IDataProvider GetDataProvider()
		{
			return _firebirdDataProvider.Value;
		}

		public static void ResolveFirebird(string path)
		{
			if (path == null) throw new ArgumentNullException(nameof(path));
			_ = new AssemblyResolver(path, FirebirdProviderAdapter.AssemblyName);
		}

		public static void ResolveFirebird(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			_ = new AssemblyResolver(assembly, FirebirdProviderAdapter.AssemblyName);
		}

		#region CreateDataConnection

		public static DataConnection CreateDataConnection(string connectionString)
		{
			return new DataConnection(_firebirdDataProvider.Value, connectionString);
		}

		public static DataConnection CreateDataConnection(DbConnection connection)
		{
			return new DataConnection(_firebirdDataProvider.Value, connection);
		}

		public static DataConnection CreateDataConnection(DbTransaction transaction)
		{
			return new DataConnection(_firebirdDataProvider.Value, transaction);
		}

		#endregion

		#region BulkCopy

		[Obsolete("Use FirebirdOptions.Default.BulkCopyType instead.")]
		public static BulkCopyType DefaultBulkCopyType
		{
			get => FirebirdOptions.Default.BulkCopyType;
			set => FirebirdOptions.Default = FirebirdOptions.Default with { BulkCopyType = value };
		}

		#endregion

		#region ClearAllPools

		public static void ClearAllPools() => FirebirdProviderAdapter.Instance.ClearAllPools();

		#endregion
	}
}
