namespace argyros_hotel_api.Domain.Services
{
    public interface IGuestSurchargeService
    {
        decimal CalculateExtraPerNight(int adults, int children, int seniors);
    }

    public class GuestSurchargeService : IGuestSurchargeService
    {
        private const int IncludedAdults = 2;
        private const decimal ExtraAdultRate = 20m;
        private const decimal ChildRate = 10m;
        private const decimal SeniorRate = 5m;

        public decimal CalculateExtraPerNight(int adults, int children, int seniors)
        {
            if (adults < 1)
                throw new ArgumentException("At least one adult is required");

            if (children < 0 || seniors < 0)
                throw new ArgumentException("Guest counts cannot be negative");

            var extraAdults = Math.Max(0, adults - IncludedAdults);

            return extraAdults * ExtraAdultRate
                 + children * ChildRate
                 + seniors * SeniorRate;
        }
    }
}