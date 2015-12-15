using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Internal;

namespace Microsoft.Data.Entity.Metadata.Internal
{
    public class MySqlModelBuilderAnnotations : MySqlModelAnnotations
    {
        public MySqlModelBuilderAnnotations(
            [NotNull] InternalModelBuilder internalBuilder,
            ConfigurationSource configurationSource)
            : base(new RelationalAnnotationsBuilder(internalBuilder, configurationSource, MySqlAnnotationNames.Prefix))
        {
        }

        public new virtual bool ValueGenerationStrategy(MySqlValueGenerationStrategy? value) => SetValueGenerationStrategy(value);
    }
}
