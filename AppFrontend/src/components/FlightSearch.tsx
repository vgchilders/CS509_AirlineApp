import React, { useState } from 'react';
import axios from 'axios';
import FlightList from './FlightList';
import './FlightSearch.css'; // Import the CSS file

function FlightSearch() {
  const [departAirport, setDepartAirport] = useState('');
  const [arriveAirport, setArriveAirport] = useState('');
  const [departureDate, setDepartureDate] = useState('');
  const [returnDate, setReturnDate] = useState('');
  const [departingFlights, setDepartingFlights] = useState([]);
  const [returningFlights, setReturningFlights] = useState([]);
  const [noRoundTrip, setNoRoundTrip] = useState(false);

  const handleSearch = async () => {
    try {
      const response = await axios.get('http://localhost:5218/api/v1/CombinedFlights/search', {
        params: {
          departAirport,
          arriveAirport,
          departureDate,
          returnDate
        }
      });
      const { directDepartFlights, directReturnFlights } = response.data;
      const allDepartingFlights = [...directDepartFlights];

      if (returnDate) {
        const allReturningFlights = [...(directReturnFlights || [])];
        if (allReturningFlights.length === 0) {
          setNoRoundTrip(true);
        } else {
          setNoRoundTrip(false);
          setReturningFlights(allReturningFlights);
        }
      } else {
        setNoRoundTrip(false);
        setReturningFlights([]);
      }

      setDepartingFlights(allDepartingFlights);
    } catch (error) {
      console.error('Error fetching flight data:', error);
    }
  };

  return (
    <div>
      <h1 style={{ textAlign: 'center' }}>Flight Search</h1>
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
        <div style={{ textAlign: 'center' }}>
          <button type="submit">Search</button>
        </div>
      </form>
      <br />
      {noRoundTrip ? (
        <div style={{ textAlign: 'center' }}>No round trip flights found</div>
      ) : (
        <div className={`flight-lists-container ${returningFlights.length > 0 ? '' : 'single-column'}`}>
          <FlightList title="Departing Flights" flights={departingFlights} />
          {returningFlights.length > 0 && <FlightList title="Returning Flights" flights={returningFlights} />}
        </div>
      )}
    </div>
  );
}

export default FlightSearch;