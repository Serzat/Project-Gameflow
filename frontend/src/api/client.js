// Een fout van de backend, met de statuscode erbij
export class ApiError extends Error {
  constructor(status, details) {
    super(`Request failed with status ${status}`);
    this.status = status;
    this.details = details;
  }
}

// een plek voor alle verzoeken naar de backend
export async function apiRequest(path, { body, ...options } = {}) {
  const request = {
    ...options,
    credentials: "include", // stuurt de sessiecookie mee
    headers: {
      ...(body !== undefined ? { "Content-Type": "application/json" } : {}),
      ...options.headers,
    },
    ...(body !== undefined ? { body: JSON.stringify(body) } : {}),
  };

  const response = await fetch(`/api${path}`, request);

  if (!response.ok) {
    let details = null;
    try {
      details = await response.json();
    } catch {
      // De backend stuurde geen JSON mee, bijvoorbeeld bij een 401
    }
    throw new ApiError(response.status, details);
  }

  // Sommige antwoorden zijn leeg, bijvoorbeeld login (200) en logout (204)
  const text = await response.text();
  return text ? JSON.parse(text) : null;
}