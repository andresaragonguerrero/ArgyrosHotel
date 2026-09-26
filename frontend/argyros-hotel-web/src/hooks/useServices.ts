import { useState, useEffect } from 'react';
import type { Service } from '../types/service';
import { serviceService } from '../services/serviceService';

export const useServices = () => {
    const [services, setServices] = useState<Service[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadServices = async () => {
            try {
                setLoading(true);
                const data = await serviceService.getAllServices();
                setServices(data);
            } catch (err) {
                setError('No se pudieron cargar los servicios del hotel.');
            } finally {
                setLoading(false);
            }
        };

        loadServices();
    }, []);

    return { services, loading, error };
};