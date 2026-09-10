import { Link } from 'react-router-dom';

export const Navbar = () => {
    return (
        <nav style={{ display: 'flex', gap: '1.5rem', padding: '1rem 2rem', backgroundColor: '#1a202c', color: 'white' }}>
            <strong style={{ fontSize: '1.2rem', marginRight: 'auto' }}>Argyros Hotel</strong>
            <Link to="/rooms" style={{ color: 'white', textDecoration: 'none' }}>Habitaciones</Link>
            <Link to="/services" style={{ color: 'white', textDecoration: 'none' }}>Servicios</Link>
            <Link to="/bookings" style={{ color: 'white', textDecoration: 'none' }}>Reservas</Link>
        </nav>
    );
};