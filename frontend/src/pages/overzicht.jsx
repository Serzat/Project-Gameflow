import { useEffect, useState } from "react";
import Layout from "../components/layout";
import Icoon from "../components/icoon";
import { getOverzicht } from "../api/overzicht";
import "./overzicht.css";

// Menu voor teams (spelers)
const TEAM_MENU = [
  { label: "Overzicht", href: "/overzicht", icoon: "overzicht" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Host", href: "/host", icoon: "host" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];

// VASTE GEGEVENS VOOR DE WEERGAVE
// De cijfers zelf komen uit de backend (GET /api/overzicht).

const RESORT_TYPES = {
  beach: { label: "Beach resort", icoon: "zon" },
  winter: { label: "Winter resort", icoon: "sneeuw" },
  cruise: { label: "Cruise resort", icoon: "anker" },
  city: { label: "City resort", icoon: "stad" },
};

const euro = new Intl.NumberFormat("nl-NL", {
  style: "currency",
  currency: "EUR",
  maximumFractionDigits: 0,
});

const getal = new Intl.NumberFormat("nl-NL", { maximumFractionDigits: 1 });

// Hoe elke KPI eruitziet. De naam (bijv. "bezettingsgraad") is de key uit de backend.
// formaat: hoe de waarde wordt getoond
// balk:    hoe vol het balkje is (0 t/m 100)
const KPI_META = {
  bezettingsgraad: {
    label: "Bezettingsgraad", kleur: "#327b81", icoon: "bed",
    formaat: (w) => `${getal.format(w)}%`,
    balk: (w) => w,
  },
  kamerprijs: {
    label: "Gem. kamerprijs", kleur: "#5c8f6b", icoon: "tag",
    formaat: (w) => euro.format(w),
    balk: (w) => (w / 300) * 100,
  },
  winst: {
    label: "Winst & cashflow", kleur: "#e58a24", icoon: "trend",
    formaat: (w) => euro.format(w),
    balk: (w, team) => (team.budget > 0 ? (w / team.budget) * 100 : 0),
  },
  gasttevredenheid: {
    label: "Gasttevredenheid", kleur: "#d4654f", icoon: "ster", eenheid: "/100",
    formaat: (w) => getal.format(w),
    balk: (w) => w,
  },
  personeelstevredenheid: {
    label: "Personeelstevredenheid", kleur: "#8a6fb0", icoon: "host", eenheid: "/100",
    formaat: (w) => getal.format(w),
    balk: (w) => w,
  },
  duurzaamheidsscore: {
    label: "Duurzaamheidsscore", kleur: "#6b9a5b", icoon: "blad", eenheid: "/100",
    formaat: (w) => getal.format(w),
    balk: (w) => w,
  },
  marktaandeel: {
    label: "Marktaandeel", kleur: "#3f7fa6", icoon: "taart",
    formaat: (w) => `${getal.format(w)}%`,
    balk: (w) => w,
  },
  bezetteKamers: {
    label: "Bezette kamers", kleur: "#264f4b", icoon: "sleutel",
    formaat: (w) => getal.format(w),
    eenheid: (team) => `/${team.aantalKamers}`,
    balk: (w, team) => (team.aantalKamers > 0 ? (w / team.aantalKamers) * 100 : 0),
  },
};

// Korte namen voor de KPI's in de effecten van events
const EFFECT_LABELS = {
  budget: "Budget",
  aantalKamers: "Kamers",
  aantalMedewerkers: "Medewerkers",
  bezettingsgraad: "Bezettingsgraad",
  kamerprijs: "Kamerprijs",
  winst: "Winst",
  gasttevredenheid: "Gasttevredenheid",
  personeelstevredenheid: "Personeelstevredenheid",
  duurzaamheidsscore: "Duurzaamheid",
  marktaandeel: "Marktaandeel",
};

const CATEGORIEEN = {
  weer: { label: "Weer", icoon: "wolk" },
  markt: { label: "Markt", icoon: "trend" },
  gasten: { label: "Gasten", icoon: "ster" },
  resort: { label: "Resort", icoon: "waarschuwing" },
};

// HULPFUNCTIES

// Houdt een getal tussen 0 en 100 (voor de balkjes)
const tussen0en100 = (n) => Math.max(0, Math.min(100, n));

// Getal met + of − ervoor, bijv. +5 of −2,5
function metTeken(n) {
  const tekst = getal.format(Math.abs(n));
  return n >= 0 ? `+${tekst}` : `−${tekst}`;
}

// Zet een KPI uit de backend ({ key, waarde, vorige }) om naar wat de kaart nodig heeft
function maakKpi(kpi, team) {
  const meta = KPI_META[kpi.key];
  if (!meta) return null;

  // In ronde 1 is er nog geen vorige waarde, dan tonen we geen trend
  const verschil = kpi.vorige === null ? null : kpi.waarde - kpi.vorige;

  return {
    key: kpi.key,
    label: meta.label,
    kleur: meta.kleur,
    icoon: meta.icoon,
    waarde: meta.formaat(kpi.waarde),
    eenheid: typeof meta.eenheid === "function" ? meta.eenheid(team) : meta.eenheid,
    balk: tussen0en100(meta.balk(kpi.waarde, team)),
    verschil,
  };
}

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

        {kpi.verschil !== null && kpi.verschil !== 0 && (
          <span className={kpi.verschil > 0 ? "ov-trend ov-trend-goed" : "ov-trend ov-trend-slecht"}>
            <Icoon naam={kpi.verschil > 0 ? "omhoog" : "omlaag"} grootte={12} />
            {metTeken(kpi.verschil)}
          </span>
        )}
      </div>

      {kpi.verschil === null && <span className="ov-kpi-subtekst">Startwaarde</span>}

      <div className="ov-kpi-balk">
        <div className="ov-kpi-vulling" style={{ width: `${kpi.balk}%` }} />
      </div>
    </div>
  );
}

