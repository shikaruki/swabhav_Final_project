using API.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;

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

        public IList<Book> GetAllBooks()
        {
            IEnumerable<Book> books = null;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = "select * from Books;";
                books = connection.Query<Book>(sql);

                foreach (var book in books)
                {
                    sql = "select * from BookCategories where Id=" + book.CategoryId;
                    book.Category = connection.QuerySingle<BookCategory>(sql);
                }
            }
            return books.ToList();
        }

        public IList<User> GetUsers()
        {
            IEnumerable<User> users;
            using (var connection = new SqlConnection(DbConnection))
            {
                //map it into user object
                users = connection.Query<User>("select * from  Users;");
                //list of order of that user for calculating the fine
                //select the user Id ,book Id on which date they order the book if already return then no fine othrrwise calculate the fine.
                var listOfOrders = connection.Query("select u.Id as UserId,o.BookId as BookId,o.OrderedOn as OrderDate,o.Returned as Returned from  Users u LEFT JOIN Orders o ON u.Id=o.UserId;");
                //iterate over the individual user for calculating the fine
                foreach (var user in users)
                {
                    //list of order belong to that particular user 
                    var orders = listOfOrders.Where(lo => lo.UserId == user.Id).ToList();
                    var fine = 0;
                    foreach (var order in orders)
                    {
                        //if book is not return excute the query otherwise no
                        if (order.BookId != null && order.Returned != null && order.Returned == false)
                        {
                            var orderDate = order.OrderDate;
                            var maxDate = orderDate.AddDays(10);
                            var currentDate = DateTime.Now;

                            var extraDays = (currentDate - maxDate).Days;
                            //if extra days are negative assign a 0 otherwise assign a extradays
                            extraDays = extraDays < 0 ? 0 : extraDays;
                            //50 rs for per day and assign it to the user fine
                            fine = extraDays * 50;
                            user.Fine += fine;
                        }
                    }
                }
            }
            return users.ToList();
        }
        public IList<Order> GetAllOrders()
        {
            IEnumerable<Order> orders;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = @"
                    select 
                        o.Id, 
                        u.Id as UserId, CONCAT(u.FirstName, ' ', u.LastName) as Name,
                        o.BookId as BookId, b.Title as BookName,
                        o.OrderedOn as OrderDate, o.Returned as Returned
                    from Users u LEFT JOIN Orders o ON u.Id=o.UserId
                    LEFT JOIN Books b ON o.BookId=b.Id
                    where o.Id IS NOT NULL;
                ";
                orders = connection.Query<Order>(sql);
            }
            return orders.ToList();
        }

        public IList<Order> GetOrdersOfUser(int userId)
        {
            IEnumerable<Order> orders;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = @"
                    select 
                        o.Id, 
                        u.Id as UserId, CONCAT(u.FirstName, ' ', u.LastName) as Name,
                        o.BookId as BookId, b.Title as BookName,
                        o.OrderedOn as OrderDate, o.Returned as Returned
                    from Users u LEFT JOIN Orders o ON u.Id=o.UserId
                    LEFT JOIN Books b ON o.BookId=b.Id
                    where o.UserId IN (@Id);
                ";
                orders = connection.Query<Order>(sql, new { Id = userId });
            }
            return orders.ToList();
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




        public bool OrderBook(int userId, int bookId)
        {
            var ordered = false;

            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"insert into Orders (UserId, BookId, OrderedOn, Returned) values ({userId}, {bookId}, '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', 0);";
                var inserted = connection.Execute(sql) == 1;
                if (inserted)
                {
                    sql = $"update Books set Ordered=1 where Id={bookId}";
                    var updated = connection.Execute(sql) == 1;
                    ordered = updated;
                }
            }

            return ordered;
        }

        public void BlockUser(int userId)
        {
            using var connection = new SqlConnection(DbConnection);
            connection.Execute("update Users set Blocked=1 where Id=@Id", new { Id = userId });
        }

        public void UnblockUser(int userId)
        {
            using var connection = new SqlConnection(DbConnection);
            connection.Execute("update Users set Blocked=0 where Id=@Id", new { Id = userId });
        }

        public void DeactivateUser(int userId)
        {
            using var connection = new SqlConnection(DbConnection);
            connection.Execute("update Users set Active=0 where Id=@Id", new { Id = userId });
        }

        public void ActivateUser(int userId)
        {
            using var connection = new SqlConnection(DbConnection);
            connection.Execute("update Users set Active=1 where Id=@Id", new { Id = userId });
        }

        

    }
}
