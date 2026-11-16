import { useState } from "react";
import "./startprofiel.css";

// DE VIER RESORT TYPES
// Locatie hoort bij het type en is in beide profielen hetzelfde.
const RESORT_TYPES = [
  { waarde: "beach", label: "Beach resort", icoon: "zon", locatie: "Bali" },
  { waarde: "winter", label: "Winter resort", icoon: "sneeuw", locatie: "Zermatt" },
  { waarde: "cruise", label: "Cruise resort", icoon: "anker", locatie: "Middellandse Zee" },
  { waarde: "city", label: "City resort", icoon: "stad", locatie: "Barcelona" },
];

// WELKE GEGEVENS EEN PROFIEL HEEFT
// weergave "tegel" = blokje bovenaan, "meter" = balkje eronder
const PROFIEL_VELDEN = [
  { key: "startbudget", kortLabel: "Budget", weergave: "tegel" },
  { key: "aantalKamers", kortLabel: "Kamers", weergave: "tegel" },
  { key: "aantalMedewerkers", kortLabel: "Medewerkers", weergave: "tegel" },
  { key: "bezettingsgraad", label: "Bezettingsgraad", eenheid: "%", weergave: "meter", kleur: "#327b81" },
  { key: "gasttevredenheid", label: "Gasttevredenheid", eenheid: "%", weergave: "meter", kleur: "#f49a32" },
  { key: "duurzaamheidsscore", label: "Duurzaamheidsscore", eenheid: "/100", weergave: "meter", kleur: "#6b9a5b" },
];

// STANDAARD PROFIEL
// Iedereen begint hetzelfde, welk type resort je ook hebt.
const STANDAARD_WAARDEN = {
  startbudget: 5000,
  aantalKamers: 10,
  aantalMedewerkers: 5,
  bezettingsgraad: 60,
  gasttevredenheid: 70,
  duurzaamheidsscore: 50,
};

// CUSTOM PROFIEL
// Elk type resort begint anders. (Voorbeeldwaarden, later aan te passen.)
const CUSTOM_WAARDEN = {
  beach: {
    startbudget: 6000,
    aantalKamers: 12,
    aantalMedewerkers: 6,
    bezettingsgraad: 75,
    gasttevredenheid: 80,
    duurzaamheidsscore: 45,
  },
  winter: {
    startbudget: 7500,
    aantalKamers: 8,
    aantalMedewerkers: 5,
    bezettingsgraad: 65,
    gasttevredenheid: 75,
    duurzaamheidsscore: 55,
  },
  cruise: {
    startbudget: 9000,
    aantalKamers: 6,
    aantalMedewerkers: 9,
    bezettingsgraad: 85,
    gasttevredenheid: 70,
    duurzaamheidsscore: 35,
  },
  city: {
    startbudget: 4500,
    aantalKamers: 15,
    aantalMedewerkers: 10,
    bezettingsgraad: 70,
    gasttevredenheid: 65,
    duurzaamheidsscore: 60,
  },
};

// De twee profielen waartussen je kunt schakelen
const PROFIELEN = {
  standaard: {
    label: "Standaard profiel",
    uitleg: "Elk resort begint met precies dezelfde startwaarden.",
    waardenVoor: () => STANDAARD_WAARDEN,
  },
  custom: {
    label: "Custom profiel",
    uitleg: "Elk type resort heeft zijn eigen startsituatie.",
    waardenVoor: (type) => CUSTOM_WAARDEN[type],
  },
};

