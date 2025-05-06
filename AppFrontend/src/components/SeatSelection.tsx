import { useLocation, useNavigate } from 'react-router-dom';
import { v4 as uuidv4 } from 'uuid';
import { useState } from 'react';

function SeatSelection() {
  const location = useLocation();
  const { departingFlightId, returningFlightId, departingFlightSource, returningFlightSource } = location.state || {};
  const navigate = useNavigate();

  const generateRandomSeats = () => {
    const rows = ['A', 'B', 'C', 'D'];
    const cols = [1, 2, 3, 4, 5];
    return rows.flatMap(row =>
      cols.map(col => ({
        seatNumber: `${row}${col}`,
        occupied: Math.random() < 0.3 // 30% seats occupied
      }))
    );
  };

  const [departingSeats, setDepartingSeats] = useState(() => generateRandomSeats());
  const [returningSeats, setReturningSeats] = useState(() => returningFlightId ? generateRandomSeats() : []);

  const [selectedDepartSeat, setSelectedDepartSeat] = useState<string | null>(null);
  const [selectedReturnSeat, setSelectedReturnSeat] = useState<string | null>(null);

  const handleBuyTickets = () => {
    const sessionId = uuidv4();
    navigate('/booking-info', {
      state: {
        departingFlightId,
        departingFlightSource,
        returningFlightId,
        returningFlightSource,
        selectedDepartSeat,
        selectedReturnSeat,
        sessionId
      }
    });
  };

  return (
    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
      <div>
        <h1>Seat Selection</h1>
        <div>
          <h2>Departing Flight Seats</h2>
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(5, 50px)', gap: '10px' }}>
            {departingSeats.map(seat => (
              <button
                key={seat.seatNumber}
                disabled={seat.occupied}
                onClick={() => !seat.occupied && setSelectedDepartSeat(seat.seatNumber)}
                style={{
                  width: 50,
                  height: 50,
                  backgroundColor: seat.occupied
                    ? '#ccc'
                    : selectedDepartSeat === seat.seatNumber
                      ? 'green'
                      : 'white',
                  border: '1px solid #999',
                  cursor: seat.occupied ? 'not-allowed' : 'pointer'
                }}
              >
                {seat.seatNumber}
              </button>
            ))}
          </div>
        </div>

        {returningFlightId && (
          <div>
            <h2>Returning Flight Seats</h2>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(5, 50px)', gap: '10px' }}>
              {returningSeats.map(seat => (
                <button
                  key={seat.seatNumber}
                  disabled={seat.occupied}
                  onClick={() => !seat.occupied && setSelectedReturnSeat(seat.seatNumber)}
                  style={{
                    width: 50,
                    height: 50,
                    backgroundColor: seat.occupied
                      ? '#ccc'
                      : selectedReturnSeat === seat.seatNumber
                        ? 'green'
                        : 'white',
                    border: '1px solid #999',
                    cursor: seat.occupied ? 'not-allowed' : 'pointer'
                  }}
                >
                  {seat.seatNumber}
                </button>
              ))}
            </div>
          </div>
        )}
      </div>

      <div style={{ position: 'fixed', top: '20px', right: '20px', textAlign: 'right' }}>
        <div>
          <h3>Selected Seats</h3>
          <p>Departing Seat: {selectedDepartSeat || 'None'}</p>
          <p>Returning Seat: {selectedReturnSeat || 'None'}</p>
        </div>

        {(selectedDepartSeat || selectedReturnSeat) && (
          <button onClick={handleBuyTickets}>Buy Tickets</button>
        )}
      </div>
    </div>
  );
}

export default SeatSelection;