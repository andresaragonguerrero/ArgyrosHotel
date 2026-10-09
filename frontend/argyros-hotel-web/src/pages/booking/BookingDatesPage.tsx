import { useState } from "react";
import type { SubmitEvent } from "react";
import { useNavigate } from "react-router-dom";
import { useBookingDraft } from "../../hooks/useBookingDraft";

export const BookingDatesPage = () => {
  const navigate = useNavigate();
  const { draft, updateDraft } = useBookingDraft();

  const [startDate, setStartDate] = useState(draft.startDate ?? "");
  const [endDate, setEndDate] = useState(draft.endDate ?? "");
  const [adults, setAdults] = useState(draft.adults ?? 2);
  const [children, setChildren] = useState(draft.children ?? 0);
  const [seniors, setSeniors] = useState(draft.seniors ?? 0);

  const handleSubmit = (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    updateDraft({
      startDate,
      endDate,
      adults,
      children,
      seniors,
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
          <label>Adultos</label>
          <input
            type="number"
            min={1}
            value={adults}
            onChange={(e) => setAdults(Number(e.target.value))}
            required
          />
        </div>

        <div>
          <label>Niños</label>
          <input
            type="number"
            min={0}
            value={children}
            onChange={(e) => setChildren(Number(e.target.value))}
            required
          />
        </div>

        <div>
          <label>Ancianos</label>
          <input
            type="number"
            min={0}
            value={seniors}
            onChange={(e) => setSeniors(Number(e.target.value))}
            required
          />
        </div>

        <button type="submit">Ver habitaciones</button>
      </form>
    </section>
  );
};
