export interface User {
  id: string;
  name: string;
  surname: string;
  email: string;
  isPremium: boolean;
}

export interface RegisterRequest {
  name: string;
  surname: string;
  email: string;
  password: string;
  isPremium: boolean;
}

export interface LoginRequest {
  email: string;
  password: string;
}
