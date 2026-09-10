import { api } from './api';
import type { Room, RoomType, AvailabilityResult } from '../types/room';

export const roomService = {
    getRoomTypes: async (): Promise<RoomType[]> => {
        const response = await api.get<RoomType[]>('/roomTypes');
        return response.data;
    },

    getRoomTypeById: async (id: string): Promise<RoomType> => {
        const response = await api.get<RoomType>(`/roomTypes/${id}`);
        return response.data;
    },

    getRooms: async (): Promise<Room[]> => {
        const response = await api.get<Room[]>('/rooms');
        return response.data;
    },

    getRoomsByRoomType: async (roomTypeId: string): Promise<Room[]> => {
        const response = await api.get<Room[]>(`/rooms/byRoomType/${roomTypeId}`);
        return response.data;
    },

    checkAvailability: async (
        roomTypeId: string,
        startDate: string,
        endDate: string
    ): Promise<AvailabilityResult> => {
        const response = await api.get<AvailabilityResult>('/availability', {
            params: { roomTypeId, startDate, endDate },
        });
        return response.data;
    },
};