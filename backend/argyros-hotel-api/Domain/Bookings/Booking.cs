using System.Text.Json.Serialization;

namespace argyros_hotel_api.Domain.Bookings
{
    public class Booking
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public Guid RoomTypeId { get; private set; }

        public Guid? RoomId { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public int Adults { get; private set; }

        public int Children { get; private set; }

        public int Seniors { get; private set; }

        [JsonIgnore]
        public int NumberOfGuests => Adults + Children + Seniors;

        public decimal BasePrice { get; private set; }

        public decimal FinalPrice { get; private set; }

#pragma warning disable S107
        [JsonConstructor]
        public Booking(
            Guid id,
            Guid userId,
            Guid roomTypeId,
            DateTime startDate,
            DateTime endDate,
            int adults,
            int children,
            int seniors,
            decimal basePrice,
            decimal finalPrice)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId inválido", nameof(userId));

            if (roomTypeId == Guid.Empty)
                throw new ArgumentException("RoomTypeId inválido", nameof(roomTypeId));

            if (startDate >= endDate)
                throw new ArgumentException("La fecha de inicio debe ser anterior a la de fin");

            if (adults < 1)
                throw new ArgumentException("Debe haber al menos un adulto", nameof(adults));

            if (children < 0)
                throw new ArgumentException("El número de niños no puede ser negativo", nameof(children));

            if (seniors < 0)
                throw new ArgumentException("El número de ancianos no puede ser negativo", nameof(seniors));

            if (basePrice < 0 || finalPrice < 0)
                throw new ArgumentException("Los precios no pueden ser negativos");

            Id = id;
            UserId = userId;
            RoomTypeId = roomTypeId;
            StartDate = startDate;
            EndDate = endDate;
            Adults = adults;
            Children = children;
            Seniors = seniors;
            BasePrice = basePrice;
            FinalPrice = finalPrice;
        }
#pragma warning restore S107

        public void AssignRoom(Guid roomId)
        {
            if (roomId == Guid.Empty)
                throw new ArgumentException("RoomId inválido", nameof(roomId));

            RoomId = roomId;
        }
    }
}