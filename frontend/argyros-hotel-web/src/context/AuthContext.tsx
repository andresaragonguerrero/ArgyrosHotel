import {
  createContext,
  useContext,
  useState,
  useMemo,
  type ReactNode,
} from "react";
import type { User, RegisterRequest, LoginRequest } from "../types/user";
import { userService } from "../services/userService";

interface AuthContextValue {
  user: User | null;
  loading: boolean;
  error: string | null;
  register: (request: RegisterRequest) => Promise<boolean>;
  login: (request: LoginRequest) => Promise<boolean>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

const STORAGE_KEY = "argyros_user";

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(() => {
    const stored = localStorage.getItem(STORAGE_KEY);
    return stored ? JSON.parse(stored) : null;
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const register = async (request: RegisterRequest): Promise<boolean> => {
    setLoading(true);
    setError(null);
    try {
      const newUser = await userService.register(request);
      setUser(newUser);
      localStorage.setItem(STORAGE_KEY, JSON.stringify(newUser));
      return true;
    } catch (err: any) {
      const detail = err?.response?.data ?? "Error al registrar el usuario.";
      setError(String(detail));
      return false;
    } finally {
      setLoading(false);
    }
  };

  const login = async (request: LoginRequest): Promise<boolean> => {
    setLoading(true);
    setError(null);
    try {
      const loggedUser = await userService.login(request);
      setUser(loggedUser);
      localStorage.setItem(STORAGE_KEY, JSON.stringify(loggedUser));
      return true;
    } catch (err: any) {
      const detail = err?.response?.data ?? "Credenciales incorrectas.";
      setError(String(detail));
      return false;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    setUser(null);
    localStorage.removeItem(STORAGE_KEY);
  };

  const value = useMemo(
    () => ({ user, loading, error, register, login, logout }),
    [user, loading, error],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuthContext = () => {
  const ctx = useContext(AuthContext);
  if (!ctx)
    throw new Error("useAuthContext debe usarse dentro de AuthProvider");
  return ctx;
};
