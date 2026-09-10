import type { Booking } from '../types/booking';

interface BookingCardProps {
    booking: Booking;
}

export const BookingCard = ({ booking }: BookingCardProps) => {
    const formatDate = (dateString: string) => {
        return new Date(dateString).toLocaleDateString('es-ES', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
        });
    };

    return (
        <article className="booking-card" style={{ border: '1px solid #e2e8f0', borderRadius: '8px', padding: '1rem', backgroundColor: '#fff' }}>
            <header style={{ marginBottom: '0.5rem', borderBottom: '1px solid #edf2f7', paddingBottom: '0.5rem' }}>
                <strong>Reserva ID:</strong> <span style={{ fontSize: '0.85rem', color: '#718096' }}>{booking.id}</span>
            </header>
            <div style={{ display: 'grid', gap: '0.25rem', fontSize: '0.95rem' }}>
                <p><strong>Usuario ID:</strong> {booking.userId}</p>
                <p><strong>Tipo de Habitación:</strong> {booking.roomTypeId}</p>
                <p><strong>Habitación Asignada:</strong> {booking.roomId ? booking.roomId : 'Pendiente'}</p>
                <p><strong>Fechas:</strong> {formatDate(booking.startDate)} — {formatDate(booking.endDate)}</p>
                <p><strong>Huéspedes:</strong> {booking.numberOfGuests}</p>
            </div>
        </article>
    );
};