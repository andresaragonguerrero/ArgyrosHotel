using argyros_hotel_api.Domain.Bookings;
using argyros_hotel_api.Domain.Hotel;

namespace argyros_hotel_api.Domain.Services
{
    // Servicio encargado de manejar la lógica relacionada con la disponibilidad de las habitaciones
    // Verifica la disponibilidad del tipo de habitación solicitada para las fechas dadas, considera reservas existentes
    public class AvailabilityResult
    {
        public bool IsAvailable { get; }
        public int AvailableRooms { get; }
        public DateTime? NextAvailableDate { get; }

        public AvailabilityResult(
            bool isAvailable, 
            int availableRooms, 
            DateTime? nextAvailableDate = null)
        {
            IsAvailable = isAvailable;
            AvailableRooms = availableRooms;
            NextAvailableDate = nextAvailableDate;
        }
    }

    public class AvailabilityService
    {
        // Recupera el rango de las fechas de la reserva:
        // InMemoryBookingRepository -> GetBookingsByRoomTypeAndDateRangeAsync(Guid roomTypeId, DateTime startDate, DateTime endDate)

        // Verifica la disponibilidad comparando el número de reservas existentes
        // con la cantidad total de habitaciones de ese tipo:
        // InMemoryRoomRepository ->  GetByRoomTypeIdAsync(Guid roomTypeId)

        // Verificar si se produce un solapamiento de fechas
        // entre las reservas existentes y las fechas solicitadas

        // Devuelve el resultado indicando si hay habitaciones disponibles
        // En caso de que haya, indicará también el número de ellas
        // De lo contrario, indicará que no hay disponibilidad
        public AvailabilityResult CheckAvailability(
            RoomType roomType,
            IEnumerable<Booking> bookings,
            DateTime requestedStart,
            DateTime requestedEnd)
        {

            if (requestedStart >= requestedEnd)
                throw new ArgumentException("Invalid date range");

            // Verificar si se produce un solapamiento de fechas
            // entre las reservas existentes y las fechas solicitadas
            var overlappingBookings = bookings.Where(b =>
                b.StartDate < requestedEnd &&
                b.EndDate > requestedStart
            );

            // Contar reservas activas en ese rango
            var activeBookingsCount = overlappingBookings.Count();

            // Calcular disponibilidad
            var availableRooms = roomType.TotalRooms - activeBookingsCount;

            if (availableRooms < 0)
                availableRooms = 0;

            // Resultado
            return new AvailabilityResult(
                availableRooms > 0,
                availableRooms
            );
        } // CheckAvailability method.end

        public DateTime? FindNextAvailableDate(
            RoomType roomType,
            IEnumerable<Booking> allBookings,
            DateTime fromDate)
        {
            var date = fromDate;

            // Buscar el siguiente día disponible dentro de un año
            for (int i = 0; i < 365; i++)
            {
                var nextDay = date.AddDays(1);

                var overlappingBookings = allBookings.Where(b =>
                    b.StartDate < nextDay &&
                    b.EndDate > date
                );

                var activeBookingsCount = overlappingBookings.Count();
                var availableRooms = roomType.TotalRooms - activeBookingsCount;

                if (availableRooms > 0)
                    return nextDay;

                date = nextDay;
            }

            return null;
        }// FindNextAvailableDate method.end
    } // AvailabilityService.end
}
