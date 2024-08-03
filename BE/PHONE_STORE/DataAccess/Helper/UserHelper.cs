using Common.Entity;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Helper
{
    public class UserHelper
    {
        private const int MAX_SIZE = 8; // randow password 8 characters

        public static string getAccessToken(User user)
        {
            byte[] key = Encoding.UTF8.GetBytes("Yh2k7QSu4l8CZg5p6X3Pna9L0Miy4D3Bvt0JVr87UcOj69Kqw5R2Nmf4FWs03Hdx");
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //  create list claim  to store user's information
            List<Claim> list = new List<Claim>()
            {
                new Claim("id", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
            };
            JwtSecurityToken token = new JwtSecurityToken("JWTAuthenticationServer",
                "JWTServicePostmanClient", list, expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            // get access token
            return handler.WriteToken(token);
        }

        public static string HashPassword(string password)
        {
            // using SHA256 for hash password
            byte[] hashPw = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashPw.Length; i++)
            {
                // convert into hexadecimal
                builder.Append(hashPw[i].ToString("x2"));
            }
            return builder.ToString();
        }
        public static string RandomPassword()
        {
            Random random = new Random();
            // password contain both alphabets and numbers
            string format = "abcdefghijklmnopqrstuvwxyz0123456789QWERTYUIOPASDFGHJKLZXCVBNM";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < MAX_SIZE; i++)
            {
                // get random index character
                int index = random.Next(format.Length);
                builder.Append(format[index]);
            }
            return builder.ToString();
        }

        public static string BodyEmailForRegister(string password)
        {
            string body = "<h1>Mật khẩu cho tài khoản mới</h1>\n" +
                            "<p>Mật khẩu của bạn là: " + password + "</p>\n";
            return body;
        }

        public static Task sendEmail(string subject, string body, string to)
        {
            // get information of mail address
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationRoot config = builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            IConfigurationSection mail = config.GetSection("MailAddress");
            // create message to send
            MimeMessage mime = new MimeMessage();
            MailboxAddress mailFrom = MailboxAddress.Parse(mail["Username"]);
            MailboxAddress mailTo = MailboxAddress.Parse(to);
            mime.From.Add(mailFrom);
            mime.To.Add(mailTo);
            mime.Subject = subject;
            mime.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            // send message
            SmtpClient smtp = new SmtpClient();
            smtp.Connect(mail["Host"]);
            smtp.Authenticate(mail["Username"], mail["Password"]);
            smtp.Send(mime);
            smtp.Disconnect(true);
            return Task.CompletedTask;
        }

        public static string BodyEmailForForgetPassword(string password)
        {
            string body = "<h1>Mật khẩu mới</h1>\n" +
                            "<p>Mật khẩu mới là: " + password + "</p>\n" +
                            "<p>Không nên chia sẻ mật khẩu của bạn với người khác.</p>";
            return body;
        }

        public static string BodyEmailForAdminReceiveOrder()
        {
            string body = "<h1>New Order</h1>\n"
                + "<p>Please check information order</p>\n";
            return body;
        }

        public static string BodyEmailForApproveOrder(List<OrderDetail> list)
        {
            StringBuilder builder = new StringBuilder("<p>Information order detail:</p>\n");
            foreach (OrderDetail item in list)
            {
                builder.AppendLine("<p> - " + item.Product.ProductName + ", quantity: " + item.Quantity + "</p>");
            }
            return builder.ToString();
        }

    }
}
