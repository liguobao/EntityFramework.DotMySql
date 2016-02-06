// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Data.Entity.FunctionalTests;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Infrastructure.Internal;
using Microsoft.Data.Entity.Internal;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Conventions.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Data.Entity.SqlServer.FunctionalTests
{
    public class TestMySqlModelSource : MySqlModelSource
    {
        private readonly TestModelSource _testModelSource;

        public TestMySqlModelSource(
            Action<ModelBuilder> onModelCreating,
            IDbSetFinder setFinder,
            ICoreConventionSetBuilder coreConventionSetBuilder,
            IModelCustomizer modelCustomizer,
            IModelCacheKeyFactory modelCacheKeyFactory)
            : base(setFinder,  coreConventionSetBuilder, modelCustomizer, modelCacheKeyFactory)
        {
            _testModelSource = new TestModelSource(onModelCreating, setFinder, coreConventionSetBuilder, modelCustomizer, modelCacheKeyFactory);
        }

        public override IModel GetModel(DbContext context, IConventionSetBuilder conventionSetBuilder, IModelValidator validator) 
            => _testModelSource.GetModel(context, conventionSetBuilder, validator);

        public static Func<IServiceProvider, MySqlModelSource> GetFactory(Action<ModelBuilder> onModelCreating) 
            => p => new TestMySqlModelSource(
                onModelCreating,
                p.GetRequiredService<IDbSetFinder>(),
                p.GetRequiredService<ICoreConventionSetBuilder>(),
                p.GetRequiredService<IModelCustomizer>(),
                p.GetRequiredService<IModelCacheKeyFactory>());
    }
}
