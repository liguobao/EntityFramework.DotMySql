// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Microsoft.Data.Entity.Query.ExpressionTranslators.Internal
{
    public class MySqlCompositeMemberTranslator : RelationalCompositeMemberTranslator
    {
        public MySqlCompositeMemberTranslator()
        {
            var MySqlTranslators = new List<IMemberTranslator>
            {
                new MySqlStringLengthTranslator(),
                new MySqlDateTimeNowTranslator()
            };

            AddTranslators(MySqlTranslators);
        }
    }
}
