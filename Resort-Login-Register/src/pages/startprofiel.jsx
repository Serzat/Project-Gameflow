import { useState } from "react";
import "./startprofiel.css";

// De vier soorten resorts waaruit je kunt kiezen.
const RESORT_TYPES = [
  { waarde: "beach", label: "Beach resort", icoon: "zon" },
  { waarde: "winter", label: "Winter resort", icoon: "sneeuw" },
  { waarde: "cruise", label: "Cruise resort", icoon: "anker" },
  { waarde: "city", label: "City resort", icoon: "stad" },
];

// Velden van een profiel.
const PROFIEL_VELDEN = [
  { key: "resortType", label: "Type resort", type: "select", weergave: "kop" },
  { key: "locatie", label: "Locatie", type: "text", placeholder: "Bijv. Bali", weergave: "kop" },
  { key: "startbudget", label: "Startbudget", kortLabel: "Budget", type: "number", min: 0, eenheid: "€", weergave: "tegel" },
  { key: "aantalKamers", label: "Aantal kamers", kortLabel: "Kamers", type: "number", min: 0, weergave: "tegel" },
  { key: "aantalMedewerkers", label: "Aantal medewerkers", kortLabel: "Medewerkers", type: "number", min: 0, weergave: "tegel" },
  { key: "bezettingsgraad", label: "Bezettingsgraad", type: "number", min: 0, max: 100, eenheid: "%", weergave: "meter", kleur: "#327b81" },
  { key: "gasttevredenheid", label: "Gasttevredenheid", type: "number", min: 0, max: 100, eenheid: "%", weergave: "meter", kleur: "#f49a32" },
  { key: "duurzaamheidsscore", label: "Duurzaamheidsscore", type: "number", min: 0, max: 100, eenheid: "/100", weergave: "meter", kleur: "#6b9a5b" },
];

