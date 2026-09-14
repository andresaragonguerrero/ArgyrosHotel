import { useState, useRef, useEffect } from "react";
import { mockRooms } from "../mocks/rooms.mock";
import "./RoomsPage.css";

export const RoomsPage = () => {
  const [selectedIndex, setSelectedIndex] = useState(0);
  const selectedRoom = mockRooms[selectedIndex];

  const itemsRef = useRef<(HTMLButtonElement | null)[]>([]);

  const goTo = (index: number) => {
    if (document.startViewTransition) {
      document.startViewTransition(() => setSelectedIndex(index));
    } else {
      setSelectedIndex(index);
    }
  };

  const handlePrev = () =>
    goTo(selectedIndex === 0 ? mockRooms.length - 1 : selectedIndex - 1);
  const handleNext = () =>
    goTo(selectedIndex === mockRooms.length - 1 ? 0 : selectedIndex + 1);

  useEffect(() => {
    itemsRef.current[selectedIndex]?.scrollIntoView({
      behavior: "smooth",
      inline: "nearest",
      block: "nearest",
    });
  }, [selectedIndex]);

  return (
    <section className="rooms-section">
      <div
        className="rooms-background"
        style={
          {
            backgroundImage: `url(${selectedRoom.imageUrl})`,
            viewTransitionName: `room-image-${selectedRoom.id}`,
          } as React.CSSProperties
        }
      />

      <div className="rooms-content">
        <div className="rooms-info">
          <h2 className="rooms-info__title">{selectedRoom.name}</h2>
          <p className="rooms-info__text">Desde {selectedRoom.basePrice}€</p>
          <div className="rooms-info__meta">
            <p className="rooms-info__meta-text">
              {selectedRoom.capacity} personas
            </p>
            <p className="rooms-info__meta-text">
              {selectedRoom.squareMeters} m²
            </p>
          </div>
          <p className="rooms-info__text">{selectedRoom.description}</p>
        </div>

        <div className="rooms-services" />

        <div className="rooms-actions">
          <button
            type="button"
            className="rooms-action__button rooms-action__button--reserve"
          >
            Reservar habitación
          </button>
          <button
            type="button"
            className="rooms-action__button"
            onClick={handlePrev}
          >
            <svg
              className="rooms-action__icon"
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              width="25"
              height="25"
            >
              <path
                fill="none"
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="m15 18l-6-6l6-6"
              />
            </svg>
          </button>
          <button
            type="button"
            className="rooms-action__button"
            onClick={handleNext}
          >
            <svg
              className="rooms-action__icon"
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 24 24"
              width="25"
              height="25"
            >
              <path
                fill="none"
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="m9 18l6-6l-6-6"
              />
            </svg>
          </button>
        </div>

        <div className="rooms-carousel">
          {mockRooms.map((room, index) => (
            <button
              key={room.id}
              ref={(el) => {
                itemsRef.current[index] = el;
              }}
              className={`carousel-item ${
                index === selectedIndex ? "carousel-item--selected" : ""
              }`}
              onClick={() => goTo(index)}
            >
              <img
                className="carousel-image"
                src={room.imageUrl}
                alt={room.name}
                style={
                  {
                    viewTransitionName:
                      room.id === selectedRoom.id
                        ? undefined
                        : `room-image-${room.id}`,
                  } as React.CSSProperties
                }
              />
            </button>
          ))}
        </div>
      </div>
    </section>
  );
};
