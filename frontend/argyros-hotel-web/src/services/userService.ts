import { api } from "./api";
import type { User, RegisterRequest, LoginRequest } from "../types/user";

export const userService = {
  register: async (request: RegisterRequest): Promise<User> => {
    const response = await api.post<User>("/users", request);
    return response.data;
  },

  login: async (request: LoginRequest): Promise<User> => {
    const response = await api.post<User>("/users/login", request);
    return response.data;
  },
};
