import { useState } from "react";
import Layout from "../components/layout";
import "./ranglijst.css";

const TEAM_MENU = [
  { label: "Overzicht", href: "/overzicht", icoon: "overzicht" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Host", href: "/host", icoon: "host" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];

// Voorbeeldgegevens uit het aangeleverde ranglijstscherm.
// Later worden de scores door de backend berekend.
const TEAMS = [
  { naam: "Team Horizon", score: 84, bezetting: 75, gast: 78, duurzaam: 70, trend: 5, eigen: false },
  { naam: "Team 1", score: 78, bezetting: 70, gast: 70, duurzaam: 60, trend: 4, eigen: true },
  { naam: "Team Nova", score: 73, bezetting: 66, gast: 68, duurzaam: 58, trend: -2, eigen: false },
  { naam: "Team Coast", score: 69, bezetting: 62, gast: 65, duurzaam: 55, trend: 1, eigen: false },
];

const OPTIES = [
  { key: "score", naam: "Totaalscore", eenheid: "punten" },
  { key: "bezetting", naam: "Bezettingsgraad", eenheid: "%" },
  { key: "gast", naam: "Gasttevredenheid", eenheid: "punten" },
  { key: "duurzaam", naam: "Duurzaamheid", eenheid: "punten" },
];

function hoogsteTeam(veld) {
  return TEAMS.reduce((hoogste, team) => team[veld] > hoogste[veld] ? team : hoogste);
}

export default function Ranglijst() {
  const [sortering, setSortering] = useState("score");
  const ranglijst = [...TEAMS].sort((a, b) => b[sortering] - a[sortering]);
  const eigenTeam = TEAMS.find((team) => team.eigen);
  const eigenPositie = ranglijst.findIndex((team) => team.eigen) + 1;
  const verschil = eigenTeam[sortering] - ranglijst[0][sortering];
  const gekozenOptie = OPTIES.find((optie) => optie.key === sortering);
  const topBezetting = hoogsteTeam("bezetting");
  const topGast = hoogsteTeam("gast");
  const topDuurzaam = hoogsteTeam("duurzaam");

  return (
    <Layout
      menuItems={TEAM_MENU}
      gebruiker={{ naam: eigenTeam.naam, rol: "Speler" }}
      kruimel={["Ronde 1", "Ranglijst"]}
    >
      <div className="ranglijst-page">
        <div className="ranglijst-heading">
          <div>
            <h1>Ranglijst</h1>
            <p>Vergelijk de prestaties van jouw team met andere teams in de competitie.</p>
          </div>
          <div className="ranglijst-ronde">
            <strong>Ronde 1 / 4</strong>
            <small>Voorbeeldgegevens</small>
          </div>
        </div>
        <div className="ranglijst-demo" role="status">
          Demo: deze ranglijst gebruikt voorbeeldscores. Er is nog geen ranglijst-API gekoppeld.
        </div>

        <div className="ranglijst-stats">
          <div className="ranglijst-stat ranglijst-stat-blauw">
            <p>Jouw positie</p><strong>{eigenPositie}e plaats</strong>
          </div>
          <div className="ranglijst-stat ranglijst-stat-groen">
            <p>Jouw totaalscore</p><strong>{eigenTeam.score} punten</strong>
          </div>
          <div className="ranglijst-stat ranglijst-stat-geel">
            <p>Verschil met nr. 1 ({gekozenOptie.naam.toLowerCase()})</p>
            <strong>{verschil > 0 ? "+" : ""}{verschil} {gekozenOptie.eenheid}</strong>
          </div>
        </div>

        <section className="ranglijst-tabel-kaart">
          <div className="ranglijst-tabel-kop">
            <div>
              <h2>Teamranglijst</h2>
              <p>Vergelijk teams op totaalscore of op een afzonderlijke KPI.</p>
            </div>
            <label>
              <span className="ranglijst-visueel-verborgen">Sorteer op</span>
              <select value={sortering} onChange={(event) => setSortering(event.target.value)}>
                {OPTIES.map((optie) => <option key={optie.key} value={optie.key}>{optie.naam}</option>)}
              </select>
            </label>
          </div>
          <div className="ranglijst-tabel-scroll">
            <table>
              <thead>
                <tr>
                  <th>Positie</th><th>Team</th><th>Totaalscore</th><th>Bezettingsgraad</th>
                  <th>Gasttevredenheid</th><th>Duurzaamheid</th><th>Trend</th>
                </tr>
              </thead>
              <tbody>
                {ranglijst.map((team, index) => (
                  <tr key={team.naam} className={team.eigen ? "ranglijst-eigen-team" : ""}>
                    <td className={index === 0 ? "ranglijst-eerste" : ""}><strong>{index + 1}</strong></td>
                    <td><strong>{team.naam}{team.eigen ? " (jouw team)" : ""}</strong></td>
                    <td><strong>{team.score}</strong></td>
                    <td>{team.bezetting}%</td><td>{team.gast} / 100</td><td>{team.duurzaam} / 100</td>
                    <td className={team.trend >= 0 ? "ranglijst-positief" : "ranglijst-negatief"}>
                      {team.trend >= 0 ? "▲ +" : "▼ "}{team.trend}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </section>

        <div className="ranglijst-onder-kaarten">
          <section className="ranglijst-onder-kaart">
            <h2>Topprestaties</h2>
            <div className="ranglijst-prestatie"><span>Hoogste bezettingsgraad</span><strong>{topBezetting.naam}</strong><strong>{topBezetting.bezetting}%</strong></div>
            <div className="ranglijst-prestatie"><span>Beste gasttevredenheid</span><strong>{topGast.naam}</strong><strong>{topGast.gast} / 100</strong></div>
            <div className="ranglijst-prestatie"><span>Beste duurzaamheid</span><strong>{topDuurzaam.naam}</strong><strong>{topDuurzaam.duurzaam} / 100</strong></div>
          </section>
          <section className="ranglijst-onder-kaart ranglijst-eigen-kaart">
            <h2>Jouw team</h2>
            <h3>{eigenTeam.naam}</h3>
            <div className="ranglijst-team-stats">
              <div><span>Positie</span><strong>{eigenPositie}e</strong></div>
              <div><span>Score</span><strong>{eigenTeam.score}</strong></div>
              <div><span>Vorige ronde</span><strong>{eigenTeam.score - eigenTeam.trend}</strong></div>
            </div>
            <p className="ranglijst-team-bericht">
              Je team is deze ronde {Math.abs(eigenTeam.trend)} punten {eigenTeam.trend >= 0 ? "gestegen" : "gedaald"}.
            </p>
          </section>
        </div>
      </div>
    </Layout>
  );
}
