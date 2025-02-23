﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace LinqToDB.DataProvider.MySql
{
	using System.Data.Common;
	using Common;
	using Configuration;
	using Data;

	public static partial class MySqlTools
	{
		static readonly Lazy<IDataProvider> _mySqlDataProvider          = DataConnection.CreateDataProvider<MySqlDataProviderMySqlOfficial>();
		static readonly Lazy<IDataProvider> _mySqlConnectorDataProvider = DataConnection.CreateDataProvider<MySqlDataProviderMySqlConnector>();

		internal static IDataProvider? ProviderDetector(IConnectionStringSettings css, string connectionString)
		{
			// ensure ClickHouse configuration over mysql protocol is not detected as mysql
			if (css.IsGlobal || css.ProviderName?.Contains("ClickHouse") == true || css.Name.Contains("ClickHouse"))
				return null;

			switch (css.ProviderName)
			{
				case ProviderName.MySqlOfficial                :
				case MySqlProviderAdapter.MySqlDataAssemblyName: return _mySqlDataProvider.Value;
				case ProviderName.MySqlConnector               : return _mySqlConnectorDataProvider.Value;

				case ""                         :
				case null                       :
					if (css.Name.Contains("MySql"))
						goto case ProviderName.MySql;
					break;
				case MySqlProviderAdapter.MySqlDataClientNamespace:
				case ProviderName.MySql                           :
					if (css.Name.Contains(MySqlProviderAdapter.MySqlConnectorAssemblyName))
						return _mySqlConnectorDataProvider.Value;

					if (css.Name.Contains(MySqlProviderAdapter.MySqlDataAssemblyName))
						return _mySqlDataProvider.Value;

					return GetDataProvider();
				case var providerName when providerName.Contains("MySql"):
					if (providerName.Contains(MySqlProviderAdapter.MySqlConnectorAssemblyName))
						return _mySqlConnectorDataProvider.Value;

					if (providerName.Contains(MySqlProviderAdapter.MySqlDataAssemblyName))
						return _mySqlDataProvider.Value;

					goto case ProviderName.MySql;
			}

			return null;
		}

		public static IDataProvider GetDataProvider(string? providerName = null)
		{
			return providerName switch
			{
				ProviderName.MySqlOfficial  => _mySqlDataProvider.Value,
				ProviderName.MySqlConnector => _mySqlConnectorDataProvider.Value,
				_                           =>
					DetectedProviderName == ProviderName.MySqlOfficial
					? _mySqlDataProvider.Value
					: _mySqlConnectorDataProvider.Value,
			};
		}

		private static string? _detectedProviderName;
		public  static string  DetectedProviderName =>
			_detectedProviderName ??= DetectProviderName();

		static string DetectProviderName()
		{
			try
			{
				var path = typeof(MySqlTools).Assembly.GetPath();

				if (!File.Exists(Path.Combine(path, $"{MySqlProviderAdapter.MySqlDataAssemblyName}.dll")))
					if (File.Exists(Path.Combine(path, $"{MySqlProviderAdapter.MySqlConnectorAssemblyName}.dll")))
						return ProviderName.MySqlConnector;
			}
			catch (Exception)
			{
			}

			return ProviderName.MySqlOfficial;
		}

		public static void ResolveMySql(string path, string? assemblyName)
		{
			if (path == null) throw new ArgumentNullException(nameof(path));
			new AssemblyResolver(
				path,
				assemblyName
					?? (DetectedProviderName == ProviderName.MySqlOfficial
						? MySqlProviderAdapter.MySqlDataAssemblyName
						: MySqlProviderAdapter.MySqlConnectorAssemblyName));
		}

		public static void ResolveMySql(Assembly assembly)
		{
			if (assembly == null) throw new ArgumentNullException(nameof(assembly));
			new AssemblyResolver(assembly, assembly.FullName!);
		}

		#region CreateDataConnection

		public static DataConnection CreateDataConnection(string connectionString, string? providerName = null)
		{
			return new DataConnection(GetDataProvider(providerName), connectionString);
		}

		public static DataConnection CreateDataConnection(DbConnection connection, string? providerName = null)
		{
			return new DataConnection(GetDataProvider(providerName), connection);
		}

		public static DataConnection CreateDataConnection(DbTransaction transaction, string? providerName = null)
		{
			return new DataConnection(GetDataProvider(providerName), transaction);
		}

		#endregion

		#region BulkCopy

		[Obsolete("Use MySqlOptions.Default.BulkCopyType instead.")]
		public static BulkCopyType DefaultBulkCopyType
		{
			get => MySqlOptions.Default.BulkCopyType;
			set => MySqlOptions.Default = MySqlOptions.Default with { BulkCopyType = value };
		}

		#endregion
	}
}
