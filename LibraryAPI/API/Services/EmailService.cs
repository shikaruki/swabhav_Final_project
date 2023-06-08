using API.Model;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace LibraryWebAPI.Services
{
    public class EmailService
    {
        public void AccountCreationMail(User user)
        {
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
            emailBody.AppendLine("<p>Enjoy your task management journey!</p>");
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
            
        }
    }
}
