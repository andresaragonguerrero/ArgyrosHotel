import { useState } from "react";
import type { SubmitEvent } from "react";
import { useNavigate } from "react-router-dom";
import { useBookingDraft } from "../../hooks/useBookingDraft";

export const BookingDatesPage = () => {
  const navigate = useNavigate();
  const { draft, updateDraft } = useBookingDraft();

  const [startDate, setStartDate] = useState(draft.startDate ?? "");
  const [endDate, setEndDate] = useState(draft.endDate ?? "");
  const [numberOfGuests, setNumberOfGuests] = useState(
    draft.numberOfGuests ?? 1,
  );

  const handleSubmit = (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    updateDraft({
      startDate,
      endDate,
      numberOfGuests,
      roomTypeId: null,
      selectedServices: [],
    });
    navigate("/booking/rooms");
  };

  return (
    <section>
      <h1>Elige tus fechas</h1>

      <form onSubmit={handleSubmit}>
        <div>
          <label>Entrada</label>
          <input
            type="date"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
            required
          />
        </div>

        <div>
          <label>Salida</label>
          <input
            type="date"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
            required
          />
        </div>

        <div>
          <label>Huéspedes</label>
          <input
            type="number"
            min={1}
            value={numberOfGuests}
            onChange={(e) => setNumberOfGuests(Number(e.target.value))}
            required
          />
        </div>

        <button type="submit">Ver habitaciones</button>
      </form>
    </section>
  );
};
