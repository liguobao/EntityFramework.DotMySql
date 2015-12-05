using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata.Conventions.Internal;
using Microsoft.Data.Entity.Metadata.Internal;

namespace EntityFramework.DotMySql.Metadata.Conventions.Internal
{
    public class MySqlValueGenerationStrategyConvention : IModelConvention
    {
        public InternalModelBuilder Apply(InternalModelBuilder modelBuilder)
        {
            //modelBuilder.MySql(ConfigurationSource.Convention)
            return modelBuilder;
            
        }
    }
}
