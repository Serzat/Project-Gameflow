import { apiRequest, ApiError } from "./client";

// Haalt de ranglijst op: alle resorts gesorteerd op score.
export async function getRanglijst(signal) {
  try {
    return await apiRequest("/ranglijst", { signal });
  } catch (err) {
    if (err.name === "AbortError") throw err;

    if (err instanceof ApiError && err.status === 401) {
      throw new Error("Log in om de ranglijst te zien.", { cause: err });
    }
    throw new Error("De ranglijst kon niet worden geladen.", { cause: err });
  }
}
