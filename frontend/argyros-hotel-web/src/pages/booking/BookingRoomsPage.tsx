import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useBookingDraft } from "../../hooks/useBookingDraft";
import { roomService } from "../../services/roomService";
import type { RoomType, AvailabilityResult } from "../../types/room";

interface AvailableRoom {
  roomType: RoomType;
  availability: AvailabilityResult;
}

export const BookingRoomsPage = () => {
  const navigate = useNavigate();
  const { draft, updateDraft } = useBookingDraft();
  const [rooms, setRooms] = useState<AvailableRoom[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!draft.startDate || !draft.endDate || !draft.numberOfGuests) {
      navigate("/booking/dates");
      return;
    }

    const load = async () => {
      try {
        setLoading(true);
        const types = await roomService.getRoomTypes();

        const checks = await Promise.all(
          types.map(async (roomType) => {
            const availability = await roomService.checkAvailability(
              roomType.id,
              draft.startDate!,
              draft.endDate!,
            );
            return { roomType, availability };
          }),
        );

        const filtered = checks.filter(
          (r) =>
            r.availability.isAvailable &&
            r.roomType.capacity >= draft.numberOfGuests!,
        );

        setRooms(filtered);
      } catch {
        setError("No se pudieron cargar las habitaciones.");
      } finally {
        setLoading(false);
      }
    };

    void load();
  }, [draft.startDate, draft.endDate, draft.numberOfGuests, navigate]);

  const handleSelect = (roomTypeId: string) => {
    updateDraft({ roomTypeId });
    navigate("/booking/services");
  };

  if (loading) return <p>Cargando habitaciones...</p>;
  if (error) return <p>{error}</p>;

  return (
    <section>
      <h1>Habitaciones disponibles</h1>
      <p>
        Del {draft.startDate} al {draft.endDate} para {draft.numberOfGuests}{" "}
        huésped(es)
      </p>

      {rooms.length === 0 ? (
        <p>No hay habitaciones disponibles para esas fechas.</p>
      ) : (
        <ul>
          {rooms.map(({ roomType }) => (
            <li key={roomType.id}>
              <h3>{roomType.name}</h3>
              <p>{roomType.description}</p>
              <p>{roomType.basePrice}€ / noche</p>
              <p>Capacidad: {roomType.capacity} personas</p>
              <button onClick={() => handleSelect(roomType.id)}>
                Seleccionar
              </button>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
};
