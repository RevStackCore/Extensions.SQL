using System;
using System.Collections.Generic;
using System.Data;
namespace RevStackCore.Extensions.SQL
{
    public class MetadataSqlSchema
    {
        public string TableName { get; set; }
        public IEnumerable<SqlDataColumn> Columns { get; set; }
    }

    public class SqlDataColumn
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public SqlDbType DbType { get; set; }
        public string TypeName { get; set; }
        public string DbTypeName { get; set; }
        public bool AllowNulls { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsAutoIncrementing { get; set; }
        public bool IsIndex { get; set; }
        public bool IsUniqueIndex { get; set; }
        public bool IsClusteredIndex { get; set; }
        public int? Size { get; set; }
        public int? Precision { get; set; }
        public SqlDataColumn()
        {
            AllowNulls = true;
            IsPrimaryKey = false;
        }
    }
}
