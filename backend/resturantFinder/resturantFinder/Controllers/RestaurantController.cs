using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace resturantFinder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantController : Controller
    {
        private readonly ILogger<RestaurantController> _logger;

        public RestaurantController(ILogger<RestaurantController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetRestaurants")]
        public IEnumerable<Restaurant>? GetRestaurantNoAddr(string? r_name = null, string? amenity = null, string? cuisine = null, string? opening_hours = null)
        {
            if (r_name == null & amenity == null & cuisine == null & opening_hours == null)
            {
                return null;
            }
            string connectionString = "Server=localhost;Database=AZRestaurantsDB;User Id=postgres;Password=password;";
            using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            string commandText = "SELECT * FROM restaurant WHERE ";
            // build the command based on supplied parameters
            int num_parms = 0;
            if (r_name != null)
            {
                num_parms++;
                commandText += $"r_name = '{r_name}'";
            }
            if (amenity != null)
            {
                if (num_parms > 0){commandText += " AND ";}
                num_parms++;
                commandText += $"amenity LIKE '%{amenity}%'";
            }
            if (cuisine != null)
            {
                if (num_parms > 0) { commandText += " AND "; }
                num_parms++;
                commandText += $"cuisine LIKE '%{cuisine}%'";
            }
            if (opening_hours != null)
            {
                if (num_parms > 0) { commandText += " AND "; }
                num_parms++;
                commandText += $"opening_hours LIKE '%{opening_hours}%'";
            }
            Console.WriteLine($"Command: {commandText}");

            NpgsqlCommand cmd = dataSource.CreateCommand(commandText);
            var reader = cmd.ExecuteReaderAsync();
            List<Restaurant> queryResults = [];
            while (reader.Result.Read())
            {
                var id_res = reader.Result.GetString(0);
                var name_res = reader.Result.GetString(1);
                var amenity_res = reader.Result.IsDBNull(2) ? "" : reader.Result.GetString(2);
                var cuisine_res = reader.Result.IsDBNull(3) ? "" : reader.Result.GetString(3);
                var hours_res = reader.Result.IsDBNull(4) ? "" : reader.Result.GetString(4);
                //queryResults.Add($"Location ID: {id_res}, Name: {name_res}, Amenity: {amenity_res}, Cuisine: {cuisine_res}, Hours: {hours_res}");
                queryResults.Add(new Restaurant(id_res, name_res, amenity_res, cuisine_res, hours_res));
            }

            dataSource.Dispose();   // maybe use a try finally block here

            return queryResults;
        }
    }
}
