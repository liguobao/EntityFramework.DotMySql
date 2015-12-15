using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Internal;

namespace Microsoft.Data.Entity.Metadata
{
    public class MySqlModelAnnotations : RelationalModelAnnotations, IMySqlModelAnnotations
    {
        public MySqlModelAnnotations([NotNull] IModel model) 
            : base (model, MySqlAnnotationNames.Prefix)
        {
            
        }

        protected MySqlModelAnnotations([NotNull] RelationalAnnotations annotations)
            : base(annotations)
        {
        }

        public virtual MySqlValueGenerationStrategy? ValueGenerationStrategy
        {
            get { return (MySqlValueGenerationStrategy?)Annotations.GetAnnotation(MySqlAnnotationNames.ValueGenerationStrategy); }
            set { SetValueGenerationStrategy(value); }
        }

        protected virtual bool SetValueGenerationStrategy(MySqlValueGenerationStrategy? value)
            => Annotations.SetAnnotation(MySqlAnnotationNames.ValueGenerationStrategy, value);
    }
}
