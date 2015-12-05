// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// ReSharper disable once CheckNamespace

using EntityFramework.DotMySql.Metadata.Conventions.Internal;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Storage;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity.Metadata.Conventions.Internal
{
    public class MySqlConventionSetBuilder : RelationalConventionSetBuilder
    {
        public MySqlConventionSetBuilder([NotNull] IRelationalTypeMapper typeMapper)
            : base(typeMapper)
        {
        }


        public override ConventionSet AddConventions(ConventionSet conventionSet)
        {
            Check.NotNull(conventionSet, nameof(conventionSet));

            base.AddConventions(conventionSet);

            conventionSet.ModelInitializedConventions.Add(new MySqlValueGenerationStrategyConvention());

            return conventionSet;
        }

        // TODO: SqlServer has identity here, do we need something?
    }
}
