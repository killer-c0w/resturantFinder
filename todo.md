
# To-do
## Backend/db:
* we really need to find a way to convert r_locid from varchar to bigint
* create index on r_locid (and possibly r_name)
* remove non-alphanumeric characters from the restaurant names, amenities, and cuisine (in both queries and the db)
* make our tables conform to at least 3rd normal form
* ~~sanitize inpu tto prevent SQL injection~~
* (optional) separate the opening_hours into its own table
## Functions we need:
* get restaurant results based on name, hours (dropdown?), amenity (dropdown?), and cuisine (dropdown?)
    * any combination of options should be valid unless all are null
* sign up / log in
* link a restaurant owner to a restaurant in the db (only if we have time to add owners)
    * only site admins can do this
* adding a new restaurant (name and id MUST be provided; the other fields are optional)
    * maybe we should sort r_locid? 
    * we absolutely need to use a unique locid for new restaurants ()
* modify an existing restaurant (compare attributes vs. new and only include necessary ones in the update)
* delete an existing restaurant

* the latter three should only be possible for restaurant owners, and the latter two for the owner of the specified restaurant
    * take a user as input and only display that user's restaurants as valid for deletion