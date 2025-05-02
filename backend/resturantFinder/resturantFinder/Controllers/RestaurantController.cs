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

        [HttpGet("GetRestaurants")]
        public async Task<IEnumerable<Restaurant>?> GetRestaurantNoAddr(string? name = null, string? amenity = null, string? cuisine = null, string? opening_hours = null, string? city = null)
        {
            if (name == null & amenity == null & cuisine == null & opening_hours == null)
            {
                return null;
            }
            string connectionString = "Server=localhost;Database=AZRestaurantsDB;User Id=postgres;Password=password;";
            await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);

            // build the command based on supplied parameters
            var conditions = new List<string>();
            List<NpgsqlParameter> parameters = [];

            if (name != null)
            {
                conditions.Add("name LIKE @name");
                parameters.Add(new NpgsqlParameter("name", $"%{name}%"));
            }

            if (amenity != null)
            {
                conditions.Add("amenity LIKE @amenity");
                parameters.Add(new NpgsqlParameter("amenity", $"%{amenity}%"));
            }

            if (cuisine != null)
            {
                conditions.Add("cuisine LIKE @cuisine");
                parameters.Add(new NpgsqlParameter("cuisine", $"%{cuisine}%"));
            }

            if (opening_hours != null)
            {
                conditions.Add("opening_hours LIKE @opening_hours");
                parameters.Add(new NpgsqlParameter("opening_hours", $"%{opening_hours}%"));
            }
            if (city != null)
            {
                conditions.Add("city = @city");
                parameters.Add(new NpgsqlParameter("city", city));
            }

            string commandText = $"SELECT * FROM restaurant NATURAL JOIN address WHERE {string.Join(" AND ", conditions)} ORDER BY locid";

            // query the database and add responses to a list
            List<Restaurant> queryResults = [];

            try
            {
                await using var connection = await dataSource.OpenConnectionAsync();
                NpgsqlCommand cmd = new(commandText, connection);
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }

                _logger.LogInformation($"Command: {cmd.ToString}");

                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id_res = reader.GetInt64(0);
                    var name_res = reader.GetString(1);
                    var amenity_res = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    var cuisine_res = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    var hours_res = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    var building_res = reader.IsDBNull(5) ? -1 : reader.GetInt32(5);
                    var address_res = reader.GetString(6);
                    var city_res = reader.GetString(7);
                    var state_res = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    var zip_res = reader.GetString(9);

                    queryResults.Add(new Restaurant(
                        id: id_res,
                        name: name_res,
                        amenity: amenity_res,
                        cuisine: cuisine_res,
                        openingHours: hours_res,
                        address: string.Join(' ', [building_res.ToString(), address_res, city_res, state_res, zip_res])));
                }
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "An error occurred while executing the query.");
                return null;
            }
            finally
            {
                // Dispose of the data source
                dataSource.Dispose();
            }

            return queryResults;
        }

        [HttpGet("AddRestaurant")]
        public async Task<IActionResult> AddRestaurant(string name, string address, string? amenity = null, string? cuisine = null, string? opening_hours = null)
        {
            if (name == null || address == null)
            {
                return BadRequest("Missing required parameters.");
            }
            string connectionString = "Server=localhost;Database=AZRestaurantsDB;User Id=postgres;Password=password;";
            await using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
            // build the command based on supplied parameters
            var values = new List<string>();
            List<NpgsqlParameter> parameters = [];

            parameters.Add(new NpgsqlParameter("name", name));
            parameters.Add(new NpgsqlParameter("amenity", amenity == null ? DBNull.Value : amenity) { IsNullable=true});
            parameters.Add(new NpgsqlParameter("cuisine", cuisine == null ? DBNull.Value : cuisine) { IsNullable = true });
            parameters.Add(new NpgsqlParameter("opening_hours", opening_hours == null ? DBNull.Value : opening_hours) { IsNullable = true });
            string commandText = $"INSERT INTO restaurant VALUES ((SELECT MAX(locid) + 1 FROM restaurant), " +
                $"@name, @amenity, @cuisine, @opening_hours)";
            // query the database and add responses to a list
            List<Restaurant> queryResults = [];
            try
            {
                await using var connection = await dataSource.OpenConnectionAsync();
                await using var transaction = await connection.BeginTransactionAsync();

                /*NpgsqlCommand getMaxID = new("SELECT MAX(locid) FROM restaurant", connection);
                await using var id_reader = await getMaxID.ExecuteReaderAsync();
                while (await id_reader.ReadAsync())
                {
                    parameters.Add(new NpgsqlParameter("id", id_reader.GetInt32(0) + 1));
                }
                await id_reader.CloseAsync();*/

                await using NpgsqlCommand cmd = new(commandText, connection, transaction);
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                _logger.LogInformation($"Command: {cmd.CommandText}");
                await cmd.ExecuteNonQueryAsync();
                //await writer.ReadAsync();

                await transaction.CommitAsync();
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(ex, "An error occurred while executing the query.");
                return UnprocessableEntity("An error occurred while executing the query.");
            }
            finally
            {
                // Dispose of the data source
                dataSource.Dispose();
            }
            return Ok();
        }
    }
}
