import { useBookings } from '../hooks/useBookings';
import { BookingCard } from '../components/BookingCard';

export const BookingsPage = () => {
    const { bookings, loading, error } = useBookings();

    if (loading) return <p>Cargando lista de reservas...</p>;
    if (error) return <p style={{ color: 'red' }}>{error}</p>;

    return (
        <main className="bookings-page" style={{ maxWidth: '1200px', margin: '0 auto', padding: '1rem' }}>
            <header style={{ marginBottom: '1.5rem' }}>
                <h1>Registro de Reservas</h1>
                <p>Consulta informativa de todas las estancias reservadas en el sistema.</p>
            </header>

            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(320px, 1fr))', gap: '1rem' }}>
                {bookings.map((booking) => (
                    <BookingCard key={booking.id} booking={booking} />
                ))}
            </div>
        </main>
    );
};