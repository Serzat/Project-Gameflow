import { apiRequest, ApiError } from "./client";

// Inloggen via Identity; de backend zet daarna een sessiecookie
export async function login(email, password) {
  try {
    await apiRequest("/auth/login?useCookies=true", {
      method: "POST",
      body: { email, password },
    });
  } catch (err) {
    if (err instanceof ApiError && err.status === 401) {
      throw new Error("Email or password is incorrect.");
    }
    throw new Error("Something went wrong. Please try again.");
  }
}

// Registreren met gebruikersnaam en team (ons eigen endpoint)
export async function signup(username, email, team, password) {
  try {
    return await apiRequest("/auth/signup", {
      method: "POST",
      body: { username, email, team, password },
    });
  } catch (err) {
    if (err instanceof ApiError && err.status === 400) {
      const errors = err.details?.errors ?? {};
      if (errors.DuplicateUserName || errors.DuplicateEmail) {
        throw new Error("An account with this email already exists.");
      }
      throw new Error("Please check your details and try again.");
    }
    throw new Error("Something went wrong. Please try again.");
  }
}

// Wie is er ingelogd? (gebruikt de cookie)
export const getMe = () => apiRequest("/auth/me");

// Uitloggen
export const logout = () => apiRequest("/auth/logout", { method: "POST" });