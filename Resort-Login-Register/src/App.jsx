import Login from "./pages/login";
import Register from "./pages/register";
import Startprofiel from "./pages/startprofiel";
import Overzicht from "./pages/overzicht";

function App() {
  const path = window.location.pathname;

  if (path === "/register") {
    return <Register />;
  }

  if (path === "/startprofiel") {
    return <Startprofiel />;
  }

  if (path === "/overzicht") {
    return <Overzicht />;
  }

  return <Login />;
}

export default App;