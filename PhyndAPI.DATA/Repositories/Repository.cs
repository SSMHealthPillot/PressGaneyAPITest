using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using PhyndAPI.DATA.Interfaces;
using System.Data.SqlClient;
using System.Configuration;

namespace PhyndAPI.DATA.Repositories
{
    public class Repository : IRepository
    {
        #region Fields
        private readonly string Connection = null;

        #endregion

        #region Constructors
        public Repository(string conn)
        {
            Connection = conn;

        }
        #endregion

        #region IRepository
        /// <summary>
        /// Returns a list of the generic type class <T> when no parameters are required.
        /// </summary>
        /// <typeparam name="T">Generic type being sent up.  Typically a class</typeparam>
        /// <param name="query">The name of a stored procedure on the database server</param>
        /// <returns></returns>
        public ICollection<T> GetList<T>(string query)
        {
            ICollection<T> dataList = null;

            using (IDbConnection db = CreateConnection())
            {
                DatabaseConnectionState(db);
                dataList = db.Query<T>(query, commandType: CommandType.StoredProcedure).AsList();
            }

            return dataList;
        }
        /// <summary>
        /// Returns a list of the generic type class <T>
        /// </summary>
        /// <typeparam name="T">Generic type being sent up.  Typically a class</typeparam>
        /// <param name="query">The name of a stored procedure on the database server</param>
        /// <param name="parameters">DynamicParameters required for the stored procedure</param>
        /// <returns></returns>
        public ICollection<T> GetList<T>(string query, DynamicParameters parameters)
        {
            ICollection<T> dataList = null;

            using (IDbConnection db = CreateConnection())
            {
                DatabaseConnectionState(db);
                dataList = db.Query<T>(query, param: parameters, commandType: CommandType.StoredProcedure).AsList();
            }

            return dataList;
        }

        /// <summary>
        /// Uses a stored procedure when returning a single record from a database table
        /// </summary>
        /// <typeparam name="T">Generic type being sent up.  Typically a class</typeparam>
        /// <param name="query">The name of a stored procedure on the database server</param>
        /// <param name="parameters">DynamicParameters required for the stored procedure</param>
        /// <returns></returns>
        public T GetSingle<T>(string query, DynamicParameters parameters)
        {
            T data;

            using (IDbConnection db = CreateConnection())
            {
                DatabaseConnectionState(db);
                data = db.Query<T>(query, param: parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }

            return data;
        }

        /// <summary>
        /// Uses a stored procedure to insert data into the databas. 
        /// Expects a return value from the SQL Database after an insert.
        /// </summary>
        /// <typeparam name="T">Generic type being sent up.  Typically a class</typeparam>
        /// <param name="query">The name of a stored procedure on the database server</param>
        /// <param name="parameters">DynamicParameters required for the stored procedure</param>
        /// <returns></returns>
        public bool Add<T>(string query, DynamicParameters parameters)
        {
            bool result = false;

            using (IDbConnection db = CreateConnection())
            {
                DatabaseConnectionState(db);
                var tResult = db.Query(query, param: parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                result = true;
            }

            return result;
        }
        /// <summary>
        /// Used to Update data in a database table
        /// </summary>
        /// <typeparam name="T">Generic type being sent up.  Typically a class</typeparam>
        /// <param name="query">The name of a stored procedure on the database server</param>
        /// <param name="parameters">DynamicParameters required for the stored procedure</param>
        /// <returns></returns>
        public bool Update<T>(string query, DynamicParameters parameters)
        {
            bool result;

            using (IDbConnection db = CreateConnection())
            {
                DatabaseConnectionState(db);
                var tResult = db.Query(query, param: parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                if (tResult != null)
                    result = tResult;
                else
                    result = false;
            }

            return result;
        }
        /// <summary>
        /// Generics class that makes use of a generic type when returning a data list.
        /// </summary>
        /// <typeparam name="T">Type, ie: string, int, list<class>, etc.</typeparam>
        /// <param name="query">The SQL Query string.</param>
        /// <returns></returns>
        public ICollection<T> GetListByQuery<T>(string query)
        {
            ICollection<T> dataList = null;

            using (IDbConnection db = CreateConnection())
            {
                DatabaseConnectionState(db);
                dataList = db.Query<T>(query, commandType: CommandType.Text).AsList();
            }

            return dataList;
        }
        /// <summary>
        /// Generics class that makes use of a generic type when returning a data list.
        /// </summary>
        /// <typeparam name="T">Type, ie: string, int, list<class>, etc.</typeparam>
        /// <param name="query">The SQL Query string.</param>
        /// <returns></returns>
        public ICollection<T> GetListByQuery<T>(string query, DynamicParameters parameters)
        {
            ICollection<T> dataList = null;

            using (IDbConnection db = CreateConnection())
            {
                DatabaseConnectionState(db);
                dataList = db.Query<T>(query, param: parameters, commandType: CommandType.Text).AsList();
            }

            return dataList;
        }
        /// <summary>
        /// Used to open the database connection
        /// </summary>
        /// <param name="conn">IDbConnection we created based on our db configuration.</param>
        public void DatabaseConnectionState(IDbConnection conn)
        {
            if (conn != null && conn.State.Equals(ConnectionState.Closed))
            {
                conn.Open();
            }
        }
        /// <summary>
        /// This is a private method that determines whether or not  we are using the SQLConnection
        /// Or the OracleConnection
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {

           return new SqlConnection(ConfigurationManager.ConnectionStrings[Connection].ConnectionString);
        }
        #endregion
    }
}

