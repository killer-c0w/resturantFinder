using Npgsql;

namespace resturantFinder
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }

        /*// needs a null check
        public List<string>? TestQuery => GetRestaurantNoAddr(r_name: "McDonald's", amenity: "fast food");

        public List<string>? GetRestaurantNoAddr(string? r_name = null, string? amenity = null, string? cuisine = null, string? opening_hours = null)
        {
            if (r_name == null & amenity == null & cuisine == null & opening_hours == null)
            {
                return null;
            }
            string connectionString = "Server=localhost;Database=AZRestaurantsDB;User Id=postgres;Password=password;";
            using NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
            NpgsqlCommand cmd = dataSource.CreateCommand("SELECT * FROM restaurant LIMIT 10");
            var reader = cmd.ExecuteReaderAsync();
            List<string> queryResults = [];
            while (reader.Result.Read())
            {
                var id_res = reader.Result.GetString(0);
                var name_res = reader.Result.GetString(1);
                var amenity_res = reader.Result.IsDBNull(2) ? "" : reader.Result.GetString(2);
                var cuisine_res = reader.Result.IsDBNull(3) ? "" : reader.Result.GetString(3);
                var hours_res = reader.Result.IsDBNull(4) ? "" : reader.Result.GetString(4);
                queryResults.Add($"Location ID: {id_res}, Name: {name_res}, Amenity: {amenity_res}, Cuisine: {cuisine_res}, Hours: {hours_res}");
            }

            dataSource.Dispose();   // maybe use a try finally block here

            return queryResults;
        }*/
    }
}
