﻿using JetBrains.Annotations;

namespace LinqToDB.DataProvider.SqlCe;

using System.Collections.Generic;
using Configuration;

[UsedImplicitly]
class SqlCeFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return SqlCeTools.GetDataProvider();
	}
}
