using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata;

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

        
    }
}