const MENU_ITEMS = [
  { label: "Startprofiel", href: "/startprofiel", icoon: "profiel" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Host", href: "/host", icoon: "host" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];

// ICONEN

const ICONEN = {
  profiel: (
    <>
      <circle cx="12" cy="7" r="4" />
      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
    </>
  ),
  beslissingen: (
    <>
      <polyline points="9 11 12 14 22 4" />
      <path d="M21 12v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11" />
    </>
  ),
  ranglijst: <path d="M18 20V10M12 20V4M6 20v-6" />,
  host: (
    <>
      <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
      <circle cx="9" cy="7" r="4" />
      <path d="M23 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
    </>
  ),
  feedback: <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z" />,
  settings: <path d="M4 21v-7M4 10V3M12 21v-9M12 8V3M20 21v-5M20 12V3M1 14h6M9 8h6M17 16h6" />,
  logout: (
    <>
      <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" />
      <polyline points="16 17 21 12 16 7" />
      <path d="M21 12H9" />
    </>
  ),
  pin: (
    <>
      <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
      <circle cx="12" cy="10" r="3" />
    </>
  ),
  slot: (
    <>
      <rect x="3" y="11" width="18" height="11" rx="2" />
      <path d="M7 11V7a5 5 0 0 1 10 0v4" />
    </>
  ),
  // Iconen voor de resort types
  zon: (
    <>
      <circle cx="12" cy="12" r="4" />
      <path d="M12 2v2M12 20v2M4.93 4.93l1.41 1.41M17.66 17.66l1.41 1.41M2 12h2M20 12h2M6.34 17.66l-1.41 1.41M19.07 4.93l-1.41 1.41" />
    </>
  ),
  sneeuw: <path d="M12 2v20M2 12h20M4.93 4.93l14.14 14.14M19.07 4.93 4.93 19.07" />,
  anker: (
    <>
      <circle cx="12" cy="5" r="3" />
      <path d="M12 22V8M5 12H2a10 10 0 0 0 20 0h-3" />
    </>
  ),
  stad: <path d="M3 21h18M6 21V5h8v16M14 9h4v12M9 9h2M9 13h2M9 17h2" />,
};

function Icoon({ naam, grootte = 18 }) {
  return (
    <svg
      width={grootte}
      height={grootte}
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      {ICONEN[naam]}
    </svg>
  );
}

// HULPFUNCTIES

const euroKort = new Intl.NumberFormat("nl-NL", {
  style: "currency",
  currency: "EUR",
  notation: "compact",
  maximumFractionDigits: 1,
});

function formatWaarde(veld, waarde) {
  if (waarde === undefined || waarde === null) return "—";
  if (veld.key === "startbudget") return euroKort.format(waarde);
  if (veld.eenheid === "%") return `${waarde}%`;
  if (veld.eenheid === "/100") return `${waarde}/100`;
  return waarde.toLocaleString("nl-NL");
}

// RESORTKAART (alleen weergeven, niet bewerken)

function ResortKaart({ type, waarden, profielLabel }) {
  const tegels = PROFIEL_VELDEN.filter((veld) => veld.weergave === "tegel");
  const meters = PROFIEL_VELDEN.filter((veld) => veld.weergave === "meter");

  return (
    <article className="sp-kaart">
      <header className="sp-kaart-kop">
        <div className={`sp-badge sp-badge-${type.waarde}`}>
          <Icoon naam={type.icoon} grootte={20} />
        </div>

        <div className="sp-kaart-titels">
          <h2>{type.label.toUpperCase()}</h2>
          <span className="sp-locatie">
            <Icoon naam="pin" grootte={13} />
            {type.locatie}
          </span>
        </div>

        <span className="sp-status">
          <Icoon naam="slot" grootte={11} />
          {profielLabel}
        </span>
      </header>

      <div className="sp-kaart-body">
        <div className="sp-tegels">
          {tegels.map((veld) => (
            <div className="sp-tegel" key={veld.key}>
              <span className="sp-tegel-label">{veld.kortLabel}</span>
              <span className="sp-tegel-waarde">
                {formatWaarde(veld, waarden[veld.key])}
              </span>
            </div>
          ))}
        </div>

        <div className="sp-meters">
          {meters.map((veld) => {
            const procent = Math.min(100, Math.max(0, waarden[veld.key] ?? 0));
            return (
              <div className="sp-meter" key={veld.key}>
                <div className="sp-meter-tekst">
                  <span>{veld.label}</span>
                  <strong>{formatWaarde(veld, waarden[veld.key])}</strong>
                </div>
                <div className="sp-meter-balk">
                  <div
                    className="sp-meter-vulling"
                    style={{ width: `${procent}%`, background: veld.kleur }}
                  />
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </article>
  );
}

// PAGINA

function Startprofiel() {
  // Welk profiel staat aan: "standaard" of "custom"
  const [gekozenProfiel, setGekozenProfiel] = useState("standaard");
  const profiel = PROFIELEN[gekozenProfiel];

  const huidigPad = window.location.pathname;

  return (
    <div className="dash-page">
      {/* ZIJBALK */}
      <aside className="dash-sidebar">
        <div className="dash-brand">
          <span className="dash-brand-titel">
            RESORT<br />MANAGEMENT
          </span>
        </div>

        <nav className="dash-nav">
          {MENU_ITEMS.map((item) => {
            const actief = item.href === huidigPad;
            return (
              <a
                key={item.href}
                href={item.href}
                className={actief ? "dash-link dash-link-actief" : "dash-link"}
                aria-current={actief ? "page" : undefined}
              >
                <Icoon naam={item.icoon} />
                <span>{item.label}</span>
              </a>
            );
          })}
        </nav>

        <div className="dash-nav-onder">
          <a href="/settings" className="dash-link">
            <Icoon naam="settings" />
            <span>Settings</span>
          </a>
          <a href="/" className="dash-link">
            <Icoon naam="logout" />
            <span>Logout</span>
          </a>
        </div>
      </aside>

      {/* HOOFDGEDEELTE */}
      <div className="dash-main">
        <header className="dash-topbar">
          <div className="dash-kruimel">
            <span>Spelopzet</span>
            <span className="dash-kruimel-scheiding">/</span>
            <strong>Startprofiel</strong>
          </div>

          <div className="dash-gebruiker">
            <div className="dash-avatar">
              <Icoon naam="profiel" grootte={18} />
            </div>
            <div className="dash-gebruiker-tekst">
              <span className="dash-gebruiker-naam">Spelbegeleider</span>
              <span className="dash-gebruiker-rol">Beheerder</span>
            </div>
          </div>
        </header>

        <main className="dash-content">
          <section className="sp-hero">
            <div className="sp-hero-tekst">
              <span className="sp-hero-deco">✦ SPELOPZET ✦</span>
              <h1>Startprofielen</h1>
              <p>Kies met welke startsituatie de resorts beginnen.</p>
            </div>

            <div className="sp-hero-stat">
              <span className="sp-hero-getal">{RESORT_TYPES.length}</span>
              <span className="sp-hero-stat-label">RESORT TYPES</span>
            </div>
          </section>

          {/* SCHAKELAAR STANDAARD / CUSTOM */}
          <div className="sp-schakel-rij">
            <div className="sp-schakelaar" role="group" aria-label="Kies een profiel">
              {Object.entries(PROFIELEN).map(([sleutel, p]) => (
                <button
                  key={sleutel}
                  type="button"
                  className={
                    gekozenProfiel === sleutel
                      ? "sp-schakel-knop sp-schakel-knop-actief"
                      : "sp-schakel-knop"
                  }
                  aria-pressed={gekozenProfiel === sleutel}
                  onClick={() => setGekozenProfiel(sleutel)}
                >
                  {p.label}
                </button>
              ))}
            </div>

            <p className="sp-schakel-uitleg">{profiel.uitleg}</p>
          </div>

          <div className="sp-grid">
            {RESORT_TYPES.map((type) => (
              <ResortKaart
                key={type.waarde}
                type={type}
                waarden={profiel.waardenVoor(type.waarde)}
                profielLabel={gekozenProfiel === "standaard" ? "Standaard" : "Custom"}
              />
            ))}
          </div>
        </main>
      </div>
    </div>
  );
}

export default Startprofiel;