using Dapper;
using groupOrdering.Domain;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static groupOrdering.Technical.DTO;

namespace groupOrdering.Technical
{
    public class DAO
    {
        static readonly string connectionString = "Server=220.134.59.172; User ID=admin; Password=ntutguest@; Database=groupordering";
        public List<T> GetData<T>(string command)
        {
            using var connection = new MySqlConnection(connectionString);
            IEnumerable<T> result = connection.Query<T>(command);
            return result.ToList<T>();
        }
    }
}
