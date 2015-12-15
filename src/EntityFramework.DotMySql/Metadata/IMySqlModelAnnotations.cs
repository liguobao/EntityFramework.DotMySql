using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Metadata;

namespace Microsoft.Data.Entity.Metadata
{
    public interface IMySqlModelAnnotations : IRelationalModelAnnotations
    {
        MySqlValueGenerationStrategy? ValueGenerationStrategy { get; }
    }
}
