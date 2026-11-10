
import { useState } from "react";
import "./register.css";

function Register() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");

  const handleRegister = (e) => {
    e.preventDefault();
    setError("");

    if (password !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    if (password.length < 8) {
      setError("Password must be at least 8 characters.");
      return;
    }

    // Later koppelen we dit aan de C# backend.
    // Stuur wachtwoorden niet naar console.log.
    alert("Registration form is valid. Backend not connected yet.");
  };

  return (
    <div className="register-page">
      <section className="register-left">
        <div className="register-content">

          <div className="register-brand">

            <h1>
              RESORT<br />
              MANAGEMENT
            </h1>

            <span className="register-subtitle">
              YOUR PARADISE AWAITS
            </span>
          </div>

          <div className="register-welcome">
            <div className="register-sun">☀</div>
            <h2>Join Paradise!</h2>
            <p>Start your resort adventure today.</p>
          </div>

          <form
            className="register-form"
            onSubmit={handleRegister}
          >
            <label htmlFor="username">USERNAME</label>
            <input
              id="username"
              type="text"
              placeholder="Choose a username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              autoComplete="username"
              required
            />

            <label htmlFor="register-email">
              EMAIL ADDRESS
            </label>
            <input
              id="register-email"
              type="email"
              placeholder="you@example.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              autoComplete="email"
              required
            />

            <label htmlFor="register-password">
              PASSWORD
            </label>
            <input
              id="register-password"
              type="password"
              placeholder="Create a password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              autoComplete="new-password"
              minLength={8}
              required
            />

            <label htmlFor="confirm-password">
              CONFIRM PASSWORD
            </label>
            <input
              id="confirm-password"
              type="password"
              placeholder="Repeat your password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              autoComplete="new-password"
              required
            />

            {error && (
              <p className="register-error" role="alert">
                {error}
              </p>
            )}

            <button
              type="submit"
              className="register-button"
            >
              CREATE ACCOUNT →
            </button>
          </form>

          <p className="register-login-link">
            Already have an account?{" "}
            <a href="/">Log in</a>
          </p>

        </div>

      </section>

      <section className="register-right">
        <div className="register-image-overlay">
          <span>YOUR ADVENTURE BEGINS</span>
        </div>
      </section>
    </div>
  );
}

export default Register;
