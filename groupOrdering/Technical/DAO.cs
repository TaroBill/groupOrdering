using Dapper;
using groupOrdering.Domain;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace groupOrdering.Technical
{
    public class DAO
    {
        static readonly string connectionString = File.ReadAllText("./Technical/DBConnect.txt");
        public List<T> GetData<T>(string command)
        {
            using var connection = new MySqlConnection(connectionString);
            IEnumerable<T> result = connection.Query<T>(command);
            return result.ToList<T>();
        }

        public int SetData(string command)
        {
            using var connection = new MySqlConnection(connectionString);
            int result = connection.Execute(command);
            return result;
        }
    }
}
