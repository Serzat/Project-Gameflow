import Icoon from "./icoon";
import "./Layout.css";

// Zijbalk + bovenbalk die je om elke pagina heen zet.
// menuItems: lijst met { label, href, icoon }
// gebruiker: { naam, rol }
// kruimel:   lijst met teksten, bijv. ["Ronde 1", "Overzicht"]
function Layout({ menuItems, gebruiker, kruimel = [], children }) {
  const huidigPad = window.location.pathname;

  return (
    <div className="layout-page">
      {/* ZIJBALK */}
      <aside className="layout-sidebar">
        <div className="layout-brand">
          RESORT<br />MANAGEMENT
        </div>

        <nav className="layout-nav">
          {menuItems.map((item) => {
            const actief = item.href === huidigPad;
            return (
              <a
                key={item.href}
                href={item.href}
                className={actief ? "layout-link layout-link-actief" : "layout-link"}
                aria-current={actief ? "page" : undefined}
              >
                <Icoon naam={item.icoon} />
                <span>{item.label}</span>
              </a>
            );
          })}
        </nav>

        <div className="layout-nav-onder">
          <a href="/settings" className="layout-link">
            <Icoon naam="settings" />
            <span>Settings</span>
          </a>
          <a href="/" className="layout-link">
            <Icoon naam="logout" />
            <span>Logout</span>
          </a>
        </div>
      </aside>

      {/* HOOFDGEDEELTE */}
      <div className="layout-main">
        <header className="layout-topbar">
          <div className="layout-kruimel">
            {kruimel.map((deel, index) => {
              const laatste = index === kruimel.length - 1;
              return (
                <span key={deel} className="layout-kruimel-deel">
                  {laatste ? <strong>{deel}</strong> : deel}
                  {!laatste && <span className="layout-kruimel-scheiding">/</span>}
                </span>
              );
            })}
          </div>

          <div className="layout-gebruiker">
            <div className="layout-avatar">
              <Icoon naam="profiel" grootte={18} />
            </div>
            <div className="layout-gebruiker-tekst">
              <span className="layout-gebruiker-naam">{gebruiker.naam}</span>
              <span className="layout-gebruiker-rol">{gebruiker.rol}</span>
            </div>
          </div>
        </header>

        <main className="layout-content">{children}</main>
      </div>
    </div>
  );
}

export default Layout;