function EventEffecten({ event }) {
  if (event.geenInvloed) {
    return (
      <div className="ov-effecten">
        <span className="ov-effect ov-effect-neutraal">Geen invloed op jouw resort</span>
      </div>
    );
  }

  return (
    <div className="ov-effecten">
      {event.effecten.map((effect) => (
        <span
          key={effect.kpi}
          className={effect.waarde >= 0 ? "ov-effect ov-effect-goed" : "ov-effect ov-effect-slecht"}
        >
          {EFFECT_LABELS[effect.kpi] ?? effect.kpi} {metTeken(effect.waarde)}
        </span>
      ))}
    </div>
  );
}

function Nieuwsfeed({ events }) {
  const [filter, setFilter] = useState("alle");

  const zichtbareEvents =
    filter === "alle"
      ? events
      : events.filter((event) => event.categorie === filter);

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
          const categorie = CATEGORIEEN[event.categorie] ?? { label: event.categorie, icoon: "nieuws" };
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
                  <span>{event.soort === "globaal" ? "Voor alle resorts" : "Alleen jouw resort"}</span>
                </div>
              </div>

              <EventEffecten event={event} />
            </li>
          );
        })}

        {events.length === 0 && (
          <li className="ov-feed-leeg">
            Er is nog niets gebeurd. Nieuws verschijnt hier zodra er een event plaatsvindt.
          </li>
        )}
        {events.length > 0 && zichtbareEvents.length === 0 && (
          <li className="ov-feed-leeg">Geen events in deze categorie.</li>
        )}
      </ul>
    </section>
  );
}

// Waarde van een doel netjes tonen (euro's, procenten of een getal)
function formatDoelwaarde(kpi, waarde) {
  if (kpi === "winst" || kpi === "budget" || kpi === "kamerprijs") return euro.format(waarde);
  if (kpi === "bezettingsgraad" || kpi === "marktaandeel") return `${getal.format(waarde)}%`;
  return getal.format(waarde);
}

function Doelen({ doelen }) {
  const volgendLevel = doelen.level + 1;

  let subtekst;
  if (doelen.aantalTotaal === 0) {
    subtekst = "Er zijn nog geen doelen ingesteld.";
  } else if (doelen.allesBehaald) {
    subtekst = `Alle doelen gehaald! Jouw resort is level ${doelen.level}.`;
  } else {
    subtekst = `${doelen.aantalBehaald}/${doelen.aantalTotaal} behaald · Haal alle doelen om naar level ${volgendLevel} te gaan`;
  }

  return (
    <section className="ov-doelen">
      <div className="ov-feed-kop">
        <div className="ov-feed-titel">
          <span className="ov-feed-titel-icoon">
            <Icoon naam="ster" grootte={18} />
          </span>
          <div>
            <h2>Doelen</h2>
            <p>{subtekst}</p>
          </div>
        </div>
        <span className="ov-level">LEVEL {doelen.level}</span>
      </div>

      <ul className="ov-doelen-lijst">
        {doelen.doelen.map((doel) => {
          // Voortgang richting de doelwaarde (0 t/m 100)
          const voortgang = doel.behaald
            ? 100
            : doel.doelwaarde > 0
              ? tussen0en100((doel.huidig / doel.doelwaarde) * 100)
              : 0;

          return (
            <li key={doel.id} className={doel.behaald ? "ov-doel ov-doel-behaald" : "ov-doel"}>
              <span className="ov-doel-nummer">{doel.nummer}</span>

              <div className="ov-doel-tekst">
                <h3>{doel.titel}</h3>
                <div className="ov-doel-balk">
                  <div className="ov-doel-vulling" style={{ width: `${voortgang}%` }} />
                </div>
                <span className="ov-doel-stand">
                  {doel.behaald
                    ? `Gehaald in ronde ${doel.behaaldInRonde}`
                    : `Nu ${formatDoelwaarde(doel.kpi, doel.huidig)} van ${formatDoelwaarde(doel.kpi, doel.doelwaarde)}`}
                </span>
              </div>

              <span
                className="ov-doel-vinkje"
                aria-label={doel.behaald ? "Behaald" : "Nog niet behaald"}
              >
                {doel.behaald && "✓"}
              </span>
            </li>
          );
        })}
      </ul>
    </section>
  );
}