const LEEG_PROFIEL = {
  resortType: "",
  locatie: "",
  startbudget: "",
  aantalKamers: "",
  aantalMedewerkers: "",
  bezettingsgraad: "",
  gasttevredenheid: "",
  duurzaamheidsscore: "",
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
  potlood: (
    <>
      <path d="M12 20h9" />
      <path d="M16.5 3.5a2.12 2.12 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z" />
    </>
  ),
  check: <path d="M20 6 9 17l-5-5" />,
  pin: (
    <>
      <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z" />
      <circle cx="12" cy="10" r="3" />
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
  if (waarde === "") return "—";
  if (veld.key === "startbudget") return euroKort.format(Number(waarde));
  if (veld.eenheid === "%") return `${waarde}%`;
  if (veld.eenheid === "/100") return `${waarde}/100`;
  return Number(waarde).toLocaleString("nl-NL");
}

function aantalIngevuld(profiel) {
  return PROFIEL_VELDEN.filter((veld) => profiel[veld.key] !== "").length;
}

// PROFIELKAART

function ProfielKaart({ naam, data, onOpslaan }) {
  const [bewerken, setBewerken] = useState(false);
  const [concept, setConcept] = useState(data);

  const ingevuld = aantalIngevuld(data);
  const compleet = ingevuld === PROFIEL_VELDEN.length;

  const tegels = PROFIEL_VELDEN.filter((veld) => veld.weergave === "tegel");
  const meters = PROFIEL_VELDEN.filter((veld) => veld.weergave === "meter");

  const resortType = RESORT_TYPES.find((type) => type.waarde === data.resortType);

  const startBewerken = () => {
    setConcept(data);
    setBewerken(true);
  };

  const handleOpslaan = (e) => {
    e.preventDefault();
    onOpslaan(concept);
    setBewerken(false);
    // Later koppelen we dit aan de C# backend
  };

  return (
    <form
      className={bewerken ? "sp-kaart sp-kaart-bewerken" : "sp-kaart"}
      onSubmit={handleOpslaan}
    >
      <header className="sp-kaart-kop">
        <div className="sp-badge">{naam}</div>

        <div className="sp-kaart-titels">
          <h2>PROFIEL {naam}</h2>

          <div className="sp-kaart-info">
            {resortType ? (
              <span className={`sp-type sp-type-${resortType.waarde}`}>
                <Icoon naam={resortType.icoon} grootte={12} />
                {resortType.label}
              </span>
            ) : (
              <span className="sp-type sp-type-leeg">Geen type</span>
            )}

            <span className="sp-locatie">
              <Icoon naam="pin" grootte={13} />
              {data.locatie || "Nog geen locatie"}
            </span>
          </div>
        </div>

        <span className={compleet ? "sp-status sp-status-compleet" : "sp-status"}>
          {compleet ? "Compleet" : `${ingevuld}/${PROFIEL_VELDEN.length} ingevuld`}
        </span>
      </header>

      {bewerken ? (
        <div className="sp-formulier">
          {PROFIEL_VELDEN.map((veld) => {
            const id = `profiel-${naam}-${veld.key}`;
            return (
              <div key={veld.key} className="sp-veld">
                <label htmlFor={id}>{veld.label.toUpperCase()}</label>

                <div className="sp-input-wrap">
                  {veld.type === "select" ? (
                    <select
                      id={id}
                      value={concept[veld.key]}
                      onChange={(e) =>
                        setConcept({ ...concept, [veld.key]: e.target.value })
                      }
                      className={concept[veld.key] === "" ? "sp-select-leeg" : ""}
                    >
                      <option value="" disabled>
                        Kies een type
                      </option>
                      {RESORT_TYPES.map((type) => (
                        <option key={type.waarde} value={type.waarde}>
                          {type.label}
                        </option>
                      ))}
                    </select>
                  ) : (
                    <input
                      id={id}
                      type={veld.type}
                      min={veld.min}
                      max={veld.max}
                      placeholder={veld.placeholder || "0"}
                      value={concept[veld.key]}
                      onChange={(e) =>
                        setConcept({ ...concept, [veld.key]: e.target.value })
                      }
                      className={veld.eenheid ? "sp-met-eenheid" : ""}
                    />
                  )}

                  {veld.eenheid && (
                    <span className="sp-eenheid">{veld.eenheid}</span>
                  )}
                </div>
              </div>
            );
          })}
        </div>
      ) : (
        <div className="sp-kaart-body">
          <div className="sp-tegels">
            {tegels.map((veld) => (
              <div className="sp-tegel" key={veld.key}>
                <span className="sp-tegel-label">{veld.kortLabel}</span>
                <span className="sp-tegel-waarde">
                  {formatWaarde(veld, data[veld.key])}
                </span>
              </div>
            ))}
          </div>

          <div className="sp-meters">
            {meters.map((veld) => {
              const procent = Math.min(100, Math.max(0, Number(data[veld.key]) || 0));
              return (
                <div className="sp-meter" key={veld.key}>
                  <div className="sp-meter-tekst">
                    <span>{veld.label}</span>
                    <strong>{formatWaarde(veld, data[veld.key])}</strong>
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
      )}

      <footer className="sp-kaart-voet">
        {bewerken ? (
          <>
            <button
              key="annuleren"
              type="button"
              className="sp-knop sp-knop-tekst"
              onClick={() => setBewerken(false)}
            >
              Annuleren
            </button>
            <button key="opslaan" type="submit" className="sp-knop sp-knop-primair">
              <Icoon naam="check" grootte={16} />
              OPSLAAN
            </button>
          </>
        ) : (
          <button
            key="bewerken"
            type="button"
            className="sp-knop sp-knop-secundair"
            onClick={startBewerken}
          >
            <Icoon naam="potlood" grootte={15} />
            Bewerken
          </button>
        )}
      </footer>
    </form>
  );
}

// PAGINA

function Startprofiel() {
  const [profielen, setProfielen] = useState({
    A: { ...LEEG_PROFIEL },
    B: { ...LEEG_PROFIEL },
    C: { ...LEEG_PROFIEL },
    D: { ...LEEG_PROFIEL },
  });

  const opslaanProfiel = (naam, nieuweData) => {
    setProfielen((vorige) => ({ ...vorige, [naam]: nieuweData }));
  };

  const aantalCompleet = Object.values(profielen).filter(
    (profiel) => aantalIngevuld(profiel) === PROFIEL_VELDEN.length
  ).length;

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
              <p>Stel per profiel de beginsituatie van het resort in.</p>
            </div>

            <div className="sp-hero-stat">
              <span className="sp-hero-getal">
                {aantalCompleet}<small>/4</small>
              </span>
              <span className="sp-hero-stat-label">PROFIELEN COMPLEET</span>
            </div>
          </section>

          <div className="sp-grid">
            {Object.keys(profielen).map((naam) => (
              <ProfielKaart
                key={naam}
                naam={naam}
                data={profielen[naam]}
                onOpslaan={(nieuweData) => opslaanProfiel(naam, nieuweData)}
              />
            ))}
          </div>
        </main>
      </div>
    </div>
  );
}

export default Startprofiel;