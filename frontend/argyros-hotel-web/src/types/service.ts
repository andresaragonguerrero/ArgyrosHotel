export interface Service {
  id: string;
  name: string;
  description: string;
  price: number;
}

export interface BookingAddOn {
  id: string;
  bookingId: string;
  serviceId: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface CreateBookingAddOnRequest {
  bookingId: string;
  serviceId: string;
  quantity: number;
}
