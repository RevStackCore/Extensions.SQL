using System;
using System.Collections.Generic;
using System.Data;

namespace RevStackCore.Extensions.SQL
{
    public class SqlUtility<TConnection, TCommand>
        where TConnection : class, IDbConnection, new()
        where TCommand : class, IDbCommand, new()
    {
        private string _connectionString;
        public SqlUtility(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool TableExists(string tableName)
        {
            var connection = new TConnection();
            connection.ConnectionString = _connectionString;
            bool exists = true;
            using (IDbConnection db = connection)
            {
                db.Open();
                try
                {
                    // ANSI SQL way.  Works in PostgreSQL, MSSQL, MySQL
                    var cmd = new TCommand();
                    cmd.Connection = db;
                    cmd.CommandText = "select case when exists((select * from information_schema.tables where table_name = '" + tableName + "')) then 1 else 0 end";
                    exists = (int)cmd.ExecuteScalar() == 1;
                }
                catch
                {
                    try
                    {
                        var cmd = new TCommand();
                        cmd.Connection = db;
                        cmd.CommandText = "select 1 from " + tableName + " where 1 = 0";
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        exists = false;
                    }
                }

                db.Close();
            }
            return exists;
        }
    

        public void ExecuteSqlCommand(string commandText)
        {
            var connection = new TConnection();
            connection.ConnectionString = _connectionString;
            using (IDbConnection db = connection)
            {
                db.Open();
                var cmd = new TCommand();
                cmd.Connection = db;
                cmd.CommandText = commandText;
                cmd.ExecuteNonQuery();
          
                db.Close();
            }
        }
    }

}
