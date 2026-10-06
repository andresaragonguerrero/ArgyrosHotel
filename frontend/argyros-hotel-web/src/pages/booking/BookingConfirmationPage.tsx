import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useBookingDraft } from "../../hooks/useBookingDraft";
import { useAuth } from "../../hooks/useAuth";
import { getQuote, createBooking } from "../../services/bookingApi";
import { serviceService } from "../../services/serviceService";
import type { QuoteResponse } from "../../types/booking";

export const BookingConfirmationPage = () => {
  const navigate = useNavigate();
  const { draft, clearDraft } = useBookingDraft();
  const { user } = useAuth();

  const [quote, setQuote] = useState<QuoteResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (
      !draft.roomTypeId ||
      !draft.startDate ||
      !draft.endDate ||
      !draft.guestData
    ) {
      navigate("/booking/dates");
      return;
    }

    const load = async () => {
      try {
        setLoading(true);
        const data = await getQuote({
          userId: user?.id ?? "",
          roomTypeId: draft.roomTypeId!,
          startDate: draft.startDate!,
          endDate: draft.endDate!,
          numberOfGuests: draft.numberOfGuests!,
          selectedServices: draft.selectedServices,
        });
        setQuote(data);
      } catch {
        setError("No se pudo calcular el precio.");
      } finally {
        setLoading(false);
      }
    };

    void load();
  }, [draft, user, navigate]);

  const handleConfirm = async () => {
    if (!user) {
      navigate("/login");
      return;
    }

    setSubmitting(true);
    setError(null);

    try {
      const booking = await createBooking({
        userId: user.id,
        roomTypeId: draft.roomTypeId!,
        startDate: draft.startDate!,
        endDate: draft.endDate!,
        numberOfGuests: draft.numberOfGuests!,
      });

      await Promise.all(
        draft.selectedServices.map((item) =>
          serviceService.createAddOn({
            bookingId: booking.id,
            serviceId: item.serviceId,
            quantity: item.quantity,
          }),
        ),
      );

      clearDraft();
      navigate(`/booking/receipt/${booking.id}`);
    } catch {
      setError("No se pudo confirmar la reserva.");
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) return <p>Calculando precio...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (!quote) return null;

  return (
    <section>
      <h1>Confirmar reserva</h1>

      <div>
        <p>Entrada: {draft.startDate}</p>
        <p>Salida: {draft.endDate}</p>
        <p>Huéspedes: {draft.numberOfGuests}</p>
      </div>

      <div>
        <h2>Desglose</h2>
        <p>Habitación: {quote.roomFinalPrice}€</p>
        {quote.services.length > 0 && (
          <ul>
            {quote.services.map((line) => (
              <li key={line.serviceId}>
                {line.name} × {line.quantity} = {line.totalPrice}€
              </li>
            ))}
          </ul>
        )}
        <p>
          <strong>Total: {quote.grandTotal}€</strong>
        </p>
      </div>

      {!user && (
        <p style={{ color: "orange" }}>
          Debes iniciar sesión para confirmar la reserva.
        </p>
      )}

      <button onClick={handleConfirm} disabled={submitting}>
        {submitting ? "Confirmando..." : "Confirmar reserva"}
      </button>
    </section>
  );
};
