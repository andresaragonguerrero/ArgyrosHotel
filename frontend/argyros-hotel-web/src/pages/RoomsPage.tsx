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
                        <img className="carousel-image" src="" alt="Single room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="" alt="Double room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="" alt="Family room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="" alt="Deluxe room" />
                    </button>
                    <button className="carousel-item">
                        <img className="carousel-image" src="" alt="Suite room" />
                    </button>
                </div>
            </div>
        </section>
    );
};