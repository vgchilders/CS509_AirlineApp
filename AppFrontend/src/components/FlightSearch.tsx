import React, { useState } from 'react';
import axios from 'axios';
import FlightList from './FlightList';

function FlightSearch() {
  const [departAirport, setDepartAirport] = useState('');
  const [arriveAirport, setArriveAirport] = useState('');
  const [departureDate, setDepartureDate] = useState('');
  const [returnDate, setReturnDate] = useState('');
  const [flights, setFlights] = useState([]);

  const handleSearch = async () => {
    try {
      const response = await axios.get('http://localhost:5218/api/v1/CombinedFlightsController/search', {
        params: {
          departAirport,
          arriveAirport,
          departureDate,
          returnDate
        }
      });
      setFlights(response.data);
    } catch (error) {
      console.error('Error fetching flight data:', error);
    }
  };

  return (
    <div>
      <h2>Flight Search</h2>
      <form onSubmit={(e) => { e.preventDefault(); handleSearch(); }}>
        <div>
          <label>Depart Airport:</label>
          <input type="text" value={departAirport} onChange={(e) => setDepartAirport(e.target.value)} />
        </div>
        <div>
          <label>Arrive Airport:</label>
          <input type="text" value={arriveAirport} onChange={(e) => setArriveAirport(e.target.value)} />
        </div>
        <div>
          <label>Departure Date:</label>
          <input type="date" value={departureDate} onChange={(e) => setDepartureDate(e.target.value)} />
        </div>
        <div>
          <label>Return Date:</label>
          <input type="date" value={returnDate} onChange={(e) => setReturnDate(e.target.value)} />
        </div>
        <button type="submit">Search</button>
      </form>
      <FlightList flights={flights} />
    </div>
  );
}

export default FlightSearch;
