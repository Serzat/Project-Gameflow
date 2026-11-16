import { useState } from "react";
import { loginUser } from "../api/accounts";
import "./login.css";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const user = await loginUser(email, password);

      // Onthoud wie er ingelogd is, zodat andere pagina's dit kunnen gebruiken
      localStorage.setItem("user", JSON.stringify(user));

      alert(`Welcome back, ${user.username}! (Team: ${user.team})`);
      // Later: doorsturen naar de volgende pagina
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <section className="login-left">
        <div className="login-content">
          <div className="login-brand">
            <h1>RESORT<br />MANAGEMENT</h1>
            <span className="brand-subtitle">
              YOUR PARADISE AWAITS
            </span>
          </div>

          <div className="login-welcome">
            <div className="retro-sun">☀</div>
            <h2>Welcome Back!</h2>
            <p>Ready for another day in paradise?</p>
          </div>

          <form onSubmit={handleLogin} className="login-form">
            <label htmlFor="email">EMAIL ADDRESS</label>
            <input
              id="email"
              type="email"
              placeholder="you@example.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />

            <label htmlFor="password">PASSWORD</label>
            <div className="password-field">
              <input
                id="password"
                type={showPassword ? "text" : "password"}
                placeholder="Enter your password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
              <button
                type="button"
                className="show-password"
                onClick={() => setShowPassword(!showPassword)}
              >
                {showPassword ? "Hide" : "Show"}
              </button>
            </div>

            <a href="/forgot-password" className="forgot-link">
              Forgot password?
            </a>

            {error && (
              <p className="login-error" role="alert">
                {error}
              </p>
            )}

            <button type="submit" className="login-button" disabled={loading}>
              {loading ? "LOGGING IN..." : "LOG IN →"}
            </button>
          </form>

          <div className="login-divider">
            <span>OR</span>
          </div>

          <button type="button" className="google-button">
            Continue with Google
          </button>

          <p className="register-link">
            New to paradise? <a href="/register">Sign up</a>
          </p>
        </div>
      </section>

      <section className="login-right">
        <div className="image-overlay">
          <span>WELCOME TO PARADISE</span>
        </div>
      </section>
    </div>
  );
}

export default Login;