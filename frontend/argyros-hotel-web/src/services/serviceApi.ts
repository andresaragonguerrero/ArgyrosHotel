import { api } from './api';
import type { Service } from '../types/service';

export const getAllServices = async (): Promise<Service[]> => {
    const { data } = await api.get<Service[]>('/services');
    return data;
};