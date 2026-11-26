import { apiRequest, ApiError } from "./client";

// Haalt de feedback op over de laatste ronde van het ingelogde team:
// KPI-veranderingen, score, positie en adviezen.
export async function getFeedback(signal) {
  try {
    return await apiRequest("/feedback", { signal });
  } catch (err) {
    if (err.name === "AbortError") throw err;

    if (err instanceof ApiError && err.status === 401) {
      throw new Error("Log in om de feedback van je resort te zien.", { cause: err });
    }
    if (err instanceof ApiError && err.status === 400) {
      throw new Error("Je account hoort nog niet bij een team.", { cause: err });
    }
    if (err instanceof ApiError && err.status === 404) {
      throw new Error("Er is nog geen ronde berekend voor jouw resort.", { cause: err });
    }
    throw new Error("De feedback kon niet worden geladen.", { cause: err });
  }
}
