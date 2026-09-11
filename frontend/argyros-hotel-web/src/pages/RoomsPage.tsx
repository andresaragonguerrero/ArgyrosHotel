import './RoomsPage.css';

export const RoomsPage = () => {
    return (
        <section className="rooms-section">
            <div className="rooms-background" />

            <div className="rooms-content">
                <div className="rooms-info">
                    <h2 className="rooms-info-title"></h2>
                    <p className="rooms-info-text"></p>
                </div>

                <div className="rooms-carousel">
                    <button className="carousel-item">
                        <img className="carousel-image" src="../assets/images/rooms/single-room.jpg" alt="Single room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="../assets/images/rooms/double-room.jpg" alt="Double room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="../assets/images/rooms/family-room.jpg" alt="Family room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="../assets/images/rooms/deluxe-room.jpg" alt="Deluxe room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="../assets/images/rooms/suite.jpg" alt="Suite room" />
                    </button>
                </div>
            </div>
        </section>
    );
};