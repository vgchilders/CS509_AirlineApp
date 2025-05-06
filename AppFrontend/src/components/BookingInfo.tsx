import { useLocation, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import axios from 'axios';
import type { paths, components } from '../types/api';

// Types for booking info and session creation
type BookingInfoDto = paths['/api/v1/Booking/info']['post']['requestBody']['content']['application/json'];
type FlightLegDto = components['schemas']['FlightLegDto'];

function BookingInfo() {
  const navigate = useNavigate();
  const { departingFlightId, returningFlightId, selectedDepartSeat, selectedReturnSeat, sessionId, departingFlightSource, returningFlightSource } = useLocation().state as Record<string, any>;

  // passenger info state
  const [firstName, setFirstName] = useState('David');
  const [lastName, setLastName] = useState('Gobran');
  const [email, setEmail] = useState('dygobran@gmail.com');
  const [phoneNumber, setPhoneNumber] = useState('1234567890');
  const [dateOfBirth, setDateOfBirth] = useState('2000-01-01');
  const [gender, setGender] = useState('Male');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      // hold seats for direct flights
      const holdDepartReq = {
        flightType: 1,
        flightId: departingFlightId,
        seatNumber: selectedDepartSeat!,
        sessionId,
        direction: 'outbound',
        flightSource: departingFlightSource
      };
      await axios.post('http://localhost:5000/api/v1/Booking/selectseat/direct', holdDepartReq);

      if (returningFlightId && selectedReturnSeat) {
        const holdReturnReq = {
          flightId: returningFlightId,
          flightSource: returningFlightSource,
          seatNumber: selectedReturnSeat,
          sessionId,
          direction: 'return'
        };
        await axios.post('http://localhost:5000/api/v1/Booking/selectseat/direct', holdReturnReq);
      }

      // build flights array with correct flightSource and direction
      const flights: FlightLegDto[] = [];
      flights.push({ flightId: departingFlightId, direction: 'outbound', flightSource: departingFlightSource });
      if (returningFlightId) {
        flights.push({ flightId: returningFlightId, direction: 'return', flightSource: returningFlightSource });
      }

      // calculate price client-side? skip; let backend compute
      const bookingInfo: BookingInfoDto = {
        sessionId,
        firstName,
        lastName,
        email,
        phoneNumber,
        dateOfBirth,
        gender,
        price: 0,
        flights
      };

      // save booking info
      const infoResp = await axios.post('http://localhost:5000/api/v1/Booking/info', bookingInfo);
      const bookingReference = infoResp.data.bookingReference;

      // initiate payment with correct payload keys
      const paymentPayload = {
        SessionId: sessionId,
        BookingReference: bookingReference
      };
      const payResp = await axios.post('http://localhost:5000/api/v1/Payment/create-session', paymentPayload);
      const { url } = payResp.data;

      // redirect to Stripe
      window.location.href = url;
    } catch (err) {
      console.error('Error saving booking or creating payment session:', err);
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: 'auto' }}>
      <h1>Passenger Details</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>First Name</label>
          <input required value={firstName} onChange={e => setFirstName(e.target.value)} />
        </div>
        <div>
          <label>Last Name</label>
          <input required value={lastName} onChange={e => setLastName(e.target.value)} />
        </div>
        <div>
          <label>Email</label>
          <input type="email" required value={email} onChange={e => setEmail(e.target.value)} />
        </div>
        <div>
          <label>Phone Number</label>
          <input value={phoneNumber} onChange={e => setPhoneNumber(e.target.value)} />
        </div>
        <div>
          <label>Date of Birth</label>
          <input type="date" value={dateOfBirth} onChange={e => setDateOfBirth(e.target.value)} />
        </div>
        <div>
          <label>Gender</label>
          <select value={gender} onChange={e => setGender(e.target.value)}>
            <option value="">Select</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
          </select>
        </div>
        <button type="submit">Proceed to Payment</button>
      </form>
    </div>
  );
}

export default BookingInfo;