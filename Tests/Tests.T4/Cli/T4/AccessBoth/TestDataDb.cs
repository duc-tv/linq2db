// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB;
using LinqToDB.Configuration;
using LinqToDB.Data;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.T4.Access.Both
{
	public partial class TestDataDB : DataConnection
	{
		public TestDataDB()
		{
			InitDataContext();
		}

		public TestDataDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		public TestDataDB(LinqToDBConnectionOptions options)
			: base(options)
		{
			InitDataContext();
		}

		public TestDataDB(LinqToDBConnectionOptions<TestDataDB> options)
			: base(options)
		{
			InitDataContext();
		}

		partial void InitDataContext();

		public ITable<AllType>             AllTypes             => this.GetTable<AllType>();
		public ITable<Child>               Children             => this.GetTable<Child>();
		public ITable<DataTypeTest>        DataTypeTests        => this.GetTable<DataTypeTest>();
		public ITable<Doctor>              Doctors              => this.GetTable<Doctor>();
		public ITable<Dual>                Duals                => this.GetTable<Dual>();
		public ITable<GrandChild>          GrandChildren        => this.GetTable<GrandChild>();
		public ITable<InheritanceChild>    InheritanceChildren  => this.GetTable<InheritanceChild>();
		public ITable<InheritanceParent>   InheritanceParents   => this.GetTable<InheritanceParent>();
		public ITable<LinqDataType>        LinqDataTypes        => this.GetTable<LinqDataType>();
		public ITable<Parent>              Parents              => this.GetTable<Parent>();
		public ITable<Patient>             Patients             => this.GetTable<Patient>();
		public ITable<Person>              People               => this.GetTable<Person>();
		public ITable<TestIdentity>        TestIdentities       => this.GetTable<TestIdentity>();
		public ITable<TestMerge1>          TestMerge1           => this.GetTable<TestMerge1>();
		public ITable<TestMerge2>          TestMerge2           => this.GetTable<TestMerge2>();
		public ITable<LinqDataTypesQuery>  LinqDataTypesQueries => this.GetTable<LinqDataTypesQuery>();
		public ITable<LinqDataTypesQuery1> LinqDataTypesQuery1  => this.GetTable<LinqDataTypesQuery1>();
		public ITable<LinqDataTypesQuery2> LinqDataTypesQuery2  => this.GetTable<LinqDataTypesQuery2>();
		public ITable<PatientSelectAll>    PatientSelectAll     => this.GetTable<PatientSelectAll>();
		public ITable<PersonSelectAll>     PersonSelectAll      => this.GetTable<PersonSelectAll>();
		public ITable<ScalarDataReader>    ScalarDataReaders    => this.GetTable<ScalarDataReader>();
	}

	[Table("AllTypes")]
	public partial class AllType
	{
		[Column("ID"                      , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       ID                       { get; set; } // COUNTER
		[Column("bitDataType"                                                                          )] public bool      BitDataType              { get; set; } // Bit
		[Column("smallintDataType"                                                                     )] public short?    SmallintDataType         { get; set; } // Short
		[Column("decimalDataType"                                                                      )] public decimal?  DecimalDataType          { get; set; } // Decimal(18, 0)
		[Column("intDataType"                                                                          )] public int?      IntDataType              { get; set; } // Long
		[Column("tinyintDataType"                                                                      )] public byte?     TinyintDataType          { get; set; } // Byte
		[Column("moneyDataType"                                                                        )] public decimal?  MoneyDataType            { get; set; } // Currency
		[Column("floatDataType"                                                                        )] public double?   FloatDataType            { get; set; } // Double
		[Column("realDataType"                                                                         )] public float?    RealDataType             { get; set; } // Single
		[Column("datetimeDataType"                                                                     )] public DateTime? DatetimeDataType         { get; set; } // DateTime
		[Column("charDataType"                                                                         )] public char?     CharDataType             { get; set; } // CHAR(1)
		[Column("char20DataType"                                                                       )] public string?   Char20DataType           { get; set; } // CHAR(20)
		[Column("varcharDataType"                                                                      )] public string?   VarcharDataType          { get; set; } // VarChar(20)
		[Column("textDataType"                                                                         )] public string?   TextDataType             { get; set; } // LongText
		[Column("ncharDataType"                                                                        )] public string?   NcharDataType            { get; set; } // CHAR(20)
		[Column("nvarcharDataType"                                                                     )] public string?   NvarcharDataType         { get; set; } // VarChar(20)
		[Column("ntextDataType"                                                                        )] public string?   NtextDataType            { get; set; } // LongText
		[Column("binaryDataType"                                                                       )] public byte[]?   BinaryDataType           { get; set; } // VARBINARY(10)
		[Column("varbinaryDataType"                                                                    )] public byte[]?   VarbinaryDataType        { get; set; } // VARBINARY(510)
		[Column("imageDataType"                                                                        )] public byte[]?   ImageDataType            { get; set; } // LongBinary
		[Column("oleObjectDataType"                                                                    )] public byte[]?   OleObjectDataType        { get; set; } // LongBinary
		[Column("uniqueidentifierDataType"                                                             )] public Guid?     UniqueidentifierDataType { get; set; } // GUID
	}

	[Table("Child")]
	public partial class Child
	{
		[Column("ParentID")] public int? ParentID { get; set; } // Long
		[Column("ChildID" )] public int? ChildID  { get; set; } // Long
	}

	[Table("DataTypeTest")]
	public partial class DataTypeTest
	{
		[Column("DataTypeID", IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       DataTypeID { get; set; } // COUNTER
		[Column("Binary_"                                                                                     )] public byte[]?   Binary     { get; set; } // LongBinary
		[Column("Boolean_"                                                                                    )] public int?      Boolean    { get; set; } // Long
		[Column("Byte_"                                                                                       )] public byte?     Byte       { get; set; } // Byte
		[Column("Bytes_"                                                                                      )] public byte[]?   Bytes      { get; set; } // LongBinary
		[Column("Char_"                                                                                       )] public char?     Char       { get; set; } // VarChar(1)
		[Column("DateTime_"                                                                                   )] public DateTime? DateTime   { get; set; } // DateTime
		[Column("Decimal_"                                                                                    )] public decimal?  Decimal    { get; set; } // Currency
		[Column("Double_"                                                                                     )] public double?   Double     { get; set; } // Double
		[Column("Guid_"                                                                                       )] public Guid?     Guid       { get; set; } // GUID
		[Column("Int16_"                                                                                      )] public short?    Int16      { get; set; } // Short
		[Column("Int32_"                                                                                      )] public int?      Int32      { get; set; } // Long
		[Column("Int64_"                                                                                      )] public int?      Int64      { get; set; } // Long
		[Column("Money_"                                                                                      )] public decimal?  Money      { get; set; } // Currency
		[Column("SByte_"                                                                                      )] public byte?     SByte      { get; set; } // Byte
		[Column("Single_"                                                                                     )] public float?    Single     { get; set; } // Single
		[Column("Stream_"                                                                                     )] public byte[]?   Stream     { get; set; } // LongBinary
		[Column("String_"                                                                                     )] public string?   String     { get; set; } // VarChar(50)
		[Column("UInt16_"                                                                                     )] public short?    UInt16     { get; set; } // Short
		[Column("UInt32_"                                                                                     )] public int?      UInt32     { get; set; } // Long
		[Column("UInt64_"                                                                                     )] public int?      UInt64     { get; set; } // Long
		[Column("Xml_"                                                                                        )] public string?   Xml        { get; set; } // LongText
	}

	public static partial class ExtensionMethods
	{
		#region Table Extensions
		public static DataTypeTest? Find(this ITable<DataTypeTest> table, int dataTypeId)
		{
			return table.FirstOrDefault(e => e.DataTypeID == dataTypeId);
		}

		public static Doctor? Find(this ITable<Doctor> table, int personId)
		{
			return table.FirstOrDefault(e => e.PersonID == personId);
		}

		public static InheritanceChild? Find(this ITable<InheritanceChild> table, int inheritanceChildId)
		{
			return table.FirstOrDefault(e => e.InheritanceChildId == inheritanceChildId);
		}

		public static InheritanceParent? Find(this ITable<InheritanceParent> table, int inheritanceParentId)
		{
			return table.FirstOrDefault(e => e.InheritanceParentId == inheritanceParentId);
		}

		public static Patient? Find(this ITable<Patient> table, int personId)
		{
			return table.FirstOrDefault(e => e.PersonID == personId);
		}

		public static Person? Find(this ITable<Person> table, int personId)
		{
			return table.FirstOrDefault(e => e.PersonID == personId);
		}

		public static TestIdentity? Find(this ITable<TestIdentity> table, int id)
		{
			return table.FirstOrDefault(e => e.ID == id);
		}

		public static TestMerge1? Find(this ITable<TestMerge1> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static TestMerge2? Find(this ITable<TestMerge2> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}
		#endregion

		#region Stored Procedures
		#region AddIssue792Record
		public static int AddIssue792Record(this TestDataDB dataConnection)
		{
			return dataConnection.ExecuteProc("[AddIssue792Record]");
		}
		#endregion

		#region PatientSelectByName
		public static IEnumerable<PatientSelectByNameResult> PatientSelectByName(this TestDataDB dataConnection, string? firstName, string? lastName)
		{
			var parameters = new []
			{
				new DataParameter("@firstName", firstName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@lastName", lastName, DataType.NText)
				{
					Size = 50
				}
			};
			return dataConnection.QueryProc<PatientSelectByNameResult>("[Patient_SelectByName]", parameters);
		}

		public partial class PatientSelectByNameResult
		{
			[Column("PersonID"  )] public int     PersonID   { get; set; }
			[Column("FirstName" )] public string? FirstName  { get; set; }
			[Column("LastName"  )] public string? LastName   { get; set; }
			[Column("MiddleName")] public string? MiddleName { get; set; }
			[Column("Gender"    )] public char?   Gender     { get; set; }
			[Column("Diagnosis" )] public string? Diagnosis  { get; set; }
		}
		#endregion

		#region PersonDelete
		public static int PersonDelete(this TestDataDB dataConnection, int? personId)
		{
			var parameters = new []
			{
				new DataParameter("@PersonID", personId, DataType.Int32)
			};
			return dataConnection.ExecuteProc("[Person_Delete]", parameters);
		}
		#endregion

		#region PersonInsert
		public static int PersonInsert(this TestDataDB dataConnection, string? firstName, string? middleName, string? lastName, char? gender)
		{
			var parameters = new []
			{
				new DataParameter("@FirstName", firstName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@MiddleName", middleName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@LastName", lastName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@Gender", gender, DataType.NText)
				{
					Size = 1
				}
			};
			return dataConnection.ExecuteProc("[Person_Insert]", parameters);
		}
		#endregion

		#region PersonSelectByKey
		public static IEnumerable<PersonSelectByKeyResult> PersonSelectByKey(this TestDataDB dataConnection, int? id)
		{
			var parameters = new []
			{
				new DataParameter("@id", id, DataType.Int32)
			};
			return dataConnection.QueryProc<PersonSelectByKeyResult>("[Person_SelectByKey]", parameters);
		}

		public partial class PersonSelectByKeyResult
		{
			[Column("PersonID"  )] public int     PersonID   { get; set; }
			[Column("FirstName" )] public string? FirstName  { get; set; }
			[Column("LastName"  )] public string? LastName   { get; set; }
			[Column("MiddleName")] public string? MiddleName { get; set; }
			[Column("Gender"    )] public char?   Gender     { get; set; }
		}
		#endregion

		#region PersonSelectByName
		public static IEnumerable<PersonSelectByNameResult> PersonSelectByName(this TestDataDB dataConnection, string? firstName, string? lastName)
		{
			var parameters = new []
			{
				new DataParameter("@firstName", firstName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@lastName", lastName, DataType.NText)
				{
					Size = 50
				}
			};
			return dataConnection.QueryProc<PersonSelectByNameResult>("[Person_SelectByName]", parameters);
		}

		public partial class PersonSelectByNameResult
		{
			[Column("PersonID"  )] public int     PersonID   { get; set; }
			[Column("FirstName" )] public string? FirstName  { get; set; }
			[Column("LastName"  )] public string? LastName   { get; set; }
			[Column("MiddleName")] public string? MiddleName { get; set; }
			[Column("Gender"    )] public char?   Gender     { get; set; }
		}
		#endregion

		#region PersonSelectListByName
		public static IEnumerable<PersonSelectListByNameResult> PersonSelectListByName(this TestDataDB dataConnection, string? firstName, string? lastName)
		{
			var parameters = new []
			{
				new DataParameter("@firstName", firstName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@lastName", lastName, DataType.NText)
				{
					Size = 50
				}
			};
			return dataConnection.QueryProc<PersonSelectListByNameResult>("[Person_SelectListByName]", parameters);
		}

		public partial class PersonSelectListByNameResult
		{
			[Column("PersonID"  )] public int     PersonID   { get; set; }
			[Column("FirstName" )] public string? FirstName  { get; set; }
			[Column("LastName"  )] public string? LastName   { get; set; }
			[Column("MiddleName")] public string? MiddleName { get; set; }
			[Column("Gender"    )] public char?   Gender     { get; set; }
		}
		#endregion

		#region PersonUpdate
		public static int PersonUpdate(this TestDataDB dataConnection, int? id, int? personId, string? firstName, string? middleName, string? lastName, char? gender)
		{
			var parameters = new []
			{
				new DataParameter("@id", id, DataType.Int32),
				new DataParameter("@PersonID", personId, DataType.Int32),
				new DataParameter("@FirstName", firstName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@MiddleName", middleName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@LastName", lastName, DataType.NText)
				{
					Size = 50
				},
				new DataParameter("@Gender", gender, DataType.NText)
				{
					Size = 1
				}
			};
			return dataConnection.ExecuteProc("[Person_Update]", parameters);
		}
		#endregion
		#endregion
	}

	[Table("Doctor")]
	public partial class Doctor
	{
		[Column("PersonID", IsPrimaryKey = true )] public int    PersonID { get; set; } // Long
		[Column("Taxonomy", CanBeNull    = false)] public string Taxonomy { get; set; } = null!; // VarChar(50)

		#region Associations
		/// <summary>
		/// PersonDoctor
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(PersonID), OtherKey = nameof(Person.PersonID))]
		public Person PersonDoctor { get; set; } = null!;
		#endregion
	}

	[Table("Dual")]
	public partial class Dual
	{
		[Column("Dummy")] public string? Dummy { get; set; } // VarChar(10)
	}

	[Table("GrandChild")]
	public partial class GrandChild
	{
		[Column("ParentID"    )] public int? ParentID     { get; set; } // Long
		[Column("ChildID"     )] public int? ChildID      { get; set; } // Long
		[Column("GrandChildID")] public int? GrandChildID { get; set; } // Long
	}

	[Table("InheritanceChild")]
	public partial class InheritanceChild
	{
		[Column("InheritanceChildId" , IsPrimaryKey = true)] public int     InheritanceChildId  { get; set; } // Long
		[Column("InheritanceParentId"                     )] public int     InheritanceParentId { get; set; } // Long
		[Column("TypeDiscriminator"                       )] public int?    TypeDiscriminator   { get; set; } // Long
		[Column("Name"                                    )] public string? Name                { get; set; } // VarChar(50)
	}

	[Table("InheritanceParent")]
	public partial class InheritanceParent
	{
		[Column("InheritanceParentId", IsPrimaryKey = true)] public int     InheritanceParentId { get; set; } // Long
		[Column("TypeDiscriminator"                       )] public int?    TypeDiscriminator   { get; set; } // Long
		[Column("Name"                                    )] public string? Name                { get; set; } // VarChar(50)
	}

	[Table("LinqDataTypes")]
	public partial class LinqDataType
	{
		[Column("ID"            )] public int?      ID             { get; set; } // Long
		[Column("MoneyValue"    )] public decimal?  MoneyValue     { get; set; } // Decimal(10, 4)
		[Column("DateTimeValue" )] public DateTime? DateTimeValue  { get; set; } // DateTime
		[Column("DateTimeValue2")] public DateTime? DateTimeValue2 { get; set; } // DateTime
		[Column("BoolValue"     )] public bool      BoolValue      { get; set; } // Bit
		[Column("GuidValue"     )] public Guid?     GuidValue      { get; set; } // GUID
		[Column("BinaryValue"   )] public byte[]?   BinaryValue    { get; set; } // LongBinary
		[Column("SmallIntValue" )] public short?    SmallIntValue  { get; set; } // Short
		[Column("IntValue"      )] public int?      IntValue       { get; set; } // Long
		[Column("BigIntValue"   )] public int?      BigIntValue    { get; set; } // Long
		[Column("StringValue"   )] public string?   StringValue    { get; set; } // VarChar(50)
	}

	[Table("Parent")]
	public partial class Parent
	{
		[Column("ParentID")] public int? ParentID { get; set; } // Long
		[Column("Value1"  )] public int? Value1   { get; set; } // Long
	}

	[Table("Patient")]
	public partial class Patient
	{
		[Column("PersonID" , IsPrimaryKey = true )] public int    PersonID  { get; set; } // Long
		[Column("Diagnosis", CanBeNull    = false)] public string Diagnosis { get; set; } = null!; // VarChar(255)

		#region Associations
		/// <summary>
		/// PersonPatient
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(PersonID), OtherKey = nameof(Person.PersonID))]
		public Person PersonPatient { get; set; } = null!;
		#endregion
	}

	[Table("Person")]
	public partial class Person
	{
		[Column("PersonID"  , IsPrimaryKey = true , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int     PersonID   { get; set; } // COUNTER
		[Column("FirstName" , CanBeNull    = false                                                             )] public string  FirstName  { get; set; } = null!; // VarChar(50)
		[Column("LastName"  , CanBeNull    = false                                                             )] public string  LastName   { get; set; } = null!; // VarChar(50)
		[Column("MiddleName"                                                                                   )] public string? MiddleName { get; set; } // VarChar(50)
		[Column("Gender"                                                                                       )] public char    Gender     { get; set; } // VarChar(1)

		#region Associations
		/// <summary>
		/// PersonDoctor backreference
		/// </summary>
		[Association(ThisKey = nameof(PersonID), OtherKey = nameof(Both.Doctor.PersonID))]
		public Doctor? Doctor { get; set; }

		/// <summary>
		/// PersonPatient backreference
		/// </summary>
		[Association(ThisKey = nameof(PersonID), OtherKey = nameof(Both.Patient.PersonID))]
		public Patient? Patient { get; set; }
		#endregion
	}

	[Table("TestIdentity")]
	public partial class TestIdentity
	{
		[Column("ID", IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int ID { get; set; } // COUNTER
	}

	[Table("TestMerge1")]
	public partial class TestMerge1
	{
		[Column("Id"             , IsPrimaryKey = true)] public int       Id              { get; set; } // Long
		[Column("Field1"                              )] public int?      Field1          { get; set; } // Long
		[Column("Field2"                              )] public int?      Field2          { get; set; } // Long
		[Column("Field3"                              )] public int?      Field3          { get; set; } // Long
		[Column("Field4"                              )] public int?      Field4          { get; set; } // Long
		[Column("Field5"                              )] public int?      Field5          { get; set; } // Long
		[Column("FieldBoolean"                        )] public bool      FieldBoolean    { get; set; } // Bit
		[Column("FieldString"                         )] public string?   FieldString     { get; set; } // VarChar(20)
		[Column("FieldNString"                        )] public string?   FieldNString    { get; set; } // VarChar(20)
		[Column("FieldChar"                           )] public char?     FieldChar       { get; set; } // CHAR(1)
		[Column("FieldNChar"                          )] public char?     FieldNChar      { get; set; } // CHAR(1)
		[Column("FieldFloat"                          )] public float?    FieldFloat      { get; set; } // Single
		[Column("FieldDouble"                         )] public double?   FieldDouble     { get; set; } // Double
		[Column("FieldDateTime"                       )] public DateTime? FieldDateTime   { get; set; } // DateTime
		[Column("FieldBinary"                         )] public byte[]?   FieldBinary     { get; set; } // VARBINARY(20)
		[Column("FieldGuid"                           )] public Guid?     FieldGuid       { get; set; } // GUID
		[Column("FieldDecimal"                        )] public decimal?  FieldDecimal    { get; set; } // Decimal(24, 10)
		[Column("FieldDate"                           )] public DateTime? FieldDate       { get; set; } // DateTime
		[Column("FieldTime"                           )] public DateTime? FieldTime       { get; set; } // DateTime
		[Column("FieldEnumString"                     )] public string?   FieldEnumString { get; set; } // VarChar(20)
		[Column("FieldEnumNumber"                     )] public int?      FieldEnumNumber { get; set; } // Long
	}

	[Table("TestMerge2")]
	public partial class TestMerge2
	{
		[Column("Id"             , IsPrimaryKey = true)] public int       Id              { get; set; } // Long
		[Column("Field1"                              )] public int?      Field1          { get; set; } // Long
		[Column("Field2"                              )] public int?      Field2          { get; set; } // Long
		[Column("Field3"                              )] public int?      Field3          { get; set; } // Long
		[Column("Field4"                              )] public int?      Field4          { get; set; } // Long
		[Column("Field5"                              )] public int?      Field5          { get; set; } // Long
		[Column("FieldBoolean"                        )] public bool      FieldBoolean    { get; set; } // Bit
		[Column("FieldString"                         )] public string?   FieldString     { get; set; } // VarChar(20)
		[Column("FieldNString"                        )] public string?   FieldNString    { get; set; } // VarChar(20)
		[Column("FieldChar"                           )] public char?     FieldChar       { get; set; } // CHAR(1)
		[Column("FieldNChar"                          )] public char?     FieldNChar      { get; set; } // CHAR(1)
		[Column("FieldFloat"                          )] public float?    FieldFloat      { get; set; } // Single
		[Column("FieldDouble"                         )] public double?   FieldDouble     { get; set; } // Double
		[Column("FieldDateTime"                       )] public DateTime? FieldDateTime   { get; set; } // DateTime
		[Column("FieldBinary"                         )] public byte[]?   FieldBinary     { get; set; } // VARBINARY(20)
		[Column("FieldGuid"                           )] public Guid?     FieldGuid       { get; set; } // GUID
		[Column("FieldDecimal"                        )] public decimal?  FieldDecimal    { get; set; } // Decimal(24, 10)
		[Column("FieldDate"                           )] public DateTime? FieldDate       { get; set; } // DateTime
		[Column("FieldTime"                           )] public DateTime? FieldTime       { get; set; } // DateTime
		[Column("FieldEnumString"                     )] public string?   FieldEnumString { get; set; } // VarChar(20)
		[Column("FieldEnumNumber"                     )] public int?      FieldEnumNumber { get; set; } // Long
	}

	[Table("LinqDataTypes Query", IsView = true)]
	public partial class LinqDataTypesQuery
	{
		[Column("DateTimeValue")] public DateTime? DateTimeValue { get; set; } // DateTime
	}

	[Table("LinqDataTypes Query1", IsView = true)]
	public partial class LinqDataTypesQuery1
	{
		[Column("ID")] public int? ID { get; set; } // Long
	}

	[Table("LinqDataTypes Query2", IsView = true)]
	public partial class LinqDataTypesQuery2
	{
		[Column("ID")] public int? ID { get; set; } // Long
	}

	[Table("Patient_SelectAll", IsView = true)]
	public partial class PatientSelectAll
	{
		[Column("PersonID"  , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int     PersonID   { get; set; } // COUNTER
		[Column("FirstName"                                                              )] public string? FirstName  { get; set; } // VarChar(50)
		[Column("LastName"                                                               )] public string? LastName   { get; set; } // VarChar(50)
		[Column("MiddleName"                                                             )] public string? MiddleName { get; set; } // VarChar(50)
		[Column("Gender"                                                                 )] public char?   Gender     { get; set; } // VarChar(1)
		[Column("Diagnosis"                                                              )] public string? Diagnosis  { get; set; } // VarChar(255)
	}

	[Table("Person_SelectAll", IsView = true)]
	public partial class PersonSelectAll
	{
		[Column("PersonID"  , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int     PersonID   { get; set; } // COUNTER
		[Column("FirstName"                                                              )] public string? FirstName  { get; set; } // VarChar(50)
		[Column("LastName"                                                               )] public string? LastName   { get; set; } // VarChar(50)
		[Column("MiddleName"                                                             )] public string? MiddleName { get; set; } // VarChar(50)
		[Column("Gender"                                                                 )] public char?   Gender     { get; set; } // VarChar(1)
	}

	[Table("Scalar_DataReader", IsView = true)]
	public partial class ScalarDataReader
	{
		[Column("intField"   )] public int?    IntField    { get; set; } // Long
		[Column("stringField")] public string? StringField { get; set; } // LongText
	}
}
