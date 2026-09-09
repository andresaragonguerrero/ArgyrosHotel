export interface RoomType {
    id: string;
    name: string;
    capacity: number;
    basePrice: number;
    totalRooms: number;
}

export interface Room {
    id: string;
    roomTypeId: string;
    roomNumber: string;
    isAvailable?: boolean;
}

export interface AvailabilityResult {
    isAvailable: boolean;
    availableCount: number;
    nextAvailableDate?: string | null;
}