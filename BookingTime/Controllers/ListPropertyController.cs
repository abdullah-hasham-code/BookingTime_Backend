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
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

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
        [EnableCors("AllowAngularApp")]
        public object GetListOFProperty()
        {
            BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
            var listOfProperty = bTMContext.PropertyDetails.ToList();
            return listOfProperty;
        }

        [HttpPost("/api/AddListingProperty")]
        [EnableCors("AllowAngularApp")]
        public IActionResult AddListingProperty([FromBody] JsonElement request)
        {
            if (!request.TryGetProperty("data", out JsonElement data) || data.ValueKind != JsonValueKind.Object)
                return BadRequest("Invalid request data.");
            var propertyDetail = System.Text.Json.JsonSerializer.Deserialize<PropertyDetail>(data.GetRawText());
            if (propertyDetail == null)
                return BadRequest("Failed to parse property details.");
            BookingtimeContext bTMContext = new BookingtimeContext(_configuration);
            bTMContext.PropertyDetails.Add(propertyDetail);
            bTMContext.SaveChanges();
            return Ok(new { code = 200, msg = "Property added successfully!" });
        }

    }
}
