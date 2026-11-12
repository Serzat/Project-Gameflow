const API_URL = "http://localhost:5030";

export async function loginUser(email, password) {
  const response = await fetch(`${API_URL}/accounts/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });

  if (response.status === 401) {
    throw new Error("Email or password is incorrect.");
  }

  if (!response.ok) {
    throw new Error("Something went wrong. Please try again.");
  }

  return response.json();
}

export async function registerUser(username, email, team, password) {
  const response = await fetch(`${API_URL}/accounts/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, email, team, password }),
  });

  if (response.status === 409) {
    throw new Error("An account with this email already exists.");
  }

  if (response.status === 400) {
    throw new Error("Please check your details and try again.");
  }

  if (!response.ok) {
    throw new Error("Something went wrong. Please try again.");
  }

  return response.json();
}