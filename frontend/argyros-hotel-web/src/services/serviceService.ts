import { api } from './api';
import type { Service } from '../types/service';

export const serviceService = {
    getAllServices: async (): Promise<Service[]> => {
        const response = await api.get<Service[]>('/services');
        return response.data;
    },

    getServiceById: async (id: string): Promise<Service> => {
        const response = await api.get<Service>(`/services/${id}`);
        return response.data;
    },
};