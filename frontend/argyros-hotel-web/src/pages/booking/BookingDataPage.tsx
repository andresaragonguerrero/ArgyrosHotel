import { useState } from "react";
import type { SubmitEvent } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useBookingDraft } from "../../hooks/useBookingDraft";
import { useAuth } from "../../hooks/useAuth";

export const BookingDataPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { draft, updateDraft } = useBookingDraft();
  const { user } = useAuth();

  const [name, setName] = useState(draft.guestData?.name ?? user?.name ?? "");
  const [surname, setSurname] = useState(
    draft.guestData?.surname ?? user?.surname ?? "",
  );
  const [email, setEmail] = useState(
    draft.guestData?.email ?? user?.email ?? "",
  );
  const [emailConfirm, setEmailConfirm] = useState(
    draft.guestData?.email ?? user?.email ?? "",
  );
  const [acceptTerms, setAcceptTerms] = useState(
    draft.guestData?.acceptTerms ?? false,
  );
  const [acceptPrivacy, setAcceptPrivacy] = useState(
    draft.guestData?.acceptPrivacy ?? false,
  );
  const [localError, setLocalError] = useState<string | null>(null);

  if (!draft.roomTypeId) {
    navigate("/booking/rooms");
    return null;
  }

  const handleSubmit = (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLocalError(null);

    if (email !== emailConfirm) {
      setLocalError("Los correos no coinciden.");
      return;
    }

    if (!acceptTerms || !acceptPrivacy) {
      setLocalError("Debes aceptar las condiciones y las políticas.");
      return;
    }

    updateDraft({
      guestData: { name, surname, email, acceptTerms, acceptPrivacy },
    });

    if (!user) {
      navigate("/login", { state: { from: location.pathname } });
      return;
    }

    navigate("/booking/confirmation");
  };

  return (
    <section>
      <h1>Datos personales</h1>

      {!user && (
        <p>
          ¿Ya tienes cuenta? <a href="/login">Inicia sesión</a> para aplicar tus
          descuentos.
        </p>
      )}

      <form onSubmit={handleSubmit}>
        <div>
          <label>Nombre</label>
          <input
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>

        <div>
          <label>Apellidos</label>
          <input
            value={surname}
            onChange={(e) => setSurname(e.target.value)}
            required
          />
        </div>

        <div>
          <label>Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <div>
          <label>Confirmar email</label>
          <input
            type="email"
            value={emailConfirm}
            onChange={(e) => setEmailConfirm(e.target.value)}
            required
          />
        </div>

        <div>
          <label>
            <input
              type="checkbox"
              checked={acceptTerms}
              onChange={(e) => setAcceptTerms(e.target.checked)}
            />
            Acepto las condiciones de la reserva
          </label>
        </div>

        <div>
          <label>
            <input
              type="checkbox"
              checked={acceptPrivacy}
              onChange={(e) => setAcceptPrivacy(e.target.checked)}
            />
            Acepto las políticas de privacidad
          </label>
        </div>

        {localError && <p style={{ color: "red" }}>{localError}</p>}

        <button type="submit">Continuar</button>
      </form>
    </section>
  );
};