// PAGINA

function Overzicht() {
  const [overzicht, setOverzicht] = useState(null);
  const [status, setStatus] = useState("laden"); // "laden" | "fout" | "klaar"
  const [foutmelding, setFoutmelding] = useState("");

  // Overzicht ophalen zodra de pagina opent
  useEffect(() => {
    const controller = new AbortController();

    getOverzicht(controller.signal)
      .then((data) => {
        setOverzicht(data);
        setStatus("klaar");
      })
      .catch((err) => {
        if (err.name === "AbortError") return;
        setFoutmelding(err.message);
        setStatus("fout");
      });

    return () => controller.abort();
  }, []);

  // Tijdens het laden of bij een fout: alleen een melding tonen
  if (status !== "klaar") {
    return (
      <Layout menuItems={TEAM_MENU} gebruiker={{ naam: "", rol: "" }} kruimel={["Overzicht"]}>
        <section className="ov-kop">
          <div>
            <h1>Overzicht</h1>
            {status === "laden" && <p className="ov-melding">Overzicht wordt geladen…</p>}
            {status === "fout" && (
              <p className="ov-melding ov-melding-fout">
                {foutmelding}
                {foutmelding.startsWith("Log in") && <a href="/">Naar inloggen</a>}
              </p>
            )}
          </div>
        </section>
      </Layout>
    );
  }

  const { team, ronde, kpis, events, doelen } = overzicht;
  const resortType = RESORT_TYPES[team.resortType] ?? { label: team.resortType, icoon: "zon" };
  const kpiKaarten = kpis.map((kpi) => maakKpi(kpi, team)).filter(Boolean);

  return (
    <Layout
      menuItems={TEAM_MENU}
      gebruiker={{ naam: team.speler, rol: resortType.label }}
      kruimel={[`Ronde ${ronde.huidig}`, "Overzicht"]}
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
                {ronde.huidig}
                <small>/{ronde.totaal}</small>
              </strong>
            </div>
            <div className="ov-ronde-stappen">
              {Array.from({ length: ronde.totaal }).map((_, index) => (
                <span
                  key={index}
                  className={index < ronde.huidig ? "ov-stap ov-stap-klaar" : "ov-stap"}
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
            <Icoon naam={resortType.icoon} grootte={12} />
            {resortType.label}
          </span>
          <h2>{resortType.label} {team.locatie}</h2>
          <span className="ov-team">{team.speler}</span>
          <div className="ov-resort-meta">
            <span>
              <Icoon naam="pin" grootte={13} />
              {team.locatie}
            </span>
            <span>Level {doelen.level}</span>
          </div>
        </div>

        <div className="ov-resort-stats">
          <div className="ov-stat">
            <span className="ov-stat-label">BUDGET</span>
            <span className="ov-stat-waarde">{euro.format(team.budget)}</span>
          </div>
          <div className="ov-stat">
            <span className="ov-stat-label">KAMERS</span>
            <span className="ov-stat-waarde">{team.aantalKamers}</span>
          </div>
          <div className="ov-stat">
            <span className="ov-stat-label">MEDEWERKERS</span>
            <span className="ov-stat-waarde">{team.aantalMedewerkers}</span>
          </div>
        </div>
      </section>

      {/* CIJFERS */}
      <section className="ov-kpi-grid">
        {kpiKaarten.map((kpi) => (
          <KpiKaart key={kpi.key} kpi={kpi} />
        ))}
      </section>

      {/* NIEUWSFEED (links) EN DOELEN (rechts) */}
      <div className="ov-onder">
        <Nieuwsfeed events={events} />
        <Doelen doelen={doelen} />
      </div>
    </Layout>
  );
}

export default Overzicht;