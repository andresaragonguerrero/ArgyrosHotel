import { useServices } from '../hooks/useServices';
import { ServiceCard } from '../components/ServiceCard';
import './ServicesPage.css';

export const ServicesPage = () => {
    const { services, loading, error } = useServices();

    if (loading) return <p className="status-text">Cargando catálogo de experiencias...</p>;
    if (error) return <p className="status-text error">{error}</p>;

    return (
        <main className="services-page">
            <header className="services-header">
                <h1>Experiencias y Servicios Extra</h1>
                <p>Personaliza tu reserva con nuestros servicios exclusivos durante tu visita.</p>
            </header>

            <div className="services-grid">
                {services.map((service) => (
                    <ServiceCard key={service.id} service={service} />
                ))}
            </div>
        </main>
    );
};