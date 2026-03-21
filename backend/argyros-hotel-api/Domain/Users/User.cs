using System.Text.Json.Serialization;

namespace ArgrosHotel.Domain.Users
{
    // Los usuarios se distinguirán entre aquellos que son Premium y los que no
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsPremium { get; private set; }

        [JsonConstructor]
        public User(Guid id, string name, string surname, string email, string passwordHash, bool isPremium = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre no puede quedar vacío", nameof(name));

            if (string.IsNullOrWhiteSpace(surname))
                throw new ArgumentException("El apellido no puede quedar vacío", nameof(surname));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Se debe introducir una contraseña", nameof(passwordHash));

            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            PasswordHash = passwordHash;
            IsPremium = isPremium;
        }

        public void UpgradeToPremium() => IsPremium = true;
        public void DowngradePremium() => IsPremium = false;
    }
}