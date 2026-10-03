import { useState } from "react";
import type { User, RegisterRequest, LoginRequest } from "../types/user";
import { userService } from "../services/userService";

const STORAGE_KEY = "argyros_user";

export const useAuth = () => {
  const [user, setUser] = useState<User | null>(() => {
    const stored = localStorage.getItem(STORAGE_KEY);
    return stored ? JSON.parse(stored) : null;
  });
  const [loading, setLoading] = useState<boolean>(false);
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

  return { user, loading, error, register, login, logout };
};
