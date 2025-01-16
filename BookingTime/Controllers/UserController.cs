using BookingTime.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using BookingTime.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;
using Azure.Core;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BookingTime.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        BookingtimeContext bTMContext = new BookingtimeContext();

        [HttpPost]
        [Route("/api/login")]
        public object Login(User request)
        {
            try
            {
                try
                {
                    var emailChk = bTMContext.Users.SingleOrDefault(u => u.Email == request.Email);
                    if (emailChk == null)
                        return JsonConvert.SerializeObject(new { code = 200, msg = "Login detail not found!" });
                    if (emailChk!=null &&emailChk.IsVerified==false)
                        return JsonConvert.SerializeObject(new { code = 200, msg = "Please verify your account!" });
                    if (emailChk != null && emailChk.Email == request.Email && emailChk.Password != request.Password)
                        return JsonConvert.SerializeObject(new { code = 200, msg = "Please enter correct password!" });
                    if (emailChk != null && emailChk.Email == request.Email && emailChk.Password == request.Password) 
                    {
                        var token = GenerateJwtToken(emailChk);
                        return JsonConvert.SerializeObject(new { code = 200, msg = "Loogged In successfully!",data=token });
                    }
                   
                   
                }

                catch (Exception ex)
                {
                    JsonConvert.SerializeObject(new { msg = ex.Message });
                }
                return JsonConvert.SerializeObject(new { msg = "Message" });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("/api/signUp")]
        public object signUp(User request)
        {
            try
            {
                try
                {
                    var emailChk = bTMContext.Users.SingleOrDefault(u => u.Email == request.Email);
                    if (emailChk!=null)
                    {
                        return JsonConvert.SerializeObject(new { code = 200, msg = "User already Exist with this email!" });
                    }
                    User user = new User();
                    user.Email = request.Email;
                    user.Password = request.Password;
                    user.IsVerified = false;
                    user.VerificationToken= Guid.NewGuid().ToString();
                    bTMContext.Users.Add(user);
                    bTMContext.SaveChanges();
                    var emailSend=SendVerificationEmail(user.Email, user.VerificationToken);
                    if(emailSend!=null)
                        return JsonConvert.SerializeObject(new { code = 200, msg = "We have send a verifcation email to your account please verify it!" });

                }

                catch (Exception ex)
                {
                    JsonConvert.SerializeObject(new { msg = ex.Message });
                }
                return JsonConvert.SerializeObject(new { msg = "Message" });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        [Route("/api/createUser")]
        public object createUser(User user)
        {
            try
            {
                try
                {
                    var usrCheck = bTMContext.Users.SingleOrDefault(u => u.Email == user.Email);
                    if(usrCheck!=null)
                    if (usrCheck != null)
                    {
                        usrCheck.Id = user.Id;
                        usrCheck.Email = user.Email;
                        bTMContext.Users.Update(usrCheck);
                        bTMContext.SaveChanges();
                        //return JsonConvert.SerializeObject(new { id = usrCheck.UserId });
                    }
                    else
                    {
                        User usr = new User();

                        bTMContext.Users.Add(usr);
                        bTMContext.SaveChanges();
                        //return JsonConvert.SerializeObject(new { id = usr.UserId });
                    }
                }

                catch (Exception ex)
                {
                    JsonConvert.SerializeObject(new { msg = ex.Message });
                }
                return JsonConvert.SerializeObject(new { msg = "Message" });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object SendVerificationEmail(string email, string token) 
        {
            try 
            {

                var baseUrl = "https://localhost:7087/";
                var fromEmail = "from@example.com";
                var verificationUrl = $"{baseUrl}verify/{token}";
                var subject = "Email Verification";
                var body = $"Please click the following link to verify your email: <a href='{verificationUrl}'>Verify Email</a>";

                // Create the SMTP client and configure it
                var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("2cda058f64fa9e", "de2ffa2a620b92"),
                    EnableSsl = true
                };

                // Create a MailMessage object to send the email
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true  // Set this to true to send HTML content
                };

                // Add the recipient email address
                mailMessage.To.Add(email);

                // Send the email
                client.Send(mailMessage);

                System.Console.WriteLine("Sent");


                return true;
            }
            catch(Exception ex) 
            {
                return null;
            }
        }
        [HttpGet("verify/{token}")]
        public object VerifyEmail(string token) 
        {
            var userChk = bTMContext.Users.SingleOrDefault(u => u.VerificationToken == token);
            if(userChk != null)
            {
                userChk.IsVerified = true;
                bTMContext.Users.Update(userChk);
                bTMContext.SaveChanges();
            }
            return null;
        }

        private string GenerateJwtToken(User user)
        {
            // Define the secret key and algorithm for signing the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Set the claims for the JWT token (user information)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),  // Subject: the user's email
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // JWT ID (unique)
                new Claim(ClaimTypes.Name, user.FullName),  // User's full name (example)
                new Claim("IsVerified", user.IsVerified.ToString())  // Additional claim indicating if the account is verified
             };

            // Create the JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),  // Set the token expiry time
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the JWT token as a string
            return tokenHandler.WriteToken(token);
        }
    }
}
