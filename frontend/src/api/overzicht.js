import { apiRequest, ApiError } from "./client";

// Haalt het overzicht van het ingelogde team op:
// team, ronde, KPI's (met vorige waarde) en de events tot nu toe.
export async function getOverzicht(signal) {
  try {
    return await apiRequest("/overzicht", { signal });
  } catch (err) {
    // Afbreken (bijv. pagina verlaten) gewoon doorgeven
    if (err.name === "AbortError") throw err;

    if (err instanceof ApiError && err.status === 401) {
      throw new Error("Log in om het overzicht van je resort te zien.", { cause: err });
    }
    if (err instanceof ApiError && err.status === 400) {
      throw new Error("Je account hoort nog niet bij een team.", { cause: err });
    }
    if (err instanceof ApiError && err.status === 404) {
      throw new Error("Er is geen startprofiel gevonden voor jouw resort.", { cause: err });
    }
    throw new Error("Het overzicht kon niet worden geladen.", { cause: err });
  }
}