import { useState } from 'react'
import './App.css'
//import RestaurantFinder from './findResturants'


function App() {
  //variables for delete submitted text is sent to backend while extra text is the value contained in the form bar
  const [deleteText, setDeleteText] = useState('');
  const [submittedText, setSubmittedText] = useState('');

  //state to determine if the form/delete bar is hidden
  const [showDeleteInput, setShowDeleteInput] = useState(false);
  const [showInsertForm, setShowInsertForm] = useState(false);

  const [searchText, setSearchText] = useState('');

  //the state of the returned data
  const [results, setResults] = useState<Restaurant[]>([]);

  //the type displayed on the results bar using Restaurant.id .. etc only id name and address are mandatory to create the object
  type Restaurant = {
    id: number;
    name: string;
    amenity?: string;
    cuisine?: string;
    openingHours?: string;
    address: string;
  };
  //test shops 
  /*const sampleRestaurants: Restaurant[] = [
    {
      id: 1,
      name: "Taco Bell",
      amenity: "Takeout",
      cuisine: "Tacos",
      openingHours: "10am - 9pm",
      address: "123 Main St Phoenix AZ 85001"
    },
    {
      id: 2,
      name: "Babbos",
      amenity: "Great Service",
      cuisine: "Italian",
      openingHours: "11am - 10pm",
      address: "123 Main St Phoenix AZ 85001"
    },
    {
      id: 3,
      name: "Burger Barn",
      amenity: "",
      cuisine: "Burger",
      openingHours: "8am - 8pm",
      address: "123 Main St Phoenix AZ 85001"
    }
  ];*/
  const [form, setForm] = useState({
    name: '',
    street_addr: '',
    city: '',
    zipcode: '',
    amenity: '',
    cuisine: '',
    opening_hours: '',
    building_number: '',
    state_abrv: ''
  });

  const handleSearchClick = async () => {
    //if blank popup 
    if (!searchText.trim()) {
      alert('Please enter a city, state abbreviation, or ZIP code');
      return;
    }
    //setResults(sampleRestaurants);
    const params = new URLSearchParams({
      city: searchText,
      state_abrv: searchText,
      zipcode: searchText
    }).toString();
  
    try {
      const response = await fetch(`http://localhost:5000/Restaurant/GetRestaurants?${params}`);
  
      if (!response.ok) throw new Error('Search failed');
  
      const data = await response.json();
      setResults(data ?? []);
      

    } catch (error) {
      console.error(error);
      alert('Failed to fetch search results');
    }
  };
  //display form if insert button is clicked
  const handleInsertClick = () => {
    setShowInsertForm(!showInsertForm);

  };

  
  const handleDeleteClick = async () => {
    if (!showDeleteInput) {
      setShowDeleteInput(true); // First click will show bar
    } else {
      try {
        const response = await fetch(
          `http://localhost:5000/Restaurant/DeleteRestaurant?id=${deleteText}`,
          { method: 'GET' }
        );

        if (!response.ok) throw new Error('Delete failed');

        const result = await response.text();
        alert(`Deleted restaurant with ID: ${deleteText}`);
        setSubmittedText(deleteText);
        setDeleteText('');
        setShowDeleteInput(false); // Hide the input again
      } catch (error) {
        console.error(error);
        alert('Delete failed');
      }
    }
  };

  //handle form text changes
  const handleFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleInsertSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const params = new URLSearchParams(form as Record<string, string>).toString();
    alert("Query string:\n" + params);
    try {
      const response = await fetch(
        `http://localhost:5000/Restaurant/AddRestaurant?${params}`,
        { method: 'GET' }
      );
      
      if (!response.ok) throw new Error('Insert failed');

      alert('Restaurant added');
      setShowInsertForm(false);
      setForm({
        name: '',
        street_addr: '',
        city: '',
        zipcode: '',
        amenity: '',
        cuisine: '',
        opening_hours: '',
        building_number: '',
        state_abrv: ''
      });
    } catch (error) {
      console.error(error);
      alert('Failed to insert restaurant');
    }
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gray-50">
      <div className="w-full max-w-2xl mx-auto p-6 font-unkempt">
        <h1 className="text-4xl font-bold text-center mb-8 font-unkempt-bold">
          Restaurant Finder
        </h1>

        {/* Search section */}
        <div className="mb-6 flex flex-col gap-4">
          {/* Search bar */}
          <div className="flex gap-4">
            <input
              type="text"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              placeholder="city, state, or zip"
              className="flex-1 px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
            <button
              onClick={handleSearchClick}
              className="px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
            >
              Search
            </button>
          </div>

          {/* Action buttons */}
          <div className="flex flex-col sm:flex-row gap-4">
            <button
              onClick={handleInsertClick}
              className="px-6 py-2 bg-green-500 text-white rounded-lg hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-offset-2"
            >
              {showInsertForm ? 'Close Form' : 'Insert'}
            </button>

            <button
              onClick={handleDeleteClick}
              className="px-6 py-2 bg-red-500 text-white rounded-lg hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2"
            >
              Delete
            </button>
          </div>

          {/*Hidden input for Delete */}
          {showDeleteInput && (
            <input
              type="text"
              value={deleteText}
              onChange={(e) => setDeleteText(e.target.value)}
              placeholder="Enter restaurant ID to delete"
              className="px-4 py-2 rounded-lg border border-gray-400 focus:outline-none focus:ring-2 focus:ring-red-500"
            />
          )}

          {/*Insert form */}
          {showInsertForm && (
            <form onSubmit={handleInsertSubmit} className="bg-white shadow p-4 rounded-lg flex flex-col gap-3 mt-4">
              <input name="name" placeholder="Name" value={form.name} onChange={handleFormChange} required className="p-2 border rounded" />
              <input name="street_addr" placeholder="Street Address" value={form.street_addr} onChange={handleFormChange} required className="p-2 border rounded" />
              <input name="city" placeholder="City" value={form.city} onChange={handleFormChange} required className="p-2 border rounded" />
              <input name="zipcode" placeholder="Zip Code" value={form.zipcode} onChange={handleFormChange} required className="p-2 border rounded" />
              <input name="amenity" placeholder="Amenity (optional)" value={form.amenity} onChange={handleFormChange} className="p-2 border rounded" />
              <input name="cuisine" placeholder="Cuisine (optional)" value={form.cuisine} onChange={handleFormChange} className="p-2 border rounded" />
              <input name="opening_hours" placeholder="Opening Hours (optional)" value={form.opening_hours} onChange={handleFormChange} className="p-2 border rounded" />
              <input name="building_number" placeholder="Building Number (optional)" value={form.building_number} onChange={handleFormChange} className="p-2 border rounded" />
              <input name="state_abrv" placeholder="State Abbreviation (optional)" value={form.state_abrv} onChange={handleFormChange} className="p-2 border rounded" />
              <button type="submit" className="mt-2 bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
                Submit
              </button>
            </form>
          )}
        </div>

        <div className="bg-white rounded-lg shadow-lg p-4 min-h-[100px] mt-4 space-y-4">
          {/* 🔹 Show last deleted ID if exists */}
          {submittedText && (
            <p className="text-gray-700 text-center">
              Deleted restaurant ID: <strong>{submittedText}</strong>
            </p>
          )}

          {/* 🔹 Show search results or fallback */}
          {results.length > 0 ? (
            <ul className="space-y-4">
              {results.map((restaurant, index) => (
                <li key={index} className="p-4 border rounded shadow-sm">
                  <h2 className="text-xl font-semibold">{restaurant.name}</h2>
                  <p className="text-sm text-gray-600">{restaurant.address}</p>
                  {(restaurant.amenity || restaurant.cuisine) && (
                    <p className="text-sm mt-1">
                      {restaurant.amenity && <>Amenity: {restaurant.amenity} </>}
                      {restaurant.cuisine && <>| Cuisine: {restaurant.cuisine}</>}
                    </p>
                  )}
                  {restaurant.openingHours && (
                    <p className="text-sm text-gray-500 mt-1">Hours: {restaurant.openingHours}</p>
                  )}
                </li>
              ))}
            </ul>
          ) : (
            <p className="text-gray-500 text-center">Results.</p>
          )}
        </div>
      </div>
    </div>
  );
}


export default App
