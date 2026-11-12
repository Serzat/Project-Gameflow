import { useState } from "react";
import { registerUser } from "../api/accounts";
import "./register.css";

function Register() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [team, setTeam] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleRegister = async (e) => {
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

    setLoading(true);

    try {
      const user = await registerUser(username, email, team, password);

      alert(`Account created! Welcome to the team, ${user.username}. You can now log in.`);

      // Na registreren naar de loginpagina
      window.location.href = "/";
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
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
              minLength={3}
              maxLength={30}
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

            <label htmlFor="team">TEAM</label>
            <select
              id="team"
              value={team}
              onChange={(e) => setTeam(e.target.value)}
              required
            >
              <option value="" disabled>
                Choose your resort
              </option>
              <option value="beach">Beach resort</option>
              <option value="winter">Winter resort</option>
              <option value="cruise">Cruise resort</option>
              <option value="city">City resort</option>
            </select>

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
              disabled={loading}
            >
              {loading ? "CREATING ACCOUNT..." : "CREATE ACCOUNT →"}
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