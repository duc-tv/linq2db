﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

using NUnit.Framework;

#region ReSharper disable
// ReSharper disable ConvertToConstant.Local
#endregion

namespace Tests.xUpdate
{
	using LinqToDB.Common;
	using Model;

	[TestFixture]
//	[Order(10000)]
	public class UpdateTests : TestBase
	{
		[Test]
		public void Update1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				db.Insert(parent);

				Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID));

				var cnt = db.Parent.Update(p => p.ParentID == parent.ParentID, p => new Parent { ParentID = p.ParentID + 1 });
				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID + 1));
			}
		}

		[Test]
		public async Task Update1Async([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				await db.InsertAsync(parent);

				Assert.AreEqual(1, await db.Parent.CountAsync(p => p.ParentID == parent.ParentID));

				var cnt = await db.Parent.UpdateAsync(p => p.ParentID == parent.ParentID, p => new Parent { ParentID = p.ParentID + 1 });
				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				Assert.AreEqual(1, await db.Parent.CountAsync(p => p.ParentID == parent.ParentID + 1));
			}
		}

		[Test]
		public void Update2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				db.Insert(parent);

				Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID));

				var cnt = db.Parent.Where(p => p.ParentID == parent.ParentID).Update(p => new Parent { ParentID = p.ParentID + 1 });
				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID + 1));
			}
		}

		[Test]
		public async Task Update2Async([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				await db.InsertAsync(parent);

				Assert.AreEqual(1, await db.Parent.CountAsync(p => p.ParentID == parent.ParentID));

				var cnt = await db.Parent.Where(p => p.ParentID == parent.ParentID).UpdateAsync(p => new Parent { ParentID = p.ParentID + 1 });
				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				Assert.AreEqual(1, await db.Parent.CountAsync(p => p.ParentID == parent.ParentID + 1));
			}
		}

		[Test]
		public void Update3([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Child.Insert(() => new Child { ParentID = 1, ChildID = id });

				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				Assert.AreEqual(1, db.Child.Where(c => c.ChildID == id && c.Parent!.Value1 == 1).Update(c => new Child { ChildID = c.ChildID + 1 }));
				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
			}
		}

		[Test]
		public void Update4([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Child.Insert(() => new Child { ParentID = 1, ChildID = id });

				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				Assert.AreEqual(1,
					db.Child
						.Where(c => c.ChildID == id && c.Parent!.Value1 == 1)
							.Set(c => c.ChildID, c => c.ChildID + 1)
						.Update());
				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
			}
		}

		[Test]
		public void Update4String([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			{
				var id = 1001;

				var updatable =
					db.Child
						.Where(c => c.ChildID == id && c.Parent!.Value1 == 1)
						.Set(c => c.ChildID, c => c.ChildID + 1);

				var sql = updatable.ToString();
				TestContext.WriteLine(sql);

				Assert.That(sql, Does.Contain("UPDATE"));
			}
		}

		[Test]
		public async Task Update4Async([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				await db.Child.InsertAsync(() => new Child { ParentID = 1, ChildID = id });

				Assert.AreEqual(1, await db.Child.CountAsync(c => c.ChildID == id));
				Assert.AreEqual(1,
					await db.Child
						.Where(c => c.ChildID == id && c.Parent!.Value1 == 1)
							.Set(c => c.ChildID, c => c.ChildID + 1)
						.UpdateAsync());
				Assert.AreEqual(1, await db.Child.CountAsync(c => c.ChildID == id + 1));
			}
		}

		[Test]
		public void Update5([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Child.Insert(() => new Child { ParentID = 1, ChildID = id });

				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				Assert.AreEqual(1,
					db.Child
						.Where(c => c.ChildID == id && c.Parent!.Value1 == 1)
							.Set(c => c.ChildID, () => id + 1)
						.Update());
				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
			}
		}

		[Test]
		public void Update6([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Insert(new Parent4 { ParentID = id, Value1 = TypeValue.Value1 });

				Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value1));

				var cnt = db.Parent4
						.Where(p => p.ParentID == id)
							.Set(p => p.Value1, () => TypeValue.Value2)
						.Update();
				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value2));
			}
		}

		[Test]
		public void Update7([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Insert(new Parent4 { ParentID = id, Value1 = TypeValue.Value1 });

				Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value1));
				var cnt = db.Parent4
						.Where(p => p.ParentID == id)
							.Set(p => p.Value1, TypeValue.Value2)
						.Update();
				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);
				Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value2));

				cnt = db.Parent4
						.Where(p => p.ParentID == id)
							.Set(p => p.Value1, TypeValue.Value3)
						.Update();
				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value3));
			}
		}

		[Test]
		public void Update8([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				db.Insert(parent);

				parent.Value1++;

				db.Update(parent);

				Assert.AreEqual(1002, db.Parent.Single(p => p.ParentID == parent.ParentID).Value1);
			}
		}

		[Test]
		public void Update9(
			[DataSources(
				TestProvName.AllInformix,
				TestProvName.AllClickHouse,
				ProviderName.SqlCe,
				ProviderName.DB2,
				TestProvName.AllFirebird,
				TestProvName.AllOracle,
				TestProvName.AllMySql,
				TestProvName.AllSQLite,
				ProviderName.Access,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Child.Insert(() => new Child { ParentID = 1, ChildID = id });

				var q =
						from c in db.Child
						join p in db.Parent on c.ParentID equals p.ParentID
						where c.ChildID == id && c.Parent!.Value1 == 1
						select new { c, p };

				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				Assert.AreEqual(1, q.Update(db.Child, _ => new Child { ChildID = _.c.ChildID + 1, ParentID = _.p.ParentID }));
				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
			}
		}

		[Test]
		public void Update10(
			[DataSources(
				TestProvName.AllClickHouse,
				TestProvName.AllInformix,
				ProviderName.SqlCe,
				ProviderName.DB2,
				TestProvName.AllFirebird,
				TestProvName.AllOracle,
				TestProvName.AllMySql,
				TestProvName.AllSQLite,
				ProviderName.Access,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Child.Insert(() => new Child { ParentID = 1, ChildID = id });

				var q =
						from p in db.Parent
						join c in db.Child on p.ParentID equals c.ParentID
						where c.ChildID == id && c.Parent!.Value1 == 1
						select new { c, p };

				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				Assert.AreEqual(1, q.Update(db.Child, _ => new Child { ChildID = _.c.ChildID + 1, ParentID = _.p.ParentID }));
				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
			}
		}

		//[Test]
		public void Update11([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				db.BeginTransaction();

				var q = db.GetTable<LinqDataTypes2>().Union(db.GetTable<LinqDataTypes2>());

				//db.GetTable<LinqDataTypes2>().Update(_ => q.Contains(_), _ => new LinqDataTypes2 { GuidValue = _.GuidValue });

				q.Update(_ => new LinqDataTypes2 { GuidValue = _.GuidValue });
			}
		}

		[Test]
		public void Update12(
			[DataSources(
				ProviderName.SqlCe,
				ProviderName.DB2,
				TestProvName.AllClickHouse,
				TestProvName.AllInformix,
				TestProvName.AllFirebird,
				TestProvName.AllOracle,
				TestProvName.AllSQLite,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			{
				(
					from p1 in db.Parent
					join p2 in db.Parent on p1.ParentID equals p2.ParentID
					where p1.ParentID < 3
					select new { p1, p2 }
				)
				.Update(q => q.p1, q => new Parent { ParentID = q.p2.ParentID });
			}
		}

		[Test]
		public async Task Update12Async(
			[DataSources(
				ProviderName.SqlCe,
				ProviderName.DB2,
				TestProvName.AllClickHouse,
				TestProvName.AllInformix,
				TestProvName.AllFirebird,
				TestProvName.AllOracle,
				TestProvName.AllSQLite,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			{
				await (
					from p1 in db.Parent
					join p2 in db.Parent on p1.ParentID equals p2.ParentID
					where p1.ParentID < 3
					select new { p1, p2 }
				)
				.UpdateAsync(q => q.p1, q => new Parent { ParentID = q.p2.ParentID });
			}
		}

		[Test]
		public void Update13(
			[DataSources(
				ProviderName.SqlCe,
				ProviderName.DB2,
				TestProvName.AllClickHouse,
				TestProvName.AllInformix,
				TestProvName.AllFirebird,
				TestProvName.AllOracle,
				TestProvName.AllSQLite,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			{
				(
					from p1 in db.Parent
					join p2 in db.Parent on p1.ParentID equals p2.ParentID
					where p1.ParentID < 3
					select new { p1, p2 }
				)
				.Update(q => q.p2, q => new Parent { ParentID = q.p1.ParentID });
			}
		}

		[Test]
		public void Update14([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				db.Insert(new Person()
				{
					ID        = 100,
					FirstName = "Update14",
					LastName  = "whatever"
				});

				var name = "Update14";
				var idx = 4;

				db.Person
					.Where(_ => _.FirstName.StartsWith("Update14"))
					.Update(p => new Person()
					{
						LastName = (Sql.AsSql(name).Length + idx).ToString(),
					});

				var cnt = db.Person.Where(_ => _.FirstName.StartsWith("Update14")).Count();
				Assert.AreEqual(1, cnt);
			}
		}

		[Test]
		public void TestUpdateWithColumnFilter1([DataSources] string context, [Values] bool withMiddleName)
		{
			ResetPersonIdentity(context);

			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var newName = "UpdateColumnFilterUpdated";
				var p = new Person()
				{
					ID         = 100,
					FirstName  = newName,
					LastName   = "whatever",
					MiddleName = "som middle name",
					Gender     = Gender.Male
				};

				db.Insert(p);

				p = db.GetTable<Person>().Where(x => x.FirstName == p.FirstName).First();

				p.MiddleName = "updated name";

				db.Update(p, (a, b) => b.ColumnName != nameof(Model.Person.MiddleName) || withMiddleName);

				p = db.GetTable<Person>().Where(x => x.FirstName == p.FirstName).First();

				if (withMiddleName)
					Assert.AreEqual("updated name", p.MiddleName);
				else
					Assert.AreNotEqual("updated name", p.MiddleName);
			}
		}

		[Test]
		public void TestUpdateWithColumnFilter2([DataSources] string context)
		{
			ResetPersonIdentity(context);

			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var newName = "UpdateColumnFilterUpdated";
				var p = new Person()
				{
					ID        = 100,
					FirstName = "UpdateColumnFilter",
					LastName  = "whatever"
				};

				db.Insert(p);

				p = db.GetTable<Person>().Where(x => x.FirstName == p.FirstName).Single();

				p.FirstName = newName;
				p.LastName  = newName;

				var columsToUpdate = new HashSet<string> { nameof(p.FirstName) };

				db.Update(p, (a, b) => columsToUpdate.Contains(b.ColumnName));

				var updatedPerson = db.GetTable<Person>().Where(x => x.ID == p.ID).Single();
				Assert.AreEqual("whatever", updatedPerson.LastName);
				Assert.AreEqual(newName, updatedPerson.FirstName);

				// test for cached update query - must update both columns
				db.Update(p);
				updatedPerson = db.GetTable<Person>().Where(_ => _.ID == p.ID).Single();

				Assert.AreEqual(newName, updatedPerson.LastName);
				Assert.AreEqual(newName, updatedPerson.FirstName);
			}
		}

		[Test]
		public void UpdateComplex1([DataSources] string context)
		{
			ResetPersonIdentity(context);

			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var person = new ComplexPerson2
				{
					Name = new FullName
					{
						FirstName = "UpdateComplex",
						LastName  = "Empty"
					}
				};

				int id;
				if (context.IsAnyOf(TestProvName.AllClickHouse))
				{
					person.ID = id = 100;
					db.Insert(person);
				}
				else
					id = db.InsertWithInt32Identity(person);

				var obj = db.GetTable<ComplexPerson2>().First(_ => _.ID == id);
				obj.Name.LastName = obj.Name.FirstName;

				db.Update(obj);

				obj = db.GetTable<ComplexPerson2>().First(_ => _.ID == id);

				Assert.AreEqual(obj.Name.FirstName, obj.Name.LastName);
			}
		}

		[Test]
		public async Task UpdateComplex1Async([DataSources] string context)
		{
			ResetPersonIdentity(context);

			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var person = new ComplexPerson2
				{
					Name = new FullName
					{
						FirstName = "UpdateComplex",
						LastName  = "Empty"
					}
				};

				int id;
				if (context.IsAnyOf(TestProvName.AllClickHouse))
				{
					person.ID = id = 100;
					db.Insert(person);
				}
				else
					id = db.InsertWithInt32Identity(person);

				var obj = await db.GetTable<ComplexPerson2>().FirstAsync(_ => _.ID == id);
				obj.Name.LastName = obj.Name.FirstName;

				await db.UpdateAsync(obj);

				obj = await db.GetTable<ComplexPerson2>().FirstAsync(_ => _.ID == id);

				Assert.AreEqual(obj.Name.FirstName, obj.Name.LastName);
			}
		}

		[Test]
		public void SetWithTernaryOperatorIssue([DataSources] string context)
		{
			Gender? nullableGender = Gender.Other;

			ResetPersonIdentity(context);

			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var person = new ComplexPerson2
				{
					Name = new FullName
					{
						FirstName = "UpdateComplex",
						LastName  = "Empty",
					},
					Gender = Gender.Male
				};

				int id;
				if (context.IsAnyOf(TestProvName.AllClickHouse))
				{
					person.ID = id = 100;
					db.Insert(person);
				}
				else
					id = db.InsertWithInt32Identity(person);

				var cnt = db.GetTable<ComplexPerson2>()
						.Where(_ => _.Name.FirstName.StartsWith("UpdateComplex"))
						.Set(_ => _.Gender, _ => nullableGender.HasValue ? nullableGender.Value : _.Gender)
						.Update();

				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				var obj = db.GetTable<ComplexPerson2>()
						.First(_ => _.ID == id);

				Assert.AreEqual(Gender.Other, obj.Gender);
			}
		}

		[Test]
		public void UpdateComplex2([DataSources] string context)
		{
			ResetPersonIdentity(context);

			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var person = new ComplexPerson2
				{
					Name = new FullName
					{
						FirstName = "UpdateComplex",
						LastName  = "Empty",
					}
				};

				int id;
				if (context.IsAnyOf(TestProvName.AllClickHouse))
				{
					person.ID = id = 100;
					db.Insert(person);
				}
				else
					id = db.InsertWithInt32Identity(person);

				var cnt = db.GetTable<ComplexPerson2>()
						.Where(_ => _.Name.FirstName.StartsWith("UpdateComplex"))
						.Set(_ => _.Name.LastName, _ => _.Name.FirstName)
						.Update();

				if (!context.IsAnyOf(TestProvName.AllClickHouse))
					Assert.AreEqual(1, cnt);

				var obj = db.GetTable<ComplexPerson2>().First(_ => _.ID == id);

				Assert.AreEqual(obj.Name.FirstName, obj.Name.LastName);
			}
		}

		[Test]
		public void UpdateAssociation1([DataSources(TestProvName.AllSybase, TestProvName.AllClickHouse, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
				db.Child .Insert(() => new Child { ChildID = childId, ParentID = parentId });

				var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

				Assert.AreEqual(1, parents.Update(db.Parent, x => new Parent { Value1 = 5 }));
			}
		}

		[Test]
		public async Task UpdateAssociation1Async([DataSources(TestProvName.AllSybase, TestProvName.AllClickHouse, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				await db.Parent.InsertAsync(() => new Parent { ParentID = parentId, Value1 = parentId });
				await db.Child.InsertAsync(() => new Child { ChildID = childId, ParentID = parentId });

				var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

				Assert.AreEqual(1, await parents.UpdateAsync(db.Parent, x => new Parent { Value1 = 5 }));
			}
		}

		[Test]
		public void UpdateAssociation2([DataSources(TestProvName.AllSybase, TestProvName.AllClickHouse, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
				db.Child.Insert(() => new Child { ChildID = childId, ParentID = parentId });

				var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

				Assert.AreEqual(1, parents.Update(x => new Parent { Value1 = 5 }));
			}
		}

		[Test]
		public void UpdateAssociation3([DataSources(TestProvName.AllSybase, TestProvName.AllClickHouse, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
				db.Child.Insert(() => new Child { ChildID = childId, ParentID = parentId });

				var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

				Assert.AreEqual(1, parents.Update(x => x.ParentID > 0, x => new Parent { Value1 = 5 }));
			}
		}

		[Test]
		public void UpdateAssociation4([DataSources(TestProvName.AllSybase, TestProvName.AllClickHouse, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
				db.Child.Insert(() => new Child { ChildID = childId, ParentID = parentId });

				var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

				Assert.AreEqual(1, parents.Set(x => x.Value1, 5).Update());
			}
		}

		static readonly Func<TestDataConnection,int,string,int> _updateQuery =
			CompiledQuery.Compile<TestDataConnection,int,string,int>((ctx,key,value) =>
				ctx.Person
					.Where(_ => _.ID == key)
					.Set(_ => _.FirstName, value)
					.Update());

		[Test]
		public void CompiledUpdate()
		{
			using (var ctx = new TestDataConnection())
			{
				_updateQuery(ctx, 12345, "54321");
			}
		}

		[Table("LinqDataTypes")]
		sealed class Table1
		{
			[Column] public int  ID;
			[Column] public bool BoolValue;

			[Association(ThisKey = "ID", OtherKey = "ParentID", CanBeNull = false)]
			public List<Table2> Tables2 = null!;
		}

		[Table("Parent")]
		sealed class Table2
		{
			[Column] public int  ParentID;
			[Column] public int? Value1;

			[Association(ThisKey = "ParentID", OtherKey = "ID", CanBeNull = false)]
			public Table1 Table1 = null!;
		}

		[Test]
		public void UpdateAssociation5(
			[DataSources(
				false,
				TestProvName.AllAccess,
				TestProvName.AllClickHouse,
				ProviderName.DB2,
				TestProvName.AllInformix,
				TestProvName.AllOracle,
				TestProvName.AllSQLite,
				TestProvName.AllFirebird,
				ProviderName.SqlCe,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataConnection(context))
			{
				var ids = new[] { 10000, 20000 };

				db.GetTable<Table2>()
					.Where (x => ids.Contains(x.ParentID))
					.Select(x => x.Table1)
					.Distinct()
					.Set(y => y.BoolValue, y => y.Tables2.All(x => x.Value1 == 1))
					.Update();

				if (!context.IsAnyOf(TestProvName.AllSybase))
				{
					var idx = db.LastQuery!.IndexOf("INNER JOIN");

					Assert.That(idx, Is.Not.EqualTo(-1));

					idx = db.LastQuery.IndexOf("INNER JOIN", idx + 1);

					Assert.That(idx, Is.EqualTo(-1));
				}
				else
				{
					var idx = db.LastQuery!.IndexOf("INNER JOIN");

					Assert.That(idx, Is.EqualTo(-1));
				}
			}
		}

		[Test]
		public void AsUpdatableTest([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Child.Delete(c => c.ChildID > 1000);
				db.Child.Insert(() => new Child { ParentID = 1, ChildID = id });

				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));

				var q  = db.Child.Where(c => c.ChildID == id && c.Parent!.Value1 == 1);
				var uq = q.AsUpdatable();

				uq = uq.Set(c => c.ChildID, c => c.ChildID + 1);

				Assert.AreEqual(1, uq.Update());
				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
			}
		}

		[Test]
		public void AsUpdatableDuplicate([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 1001;

				db.Child.Delete(c => c.ChildID > 1000);
				db.Child.Insert(() => new Child { ParentID = 1, ChildID = id });

				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));

				var q  = db.Child.Where(c => c.ChildID == id && c.Parent!.Value1 == 1);
				var uq = q.AsUpdatable();

				uq = uq.Set(c => c.ChildID, c => c.ChildID + 1);
				uq = uq.Set(c => c.ChildID, c => c.ChildID + 2);

				Assert.AreEqual(1, uq.Update());
				Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 2));
			}
		}

		[Table("GrandChild")]
		sealed class Table3
		{
			[PrimaryKey(1)] public int? ParentID;
			[PrimaryKey(2)] public int? ChildID;
			[Column]        public int? GrandChildID;
		}

		[Test]
		public void UpdateNullablePrimaryKey([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				db.Update(new Table3 { ParentID = 10000, ChildID = null, GrandChildID = 1000 });

				if (db is DataConnection)
					Assert.IsTrue(((DataConnection)db).LastQuery!.Contains("IS NULL"));

				db.Update(new Table3 { ParentID = 10000, ChildID = 111, GrandChildID = 1000 });

				if (db is DataConnection)
					Assert.IsFalse(((DataConnection)db).LastQuery!.Contains("IS NULL"));
			}
		}

		[Test]
		public void UpdateTop(
			[DataSources(
				TestProvName.AllAccess,
				ProviderName.DB2,
				TestProvName.AllClickHouse,
				TestProvName.AllInformix,
				TestProvName.AllFirebird,
				TestProvName.AllPostgreSQL,
				TestProvName.AllSQLite,
				ProviderName.SqlCe,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				using (new DisableLogging())
				{
					for (var i = 0; i < 10; i++)
						db.Insert(new Parent { ParentID = 1000 + i });
				}

				var rowsAffected = db.Parent
					.Where(p => p.ParentID >= 1000)
					.Take(5)
					.Set(p => p.Value1, 1)
					.Update();

				Assert.That(rowsAffected, Is.EqualTo(5));
			}
		}

		[Test]
		public void TestUpdateTakeOrdered(
			[DataSources(
				ProviderName.Access,
				ProviderName.DB2,
				TestProvName.AllClickHouse,
				TestProvName.AllInformix,
				ProviderName.SqlCe,
				TestProvName.AllSapHana,
				TestProvName.AllFirebird,
				TestProvName.AllSQLite,
				TestProvName.AllMySql,
				TestProvName.AllSybase,
				TestProvName.AllOracle)]
			string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				using (new DisableLogging())
				{
					for (var i = 0; i < 10; i++)
						db.Insert(new Parent { ParentID = 1000 + i });
				}

				var entities =
					from x in db.Parent
					where x.ParentID > 1000
					orderby x.ParentID descending
					select x;

				var rowsAffected = entities
					.Take(5)
					.Update(x => new Parent { Value1 = 1 });

				Assert.That(rowsAffected, Is.EqualTo(5));
			}
		}

		[Test]
		public void TestUpdateSkipTake(
			[DataSources(
				TestProvName.AllAccess,
				TestProvName.AllClickHouse,
				ProviderName.DB2,
				TestProvName.AllInformix,
				ProviderName.SqlCe,
				TestProvName.AllSapHana,
				TestProvName.AllFirebird,
				TestProvName.AllSQLite,
				TestProvName.AllMySql,
				TestProvName.AllSybase,
				TestProvName.AllOracle)]
			string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				using (new DisableLogging())
				{
					for (var i = 0; i < 10; i++)
						db.Insert(new Parent { ParentID = 1000 + i });
				}

				var entities =
					from x in db.Parent
					where x.ParentID > 1000
					orderby x.ParentID descending
					select x;

				var rowsAffected = entities
					.Skip(1)
					.Take(5)
					.Update(x => new Parent { Value1 = 1 });

				Assert.That(rowsAffected, Is.EqualTo(5));

				Assert.False(db.Parent.Where(p => p.ParentID == 1000 + 9).Single().Value1 == 1);
			}
		}

		[Test]
		public void TestUpdateTakeNotOrdered(
			[DataSources(
				TestProvName.AllAccess,
				TestProvName.AllClickHouse,
				ProviderName.DB2,
				TestProvName.AllInformix,
				TestProvName.AllFirebird,
				TestProvName.AllSQLite,
				ProviderName.SqlCe,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				using (new DisableLogging())
				{
					for (var i = 0; i < 10; i++)
						db.Insert(new Parent { ParentID = 1000 + i });
				}

				var entities =
					from x in db.Parent
					where x.ParentID > 1000
					select x;

				var rowsAffected = entities
					.Take(5)
					.Update(x => new Parent { Value1 = 1 });

				Assert.That(rowsAffected, Is.EqualTo(5));
			}
		}

		[Test]
		public void UpdateSetSelect([DataSources(
			TestProvName.AllAccess, TestProvName.AllClickHouse, TestProvName.AllInformix, ProviderName.SqlCe)]
			string context)
		{
			using (var db = GetDataContext(context))
			{
				db.Parent.Delete(_ => _.ParentID > 1000);

				var res =
				(
					from p in db.Parent
					join c in db.Child on p.ParentID equals c.ParentID
					where p.ParentID == 1
					select p
				)
				.Set(p => p.ParentID, p => db.Child.SingleOrDefault(c => c.ChildID == 11)!.ParentID + 1000)
				.Update();

				Assert.AreEqual(1, res);

				res = db.Parent.Where(_ => _.ParentID == 1001).Set(_ => _.ParentID, 1).Update();
				Assert.AreEqual(1, res);
			}
		}

		[Test]
		public void UpdateIssue319Regression(
			[DataSources(
				TestProvName.AllAccess,
				TestProvName.AllClickHouse,
				TestProvName.AllInformix,
				TestProvName.AllFirebird,
				TestProvName.AllSQLite,
				TestProvName.AllMySql,
				TestProvName.AllSybase,
				TestProvName.AllSapHana)]
			string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 100500;
				db.Insert(new Parent1()
				{
					ParentID = id
				});

				var query = db.GetTable<Parent1>()
						.Where(_ => _.ParentID == id)
						.Select(_ => new Parent1()
						{
							ParentID = _.ParentID
						});

				var queryResult = new Lazy<Parent1>(() => query.First());

				var cnt = db.GetTable<Parent1>()
						.Where(_ => _.ParentID == id && query.Count() > 0)
						.Update(_ => new Parent1()
						{
							Value1 = queryResult.Value.ParentID
						});

				Assert.AreEqual(1, cnt);
			}
		}

		// looks like managed provider handle null bit parameters as false, because it doesn't fail
		// maybe we need to do the same for unmanaged
		[ActiveIssue("AseException : Null value is not allowed in BIT TYPE", Configuration = ProviderName.Sybase)]
		[Test]
		public void UpdateIssue321Regression([DataSources(ProviderName.DB2, TestProvName.AllInformix, TestProvName.AllFirebird)] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var id = 100500;

				var value1 = 3000m;
				var value2 = 13621m;
				var value3 = 60;

				db.Insert(new LinqDataTypes2()
				{
					ID = id,
					MoneyValue = value1,
					IntValue = value3
				});

				db.GetTable<LinqDataTypes2>()
					.Update(
						_ => _.ID == id,
						_ => new LinqDataTypes2
						{
							SmallIntValue = (short)(_.MoneyValue / (value2 / _.IntValue!))
						});

				var dbResult = db.GetTable<LinqDataTypes2>()
						.Where(_ => _.ID == id)
						.Select(_ => _.SmallIntValue).First();

				var expected = (short)(value1 / (value2 / value3));

				Assert.AreEqual(expected, dbResult);
			}
		}

		[Test()]
		public void UpdateMultipleColumns([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (new RestoreBaseTables(db))
			{
				var ldt = new LinqDataTypes
				{
					ID            = 1001,
					MoneyValue    = 1000,
					SmallIntValue = 100,
				};

				db.Types
					.Value(t => t.ID, ldt.ID)
					.Value(t => t.MoneyValue, () => ldt.MoneyValue)
					.Value(t => t.SmallIntValue, () => ldt.SmallIntValue)
					.Insert()
					;

				db.Types
					.Where(t => t.ID == ldt.ID)
					.Set(t => t.MoneyValue, () => 2000)
					.Set(t => t.SmallIntValue, () => 200)
					.Update()
					;

				var udt = db.Types.Single(t => t.ID == ldt.ID);

				Assert.That(udt.MoneyValue, Is.Not.EqualTo(ldt.MoneyValue));
				Assert.That(udt.SmallIntValue, Is.Not.EqualTo(ldt.SmallIntValue));
			}
		}

		[Test]
		public void UpdateByTableName([DataSources] string context)
		{
			const string? schemaName = null;
			var tableName  = InsertTests.GetTableName(context, "32");

			using (var db = GetDataContext(context))
			{
				db.DropTable<Person>(tableName, schemaName: schemaName, throwExceptionIfNotExists: false);
			}

			using (var db = GetDataContext(context))
			{
				var table = db.CreateTable<Person>(tableName, schemaName: schemaName);

				Assert.AreEqual(tableName,  table.TableName);
				Assert.AreEqual(schemaName, table.SchemaName);

				var person = new Person()
				{
					FirstName = "Steven",
					LastName  = "King",
					Gender    = Gender.Male,
				};

				// insert a row into the table
				db.Insert(person, tableName: tableName, schemaName: schemaName);
				var newCount  = table.Count();
				Assert.AreEqual(1, newCount);

				var personForUpdate = table.Single();

				// update that row
				personForUpdate.MiddleName = "None";
				db.Update(personForUpdate, tableName: tableName, schemaName: schemaName);

				var updatedPerson = table.Single();
				Assert.AreEqual("None", updatedPerson.MiddleName);

				if (db is DataConnection { Connection: FirebirdSql.Data.FirebirdClient.FbConnection })
					db.Close();

				table.Drop();
			}
		}

		[Test]
		public async Task UpdateByTableNameAsync([DataSources] string context)
		{
			const string? schemaName = null;
			var tableName  = InsertTests.GetTableName(context, "33");

			using (var db = GetDataContext(context))
			{
				await db.DropTableAsync<Person>(tableName, schemaName: schemaName, throwExceptionIfNotExists: false);
			}

			using (var db = GetDataContext(context))
			{
				var table = await db.CreateTableAsync<Person>(tableName, schemaName: schemaName);

				Assert.AreEqual(tableName,  table.TableName);
				Assert.AreEqual(schemaName, table.SchemaName);

				var person = new Person()
				{
					FirstName = "Steven",
					LastName  = "King",
					Gender    = Gender.Male,
				};

				// insert a row into the table
				await db.InsertAsync(person, tableName: tableName, schemaName: schemaName);
				var newCount  = await table.CountAsync();
				Assert.AreEqual(1, newCount);

				var personForUpdate = await table.SingleAsync();

				// update that row
				personForUpdate.MiddleName = "None";
				await db.UpdateAsync(personForUpdate, tableName: tableName, schemaName: schemaName);

				var updatedPerson = await table.SingleAsync();
				Assert.AreEqual("None", updatedPerson.MiddleName);

				//if (db is DataConnection { Connection: FirebirdSql.Data.FirebirdClient.FbConnection })
					await db.CloseAsync();

				await table.DropAsync();
			}
		}

		[Table("gt_s_one")]
		sealed class UpdateFromJoin
		{
			[PrimaryKey          ] public int     id   { get; set; }
			[Column(Length = 100)] public string? col1 { get; set; }
			[Column(Length = 100)] public string? col2 { get; set; }
			[Column(Length = 100)] public string? col3 { get; set; }
			[Column(Length = 100)] public string? col4 { get; set; }
			[Column(Length = 100)] public string? col5 { get; set; }
			[Column(Length = 100)] public string? col6 { get; set; }

			public static UpdateFromJoin[] Data = Array<UpdateFromJoin>.Empty;
		}

		[Table("access_mode")]
		sealed class AccessMode
		{
			[PrimaryKey]
			public int id { get; set; }

			[Column]
			public string? code { get; set; }

			public static AccessMode[] Data = Array<AccessMode>.Empty;
		}

		// https://stackoverflow.com/questions/57115728/
		[Test]
		public void TestUpdateFromJoin([DataSources(
			TestProvName.AllAccess, // access doesn't have Replace mapping
			TestProvName.AllClickHouse,
			ProviderName.SqlCe,
			TestProvName.AllInformix)] string context)
		{
			using (var db          = GetDataContext(context))
			using (var gt_s_one    = db.CreateLocalTable(UpdateFromJoin.Data))
			using (var access_mode = db.CreateLocalTable(AccessMode.Data))
			{
#pragma warning disable CA1311 // Specify a culture or use an invariant version
				gt_s_one
					.GroupJoin(
						access_mode,
						l => l.col3!.Replace("auth.", "").ToUpper(),
						am => am.code!.ToUpper(),
						(l, am) => new
						{
							l,
							am
						})
					.SelectMany(
						x => x.am.DefaultIfEmpty(),
						(x1, y1) => new
						{
							gt    = x1.l,
							theAM = y1!.id
						})
					.Update(
						gt_s_one,
						s => new UpdateFromJoin()
						{
							col1 = s.gt.col1,
							col2 = s.gt.col2,
							col3 = s.gt.col3!.Replace("auth.", ""),
							col4 = s.gt.col4,
							col5 = s.gt.col3 == "empty" ? "1" : "0",
							col6 = s.gt.col3 == "empty" ? "" : s.theAM.ToString()
						});
#pragma warning restore CA1311 // Specify a culture or use an invariant version
			}
		}
		enum UpdateSetEnum
		{
			Value1 = 6,
			Value2 = 7,
			Value3 = 8
		}
		[Table]
		sealed class UpdateSetTest
		{
			[PrimaryKey] public int            Id     { get; set; }
			[Column]     public Guid           Value1 { get; set; }
			[Column]     public int            Value2 { get; set; }
			[Column]     public UpdateSetEnum  Value3 { get; set; }
			[Column]     public Guid?          Value4 { get; set; }
			[Column]     public int?           Value5 { get; set; }
			[Column]     public UpdateSetEnum? Value6 { get; set; }

			public static UpdateSetTest[] Data = new UpdateSetTest[]
			{
				new UpdateSetTest() { Id = 1, Value1 = TestData.Guid3, Value2 = 10, Value3 = UpdateSetEnum.Value1 }
			};
		}

		[Test]
		public void TestSetValueCaching1(
			[DataSources(
			TestProvName.AllSybase,
			TestProvName.AllMySql,
			TestProvName.AllFirebird,
			TestProvName.AllInformix,
			ProviderName.DB2,
			ProviderName.SqlCe)] string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(UpdateSetTest.Data))
			{
				var id = 1;
				var value = TestData.Guid1;

				table.Where(_ => _.Id == id)
					.Set(_ => _.Value1, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value1).Single());

				value = TestData.Guid2;
				table.Where(_ => _.Id == id)
					.Set(_ => _.Value1, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value1).Single());
			}
		}

		[Test]
		public void TestSetValueCaching2(
			[DataSources(
			TestProvName.AllSybase,
			TestProvName.AllMySql,
			TestProvName.AllFirebird,
			TestProvName.AllInformix,
			ProviderName.DB2,
			ProviderName.SqlCe)] string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(UpdateSetTest.Data))
			{
				var id = 1;
				var value = 11;

				table.Where(_ => _.Id == id)
					.Set(_ => _.Value2, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value2).Single());

				value = 12;
				table.Where(_ => _.Id == id)
					.Set(_ => _.Value2, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value2).Single());
			}
		}

		[Test]
		public void TestSetValueCaching3(
			[DataSources(
			TestProvName.AllSybase,
			TestProvName.AllMySql,
			TestProvName.AllFirebird,
			TestProvName.AllInformix,
			ProviderName.DB2,
			ProviderName.SqlCe)] string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(UpdateSetTest.Data))
			{
				var id = 1;
				var value = UpdateSetEnum.Value2;

				table.Where(_ => _.Id == id)
					.Set(_ => _.Value3, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value3).Single());

				value = UpdateSetEnum.Value3;
				table.Where(_ => _.Id == id)
					.Set(_ => _.Value3, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value3).Single());
			}
		}

		[Test]
		public void TestSetValueCaching4(
			[DataSources(
			TestProvName.AllSybase,
			TestProvName.AllMySql,
			TestProvName.AllFirebird,
			TestProvName.AllInformix,
			ProviderName.DB2,
			ProviderName.SqlCe)] string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(UpdateSetTest.Data))
			{
				var id = 1;
				var value = TestData.Guid1;

				table.Where(_ => _.Id == id)
					.Set(_ => _.Value4, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value4).Single());

				value = TestData.Guid2;
				table.Where(_ => _.Id == id)
					.Set(_ => _.Value4, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value4).Single());
			}
		}

		[Test]
		public void TestSetValueCaching5(
			[DataSources(
			TestProvName.AllSybase,
			TestProvName.AllMySql,
			TestProvName.AllFirebird,
			TestProvName.AllInformix,
			ProviderName.DB2,
			ProviderName.SqlCe)] string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(UpdateSetTest.Data))
			{
				var id = 1;
				var value = 11;

				table.Where(_ => _.Id == id)
					.Set(_ => _.Value5, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value5).Single());

				value = 12;
				table.Where(_ => _.Id == id)
					.Set(_ => _.Value5, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value5).Single());
			}
		}

		[Test]
		public void TestSetValueCaching6(
			[DataSources(
			TestProvName.AllSybase,
			TestProvName.AllMySql,
			TestProvName.AllFirebird,
			TestProvName.AllInformix,
			ProviderName.DB2,
			ProviderName.SqlCe)] string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(UpdateSetTest.Data))
			{
				var id = 1;
				var value = UpdateSetEnum.Value2;

				table.Where(_ => _.Id == id)
					.Set(_ => _.Value6, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value6).Single());

				value = UpdateSetEnum.Value3;
				table.Where(_ => _.Id == id)
					.Set(_ => _.Value6, value)
					.Update();

				Assert.AreEqual(value, table.Where(_ => _.Id == id).Select(_ => _.Value6).Single());
			}
		}


		sealed class TextData
		{
			[Column]
			public int Id { get; set; }

			[Column(Length = int.MaxValue)]
			public string? Items1 { get; set; }

			[Column(Length = int.MaxValue)]
			public string? Items2 { get; set; }
		}


		[Test]
		public void TestSetValueExpr(
			[IncludeDataSources(TestProvName.AllSqlServer2008Plus)] string context, [Values("zz", "yy")] string str)
		{
			var data = new[]
			{
				new TextData { Id = 1, Items1 = "T1", Items2 = "Z1" },
				new TextData { Id = 2, Items1 = "T2", Items2 = "Z2" },
			};

			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(data))
			{
				var id = 1;

				table.Where(_ => _.Id >= id)
					.Set(x => $"{x.Items1} += {str}")
					.Set(x => $"{x.Items2} += {str}")
					//.Set(x => $"{x.Items}.WRITE({item}, {2}, {2})")
					.Update();

				var result = table.ToArray();

				Assert.That(result[0].Items1, Is.EqualTo("T1" + str));
				Assert.That(result[0].Items2, Is.EqualTo("Z1" + str));

				Assert.That(result[1].Items1, Is.EqualTo("T2" + str));
				Assert.That(result[1].Items2, Is.EqualTo("Z2" + str));

			}
		}

		[Test]
		public void TestSetValueExpr2(
			[IncludeDataSources(TestProvName.AllSqlServer2008Plus, TestProvName.AllClickHouse)] string context, [Values("zz", "yy")] string str)
		{
			var data = new[]
			{
				new TextData { Id = 1, Items1 = "T1", Items2 = "Z1" },
				new TextData { Id = 2, Items1 = "T2", Items2 = "Z2" },
			};

			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable(data))
			{
				var id = 1;

				table.Where(_ => _.Id >= id)
					.Set(x => x.Items1, x => $"{x.Items1}{str}")
					.Set(x => x.Items2, x => $"{x.Items2}{str}")
					.Update();

				var result = table.OrderBy(_ => _.Id).ToArray();

				Assert.That(result[0].Items1, Is.EqualTo("T1" + str));
				Assert.That(result[0].Items2, Is.EqualTo("Z1" + str));

				Assert.That(result[1].Items1, Is.EqualTo("T2" + str));
				Assert.That(result[1].Items2, Is.EqualTo("Z2" + str));

			}
		}

		[Table]
		sealed class MainTable
		{
			[Column] public int Id;
			[Column] public string? Field;

			[Association(ThisKey = nameof(Id), OtherKey = nameof(AssociatedTable.Id))]
			public AssociatedTable AssociatedOptional = null!;

			[Association(ThisKey = nameof(Id), OtherKey = nameof(AssociatedTable.Id), CanBeNull = false)]
			public AssociatedTable AssociatedRequired = null!;

			public static readonly MainTable[] Data = new []
			{
				new MainTable() { Id = 1, Field = "value 1" },
				new MainTable() { Id = 2, Field = "value 2" },
				new MainTable() { Id = 3, Field = "value 3" },
			};
		}

		[Table]
		sealed class AssociatedTable
		{
			[Column] public int Id;

			[Association(ThisKey = nameof(Id), OtherKey = nameof(MainTable.Id))]
			public MainTable MainOptional = null!;

			[Association(ThisKey = nameof(Id), OtherKey = nameof(MainTable.Id), CanBeNull = false)]
			public MainTable MainRequired = null!;

			public static readonly AssociatedTable[] Data = new []
			{
				new AssociatedTable() { Id = 1 },
				new AssociatedTable() { Id = 3 },
			};
		}

		[Test]
		public void UpdateByAssociationOptional([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db   = GetDataContext(context))
			using (var main = db.CreateLocalTable(MainTable.Data))
			using (db.CreateLocalTable(AssociatedTable.Data))
			{
				var id = 3;
					var cnt = main
						.Where(_ => _.Id == id)
						.Select(_ => _.AssociatedOptional!.MainOptional)
						.Update(p => new MainTable()
						{
							Field = "test"
						});

				var data = main.OrderBy(_ => _.Id).ToArray();

				Assert.AreEqual(1, cnt);
				Assert.AreEqual("value 1", data[0].Field);
				Assert.AreEqual("value 2", data[1].Field);
				Assert.AreEqual("test", data[2].Field);
			}
		}

		[Test]
		public void UpdateByAssociationRequired([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (var main = db.CreateLocalTable(MainTable.Data))
			using (db.CreateLocalTable(AssociatedTable.Data))
			{
				var id = 3;
				var cnt = main
						.Where(_ => _.Id == id)
						.Select(_ => _.AssociatedRequired!.MainRequired)
						.Update(p => new MainTable()
						{
							Field = "test"
						});

				var data = main.OrderBy(_ => _.Id).ToArray();

				Assert.AreEqual(1, cnt);
				Assert.AreEqual("value 1", data[0].Field);
				Assert.AreEqual("value 2", data[1].Field);
				Assert.AreEqual("test", data[2].Field);
			}
		}

		[Test]
		public void UpdateByAssociation2Optional([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db         = GetDataContext(context))
			using (var main       = db.CreateLocalTable(MainTable.Data))
			using (var associated = db.CreateLocalTable(AssociatedTable.Data))
			{
				var id = 3;
				var cnt = associated
					.Where(pat => pat.Id == id)
					.Select(p => p.MainOptional)
					.Update(p => new MainTable()
					{
						Field = "test"
					});

				var data = main.OrderBy(_ => _.Id).ToArray();

				Assert.AreEqual(1, cnt);
				Assert.AreEqual("value 1", data[0].Field);
				Assert.AreEqual("value 2", data[1].Field);
				Assert.AreEqual("test", data[2].Field);
			}
		}

		[Test]
		public void UpdateByAssociation2Required([DataSources(TestProvName.AllInformix, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			using (var main = db.CreateLocalTable(MainTable.Data))
			using (var associated = db.CreateLocalTable(AssociatedTable.Data))
			{
				var id = 3;
				var cnt = associated
					.Where(pat => pat.Id == id)
					.Select(p => p.MainRequired)
					.Update(p => new MainTable()
					{
						Field = "test"
					});

				var data = main.OrderBy(_ => _.Id).ToArray();

				Assert.AreEqual(1, cnt);
				Assert.AreEqual("value 1", data[0].Field);
				Assert.AreEqual("value 2", data[1].Field);
				Assert.AreEqual("test", data[2].Field);
			}
		}

		[Test]
		public void AsUpdatableEmptyTest([IncludeDataSources(true, TestProvName.AllSQLite, TestProvName.AllClickHouse)] string context)
		{
			using (var db = GetDataContext(context))
			{
				var query = db.Person.AsUpdatable();

				var ex = Assert.Throws<LinqToDBException>(() => query.Update())!;
				Assert.AreEqual("Update query has no setters defined.", ex.Message);
			}
		}
	}
}
