export interface RoomType {
  id: string;
  name: string;
  capacity: number;
  basePrice: number;
  totalRooms: number;
  imageUrl: string;
  description: string;
  squareMeters: number;
}

export interface Room {
  id: string;
  roomTypeId: string;
  roomNumber: string;
}

export interface AvailabilityResult {
  isAvailable: boolean;
  availableRooms: number;
  nextAvailableDate?: string | null;
}
