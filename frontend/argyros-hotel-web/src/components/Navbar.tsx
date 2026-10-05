import { Link } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export const Navbar = () => {
  const { user, logout } = useAuth();

  return (
    <nav
      style={{
        display: "flex",
        gap: "1.5rem",
        padding: "1rem 2rem",
        backgroundColor: "#1a202c",
        color: "white",
      }}
    >
      <strong style={{ fontSize: "1.2rem", marginRight: "auto" }}>
        Argyros Hotel
      </strong>

      <Link to="/rooms" style={{ color: "white", textDecoration: "none" }}>
        Habitaciones
      </Link>
      <Link to="/services" style={{ color: "white", textDecoration: "none" }}>
        Servicios
      </Link>
      <Link to="/bookings" style={{ color: "white", textDecoration: "none" }}>
        Reservas
      </Link>

      {user ? (
        <>
          <span>Hola, {user.name}</span>
          <button onClick={logout} style={{ cursor: "pointer" }}>
            Cerrar sesión
          </button>
        </>
      ) : (
        <>
          <Link to="/login" style={{ color: "white", textDecoration: "none" }}>
            Entrar
          </Link>
          <Link
            to="/register"
            style={{ color: "white", textDecoration: "none" }}
          >
            Registro
          </Link>
        </>
      )}
    </nav>
  );
};
