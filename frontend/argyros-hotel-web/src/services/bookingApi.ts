import { api } from "./api";
import type {
  Booking,
  CreateBookingRequest,
  QuoteRequest,
  QuoteResponse,
} from "../types/booking";

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

export const getQuote = async (
  request: QuoteRequest,
): Promise<QuoteResponse> => {
  const { data } = await api.post<QuoteResponse>("/bookings/quote", request);
  return data;
};
