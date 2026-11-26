
import Feedback from "./pages/feedback";
import Login from "./pages/login";
import Register from "./pages/register";
import Startprofiel from "./pages/startprofiel";
import Overzicht from "./pages/overzicht";
import Beslissingen from "./pages/beslissingen";
import ExtraBeslissingen from "./pages/extra_beslissingen";
import Ranglijst from "./pages/ranglijst";

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

  if (path === "/feedback") {
    return <Feedback />;
  }

  if (path === "/ranglijst") {
    return <Ranglijst />;
  }

  return <Login />;
}

export default App;
