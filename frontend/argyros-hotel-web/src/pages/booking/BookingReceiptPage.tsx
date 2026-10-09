import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getBookingById } from "../../services/bookingApi";
import type { Booking } from "../../types/booking";

export const BookingReceiptPage = () => {
  const { id } = useParams<{ id: string }>();
  const [booking, setBooking] = useState<Booking | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;

    const load = async () => {
      try {
        setLoading(true);
        const data = await getBookingById(id);
        setBooking(data);
      } catch {
        setError("No se pudo cargar la reserva.");
      } finally {
        setLoading(false);
      }
    };

    void load();
  }, [id]);

  if (loading) return <p>Cargando recibo...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (!booking) return null;

  return (
    <section>
      <h1>Reserva confirmada</h1>

      <div>
        <p>Reserva: {booking.id}</p>
        <p>Entrada: {booking.startDate.slice(0, 10)}</p>
        <p>Salida: {booking.endDate.slice(0, 10)}</p>
        <p>
          Huéspedes: {booking.adults} adultos, {booking.children} niños,{" "}
          {booking.seniors} ancianos
        </p>
      </div>

      <div>
        <h2>Desglose</h2>
        <p>Habitación: {booking.finalPrice}€</p>

        {booking.addOns && booking.addOns.length > 0 && (
          <ul>
            {booking.addOns.map((a) => (
              <li key={a.id}>
                Servicio {a.serviceId} × {a.quantity} = {a.totalPrice}€
              </li>
            ))}
          </ul>
        )}

        <p>
          <strong>
            Total:{" "}
            {booking.finalPrice +
              (booking.addOns?.reduce((sum, a) => sum + a.totalPrice, 0) ?? 0)}
            €
          </strong>
        </p>
      </div>
    </section>
  );
};
