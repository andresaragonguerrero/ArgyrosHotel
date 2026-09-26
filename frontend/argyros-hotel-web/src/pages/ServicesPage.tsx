import { useEffect, useState } from "react";
import { useServices } from "../hooks/useServices";
import { serviceService } from "../services/serviceService";
import type { BookingAddOn } from "../types/service";

const TEST_BOOKING_ID = "b0000001-1111-1111-1111-111111111111";

export const ServicesPage = () => {
  const { services, loading, error } = useServices();
  const [addOns, setAddOns] = useState<BookingAddOn[]>([]);
  const [message, setMessage] = useState<string | null>(null);

  const loadAddOns = async () => {
    try {
      const data = await serviceService.getAddOnsByBooking(TEST_BOOKING_ID);
      setAddOns(data);
    } catch {
      setMessage("No se pudieron cargar los extras de la reserva.");
    }
  };

  useEffect(() => {
    loadAddOns();
  }, []);

  const handleHire = async (serviceId: string) => {
    setMessage(null);
    try {
      await serviceService.createAddOn({
        bookingId: TEST_BOOKING_ID,
        serviceId,
        quantity: 1,
      });
      setMessage("Servicio contratado correctamente.");
      await loadAddOns();
    } catch (err: any) {
      const detail = err?.response?.data ?? "Error al contratar el servicio.";
      setMessage(String(detail));
    }
  };

  const handleDelete = async (id: string) => {
    setMessage(null);
    try {
      await serviceService.deleteAddOn(id);
      setMessage("Servicio eliminado de la reserva.");
      await loadAddOns();
    } catch {
      setMessage("Error al eliminar el servicio.");
    }
  };

  if (loading) return <p>Cargando servicios...</p>;
  if (error) return <p>{error}</p>;

  return (
    <section>
      <h1>Servicios disponibles</h1>

      <ul>
        {services.map((s) => (
          <li key={s.id}>
            <strong>{s.name}</strong> — {s.description} — {s.price}€
            <button onClick={() => handleHire(s.id)}>Contratar</button>
          </li>
        ))}
      </ul>

      <h2>Extras de la reserva {TEST_BOOKING_ID}</h2>
      {addOns.length === 0 ? (
        <p>No hay extras contratados.</p>
      ) : (
        <ul>
          {addOns.map((a) => (
            <li key={a.id}>
              Servicio {a.serviceId} — {a.quantity} × {a.unitPrice}€ ={" "}
              {a.totalPrice}€
              <button onClick={() => handleDelete(a.id)}>Eliminar</button>
            </li>
          ))}
        </ul>
      )}

      {message && <p>{message}</p>}
    </section>
  );
};
