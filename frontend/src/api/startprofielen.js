import { apiRequest, ApiError } from "./client";

// De vier profielen van een modus ("standaard" of "custom")
export const getStartprofielen = (modus, signal) =>
  apiRequest(`/startprofielen?modus=${modus}`, { signal });

// Welke modus heeft de spelbegeleider gekozen?
export const getGekozenModus = (signal) =>
  apiRequest("/startprofielen/modus", { signal });

// De spelbegeleider kiest een modus (alleen met de rol Spelbegeleider)
export async function kiesModus(modus) {
  try {
    return await apiRequest("/startprofielen/modus", {
      method: "PUT",
      body: { modus },
    });
  } catch (err) {
    if (err instanceof ApiError && err.status === 401) {
      throw new Error("Log in als spelbegeleider om het profiel te kiezen.", { cause: err });
    }
    if (err instanceof ApiError && err.status === 403) {
      throw new Error("Alleen de spelbegeleider mag het profiel kiezen.", { cause: err });
    }
    throw new Error("Het profiel kon niet worden opgeslagen.", { cause: err });
  }
}