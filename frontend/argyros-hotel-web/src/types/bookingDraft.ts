export interface SelectedService {
  serviceId: string;
  quantity: number;
}

export interface GuestData {
  name: string;
  surname: string;
  email: string;
  acceptTerms: boolean;
  acceptPrivacy: boolean;
}

export interface BookingDraft {
  startDate: string | null;
  endDate: string | null;
  numberOfGuests: number | null;
  roomTypeId: string | null;
  selectedServices: SelectedService[];
  guestData: GuestData | null;
}

export const EMPTY_DRAFT: BookingDraft = {
  startDate: null,
  endDate: null,
  numberOfGuests: null,
  roomTypeId: null,
  selectedServices: [],
  guestData: null,
};
