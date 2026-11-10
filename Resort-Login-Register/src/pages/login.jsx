
import { useState } from "react";
import "./login.css";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const handleLogin = (e) => {
    e.preventDefault();

    // Later koppelen we dit aan de C# backend
    console.log("Login attempt:", email);
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

            <button type="submit" className="login-button">
              LOG IN →
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
