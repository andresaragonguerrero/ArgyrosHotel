import { api } from './api';
import type { Booking } from '../types/booking';

export const getAllBookings = async (): Promise<Booking[]> => {
    const { data } = await api.get<Booking[]>('/bookings');
    return data;
};