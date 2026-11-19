import { useState } from "react";
import Layout from "../components/layout";
import "./competitie.css";

// Dit scherm is bedoeld voor de spelbegeleider.
// De daadwerkelijke rolcontrole wordt later aan de login gekoppeld.
const HOST_MENU = [
  { label: "Startprofiel", href: "/startprofiel", icoon: "overzicht" },
  { label: "Competitie", href: "/competitie", icoon: "host" },
  { label: "Overzicht", href: "/overzicht", icoon: "overzicht" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];

const PROFIELEN = ["A", "B", "C", "D"];
const RONDE_DUREN = [15, 30, 45];

export default function Competitie() {
  const [naam, setNaam] = useState("");
  const [aantalTeams, setAantalTeams] = useState(2);
  const [teams, setTeams] = useState([
    { naam: "", profiel: "" },
    { naam: "", profiel: "" },
  ]);
  const [rondes, setRondes] = useState([30, 30, 30, 30]);
  const [bezig, setBezig] = useState(false);
  const [fout, setFout] = useState("");
  const [resultaat, setResultaat] = useState(null);

  function wijzigAantalTeams(waarde) {
    const aantal = Number(waarde);
    setAantalTeams(aantal);
    setTeams((vorige) =>
      Array.from({ length: aantal }, (_, index) =>
        vorige[index] || { naam: "", profiel: "" }
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

  async function maakCompetitie(event) {
    event.preventDefault();
    setFout("");
    setResultaat(null);

    if (!naam.trim() || teams.some((team) => !team.naam.trim() || !team.profiel)) {
      setFout("Vul een competitienaam in en geef elk team een naam en een profiel.");
      return;
    }

    setBezig(true);
    try {
      const antwoord = await fetch("/api/competities", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          naam: naam.trim(),
          aantalTeams,
          teams: teams.map((team) => ({ naam: team.naam.trim(), profiel: team.profiel })),
          rondes: rondes.map((duurMinuten) => ({ duurMinuten })),
        }),
      });

      const tekst = await antwoord.text();
      let gegevens;
      try {
        gegevens = tekst ? JSON.parse(tekst) : null;
      } catch {
        gegevens = tekst;
      }

      if (!antwoord.ok) {
        const melding = typeof gegevens === "string"
          ? gegevens
          : gegevens?.title || "De competitie kon niet worden aangemaakt.";
        setFout(melding);
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

        <div className="competitie-tip">
          Kies eerst de modus en sla de profielen A, B, C en D op via
          {" "}<a href="/startprofiel">Startprofiel</a>. Daarna kun je deze aan teams toewijzen.
          Een ronde wordt later handmatig door de spelbegeleider gestart.
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

              <div className="competitie-start-info">
                <strong>Start van de ronde</strong>
                <span>Handmatig door de spelbegeleider</span>
              </div>
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
          <div className="competitie-teamlijst">
            <div className="competitie-team-koppen" aria-hidden="true">
              <span>Team</span><span>Naam</span><span>Profiel</span>
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
                  aria-label={`Startprofiel team ${index + 1}`}
                  value={team.profiel}
                  onChange={(event) => wijzigTeam(index, "profiel", event.target.value)}
                  required
                >
                  <option value="">Kies profiel</option>
                  {PROFIELEN.map((profiel) => (
                    <option key={profiel} value={profiel}>Profiel {profiel}</option>
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
