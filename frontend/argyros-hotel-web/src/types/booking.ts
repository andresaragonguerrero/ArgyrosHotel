export interface Booking {
    id: string;
    userId: string;
    roomTypeId: string;
    roomId?: string | null;
    startDate: string;
    endDate: string;
    numberOfGuests: number;
}