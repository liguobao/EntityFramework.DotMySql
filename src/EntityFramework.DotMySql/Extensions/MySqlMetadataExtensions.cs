using EntityFramework.DotMySql.Metadata;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Internal;
using Microsoft.Data.Entity.Utilities;

// ReSharper disable once CheckNamespace

namespace Microsoft.Data.Entity
{
    public static class MySqlMetadataExtensions
    {
        public static IRelationalEntityTypeAnnotations MySql([NotNull] this IEntityType entityType)
            => new RelationalEntityTypeAnnotations(Check.NotNull(entityType, nameof(entityType)), MySqlAnnotationNames.Prefix);

        public static RelationalEntityTypeAnnotations MySql([NotNull] this IMutableEntityType entityType)
            => (RelationalEntityTypeAnnotations)MySql((IEntityType)entityType);

        public static RelationalEntityTypeAnnotations MySql([NotNull] this EntityType entityType)
            => (RelationalEntityTypeAnnotations)MySql((IEntityType)entityType);

        public static IRelationalForeignKeyAnnotations MySql([NotNull] this IForeignKey foreignKey)
            => new RelationalForeignKeyAnnotations(Check.NotNull(foreignKey, nameof(foreignKey)), MySqlAnnotationNames.Prefix);

        public static RelationalForeignKeyAnnotations MySql([NotNull] this ForeignKey foreignKey)
            => (RelationalForeignKeyAnnotations)MySql((IForeignKey)foreignKey);

        public static MySqlIndexAnnotations MySql([NotNull] this IIndex index)
            => new MySqlIndexAnnotations(Check.NotNull(index, nameof(index)));

        public static RelationalIndexAnnotations MySql([NotNull] this Index index)
            => MySql((IIndex)index);

        public static IRelationalKeyAnnotations MySql([NotNull] this IKey key)
            => new RelationalKeyAnnotations(Check.NotNull(key, nameof(key)), MySqlAnnotationNames.Prefix);

        public static RelationalKeyAnnotations MySql([NotNull] this Key key)
            => (RelationalKeyAnnotations)MySql((IKey)key);

        public static MySqlModelAnnotations MySql([NotNull] this Model model)
            => (MySqlModelAnnotations)MySql((IModel)model);

        public static IMySqlModelAnnotations MySql([NotNull] this IModel model)
            => new MySqlModelAnnotations(Check.NotNull(model, nameof(model)));

        public static MySqlPropertyAnnotations MySql([NotNull] this IMutableProperty property)
            => (MySqlPropertyAnnotations)MySql((IProperty)property);

        public static IMySqlPropertyAnnotations MySql([NotNull] this IProperty property)
            => new MySqlPropertyAnnotations(Check.NotNull(property, nameof(property)), MySqlAnnotationNames.Prefix);

        public static MySqlPropertyAnnotations MySql([NotNull] this Property property)
            => (MySqlPropertyAnnotations)MySql((IProperty)property);


    }
}
