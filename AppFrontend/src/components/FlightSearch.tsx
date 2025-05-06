import { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import type { paths } from '../types/api';
import FlightList from './FlightList';
import './FlightSearch.css';

type CombinedFlightDto = paths['/api/v1/CombinedFlights/search']['get']['responses']['200']['content']['application/json']['directDepartFlights'][number];
type BookingInfoDto = paths['/api/v1/Booking/info']['post']['requestBody']['content']['application/json'];
type BookingDirectSeatsDto = paths['/api/v1/Booking/selectseat']['post']['requestBody']['content']['application/json'];
type BookingConnectingSeatsDto = paths['/api/v1/Booking/selectseat']['post']['requestBody']['content']['application/json'];

type SeatSelection = {
  flightId: number;
  seatNumber: string;
};

function FlightSearch() {
  const [departAirport, setDepartAirport] = useState('Boston (BOS)');
  const [arriveAirport, setArriveAirport] = useState('Orlando (MCO)');
  const [departureDate, setDepartureDate] = useState('2023-01-04');
  const [returnDate, setReturnDate] = useState('2023-01-05');

  const [departingFlights, setDepartingFlights] = useState<CombinedFlightDto[]>([]);
  const [connectingDepartFlights, setConnectingDepartFlights] = useState<CombinedFlightDto[]>([]);
  const [returningFlights, setReturningFlights] = useState<CombinedFlightDto[]>([]);
  const [connectingReturnFlights, setConnectingReturnFlights] = useState<CombinedFlightDto[]>([]);

  const [noRoundTrip, setNoRoundTrip] = useState(false);
  const [selectedSeats, setSelectedSeats] = useState<SeatSelection[]>([]);

  const [selectedDepartingFlight, setSelectedDepartingFlight] = useState<CombinedFlightDto | null>(null);
  const [selectedReturningFlight, setSelectedReturningFlight] = useState<CombinedFlightDto | null>(null);

  const navigate = useNavigate();

  const handleSearch = async () => {
    try {
      const response = await axios.get('http://localhost:5000/api/v1/CombinedFlights/search', {
        params: {
          departAirport,
          arriveAirport,
          departureDate,
          returnDate,
        },
      });

      const value = response.data;

      const allDeparting = value?.directDepartFlights?.$values ?? [];
      const allConnectingDepart = value?.connectingDepartFlights?.$values ?? [];
      const allReturning = value?.directReturnFlights?.$values ?? [];
      const allConnectingReturn = value?.connectingReturnFlights?.$values ?? [];

      setDepartingFlights(allDeparting);
      setConnectingDepartFlights(allConnectingDepart);

      if (returnDate) {
        const hasReturns = allReturning.length > 0 || allConnectingReturn.length > 0;
        setNoRoundTrip(!hasReturns);
        setReturningFlights(allReturning);
        setConnectingReturnFlights(allConnectingReturn);
      } else {
        setNoRoundTrip(false);
        setReturningFlights([]);
        setConnectingReturnFlights([]);
      }
    } catch (error) {
      console.error('Error fetching flight data:', error);
    }
  };

  const handleSeatSelection = async (seat: SeatSelection) => {
    try {
      const response = await axios.post('http://localhost:5000/api/v1/Booking/selectseat/direct', {
        flightId: seat.flightId,
        seatNumber: seat.seatNumber,
      });
      console.log('Seat booked successfully:', response.data);
    } catch (error) {
      console.error('Error booking seat:', error);
    }
  };

  const handleSaveBookingInfo = async (bookingInfo: BookingInfoDto) => {
    try {
      const response = await axios.post('http://localhost:5000/api/v1/Booking/info', bookingInfo);
      console.log('Booking info saved successfully:', response.data);
    } catch (error) {
      console.error('Error saving booking info:', error);
    }
  };

  const handleCreatePaymentSession = async (sessionId: string, bookingReference: string) => {
    try {
      const response = await axios.post('http://localhost:5000/api/v1/Payment/create-session', {
        sessionId,
        bookingReference,
      });
      console.log('Payment session created successfully:', response.data);
      window.location.href = response.data.url;
    } catch (error) {
      console.error('Error creating payment session:', error);
    }
  };

  const handleSendTicket = async (email: string, lastName: string, confirmationCode: string) => {
    try {
      const response = await axios.post('http://localhost:5000/api/v1/SendTicket/send-ticket', {
        email,
        lastName,
        confirmationCode,
      });
      console.log('Ticket sent successfully:', response.data);
    } catch (error) {
      console.error('Error sending ticket:', error);
    }
  };

  const handleBookFlights = () => {
    if (selectedDepartingFlight) {
      const departingFlightId = selectedDepartingFlight.id!;
      const departingFlightSource = selectedDepartingFlight.flight_source;
      const state: any = { departingFlightId, departingFlightSource };
      if (selectedReturningFlight) {
        state.returningFlightId = selectedReturningFlight.id!;
        state.returningFlightSource = selectedReturningFlight.flight_source;
      }
      navigate('/seat-selection', { state });
    }
  };

  const handleSelectDepartingFlight = (flight: CombinedFlightDto) => {
    setSelectedDepartingFlight(flight);
  };

  const handleSelectReturningFlight = (flight: CombinedFlightDto) => {
    setSelectedReturningFlight(flight);
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
        <div className={`flight-lists-container ${returningFlights.length > 0 || connectingReturnFlights.length > 0 ? '' : 'single-column'}`}>
          <FlightList title="Direct Departing Flights" flights={departingFlights} onSelectFlight={handleSelectDepartingFlight} />
          {/* <FlightList title="Connecting Departing Flights" flights={connectingDepartFlights} onSelectFlight={handleSelectDepartingFlight} /> */}
          {returningFlights.length > 0 && (
            <FlightList title="Direct Returning Flights" flights={returningFlights} onSelectFlight={handleSelectReturningFlight} />
          )}
          {connectingReturnFlights.length > 0 && (
            <FlightList title="Connecting Returning Flights" flights={connectingReturnFlights} onSelectFlight={handleSelectReturningFlight} />
          )}
        </div>
      )}

      {selectedDepartingFlight || selectedReturningFlight ? (
        <div style={{ position: 'fixed', top: '20px', right: '20px', textAlign: 'right' }}>
          {selectedDepartingFlight && (
            <div>
              <p>Departing Flight: {selectedDepartingFlight.flightNumber} - {selectedDepartingFlight.departAirport} to {selectedDepartingFlight.arriveAirport}</p>
            </div>
          )}
          {selectedReturningFlight && (
            <div>
              <p>Returning Flight: {selectedReturningFlight.flightNumber} - {selectedReturningFlight.departAirport} to {selectedReturningFlight.arriveAirport}</p>
            </div>
          )}
          <button onClick={handleBookFlights}>Select Seats</button>
        </div>
      ) : null}
    </div>
  );
}

export default FlightSearch;
