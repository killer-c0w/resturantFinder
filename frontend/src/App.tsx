import { useState } from 'react'
import './App.css'
//import RestaurantFinder from './findResturants'


function App() {

  return (
    <>
      <div className="min-h-screen flex flex-col items-center justify-center">
        <div className="w-full max-w-2xl mx-auto p-6 font-unkempt">
          <h1 className="text-4xl font-bold text-center mb-8 font-unkempt-bold">Restaurant Finder</h1>
          <div className="mb-6 flex gap-4">
            <input 
              type="text"
              placeholder="city, state, or zip"
              className="flex-1 px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
            <button
              className="px-6 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
              
            >
              Search
            </button>
          </div>
          <div className="bg-white rounded-lg shadow-lg p-4">
            {}
          </div>
        </div>
      </div>
    </>
  )
}

export default App
