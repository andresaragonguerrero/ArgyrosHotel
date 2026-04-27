using System.Text.Json.Serialization;

namespace HotelManagement.Domain.Hotel
{
    // En este archivo se define la configuración del hotel
    // El hotel cerrará durante la temporada baja;
    // por eso se acota el rango de fechas 
    public class HotelConfig
    {
        public DateTime SeasonStart { get; private set; }

        public DateTime SeasonEnd { get; private set; }

        [JsonConstructor]
        public HotelConfig(DateTime seasonStart, DateTime seasonEnd)
        {
            if (seasonEnd <= seasonStart)
                throw new ArgumentException("Season end must be after season start.");

            SeasonStart = seasonStart;
            SeasonEnd = seasonEnd;
        }

        public bool IsDateWithinSeason(DateTime date)
        {
            return date >= SeasonStart && date <= SeasonEnd;
        }
    }
}