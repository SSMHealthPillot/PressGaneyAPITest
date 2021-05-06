using Dapper;
using System.Collections.Generic;
using System.Data;

namespace PhyndAPI.DATA.Interfaces
{
    public interface IRepository
    {
        void DatabaseConnectionState(IDbConnection conn);
        ICollection<T> GetList<T>(string query);
        ICollection<T> GetList<T>(string query, DynamicParameters parameters);
        T GetSingle<T>(string query, DynamicParameters parameters);
        bool Add<T>(string query, DynamicParameters parameters);
        bool Update<T>(string query, DynamicParameters parameters);
        ICollection<T> GetListByQuery<T>(string query);
    }
}

