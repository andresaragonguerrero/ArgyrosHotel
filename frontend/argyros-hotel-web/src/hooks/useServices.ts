import { useState, useEffect } from 'react';
import type { Service } from '../types/service';
import { getAllServices } from '../services/serviceApi';

export const useServices = () => {
    const [services, setServices] = useState<Service[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadServices = async () => {
            try {
                setLoading(true);
                const data = await getAllServices();
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