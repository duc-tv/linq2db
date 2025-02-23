﻿using System;
using System.Data.Common;
using System.Reflection;

namespace LinqToDB.DataProvider.SqlCe
{
	using Configuration;
	using Data;

	public static class SqlCeTools
	{
		static readonly Lazy<IDataProvider> _sqlCeDataProvider = DataConnection.CreateDataProvider<SqlCeDataProvider>();

		internal static IDataProvider? ProviderDetector(IConnectionStringSettings css, string connectionString)
		{
			if (css.ProviderName?.Contains("SqlCe") == true
				|| css.ProviderName?.Contains("SqlServerCe") == true
				|| css.Name.Contains("SqlCe")
				|| css.Name.Contains("SqlServerCe"))
				return _sqlCeDataProvider.Value;

			return null;
		}

		public static IDataProvider GetDataProvider() => _sqlCeDataProvider.Value;

		public static void ResolveSqlCe(string path)
		{
			_ = new AssemblyResolver(path, SqlCeProviderAdapter.AssemblyName);
		}

		public static void ResolveSqlCe(Assembly assembly)
		{
			_ = new AssemblyResolver(assembly, assembly.FullName!);
		}

		#region CreateDataConnection

		public static DataConnection CreateDataConnection(string connectionString)
		{
			return new DataConnection(_sqlCeDataProvider.Value, connectionString);
		}

		public static DataConnection CreateDataConnection(DbConnection connection)
		{
			return new DataConnection(_sqlCeDataProvider.Value, connection);
		}

		public static DataConnection CreateDataConnection(DbTransaction transaction)
		{
			return new DataConnection(_sqlCeDataProvider.Value, transaction);
		}

		#endregion

		public static void CreateDatabase(string databaseName, bool deleteIfExists = false)
		{
			if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));

			DataTools.CreateFileDatabase(
				databaseName, deleteIfExists, ".sdf",
				dbName =>
				{
					using (var engine = SqlCeProviderAdapter.GetInstance().CreateSqlCeEngine("Data Source=" + dbName))
						engine.CreateDatabase();
				});
		}

		public static void DropDatabase(string databaseName)
		{
			if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));

			DataTools.DropFileDatabase(databaseName, ".sdf");
		}

		#region BulkCopy

		[Obsolete("Use SqlCeOptions.Default.BulkCopyType instead.")]
		public static BulkCopyType DefaultBulkCopyType
		{
			get => SqlCeOptions.Default.BulkCopyType;
			set => SqlCeOptions.Default = SqlCeOptions.Default with { BulkCopyType = value };
		}

		#endregion
	}
}
