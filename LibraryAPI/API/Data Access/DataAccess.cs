﻿using API.Model;
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
            using(var connection = new SqlConnection(DbConnection))
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
                    var orders =listOfOrders.Where(lo=>lo.UserId == user.Id).ToList();
                    var fine = 0;
                    foreach (var order in orders)
                    {
                        //if book is not return excute the query otherwise no
                        if(order.BookId !=null && order.Returned !=null && order.Returned ==false)
                        {
                            var orderDate=order.OrderDate;
                            var maxDate = orderDate.AddDays(10);
                            var currentDate=DateTime.Now;

                            var extraDays=(currentDate-maxDate).Days;
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
