import { useState } from "react";
import { mockRooms } from "../mocks/rooms.mock";
import "./RoomsPage.css";

export const RoomsPage = () => {
  const [selectedRoom, setSelectedRoom] = useState(mockRooms[0]);

  return (
    <section className="rooms-section">
      <div
        className="rooms-background"
        style={{ backgroundImage: `url(${selectedRoom.imageUrl})` }}
      />

      <div className="rooms-content">
        <div className="rooms-info">
          <h2 className="rooms-info-title">{selectedRoom.name}</h2>
          <p className="rooms-info-text">
            {selectedRoom.capacity} personas · desde {selectedRoom.basePrice}€
          </p>
        </div>

        <div className="rooms-carousel">
          {mockRooms.map((room) => (
            <button
              key={room.id}
              className="carousel-item"
              onClick={() => setSelectedRoom(room)}
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
