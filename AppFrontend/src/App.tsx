import City from './components/City';
import FlightSearch from './components/FlightSearch';

function App() {
  return (
    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', overflow: 'auto' }}>
      <div>
        {/* <City /> */}
        <FlightSearch />
      </div>
    </div>
  );
}

export default App;