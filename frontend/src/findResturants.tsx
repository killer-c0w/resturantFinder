import { use, useState } from 'react';
import {resturant} from './types/resturant'


function RestaurantFinder () {

    const [resturants , setResturants] = useState<resturant[]>([])

    const fetchResturants = async () => {

        const response = await fetch('http://localhost:5062/WeatherForecast');      // URL will be changed when API is merged into main branch
        const data = await response.json();                                         // ^ wont work now becuase difference in types ^
        setResturants(data);
    }
    return(

        <>
            <div>
                
            </div>
        </>

    );


}

export default RestaurantFinder;