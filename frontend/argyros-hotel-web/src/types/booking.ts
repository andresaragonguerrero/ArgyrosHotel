import type { BookingAddOn } from "./service";

export interface Booking {
  id: string;
  userId: string;
  roomTypeId: string;
  roomId?: string | null;
  startDate: string;
  endDate: string;
  numberOfGuests: number;
  basePrice: number;
  finalPrice: number;
  addOns?: BookingAddOn[];
}

export interface CreateBookingRequest {
  userId: string;
  roomTypeId: string;
  startDate: string;
  endDate: string;
  numberOfGuests: number;
}

export interface QuoteServiceItem {
  serviceId: string;
  quantity: number;
}

export interface QuoteRequest {
  userId: string;
  roomTypeId: string;
  startDate: string;
  endDate: string;
  numberOfGuests: number;
  selectedServices: QuoteServiceItem[];
}

export interface QuoteServiceLine {
  serviceId: string;
  name: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface QuoteResponse {
  basePrice: number;
  roomFinalPrice: number;
  isPremium: boolean;
  services: QuoteServiceLine[];
  servicesTotal: number;
  grandTotal: number;
}
