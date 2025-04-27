namespace resturantFinder
{
    public class Restaurant(string id, string name, string? amenity = null, string? cuisine = null, string? openingHours = null)
    {
        public string Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string? Amenity { get; set; } = amenity;
        public string? Cuisine { get; set; } = cuisine;
        public string? OpeningHours { get; set; } = openingHours;
        //public Restaurant() { } // default constructor
    }
}
