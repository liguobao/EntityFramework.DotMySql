// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Data.Entity.Query.ExpressionTranslators.Internal
{
    public class MySqlCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        private static readonly IMethodCallTranslator[] _methodCallTranslators =
        {
            new MySqlStringSubstringTranslator(),
            new MySqlMathAbsTranslator(),
            new MySqlMathCeilingTranslator(),
            new MySqlMathFloorTranslator(),
            new MySqlMathPowerTranslator(),
            new MySqlMathRoundTranslator(),
            new MySqlMathTruncateTranslator(),
            new MySqlStringReplaceTranslator(),
            new MySqlStringToLowerTranslator(),
            new MySqlStringToUpperTranslator(),
            new MySqlRegexIsMatchTranslator(),
        };

        public MySqlCompositeMethodCallTranslator([NotNull] ILogger<MySqlCompositeMethodCallTranslator> logger)
            : base(logger)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            AddTranslators(_methodCallTranslators);
        }
    }
}
