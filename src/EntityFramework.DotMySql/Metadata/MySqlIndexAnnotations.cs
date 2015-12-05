using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata.Internal;

namespace Microsoft.Data.Entity.Metadata
{
    public class MySqlIndexAnnotations : RelationalIndexAnnotations
    {
        public MySqlIndexAnnotations([NotNull] IIndex index)
            : base(index, MySqlAnnotationNames.Prefix)
        {
        }

        protected MySqlIndexAnnotations([NotNull] RelationalAnnotations annotations)
            : base(annotations)
        {
        }

        /// <summary>
        /// The PostgreSQL index method to be used. Null selects the default (currently btree).
        /// </summary>
        /// <remarks>
        /// http://www.postgresql.org/docs/current/static/sql-createindex.html
        /// </remarks>
        public string Method
        {
            get { return (string) Annotations.GetAnnotation(MySqlAnnotationNames.IndexMethod); }
            set { Annotations.SetAnnotation(MySqlAnnotationNames.IndexMethod, value); }
        }
    }
}
