import { useState, useEffect } from 'react';
import type { Booking } from '../types/booking';
import { getAllBookings } from '../services/bookingApi';

export const useBookings = () => {
    const [bookings, setBookings] = useState<Booking[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadBookings = async () => {
            try {
                setLoading(true);
                const data = await getAllBookings();
                setBookings(data);
            } catch (err) {
                setError('No se pudieron obtener las reservas registradas.');
            } finally {
                setLoading(false);
            }
        };

        loadBookings();
    }, []);

    return { bookings, loading, error };
};