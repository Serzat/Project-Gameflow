import { useState } from "react";
import Layout from "../components/layout";
import Icoon from "../components/icoon";
import "./overzicht.css";

// Menu voor teams (spelers)
const TEAM_MENU = [
  { label: "Overzicht", href: "/overzicht", icoon: "overzicht" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Host", href: "/host", icoon: "host" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];

// VOORBEELDDATA
// Later halen we dit uit de C# backend.

const TEAM = {
  naam: "Team 1",
  resortNaam: "Sunny Beach Resort",
  resortType: { label: "Beach resort", icoon: "zon" },
  locatie: "Tropisch eiland",
  startprofiel: "A",
  budget: 500000,
  kamers: 80,
  medewerkers: 35,
};

const RONDE = {
  huidig: 1,
  totaal: 4,
};

const KPIS = [
  { key: "bezetting", label: "Bezettingsgraad", waarde: "70%", verandering: "+5%", richting: "omhoog", goed: true, balk: 70, kleur: "#327b81", icoon: "bed" },
  { key: "kamerprijs", label: "Gem. kamerprijs", waarde: "€ 120", verandering: "+€ 10", richting: "omhoog", goed: true, balk: 60, kleur: "#5c8f6b", icoon: "tag" },
  { key: "winst", label: "Winst & cashflow", waarde: "€ 120.000", verandering: "−€ 20.000", richting: "omlaag", goed: false, balk: 60, kleur: "#e58a24", icoon: "trend" },
  { key: "gast", label: "Gasttevredenheid", waarde: "70", eenheid: "/100", verandering: "+5", richting: "omhoog", goed: true, balk: 70, kleur: "#d4654f", icoon: "ster" },
  { key: "personeel", label: "Personeelstevredenheid", waarde: "65", eenheid: "/100", verandering: "+10", richting: "omhoog", goed: true, balk: 65, kleur: "#8a6fb0", icoon: "host" },
  { key: "duurzaam", label: "Duurzaamheidsscore", waarde: "60", eenheid: "/100", verandering: "+5", richting: "omhoog", goed: true, balk: 60, kleur: "#6b9a5b", icoon: "blad" },
  { key: "markt", label: "Marktaandeel", waarde: "12%", verandering: "+2%", richting: "omhoog", goed: true, balk: 12, kleur: "#3f7fa6", icoon: "taart", subtekst: "Potentiële gasten: 10.000" },
  { key: "bezet", label: "Bezette kamers", waarde: "56", eenheid: "/80", verandering: "+4", richting: "omhoog", goed: true, balk: 70, kleur: "#264f4b", icoon: "sleutel" },
];

const CATEGORIEEN = {
  weer: { label: "Weer", icoon: "wolk" },
  markt: { label: "Markt", icoon: "trend" },
  gasten: { label: "Gasten", icoon: "ster" },
  resort: { label: "Resort", icoon: "waarschuwing" },
};

const EVENTS = [
  {
    id: 1,
    categorie: "weer",
    titel: "Mooi weer verwacht",
    omschrijving: "Door het zonnige weer stijgt de vraag naar kamers de komende weken.",
    effect: "Vraag +8%",
    effectGoed: true,
    ronde: 1,
    nieuw: true,
  },
  {
    id: 2,
    categorie: "markt",
    titel: "Nieuwe concurrent in de regio",
    omschrijving: "Er opent een nieuw hotel in de buurt. Verwacht meer concurrentie op prijs.",
    effect: "Marktaandeel −2%",
    effectGoed: false,
    ronde: 1,
    nieuw: true,
  },
  {
    id: 3,
    categorie: "gasten",
    titel: "Positieve review van reisblogger",
    omschrijving: "Een bekende reisblogger is enthousiast over jouw resort en deelt dit online.",
    effect: "Gasttevredenheid +5",
    effectGoed: true,
    ronde: 1,
  },
  {
    id: 4,
    categorie: "resort",
    titel: "Storing in de airconditioning",
    omschrijving: "Een deel van de kamers moet tijdelijk worden gerepareerd.",
    effect: "Kosten +€ 4.000",
    effectGoed: false,
    ronde: 1,

  },
  {
    id: 5,
    categorie: "markt",
    titel: "Vakantieseizoen begint",
    omschrijving: "Scholen hebben vakantie, waardoor er meer potentiële gasten zijn.",
    effect: "Potentiële gasten +1.500",
    effectGoed: true,
    ronde: 1,
  },
];

const euro = new Intl.NumberFormat("nl-NL", {
  style: "currency",
  currency: "EUR",
  maximumFractionDigits: 0,
});

// ONDERDELEN

function KpiKaart({ kpi }) {
  return (
    <div className="ov-kpi" style={{ "--kpi-kleur": kpi.kleur }}>
      <div className="ov-kpi-kop">
        <span className="ov-kpi-icoon">
          <Icoon naam={kpi.icoon} grootte={16} />
        </span>
        <span className="ov-kpi-label">{kpi.label}</span>
      </div>

      <div className="ov-kpi-midden">
        <span className="ov-kpi-waarde">
          {kpi.waarde}
          {kpi.eenheid && <small>{kpi.eenheid}</small>}
        </span>
        <span className={kpi.goed ? "ov-trend ov-trend-goed" : "ov-trend ov-trend-slecht"}>
          <Icoon naam={kpi.richting} grootte={12} />
          {kpi.verandering}
        </span>
      </div>

      {kpi.subtekst && <span className="ov-kpi-subtekst">{kpi.subtekst}</span>}

      <div className="ov-kpi-balk">
        <div className="ov-kpi-vulling" style={{ width: `${kpi.balk}%` }} />
      </div>
    </div>
  );
}

function Nieuwsfeed() {
  const [filter, setFilter] = useState("alle");

  const zichtbareEvents =
    filter === "alle"
      ? EVENTS
      : EVENTS.filter((event) => event.categorie === filter);

  const filters = [
    { key: "alle", label: "Alle" },
    ...Object.entries(CATEGORIEEN).map(([key, cat]) => ({ key, label: cat.label })),
  ];

  return (
    <section className="ov-feed">
      <div className="ov-feed-kop">
        <div className="ov-feed-titel">
          <span className="ov-feed-titel-icoon">
            <Icoon naam="nieuws" grootte={18} />
          </span>
          <div>
            <h2>Nieuws & events</h2>
            <p>Gebeurtenissen die invloed hebben op jouw resort.</p>
          </div>
        </div>

        <div className="ov-filters">
          {filters.map((f) => (
            <button
              key={f.key}
              type="button"
              className={filter === f.key ? "ov-filter ov-filter-actief" : "ov-filter"}
              onClick={() => setFilter(f.key)}
              aria-pressed={filter === f.key}
            >
              {f.label}
            </button>
          ))}
        </div>
      </div>

      <ul className="ov-feed-lijst">
        {zichtbareEvents.map((event) => {
          const categorie = CATEGORIEEN[event.categorie];
          return (
            <li key={event.id} className="ov-event">
              <span className={`ov-event-icoon ov-cat-${event.categorie}`}>
                <Icoon naam={categorie.icoon} grootte={18} />
              </span>

              <div className="ov-event-tekst">
                <div className="ov-event-boven">
                  <h3>{event.titel}</h3>
                  {event.nieuw && <span className="ov-nieuw">NIEUW</span>}
                </div>
                <p>{event.omschrijving}</p>
                <div className="ov-event-meta">
                  <span>{categorie.label}</span>
                  <span>•</span>
                  <span>Ronde {event.ronde}</span>
                  <span>•</span>
                  <span>{event.datum}</span>
                </div>
              </div>

              <span className={event.effectGoed ? "ov-effect ov-effect-goed" : "ov-effect ov-effect-slecht"}>
                {event.effect}
              </span>
            </li>
          );
        })}

        {zichtbareEvents.length === 0 && (
          <li className="ov-feed-leeg">Geen events in deze categorie.</li>
        )}
      </ul>
    </section>
  );
}

// PAGINA

function Overzicht() {
  return (
    <Layout
      menuItems={TEAM_MENU}
      gebruiker={{ naam: TEAM.naam, rol: TEAM.resortNaam }}
      kruimel={[`Ronde ${RONDE.huidig}`, "Overzicht"]}
    >
      {/* KOP */}
      <section className="ov-kop">
        <div>
          <h1>Overzicht</h1>
          <p>
            Hier zie je de huidige situatie van jouw resort. Maak je
            beslissingen op basis van deze informatie.
          </p>
        </div>

        <div className="ov-kop-acties">
          <div className="ov-ronde">
            <div className="ov-ronde-boven">
              <span className="ov-ronde-label">RONDE</span>
              <strong>
                {RONDE.huidig}
                <small>/{RONDE.totaal}</small>
              </strong>
            </div>
            <div className="ov-ronde-stappen">
              {Array.from({ length: RONDE.totaal }).map((_, index) => (
                <span
                  key={index}
                  className={index < RONDE.huidig ? "ov-stap ov-stap-klaar" : "ov-stap"}
                />
              ))}
            </div>
           
          </div>

          <a href="/beslissingen" className="ov-knop-primair">
            NAAR BESLISSINGEN
            <Icoon naam="pijlRechts" grootte={16} />
          </a>
        </div>
      </section>

      {/* RESORTKAART */}
      <section className="ov-resort">
        <div className="ov-resort-foto" />

        <div className="ov-resort-info">
          <span className="ov-type">
            <Icoon naam={TEAM.resortType.icoon} grootte={12} />
            {TEAM.resortType.label}
          </span>
          <h2>{TEAM.resortNaam}</h2>
          <span className="ov-team">{TEAM.naam}</span>
          <div className="ov-resort-meta">
            <span>
              <Icoon naam="pin" grootte={13} />
              {TEAM.locatie}
            </span>
            <span>Startprofiel {TEAM.startprofiel}</span>
          </div>
        </div>

        <div className="ov-resort-stats">
          <div className="ov-stat">
            <span className="ov-stat-label">BUDGET</span>
            <span className="ov-stat-waarde">{euro.format(TEAM.budget)}</span>
          </div>
          <div className="ov-stat">
            <span className="ov-stat-label">KAMERS</span>
            <span className="ov-stat-waarde">{TEAM.kamers}</span>
          </div>
          <div className="ov-stat">
            <span className="ov-stat-label">MEDEWERKERS</span>
            <span className="ov-stat-waarde">{TEAM.medewerkers}</span>
          </div>
        </div>
      </section>

      {/* CIJFERS */}
      <section className="ov-kpi-grid">
        {KPIS.map((kpi) => (
          <KpiKaart key={kpi.key} kpi={kpi} />
        ))}
      </section>

      {/* NIEUWSFEED */}
      <Nieuwsfeed />
    </Layout>
  );
}

export default Overzicht;