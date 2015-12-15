using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Internal;

namespace EntityFramework.DotMySql.Metadata
{
    public class MySqlPropertyAnnotations : RelationalPropertyAnnotations, IMySqlPropertyAnnotations
    {
        public MySqlPropertyAnnotations([NotNull] IProperty property, [CanBeNull] string providerPrefix) : base(property, providerPrefix)
        {
        }

        public MySqlPropertyAnnotations([NotNull] RelationalAnnotations annotations) : base(annotations)
        {
        }

        public virtual MySqlValueGenerationStrategy? ValueGenerationStrategy
        {
            get
            {
                if ((Property.ValueGenerated != ValueGenerated.OnAdd)
                    || !Property.ClrType.UnwrapNullableType().IsInteger()
                    || (Property.MySql().GeneratedValueSql != null))
                {
                    return null;
                }

                var value = (MySqlValueGenerationStrategy?)Annotations.GetAnnotation(MySqlAnnotationNames.ValueGenerationStrategy);

                return value ?? Property.DeclaringEntityType.Model.MySql().ValueGenerationStrategy;
            }
            [param: CanBeNull] set { SetValueGenerationStrategy(value); }
        }

        protected virtual bool SetValueGenerationStrategy(MySqlValueGenerationStrategy? value)
        {
            if (value != null)
            {
                var propertyType = Property.ClrType;

                if ((value == MySqlValueGenerationStrategy.AutoIncrement)
                    && (!propertyType.IsInteger()
                        || (propertyType == typeof(byte))
                        || (propertyType == typeof(byte?))))
                {
                    throw new ArgumentException("Bad identity type");
                        //Property.Name, Property.DeclaringEntityType.Name, propertyType.Name));
                }
            }

            return Annotations.SetAnnotation(MySqlAnnotationNames.ValueGenerationStrategy, value);
        }
    }
}
