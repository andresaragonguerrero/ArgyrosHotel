import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useBookingDraft } from "../../hooks/useBookingDraft";
import { useServices } from "../../hooks/useServices";
import type { SelectedService } from "../../types/bookingDraft";

const INCLUDED_AMENITIES = [
  "WiFi gratuito",
  "Piscina",
  "Gimnasio",
  "Parking",
  "Atención 24 horas",
  "Comidas y cenas",
  "Servicio de habitaciones",
  "Terraza",
  "Eventos",
];

export const BookingServicesPage = () => {
  const navigate = useNavigate();
  const { draft, updateDraft } = useBookingDraft();
  const { services, loading, error } = useServices();

  const [selection, setSelection] = useState<Record<string, number>>(() => {
    const initial: Record<string, number> = {};
    draft.selectedServices.forEach((s) => {
      initial[s.serviceId] = s.quantity;
    });
    return initial;
  });

  useEffect(() => {
    if (!draft.roomTypeId) {
      navigate("/booking/rooms");
    }
  }, [draft.roomTypeId, navigate]);

  const handleQuantityChange = (serviceId: string, quantity: number) => {
    setSelection((prev) => ({ ...prev, [serviceId]: quantity }));
  };

  const handleSubmit = () => {
    const selected: SelectedService[] = Object.entries(selection)
      .filter(([, qty]) => qty > 0)
      .map(([serviceId, quantity]) => ({ serviceId, quantity }));

    updateDraft({ selectedServices: selected });
    navigate("/booking/data");
  };

  if (loading) return <p>Cargando servicios...</p>;
  if (error) return <p>{error}</p>;

  return (
    <section>
      <h1>Servicios</h1>

      <div>
        <h2>Incluido en tu estancia</h2>
        <ul>
          {INCLUDED_AMENITIES.map((name) => (
            <li key={name}>{name}</li>
          ))}
        </ul>
      </div>

      <div>
        <h2>Servicios adicionales</h2>
        <ul>
          {services.map((service) => (
            <li key={service.id}>
              <strong>{service.name}</strong>
              <p>{service.description}</p>
              <p>{service.price}€ / unidad</p>
              <label>
                Cantidad:
                <input
                  type="number"
                  min={0}
                  value={selection[service.id] ?? 0}
                  onChange={(e) =>
                    handleQuantityChange(service.id, Number(e.target.value))
                  }
                />
              </label>
            </li>
          ))}
        </ul>
      </div>

      <button onClick={handleSubmit}>Continuar</button>
    </section>
  );
};
