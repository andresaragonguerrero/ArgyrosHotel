namespace argyros_hotel_api.Domain.Services
{
    // Servicio encargado de manejar la lógica relacionada con la disponibilidad de las habitaciones
    // Verifica la disponibilidad del tipo de habitación solicitada para las fechas dadas, considera reservas existentes
    public class AvailabilityService
    {
        // Recupera el rango de las fechas de la reserva:
        // InMemoryBookingRepository -> GetBookingsByRoomTypeAndDateRangeAsync(Guid roomTypeId, DateTime startDate, DateTime endDate)

        // Verifica la disponibilidad comparando el número de reservas existentes con la cantidad total de habitaciones de ese tipo:
        // InMemoryRoomRepository ->  GetByRoomTypeIdAsync(Guid roomTypeId)

        // Verificar si se produce un solapamiento de fechas entre las reservas existentes y las fechas solicitadas

        // Devuelve el resultado indicando si hay habitaciones disponibles
        // En caso de que haya, indicará también el número de ellas
        // De lo contrario, indicará que no hay disponibilidad
    }
}
