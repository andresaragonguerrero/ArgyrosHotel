import type { Service } from '../types/service';

interface ServiceCardProps {
    service: Service;
}

export const ServiceCard = ({ service }: ServiceCardProps) => {
    return (
        <article className="service-card">
            <div className="service-card-content">
                <h3 className="service-title">{service.name}</h3>
                <p className="service-price">${service.price.toFixed(2)} / estancia</p>
            </div>
        </article>
    );
};