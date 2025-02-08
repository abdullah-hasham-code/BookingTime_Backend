using BookingTime.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace BookingTime.Controllers
{
    public class ListPropertyController : Controller
    {
        private readonly IConfiguration _configuration;
        public ListPropertyController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        [HttpGet("/api/GetListOFProperty")]
        public object GetListOFProperty()
        {
            BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
            var listOfProperty = bTMContext.PropertyDetails.ToList();
            return listOfProperty;
        }
    }
}
