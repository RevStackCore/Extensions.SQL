using System;
using System.Collections.Generic;
using System.Reflection;
using RevStackCore.DataAnnotations;

namespace RevStackCore.Extensions.SQL
{
    public static class MetadataExtensions
    {
        public static MetadataSqlSchema ToMetadataSqlSchema(this Type type)
        {
            var metadataSchema = new MetadataSqlSchema();
            var columns = new List<SqlDataColumn>();
            metadataSchema.TableName = type.Name;
            var tableAttribute = type.GetCustomAttribute<TableAttribute>(true);

            if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name))
            {
                metadataSchema.TableName = tableAttribute.Name;
            }

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var column = new SqlDataColumn();
                column.Name = property.Name;
                column.Type = property.PropertyType;
                var autoIdAttribute = property.GetCustomAttribute<AutoIdAttribute>(false);
                var autoIncrementAttribute = property.GetCustomAttribute<AutoIncrementAttribute>(false);
                var notNullAttribute = property.GetCustomAttribute<NotNullAttribute>(false);
                var primaryKeyAttribute = property.GetCustomAttribute<PrimaryKeyAttribute>(false);
                var sqlTypeAttribute = property.GetCustomAttribute<SqlTypeAttribute>(false);
                var indexAttribute = property.GetCustomAttribute<IndexAttribute>(false);
                var ignoreAttribute = property.GetCustomAttribute<IgnoreAttribute>(false);
                if (autoIdAttribute != null || autoIncrementAttribute != null)
                {
                    column.IsAutoIncrementing = true;
                }
                else if (sqlTypeAttribute != null)
                {
                    column.DbType = sqlTypeAttribute.Type;
                    column.Size = sqlTypeAttribute.Size;
                    column.Precision = sqlTypeAttribute.Precision;
                }
                if (primaryKeyAttribute != null)
                {
                    column.IsPrimaryKey = true;
                    column.AllowNulls = false;
                }
                if(notNullAttribute !=null)
                {
                    column.AllowNulls = false;
                }
                if (indexAttribute != null)
                {
                    column.IsIndex = true;
                    if (indexAttribute.Unique == true)
                    {
                        column.IsUniqueIndex = true;
                    }
                    if (indexAttribute.Clustered == true)
                    {
                        column.IsClusteredIndex = true;
                    }
                }
                if(ignoreAttribute==null)
                {
                    columns.Add(column);
                }
            }

            metadataSchema.Columns = columns;

            return metadataSchema;
        }
    }
}
