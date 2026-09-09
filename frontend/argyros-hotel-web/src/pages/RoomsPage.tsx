import { useEffect, useState, type KeyboardEvent } from 'react';
import { roomService } from '../services/roomService';
import type { RoomType, Room, AvailabilityResult } from '../types/room';
import './RoomsPage.css';

export const RoomsPage = () => {
    const [roomTypes, setRoomTypes] = useState<RoomType[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const [startDate, setStartDate] = useState<string>('');
    const [endDate, setEndDate] = useState<string>('');
    const [availabilityResults, setAvailabilityResults] = useState<Record<string, AvailabilityResult>>({});
    const [checkingAvailability, setCheckingAvailability] = useState<boolean>(false);

    const [selectedRoomType, setSelectedRoomType] = useState<RoomType | null>(null);
    const [physicalRooms, setPhysicalRooms] = useState<Room[]>([]);
    const [loadingRooms, setLoadingRooms] = useState<boolean>(false);

    useEffect(() => {
        const fetchRoomTypes = async () => {
            try {
                setLoading(true);
                const data = await roomService.getRoomTypes();
                setRoomTypes(data);
            } catch (err) {
                console.error('Error al obtener los tipos de habitación:', err);
                setError('No se pudo obtener la información de las habitaciones.');
            } finally {
                setLoading(false);
            }
        };

        fetchRoomTypes();
    }, []);

    const handleCheckAvailability = async () => {
        if (!startDate || !endDate) {
            alert('Por favor selecciona ambas fechas.');
            return;
        }

        setCheckingAvailability(true);
        const results: Record<string, AvailabilityResult> = {};

        try {
            await Promise.all(
                roomTypes.map(async (rt) => {
                    const res = await roomService.checkAvailability(rt.id, startDate, endDate);
                    results[rt.id] = res;
                })
            );
            setAvailabilityResults(results);
        } catch (err) {
            console.error('Error al comprobar disponibilidad:', err);
        } finally {
            setCheckingAvailability(false);
        }
    };

    const handleOpenModal = async (roomType: RoomType) => {
        setSelectedRoomType(roomType);
        setLoadingRooms(true);
        try {
            const rooms = await roomService.getRoomsByRoomType(roomType.id);
            setPhysicalRooms(rooms);
        } catch (err) {
            console.error('Error al obtener habitaciones físicas:', err);
        } finally {
            setLoadingRooms(false);
        }
    };

    const handleCloseModal = () => {
        setSelectedRoomType(null);
        setPhysicalRooms([]);
    };

    const handleKeyDownModal = (event: KeyboardEvent<HTMLDialogElement>) => {
        if (event.key === 'Escape') {
            handleCloseModal();
        }
    };

    const renderAvailabilityBadge = (availability?: AvailabilityResult) => {
        if (!availability) return null;

        if (availability.isAvailable) {
            return <div className="availability-info available">Disponible</div>;
        }

        const nextDate = availability.nextAvailableDate
            ? new Date(availability.nextAvailableDate).toLocaleDateString()
            : null;

        return (
            <div className="availability-info unavailable">
                <span>
                    No disponible.{nextDate ? ` Siguiente fecha libre: ${nextDate}` : ''}
                </span>
            </div>
        );
    };

    if (loading) {
        return <div className="state-message">Cargando habitaciones...</div>;
    }

    if (error) {
        return <div className="state-message error-message">{error}</div>;
    }

    return (
        <section className="rooms-container">
            <h1 className="rooms-title">Tipos de Habitación</h1>
            <p className="rooms-subtitle">Explora las opciones de hospedaje y verifica su disponibilidad</p>

            <div className="filter-section">
                <div className="filter-group">
                    <label htmlFor="start-date">Fecha Entrada:</label>
                    <input
                        id="start-date"
                        type="date"
                        value={startDate}
                        onChange={(e) => setStartDate(e.target.value)}
                    />
                </div>
                <div className="filter-group">
                    <label htmlFor="end-date">Fecha Salida:</label>
                    <input
                        id="end-date"
                        type="date"
                        value={endDate}
                        onChange={(e) => setEndDate(e.target.value)}
                    />
                </div>
                <button
                    type="button"
                    className="btn btn-primary"
                    onClick={handleCheckAvailability}
                    disabled={checkingAvailability}
                >
                    {checkingAvailability ? 'Consultando...' : 'Comprobar Disponibilidad'}
                </button>
            </div>

            <div className="rooms-grid">
                {roomTypes.map((roomType) => (
                    <article key={roomType.id} className="room-card">
                        <div>
                            <header className="room-card-header">
                                <h2 className="room-type-title">{roomType.name}</h2>
                            </header>

                            <div className="room-details">
                                <div className="room-detail-item">
                                    <span>Capacidad máxima:</span>
                                    <strong>{roomType.capacity} personas</strong>
                                </div>
                                <div className="room-detail-item">
                                    <span>Habitaciones totales:</span>
                                    <strong>{roomType.totalRooms} unidades</strong>
                                </div>
                            </div>

                            {renderAvailabilityBadge(availabilityResults[roomType.id])}
                        </div>

                        <footer>
                            <div className="price-tag">
                                ${roomType.basePrice.toLocaleString()}
                                <span className="price-period"> / noche</span>
                            </div>
                            <button
                                type="button"
                                className="btn btn-secondary"
                                onClick={() => handleOpenModal(roomType)}
                            >
                                Ver Números de Habitación
                            </button>
                        </footer>
                    </article>
                ))}
            </div>

            {selectedRoomType && (
                <dialog
                    open
                    className="modal-content"
                    onClose={handleCloseModal}
                    onKeyDown={handleKeyDownModal}
                >
                    <div className="modal-header">
                        <h2>Habitaciones: {selectedRoomType.name}</h2>
                        <button
                            type="button"
                            className="close-btn"
                            onClick={handleCloseModal}
                            aria-label="Cerrar modal"
                        >
                            &times;
                        </button>
                    </div>
                    {loadingRooms && <p>Cargando lista de habitaciones...</p>}
                    {!loadingRooms && physicalRooms.length > 0 && (
                        <div>
                            <p>Números asignados a este tipo:</p>
                            <div className="rooms-list">
                                {physicalRooms.map((room) => (
                                    <span key={room.id} className="room-chip">
                                        Hab. {room.roomNumber}
                                    </span>
                                ))}
                            </div>
                        </div>
                    )}
                    {!loadingRooms && physicalRooms.length === 0 && (
                        <p>No se encontraron habitaciones físicas asignadas.</p>
                    )}
                </dialog>
            )}
        </section>
    );
};