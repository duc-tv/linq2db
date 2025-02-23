﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.Mapping;

using NUnit.Framework;

namespace Tests.Linq
{
	using Model;

	[TestFixture]
	public class IssueTests : TestBase
	{
		// https://github.com/linq2db/linq2db/issues/38
		//
		[Test]
		public void Issue38Test([DataSources(false, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			{
				AreEqual(
					from a in Child
					select new { Count = a.GrandChildren.Count() },
					from a in db.Child
					select new { Count = a.GrandChildren1.Count() });

				var sql = ((TestDataConnection)db).LastQuery;

				Assert.That(sql, Is.Not.Contains("INNER JOIN"));

				Debug.WriteLine(sql);
			}
		}

		// https://github.com/linq2db/linq2db/issues/42
		//
		[Test]
		public void Issue42Test([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var t1 = db.Types2.Where(r => r.ID == 1).First();

				t1.BoolValue = !t1.BoolValue;

				db.Update(t1);

				var t2 = db.Types2.First(r => r.ID == t1.ID);

				Assert.That(t2.BoolValue, Is.EqualTo(t1.BoolValue));

				t1.BoolValue = !t1.BoolValue;

				db.Update(t1);
			}
		}

		// https://github.com/linq2db/linq2db/issues/60
		//
		[Test]
		public void Issue60Test([IncludeDataSources(TestProvName.AllSqlServer, ProviderName.SqlCe, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataConnection(context))
			{
				var sp       = db.DataProvider.GetSchemaProvider();
				var dbSchema = sp.GetSchema(db);

				var q =
					from t in dbSchema.Tables
					from c in t.Columns
					where c.ColumnType!.StartsWith("tinyint") && c.MemberType.StartsWith("sbyte")
					select c;

				var column = q.FirstOrDefault();

				Assert.That(column, Is.Null);
			}
		}

		// https://github.com/linq2db/linq2db/issues/67
		//
		[Test]
		public void Issue67Test([DataSources(TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			{
				AreEqual(
					from p in Parent
					join c in Child on p.ParentID equals c.ParentID into ch
					select new { p.ParentID, count = ch.Count() } into t
					where t.count > 0
					select t,
					from p in db.Parent
					join c in db.Child on p.ParentID equals c.ParentID into ch
					select new { p.ParentID, count = ch.Count() } into t
					where t.count > 0
					select t);
			}
		}

		[Test()]
		public void Issue75Test([DataSources(TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			{
				var result = db.Child.Select(c => new
				{
					c.ChildID,
					c.ParentID,
					CountChildren  = db.Child.Count(c2 => c2.ParentID == c.ParentID),
					CountChildren2 = db.Child.Count(c2 => c2.ParentID == c.ParentID),
					HasChildren    = db.Child.Any  (c2 => c2.ParentID == c.ParentID),
					HasChildren2   = db.Child.Any  (c2 => c2.ParentID == c.ParentID),
					AllChildren    = db.Child.All  (c2 => c2.ParentID == c.ParentID),
					AllChildrenMin = db.Child.Where(c2 => c2.ParentID == c.ParentID).Min(c2 => c2.ChildID),
					AllChildrenMax = db.Child.Where(c2 => c2.ParentID == c.ParentID).Max(c2 => c2.ChildID)
				});

				result =
					from child in result
					join parent in db.Parent on child.ParentID equals parent.ParentID
					where parent.Value1 < 7
					select child;

				var expected = Child.Select(c => new
				{
					c.ChildID,
					c.ParentID,
					CountChildren  = Child.Count(c2 => c2.ParentID == c.ParentID),
					CountChildren2 = Child.Count(c2 => c2.ParentID == c.ParentID),
					HasChildren    = Child.Any  (c2 => c2.ParentID == c.ParentID),
					HasChildren2   = Child.Any  (c2 => c2.ParentID == c.ParentID),
					AllChildren    = Child.All  (c2 => c2.ParentID == c.ParentID),
					AllChildrenMin = Child.Where(c2 => c2.ParentID == c.ParentID).Min(c2 => c2.ChildID),
					AllChildrenMax = Child.Where(c2 => c2.ParentID == c.ParentID).Max(c2 => c2.ChildID)
				});

				expected =
					from child in expected
					join parent in Parent on child.ParentID equals parent.ParentID
					where parent.Value1 < 7
					select child;

				AreEqual(expected, result);
			}
		}

		[Test]
		public void Issue115Test([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var qs = (from c in db.Child
						join r in db.Parent on c.ParentID equals r.ParentID
						where r.ParentID > 4
						select c
					)
					.Union(from c in db.Child
						join r in db.Parent on c.ParentID equals r.ParentID
						where r.ParentID <= 4
						select c
					);

				var ql = (from c in Child
						join r in Parent on c.ParentID equals r.ParentID
						where r.ParentID > 4
						select c
					)
					.Union(from c in Child
						join r in Parent on c.ParentID equals r.ParentID
						where r.ParentID <= 4
						select c
					);

				AreEqual(ql, qs);
			}
		}


		[Test]
		public void Issue424Test1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				AreEqual(
					   Parent.Distinct().OrderBy(_ => _.ParentID).Take(1),
					db.Parent.Distinct().OrderBy(_ => _.ParentID).Take(1)
					);
			}
		}

		[Test]
		public void Issue424Test2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				AreEqual(
					   Parent.Distinct().OrderBy(_ => _.ParentID).Skip(1).Take(1),
					db.Parent.Distinct().OrderBy(_ => _.ParentID).Skip(1).Take(1)
					);
			}
		}

		[Test]
		public void Issue424Test3([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				AreEqual(
					   Parent.Distinct().OrderByDescending(_ => _.ParentID).Skip(1).Take(1),
					db.Parent.Distinct().OrderByDescending(_ => _.ParentID).Skip(1).Take(1)
				);
			}
		}

		// https://github.com/linq2db/linq2db/issues/498
		//
		[Test()]
		public void Issue498Test([DataSources] string context)
		{
			using (new WithoutJoinOptimization())
			using (var db = GetDataContext(context))
			{
				var q = from x in db.Child
					//join y in db.GrandChild on new { x.ParentID, x.ChildID } equals new { ParentID = (int)y.ParentID, ChildID = (int)y.ChildID }
					from y in x.GrandChildren1
					select x.ParentID;

				var r = from x in q
					group x by x
					into g
					select new { g.Key, Cghildren = g.Count() };

				var qq = from x in Child
					from y in x.GrandChildren
					select x.ParentID;

				var rr = from x in qq
					group x by x
					into g
					select new { g.Key, Cghildren = g.Count() };

				AreEqual(rr, r);

				var sql = r.ToString()!;
				Assert.Less(0, sql.IndexOf("INNER", 1), sql);
			}
		}


		[Test]
		public void Issue528Test1([DataSources] string context)
		{
			//using (new AllowMultipleQuery())
			using (new GuardGrouping(false))
			using (var db = GetDataContext(context))
			{
				var expected =    Person.GroupBy(_ => _.FirstName).Select(_ => new { _.Key, Data = _.ToList() });
				var result   = db.Person.GroupBy(_ => _.FirstName).Select(_ => new { _.Key, Data = _.ToList() });

				foreach(var re in result)
				{
					var ex = expected.Single(_ => _.Key == re.Key);

					AreEqual(ex.Data, re.Data);
				}
			}
		}

		[Test]
		public void Issue528Test2([DataSources] string context)
		{
			//using (new AllowMultipleQuery())
			using (new GuardGrouping(false))
			using (var db = GetDataContext(context))
			{
				var expected =    Person.GroupBy(_ => _.FirstName).Select(_ => new { _.Key, Data = _.ToList() }).ToList();
				var result   = db.Person.GroupBy(_ => _.FirstName).Select(_ => new { _.Key, Data = _.ToList() }).ToList();

				foreach(var re in result)
				{
					var ex = expected.Single(_ => _.Key == re.Key);

					AreEqual(ex.Data, re.Data);
				}
			}
		}

		[Test]
		public void Issue528Test3([DataSources] string context)
		{
			//using (new AllowMultipleQuery())
			using (new GuardGrouping(false))
			using (var db = GetDataContext(context))
			{
				var expected =    Person.GroupBy(_ => _.FirstName).Select(_ => new { _.Key, Data = _ });
				var result   = db.Person.GroupBy(_ => _.FirstName).Select(_ => new { _.Key, Data = _ });

				foreach(var re in result)
				{
					var ex = expected.Single(_ => _.Key == re.Key);

					AreEqual(ex.Data.ToList(), re.Data.ToList());
				}
			}
		}

		[Test]
		public void Issue508Test([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var query = (
					from c in db.Child
					join p in db.Parent on c.ParentID equals p.ParentID
					where c.ChildID == 11
					select p.ParentID
							 ).Union(
					from c in db.Child
					where c.ChildID == 11
					select c.ParentID
								   );
				var expected = (
					from c in Child
					join p in Parent on c.ParentID equals p.ParentID
					where c.ChildID == 11
					select p.ParentID
							 ).Union(
					from c in Child
					where c.ChildID == 11
					select c.ParentID
								   );

				AreEqual(expected, query);
			}
		}

		public class PersonWrapper
		{
			public int    ID;
			public string FirstName  = null!;
			public string SecondName = null!;
		}

		[Test]
		public void Issue535Test1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var q = from p in db.Person
						where p.FirstName.StartsWith("J")
						select new PersonWrapper
						{
							ID         = p.ID,
							FirstName  = p.FirstName,
							SecondName = p.LastName
						};

				q = from p in q
					where p.ID == 1 || p.SecondName == "fail"
					select p;

				Assert.IsNotNull(q.FirstOrDefault());
			}
		}

		[Table]
		sealed class CustomerBase
		{
			[PrimaryKey, Identity] public int        Id           { get; set; }
			[Column, NotNull]      public ClientType ClientType   { get; set; }
			[Column, Nullable]     public string?    Name         { get; set; }
			[Column, Nullable]     public string?    ContactEmail { get; set; }
			[Column, Nullable]     public bool?      Enabled      { get; set; }
		}

		public class PersonBase
		{
			public   int     Id              { get; set; }
			public   string? Name            { get; set; }
			internal string? CompositeEmails { get; set; }
		}

		public class PersonCustomer : PersonBase
		{
			public List<string>? Emails { get; set; }
			public bool          IsEnabled { get; set; }
		}

		enum ClientType
		{
			[MapValue("Client")]
			Client
		}

		[ActiveIssue("https://github.com/Octonica/ClickHouseClient/issues/56", Configurations = new[] { ProviderName.ClickHouseOctonica })]
		[Test]
		public void Issue535Test2([DataSources(TestProvName.AllSybase)] string context)
		{
			using (var db    = GetDataContext(context))
			using (var table = db.CreateLocalTable<CustomerBase>())
			{
				var query = from cb in table
							where cb.ClientType == ClientType.Client
						//orderby cb.Name
						select new PersonCustomer
						{
							Id = cb.Id,
							Name = cb.Name,
							CompositeEmails = cb.ContactEmail,
							IsEnabled = cb.Enabled ?? false
						};

				var filter = "test";

				query = from q in query where q.Name!.Contains(filter) || q.CompositeEmails!.Contains(filter) select q;

				query.ToList();
			}
		}

		[ActiveIssue("https://github.com/Octonica/ClickHouseClient/issues/56", Configurations = new[] { ProviderName.ClickHouseOctonica })]
		[Test]
		public void Issue535Test3([DataSources(TestProvName.AllSybase)] string context)
		{
			using (var db    = GetDataContext(context))
			using (var table = db.CreateLocalTable<CustomerBase>())
			{
				var query = from cb in table
							 where cb.ClientType == ClientType.Client
							 select new
							 {
								 Id              = cb.Id,
								 Name            = cb.Name,
								 CompositeEmails = cb.ContactEmail,
								 IsEnabled       = cb.Enabled ?? false
							 };

				query.ToList();
			}
		}

		[Table(Name = "Person")]
		public class Person376 //: Person
		{
			[SequenceName(ProviderName.Firebird, "PersonID")]
			[Column("PersonID"), Identity, PrimaryKey]
			public int ID;
			[NotNull] public string FirstName { get; set; } = null!;
			[NotNull] public string LastName = null!;
			[Nullable] public string? MiddleName;


			[Association(ThisKey = nameof(ID), OtherKey = nameof(Model.Doctor.PersonID), CanBeNull = true)]
			public Doctor? Doctor { get; set; }
		}

		public class PersonDto
		{
			public int    Id;
			public string Name = null!;

			public DoctorDto? Doc;
		}

		public class DoctorDto
		{
			public int    PersonId;
			public string Taxonomy = null!;
		}

		[ExpressionMethod("MapToDtoExpr1")]
		public static PersonDto MapToDto(Person376 person)
		{
			return MapToDtoExpr1().CompileExpression()(person);
		}

		[ExpressionMethod("MapToDtoExpr2")]
		public static DoctorDto MapToDto(Doctor doctor)
		{
			return MapToDtoExpr2().CompileExpression()(doctor);
		}

		private static Expression<Func<Person376, PersonDto>> MapToDtoExpr1()
		{
			return x => new PersonDto
			{

				Id   = x.ID,
				Name = x.FirstName,
				Doc  = x.Doctor != null ? MapToDto(x.Doctor) : null
			};
		}

		private static Expression<Func<Doctor, DoctorDto>> MapToDtoExpr2()
		{
			return x => new DoctorDto
			{
				PersonId = x.PersonID,
				Taxonomy = x.Taxonomy
			};
		}

		[Test]
		public void Issue376([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var l = db
					.GetTable<Person376>()
					.Where(_ => _.Doctor!.Taxonomy.Length >= 0 || _.Doctor.Taxonomy == null)
					.Select(_ => MapToDto(_)).ToList();

				Assert.IsNotEmpty(l);
				Assert.IsNotEmpty(l.Where(_ => _.Doc == null));
				Assert.IsNotEmpty(l.Where(_ => _.Doc != null));
			}
		}


		[Table("Person", IsColumnAttributeRequired = false)]
		public class Person88
		{
			[SequenceName(ProviderName.Firebird, "PersonID")]
			[Column("PersonID"), Identity, PrimaryKey] public int     ID;
			[NotNull]                                  public string  FirstName { get; set; } = null!;
			[NotNull]                                  public string  LastName = null!;
			[Nullable]                                 public string? MiddleName;
			                                           public char    Gender;
		}

		[Test]
		public void Issue88([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var llc = db
					.GetTable<Person88>()
					.Where(_ => _.ID == 1 && _.Gender == 'M');

				var lrc = db
					.GetTable<Person88>()
					.Where(_ => _.ID == 1 && 'M' == _.Gender);

				var gender = 'M';
				var llp = db
					.GetTable<Person88>()
					.Where(_ => _.ID == 1 && _.Gender == gender);

				var lrp = db
					.GetTable<Person88>()
					.Where(_ => _.ID == 1 && gender == _.Gender);

				Assert.IsNotEmpty(llc);
				Assert.IsNotEmpty(lrc);
				Assert.IsNotEmpty(llp);
				Assert.IsNotEmpty(lrp);
			}

		}


		[Test]
		public void Issue173([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var result =
					from r in db.GetTable<Parent>()
					select new
					{
						id = r.ParentID,
					};
				result = result.Where(_ => _.id == 1);

				var expected =
					from r in Parent
					select new
					{
						id = r.ParentID,
					};
				expected = expected.Where(_ => _.id == 1);

				AreEqual(expected, result);
			}
		}

		[Test]
		public void Issue909([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var values = new int?[] { 1, 2, 3 };

				var query = from p in db.GetTable<Parent>()
						where !values.Contains(p.Value1)
						select p;

				AssertQuery(query);
			}
		}

		[Test]
		public void Issue909Join([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var values = new int?[] { 1, 2, 3 };

				var query = from c in db.GetTable<Child>()
					from p in db.GetTable<Parent>()
					where p.ParentID == c.ParentID && !values.Contains(p.Value1)
					select c;

				AssertQuery(query);
			}
		}
		[Test]
		public void Issue909Subquery([DataSources(TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			{
				var values = new int?[] { 1, 2, 3 };

				var expected = from c in Child
					where (from p in Parent
						where p.ParentID == c.ParentID && (p.Value1 == null || !values.Contains(p.Value1.Value))
						select p).Any()
					select c;

				var actual = from c in db.GetTable<Child>()
					where (from p in db.GetTable<Parent>()
						where p.ParentID == c.ParentID && !values.Contains(p.Value1!.Value)
						select p).Any()
					select c;

				AreEqual(expected, actual);
			}
		}

		[Table("AllTypes")]
		[Table("ALLTYPES", Configuration = ProviderName.DB2)]
		private sealed class InsertIssueTest
		{
			[Column("smallintDataType")]
			[Column("SMALLINTDATATYPE", Configuration = ProviderName.DB2)]
			public short ID;

			[Column]
			[Column("INTDATATYPE", Configuration = ProviderName.DB2)]
			public int? intDataType;

			[Association(ThisKey = nameof(ID), OtherKey = nameof(intDataType), CanBeNull = true)]
			public IQueryable<InsertIssueTest> Association => throw new InvalidOperationException();
		}

		[Test]
		public void InsertFromSelectWithNullableFilter([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				Query(true);
				Query(false);

				void Query(bool isNull)
				{
					db.GetTable<InsertIssueTest>()
						.Where(_ => _.ID == GetId(isNull))
						.SelectMany(_ => _.Association)
						.Select(_ => _.ID)
						.Distinct()
						.Insert(
							db.GetTable<InsertIssueTest>(),
							_ => new InsertIssueTest()
							{
								ID = 123,
								intDataType = _
							});
				}
			}
		}

		private short? GetId(bool isNull)
		{
			return isNull ? (short?)null : 1234;
		}

		[Test]
		public void Issue2823Guid([IncludeDataSources(false, TestProvName.AllFirebird)] string context, [Values] bool inlineParameters)
		{
			using(var db    = (DataConnection)GetDataContext(context))
			using(var table = db.CreateLocalTable<TableWithGuid>())
			{
				Assert.True(db.LastQuery!.Contains("\"Default\"  CHAR(16) CHARACTER SET OCTETS"));
				Assert.True(db.LastQuery!.Contains("\"Binary\"   CHAR(16) CHARACTER SET OCTETS"));
				Assert.True(db.LastQuery!.Contains("\"String\"   CHAR(38)"));
				Assert.True(db.LastQuery!.Contains("\"DefaultN\" CHAR(16) CHARACTER SET OCTETS"));
				Assert.True(db.LastQuery!.Contains("\"BinaryN\"  CHAR(16) CHARACTER SET OCTETS"));
				Assert.True(db.LastQuery!.Contains("\"StringN\"  CHAR(38)"));

				db.InlineParameters = inlineParameters;

				table.Insert(() => new TableWithGuid {
					Default  = TestData.Guid1, Binary  = TestData.Guid2, String  = TestData.Guid3,
					DefaultN = TestData.Guid4, BinaryN = TestData.Guid5, StringN = TestData.Guid6,
				});
				
				var data = table.ToArray();
				Assert.AreEqual(1, data.Length);
				Assert.AreEqual(TestData.Guid1, data[0].Default);
				Assert.AreEqual(TestData.Guid2, data[0].Binary);
				Assert.AreEqual(TestData.Guid3, data[0].String);
				Assert.AreEqual(TestData.Guid4, data[0].DefaultN);
				Assert.AreEqual(TestData.Guid5, data[0].BinaryN);
				Assert.AreEqual(TestData.Guid6, data[0].StringN);

				Assert.AreEqual(1, table.Where(x => x.Default  == TestData.Guid1).Count());
				Assert.AreEqual(1, table.Where(x => x.Binary   == TestData.Guid2).Count());
				Assert.AreEqual(1, table.Where(x => x.String   == TestData.Guid3).Count());
				Assert.AreEqual(1, table.Where(x => x.DefaultN == TestData.Guid4).Count());
				Assert.AreEqual(1, table.Where(x => x.BinaryN  == TestData.Guid5).Count());
				Assert.AreEqual(1, table.Where(x => x.StringN  == TestData.Guid6).Count());
			}
		}

		[Table]
		sealed class TableWithGuid
		{
			[Column                           ] public Guid Default   { get; set; }
			[Column(DataType = DataType.Guid) ] public Guid Binary    { get; set; }
			[Column(DataType = DataType.Char) ] public Guid String    { get; set; }

			[Column                           ] public Guid? DefaultN { get; set; }
			[Column(DataType = DataType.Guid) ] public Guid? BinaryN  { get; set; }
			[Column(DataType = DataType.NChar)] public Guid? StringN  { get; set; }
		}
	}
}
