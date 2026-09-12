import { useState } from "react";
import { mockRooms } from "../mocks/rooms.mock";
import "./RoomsPage.css";

export const RoomsPage = () => {
  const [selectedIndex, setSelectedIndex] = useState(0);
  const selectedRoom = mockRooms[selectedIndex];

  const handlePrev = () => {
    setSelectedIndex((prev) => (prev === 0 ? mockRooms.length - 1 : prev - 1));
  };

  const handleNext = () => {
    setSelectedIndex((prev) => (prev === mockRooms.length - 1 ? 0 : prev + 1));
  };

  return (
    <section className="rooms-section">
      <div
        className="rooms-background"
        style={{ backgroundImage: `url(${selectedRoom.imageUrl})` }}
      />

      <div className="rooms-content">
        <div className="rooms-info">
          <h2 className="rooms-info-title">{selectedRoom.name}</h2>
          <p className="rooms-info-meta">
            {selectedRoom.capacity} personas · {selectedRoom.squareMeters} m² ·
            desde {selectedRoom.basePrice}€
          </p>
          <p className="rooms-info-text">{selectedRoom.description}</p>
        </div>

        <div className="rooms-empty" />

        <div className="rooms-actions">
          <button type="button" className="rooms-action-btn">
            Reservar habitación
          </button>
          <button
            type="button"
            className="rooms-action-btn"
            onClick={handlePrev}
          >
            Desplazarse izquierda
          </button>
          <button
            type="button"
            className="rooms-action-btn"
            onClick={handleNext}
          >
            Desplazarse derecha
          </button>
        </div>

        <div className="rooms-carousel">
          {mockRooms.map((room, index) => (
            <button
              key={room.id}
              className="carousel-item"
              onClick={() => setSelectedIndex(index)}
            >
              <img
                className="carousel-image"
                src={room.imageUrl}
                alt={room.name}
              />
            </button>
          ))}
        </div>
      </div>
    </section>
  );
};
