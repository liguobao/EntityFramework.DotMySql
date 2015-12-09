// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Utilities;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Microsoft.Data.Entity.Internal
{
    public class MySqlOptionsExtension : RelationalOptionsExtension
    {
        public MySqlOptionsExtension()
        {
            this.MaxBatchSize = 1;
        }

        public MySqlOptionsExtension([NotNull] MySqlOptionsExtension copyFrom)
            : base(copyFrom)
        {
        }

        public override void ApplyServices(EntityFrameworkServicesBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.AddMySql();
        }
    }
}
