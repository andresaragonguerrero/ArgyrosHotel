import { api } from "./api";
import type {
  Service,
  BookingAddOn,
  CreateBookingAddOnRequest,
} from "../types/service";

export const serviceService = {
  getAllServices: async (): Promise<Service[]> => {
    const response = await api.get<Service[]>("/services");
    return response.data;
  },

  getServiceById: async (id: string): Promise<Service> => {
    const response = await api.get<Service>(`/services/${id}`);
    return response.data;
  },

  getAddOnsByBooking: async (bookingId: string): Promise<BookingAddOn[]> => {
    const response = await api.get<BookingAddOn[]>(
      `/bookings/${bookingId}/addOns`,
    );
    return response.data;
  },

  createAddOn: async (
    request: CreateBookingAddOnRequest,
  ): Promise<BookingAddOn> => {
    const response = await api.post<BookingAddOn>("/bookingAddOns", request);
    return response.data;
  },

  deleteAddOn: async (id: string): Promise<void> => {
    await api.delete(`/bookingAddOns/${id}`);
  },
};
