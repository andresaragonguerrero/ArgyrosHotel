import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Navbar } from './components/Navbar';
import { RoomsPage } from './pages/RoomsPage';

export function App() {
  return (
    <Router>
      <Navbar />
      <main style={{ padding: '1rem' }}>
        <Routes>
          <Route path="/" element={<Navigate to="/rooms" replace />} />
          <Route path="/rooms" element={<RoomsPage />} />
          <Route path="/services" element={<div>Próximamente: Vista de Servicios</div>} />
          <Route path="/bookings" element={<div>Próximamente: Vista de Reservas</div>} />
        </Routes>
      </main>
    </Router>
  );
}

export default App;