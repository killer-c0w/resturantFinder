import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div className="min-h-screen flex flex-col items-center justify-center">
        <div className="w-full max-w-2xl mx-auto p-6 font-unkempt">
          <h1 className="text-4xl font-bold text-center mb-8 font-unkempt">Restaurant Finder</h1>
          <div className="mb-6">
            <input 
              type="text"
              placeholder="city, state, or zip"
              className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
          <div className="bg-white rounded-lg shadow-lg p-4">
            {/* Results will be displayed here */}
          </div>
        </div>
      </div>
    </>
  )
}

export default App
