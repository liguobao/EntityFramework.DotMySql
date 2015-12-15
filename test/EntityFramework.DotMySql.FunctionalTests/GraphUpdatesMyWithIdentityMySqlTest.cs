// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using EntityFramework.DotMySql.Extensions;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class GraphUpdatesMyWithIdentityMySqlTest : GraphUpdatesMySqlTestBase<GraphUpdatesMyWithIdentityMySqlTest.GraphUpdatesMyWithIdentitySqlFixture>
    {
        public GraphUpdatesMyWithIdentityMySqlTest(GraphUpdatesMyWithIdentitySqlFixture fixture)
            : base(fixture)
        {
        }

        public class GraphUpdatesMyWithIdentitySqlFixture : GraphUpdatesMySqlFixtureBase
        {
            protected override string DatabaseName => "GraphIdentityUpdatesTest";

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ForMySqlUseIdentityColumns(); // ensure model uses identity

                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
