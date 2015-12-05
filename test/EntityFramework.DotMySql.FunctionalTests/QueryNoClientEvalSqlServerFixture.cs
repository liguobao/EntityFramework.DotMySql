﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Data.Entity.Infrastructure;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class QueryNoClientEvalSqlServerFixture : NorthwindQueryMySqlFixture
    {
        protected override void ConfigureOptions(MySqlDbContextOptionsBuilder MySqlDbContextOptionsBuilder)
            => MySqlDbContextOptionsBuilder.QueryClientEvaluationBehavior(QueryClientEvaluationBehavior.Throw);
    }
}
