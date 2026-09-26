import { api } from "./api";
import type { Booking, CreateBookingRequest } from "../types/booking";

export const getAllBookings = async (): Promise<Booking[]> => {
  const { data } = await api.get<Booking[]>("/bookings");
  return data;
};

export const getBookingById = async (id: string): Promise<Booking> => {
  const { data } = await api.get<Booking>(`/bookings/${id}`);
  return data;
};

export const createBooking = async (
  request: CreateBookingRequest,
): Promise<Booking> => {
  const { data } = await api.post<Booking>("/bookings", request);
  return data;
};
