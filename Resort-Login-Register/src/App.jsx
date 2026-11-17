
import Login from "./pages/login";
import Register from "./pages/register";
import Startprofiel from "./pages/startprofiel";
import Overzicht from "./pages/overzicht";
import Beslissingen from "./pages/beslissingen";
import ExtraBeslissingen from "./pages/extra_beslissingen";

function App() {
  const path = window.location.pathname;

  if (path === "/register") {
    return <Register />;
  }

  if (path === "/startprofiel") {
    return <Startprofiel />;
  }

  if (path === "/beslissingen") {
    return <Beslissingen />;
  }

  if (path === "/extra-beslissingen") {
    return <ExtraBeslissingen />;
  }

  if (path === "/overzicht") {
    return <Overzicht />;
  }

  return <Login />;
}

export default App;
