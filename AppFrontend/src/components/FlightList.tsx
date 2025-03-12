import React from 'react';

function FlightList({ flights }) {
  return (
    <div>
      <h2>Flight List</h2>
      <ul>
        {flights.map((flight, index) => (
          <li key={index}>
            <p>Flight Number: {flight.FlightNumber}</p>
            <p>Depart Airport: {flight.DepartAirport}</p>
            <p>Arrive Airport: {flight.ArriveAirport}</p>
            <p>Departure Date: {new Date(flight.DepartDateTime).toLocaleString()}</p>
            <p>Arrival Date: {new Date(flight.ArriveDateTime).toLocaleString()}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default FlightList;
