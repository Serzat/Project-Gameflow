import { useState } from "react";
import Layout from "../components/layout";
import "./competitie.css";

// Het competitiescherm is bedoeld voor de spelbegeleider.
// De autorisatie op basis van de ingelogde rol wordt later gekoppeld.
const HOST_MENU = [
  { label: "Startprofiel", href: "/startprofiel", icoon: "overzicht" },
  { label: "Competitie", href: "/competitie", icoon: "host" },
  { label: "Overzicht", href: "/overzicht", icoon: "overzicht" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];

// Dezelfde vier resorttypes als op het Startprofiel-scherm.
const RESORT_TYPES = [
  { waarde: "beach", label: "Beach resort" },
  { waarde: "winter", label: "Winter resort" },
  { waarde: "cruise", label: "Cruise resort" },
  { waarde: "city", label: "City resort" },
];

const RONDE_DUREN = [15, 30, 45];

// Tijdelijke vertaling voor de bestaande backend.
// Daar worden de resorttypes intern nog met A, B, C of D opgeslagen.
// Dit moet later ook in Gameflow.Api worden omgezet naar resorttypes.
const BACKEND_PROFIEL_CODE = {
  beach: "A",
  winter: "B",
  city: "C",
  cruise: "D",
};

export default function Competitie() {
  const [naam, setNaam] = useState("");
  const [aantalTeams, setAantalTeams] = useState(2);
  const [teams, setTeams] = useState([
    { naam: "", resorttype: "" },
    { naam: "", resorttype: "" },
  ]);
  const [rondes, setRondes] = useState([30, 30, 30, 30]);

  // De spelbegeleider kiest Standaard of Custom op de Startprofiel-pagina.
  const [modus] = useState(() =>
    window.sessionStorage.getItem("resortStartmodus") === "custom"
      ? "custom"
      : "standaard"
  );

  const [bezig, setBezig] = useState(false);
  const [fout, setFout] = useState("");
  const [resultaat, setResultaat] = useState(null);

  function wijzigAantalTeams(waarde) {
    const aantal = Number(waarde);
    setAantalTeams(aantal);
    setTeams((vorige) =>
      Array.from({ length: aantal }, (_, index) =>
        vorige[index] || { naam: "", resorttype: "" }
      )
    );
    setResultaat(null);
  }

  function wijzigTeam(index, veld, waarde) {
    setTeams((vorige) =>
      vorige.map((team, nummer) =>
        nummer === index ? { ...team, [veld]: waarde } : team
      )
    );
    setResultaat(null);
  }

  function wijzigRonde(index, duur) {
    setRondes((vorige) =>
      vorige.map((waarde, nummer) =>
        nummer === index ? Number(duur) : waarde
      )
    );
    setResultaat(null);
  }

  async function leesAntwoord(antwoord) {
    const tekst = await antwoord.text();
    try {
      return tekst ? JSON.parse(tekst) : null;
    } catch {
      return tekst;
    }
  }

  function foutmelding(gegevens, standaardMelding) {
    if (typeof gegevens === "string") return gegevens;
    return gegevens?.title || standaardMelding;
  }

  async function maakCompetitie(event) {
    event.preventDefault();
    setFout("");
    setResultaat(null);

    if (!naam.trim() || teams.some((team) => !team.naam.trim() || !team.resorttype)) {
      setFout("Vul een competitienaam in en geef elk team een naam en een resorttype.");
      return;
    }

    setBezig(true);

    try {
      // De backend stelt eerst de startwaarden voor de gekozen modus in.
      const modusAntwoord = await fetch("/api/startprofielen/modus", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ modus: modus === "custom" ? "Custom" : "Standaard" }),
      });
      const modusGegevens = await leesAntwoord(modusAntwoord);

      if (!modusAntwoord.ok) {
        setFout(foutmelding(modusGegevens, "De startprofielmodus kon niet worden opgeslagen."));
        return;
      }

      const antwoord = await fetch("/api/competities", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          naam: naam.trim(),
          aantalTeams,
          teams: teams.map((team) => ({
            naam: team.naam.trim(),
            profiel: BACKEND_PROFIEL_CODE[team.resorttype],
          })),
          rondes: rondes.map((duurMinuten) => ({ duurMinuten })),
        }),
      });

      const gegevens = await leesAntwoord(antwoord);
      if (!antwoord.ok) {
        setFout(foutmelding(gegevens, "De competitie kon niet worden aangemaakt."));
        return;
      }

      setResultaat(gegevens);
    } catch {
      setFout("De backend is niet bereikbaar. Controleer of Gameflow.Api draait.");
    } finally {
      setBezig(false);
    }
  }

  return (
    <Layout
      menuItems={HOST_MENU}
      gebruiker={{ naam: "Spelbegeleider", rol: "Host" }}
      kruimel={["Host", "Competitie"]}
    >
      <div className="competitie-page">
        <div className="competitie-intro">
          <h1>Competitie instellen</h1>
          <p>Stel de teams en rondes in voordat het spel begint.</p>
        </div>

        <form onSubmit={maakCompetitie}>
          <h2 className="competitie-sectie-kop">Ronde-instellingen</h2>
          <div className="competitie-instellingen">
            <div className="competitie-basis">
              <label htmlFor="competitie-naam">Naam competitie</label>
              <input
                id="competitie-naam"
                type="text"
                value={naam}
                onChange={(event) => { setNaam(event.target.value); setResultaat(null); }}
                placeholder="Naam van de competitie"
                maxLength={100}
                required
              />

              <label htmlFor="competitie-aantal">Aantal teams</label>
              <select
                id="competitie-aantal"
                value={aantalTeams}
                onChange={(event) => wijzigAantalTeams(event.target.value)}
              >
                {[2, 3, 4].map((aantal) => (
                  <option key={aantal} value={aantal}>{aantal}</option>
                ))}
              </select>
            </div>

            <div className="competitie-rondes-wrap">
              <p>Hoelang duurt elke ronde? Kies 15, 30 of 45 minuten.</p>
              <div className="competitie-rondes">
                {rondes.map((duur, index) => (
                  <label className="competitie-ronde-kaart" key={index}>
                    <span>Ronde {index + 1}</span>
                    <select
                      value={duur}
                      onChange={(event) => wijzigRonde(index, event.target.value)}
                      aria-label={`Duur van ronde ${index + 1}`}
                    >
                      {RONDE_DUREN.map((minuten) => (
                        <option key={minuten} value={minuten}>{minuten} minuten</option>
                      ))}
                    </select>
                  </label>
                ))}
              </div>
            </div>
          </div>

          <h2 className="competitie-sectie-kop">Teams en startprofielen</h2>
          <div className="competitie-modus">
            <span>
              Gekozen modus: <strong>{modus === "custom" ? "Custom" : "Standaard"}</strong>
            </span>
            <a href="/startprofiel">Wijzig via Startprofiel</a>
          </div>

          <div className="competitie-teamlijst">
            <div className="competitie-team-koppen" aria-hidden="true">
              <span>Team</span><span>Naam</span><span>Resorttype</span>
            </div>
            {teams.map((team, index) => (
              <div className="competitie-team-rij" key={index}>
                <strong>Team {index + 1}</strong>
                <input
                  type="text"
                  placeholder={`Naam team ${index + 1}`}
                  aria-label={`Naam team ${index + 1}`}
                  value={team.naam}
                  onChange={(event) => wijzigTeam(index, "naam", event.target.value)}
                  required
                />
                <select
                  aria-label={`Resorttype team ${index + 1}`}
                  value={team.resorttype}
                  onChange={(event) => wijzigTeam(index, "resorttype", event.target.value)}
                  required
                >
                  <option value="">Kies resorttype</option>
                  {RESORT_TYPES.map((type) => (
                    <option key={type.waarde} value={type.waarde}>{type.label}</option>
                  ))}
                </select>
              </div>
            ))}
          </div>

          {fout && <p className="competitie-bericht competitie-fout" role="alert">{fout}</p>}
          {resultaat && (
            <p className="competitie-bericht competitie-succes" role="status">
              Competitie "{resultaat.naam}" aangemaakt (ID: {resultaat.id}).
              De rondes zijn nog niet gestart.
            </p>
          )}

          <button className="competitie-aanmaken" type="submit" disabled={bezig}>
            {bezig ? "Competitie aanmaken..." : "Competitie aanmaken"}
          </button>
        </form>
      </div>
    </Layout>
  );
}
