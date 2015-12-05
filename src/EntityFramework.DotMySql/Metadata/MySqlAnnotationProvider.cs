// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Entity.Metadata
{
    public class MySqlAnnotationProvider : IRelationalAnnotationProvider
    {
        public virtual IRelationalEntityTypeAnnotations For(IEntityType entityType) => entityType.MySql();
        public virtual IRelationalForeignKeyAnnotations For(IForeignKey foreignKey) => foreignKey.MySql();
        public virtual IRelationalIndexAnnotations For(IIndex index) => index.MySql();
        public virtual IRelationalKeyAnnotations For(IKey key) => key.MySql();
        public virtual IRelationalModelAnnotations For(IModel model) => model.MySql();
        public virtual IRelationalPropertyAnnotations For(IProperty property) => property.MySql();
    }
}
