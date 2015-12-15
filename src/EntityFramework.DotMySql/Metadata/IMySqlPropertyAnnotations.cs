// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using EntityFramework.DotMySql.Metadata;

namespace Microsoft.Data.Entity.Metadata
{
    public interface IMySqlPropertyAnnotations : IRelationalPropertyAnnotations
    {

        MySqlValueGenerationStrategy? ValueGenerationStrategy { get; }
    }
}
