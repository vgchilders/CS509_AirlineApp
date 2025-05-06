import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import FlightSearch from './components/FlightSearch';
import SeatSelection from './components/SeatSelection';
import BookingInfo from './components/BookingInfo';
import PaymentSuccess from './components/PaymentSuccess';
import PaymentCancel from './components/PaymentCancel';
import RetrieveTicket from './components/RetrieveTicket';

function App() {
  return (
    <Router>
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', overflow: 'auto' }}>
        <Routes>
          <Route path="/" element={<FlightSearch />} />
          <Route path="/seat-selection" element={<SeatSelection />} />
          <Route path="/booking-info" element={<BookingInfo />} />
          <Route path="/success" element={<PaymentSuccess />} />
          <Route path="/cancel" element={<PaymentCancel />} />
          <Route path="/retrieve-ticket" element={<RetrieveTicket />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;