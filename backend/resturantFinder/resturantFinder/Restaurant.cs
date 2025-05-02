namespace resturantFinder
{
    public class Restaurant(long id, string name, string address, string? amenity = null, string? cuisine = null, string? openingHours = null)
    {
        public long Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string? Amenity { get; set; } = amenity;
        public string? Cuisine { get; set; } = cuisine;
        public string? OpeningHours { get; set; } = openingHours;
        public string Address { get; set; } = address;
        //public Restaurant() { } // default constructor
    }
}
