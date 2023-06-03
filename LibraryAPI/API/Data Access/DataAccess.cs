using API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace API.Data_Access
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration configuration;
        private readonly string DbConnection;
        public DataAccess(IConfiguration _configuration)
        {
            configuration = _configuration;
            DbConnection = configuration["connectionStrings:DBConnect"] ?? "";
        }

        public bool AuthenticateUser(string email, string password, out User? user)
        {
            var result = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                result = connection.ExecuteScalar<bool>("select count(1) from Users where email=@email and password=@password;", new { email, password });
                if (result)
                {
                    user = connection.QueryFirst<User>("select * from Users where email=@email;", new { email });
                }
                else
                {
                    user = null;
                }
            }
            return result;
        }

        public int CreateUser(User user)
        {
            var result = 0;
            using (var connection = new SqlConnection(DbConnection))
            {
                var parameters = new
                {
                    fn = user.FirstName,
                    ln = user.LastName,
                    em = user.Email,
                    mb = user.Mobile,
                    pwd = user.Password,
                    blk = user.Blocked,
                    act = user.Active,
                    con = user.CreatedOn,
                    type = user.UserType.ToString()
                };
                var sql = "insert into Users (FirstName, LastName, Email, Mobile, Password, Blocked, Active, CreatedOn, UserType) values (@fn, @ln, @em, @mb, @pwd, @blk, @act, @con, @type);";
                result = connection.Execute(sql, parameters);
            }
            return result;
        }

        public bool IsEmailAvailable(string email)
        {
            var result = false;

            using (var connection = new SqlConnection(DbConnection))
            {
                result = connection.ExecuteScalar<bool>("select count(*) from Users where Email=@email;", new { email });
            }

            return !result;
        }
    }
}
