import React from 'react';

function FlightList({ title, flights }) {
  if (!Array.isArray(flights)) {
    return <div>No flights available</div>;
  }

  const handleSelectFlight = (flight) => {
    console.log('Selected flight:', flight);
    // Add logic to handle flight selection for purchase
  };

  return (
    <div>
      {flights.length > 0 && <h2 style={{ textAlign: 'center' }}>{title}</h2>}
      <ul>
        {flights.map((flight, index) => (
          <li key={index}>
            <p>Flight Number: {flight.flightNumber}</p>
            <p>Depart Airport: {flight.departAirport}</p>
            <p>Arrive Airport: {flight.arriveAirport}</p>
            <p>Departure Date: {new Date(flight.departDateTime).toLocaleString([], { hour: '2-digit', minute: '2-digit', year: 'numeric', month: 'numeric', day: 'numeric' })}</p>
            <p>Arrival Date: {new Date(flight.arriveDateTime).toLocaleString([], { hour: '2-digit', minute: '2-digit', year: 'numeric', month: 'numeric', day: 'numeric' })}</p>
            <div style={{ textAlign: 'center' }}>
              <button onClick={() => handleSelectFlight(flight)}>Select Flight</button>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default FlightList;