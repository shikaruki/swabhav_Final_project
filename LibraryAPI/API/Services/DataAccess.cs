using API.Model;
using Dapper;
using LibraryWebAPI.Services;
using MailKit.Net.Smtp;
using Microsoft.Data.SqlClient;
using MimeKit;
using System.Security.Cryptography;
using System.Text;

namespace API.Data_Access
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration configuration;
        private readonly string DbConnection;
        private readonly EmailService emailService;
        public DataAccess(IConfiguration _configuration)
        {
            configuration = _configuration;
            DbConnection = configuration["connectionStrings:DBConnect"] ?? "";
        }

        public bool AuthenticateUser(string email, string _password, out User? user)
        {
             var result = false;

            var password = HashPassword(_password); 
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

        public  int CreateUser(User user)
        {
            var result = 0;
            if(IsEmailAvailable(user.Email))return result;
            using (var connection = new SqlConnection(DbConnection))
            {
                var parameters = new
                {
                    fn = user.FirstName,
                    ln = user.LastName,
                    em = user.Email,
                    mb = user.Mobile,
                    pwd = HashPassword(user.Password),
                    blk = user.Blocked,
                    act = user.Active,
                    con = user.CreatedOn,
                    type = user.UserType.ToString()
                };


                //var email = new MimeMessage();
                //email.Sender = MailboxAddress.Parse("cool.ravi342@gmail.com");
                //email.To.Add(MailboxAddress.Parse(user.Email)); email.Subject = $"Welcome, {user.FirstName} in TaskMangement App";
                //email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                //{
                //    Text = $"Thank You ! For Registering to Task Management App.\n" +
                //    $"Your User Name is :{user.FirstName} \n Password is : {user.Password}\n" +
                //    $"\n Thank You by @Library Management Team"
                //};
                //using var smtp = new SmtpClient();
                //smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                //smtp.Authenticate("cool.ravi342@gmail.com", "yaak ezho vkzj ljzw");
                //smtp.Authenticate("cool.ravi342@gmail.com", "namv pwkg mpmv jyge");

                //smtp.Send(email);

                //smtp.Disconnect(true);


                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse("cool.ravi342@gmail.com");
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = $"Welcome, {user.FirstName + " " + user.LastName} in Library Management App";

                //email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                //{
                //    Text = $"Thank You ! For Registering to Task Management App.\n" +
                //    $"Your User Name is :{user.FirstName} \n Password is : {user.Password}\n" +
                //    $"\n Thank You by @Library Management Team"
                //};


                var emailBody = new StringBuilder();
                emailBody.AppendLine("<html>");
                emailBody.AppendLine("<body>");
                emailBody.AppendLine("<h1>Welcome to Library Management  App</h1>");
                emailBody.AppendLine("<p>Thank you for registering to Library Management App.</p>");
                emailBody.AppendLine("<p>Here are some of the features you can enjoy:</p>");
                emailBody.AppendLine("<ul>");
                emailBody.AppendLine("<li>You can order any book </li>");
                emailBody.AppendLine("<li>Check status of Order book like Have you returned Yet !.</li>");
                emailBody.AppendLine("<li>Total order and returned Book.</li>");
                emailBody.AppendLine("</ul>");
                emailBody.AppendLine($"<p>For your reference your :  User Name id {user.FirstName + " " + user.LastName}</p>");
                emailBody.AppendLine($"<p>Your Password is: {user.Password}</p>");
                emailBody.AppendLine("<p>Feel free to explore the app and let us know if you have any questions or feedback.</p>");
                emailBody.AppendLine("<p>Enjoy your library management App journey!</p>");
                emailBody.AppendLine("<hr />");
                emailBody.AppendLine("<p>This email is auto-generated. Please do not reply.</p>");
                emailBody.AppendLine("</body>");
                emailBody.AppendLine("</html>");

                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailBody.ToString() };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                //smtp.Authenticate("cool.ravi342@gmail.com", "yaak ezho vkzj ljzw");
                smtp.Authenticate("cool.ravi342@gmail.com", "namv pwkg mpmv jyge");

                smtp.Send(email);

                smtp.Disconnect(true);



                var sql = "insert into Users (FirstName, LastName, Email, Mobile, Password, Blocked, Active, CreatedOn, UserType) values (@fn, @ln, @em, @mb, @pwd, @blk, @act, @con, @type);";
                result = connection.Execute(sql, parameters);
            }
            return result;
        }

        //private static string HashPassword(string password)
        //{
        //    using (SHA1 sha256 = SHA1.Create())
        //    {
        //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        StringBuilder builder = new StringBuilder();
        //        foreach (byte b in bytes)
        //        {
        //            builder.Append(b.ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }


        public string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public IList<Book> GetAllBooks()
        {
            IEnumerable<Book> books ;
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

        public int GetUserViaMail(string email)
        {
            var result =-1;

            using (var connection = new SqlConnection(DbConnection))
            {

                //select Id from Users where Email = 'rancho.kartar@gmail.com';
                result = connection.Execute($"select Id from Users where Email='@email';", new { email });
                
            }
            
            return result;
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

        public bool ReturnBook(int userId, int bookId)
        {
            var returned = false;
            using(var connection = new SqlConnection(DbConnection))
            {
                //query for updating the ordered column of the book that book is aviable now
                var sql = $"update Books set Ordered =0 where Id={bookId};";
                connection.Execute(sql);
                //query for updating the ordered column of the order table after the return of the book
                sql = $"update Orders set Returned=1 where  UserId={userId}and BookId={bookId};";
                returned = connection.Execute(sql) == 1;
            }
            return returned;
        }
        public void BlockUser(int userId)
        {
            using var connection = new SqlConnection(DbConnection);
            connection.Execute("update Users set Blocked=1 where Id=@Id", new { Id = userId });
        }
        
        public void AlterPassword(int userId,string pass, string _email)
        {
            string _pass = HashPassword(pass.ToString());
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("cool.ravi342@gmail.com");
            email.To.Add(MailboxAddress.Parse(_email)); email.Subject = $"Welcome, User in Library Mangement App";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Thank You ! Your New Password For Library Management App. is \n" +
                $"Your \n Password is : {pass}\n" +
                $"\n Thank You by @Library Management Team"
            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //smtp.Authenticate("cool.ravi342@gmail.com", "yaak ezho vkzj ljzw");
            smtp.Authenticate("cool.ravi342@gmail.com", "namv pwkg mpmv jyge");

            smtp.Send(email);

            smtp.Disconnect(true);
            using var connection = new SqlConnection(DbConnection);
            connection.ExecuteAsync("update Users set Password=@pass where Id=@Id", new {pass=_pass, Id = userId });
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

        public IList<BookCategory> GetAllCategories()
        {
            IEnumerable<BookCategory> categories;

            using (var connection = new SqlConnection(DbConnection))
            {
                categories = connection.Query<BookCategory>("select * from BookCategories;");
            }

            return categories.ToList();
        }

        public void InsertNewBook(Book book)
        {
            using var conn = new SqlConnection(DbConnection);
            var sql = "select Id from BookCategories where Category=@cat and SubCategory=@subcat";
            var parameter1 = new
            {
                cat = book.Category.Category,
                subcat = book.Category.SubCategory
            };
            var categoryId = conn.ExecuteScalar<int>(sql, parameter1);

            sql = "insert into Books (Title, Author, Price, Ordered, CategoryId) values (@title, @author, @price, @ordered, @catid);";
            var parameter2 = new
            {
                title = book.Title,
                author = book.Author,
                price = book.Price,
                ordered = false,
                catid = categoryId
            };
            conn.Execute(sql, parameter2);
        }

        public bool DeleteBook(int bookId)
        {
            var deleted = false;
            using (var connection = new SqlConnection(DbConnection))
            {
                var sql = $"delete Books where Id={bookId}";
                deleted = connection.Execute(sql) == 1;
            }
            return deleted;
        }

        public void CreateCategory(BookCategory bookCategory)
        {
            using var connection = new SqlConnection(DbConnection);
            var parameter = new
            {
                cat = bookCategory.Category,
                subcat = bookCategory.SubCategory
            };
            connection.Execute("insert into BookCategories (category, subcategory) values (@cat, @subcat);", parameter);
        }
    }
}
