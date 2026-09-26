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
