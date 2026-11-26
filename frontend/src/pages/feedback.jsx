import { useEffect, useState } from "react";
import Layout from "../components/layout";
import { getFeedback } from "../api/feedback";
import "./feedback.css";

// Hetzelfde menu als bij Overzicht en Beslissingen.
const TEAM_MENU = [
  { label: "Overzicht", href: "/overzicht", icoon: "overzicht" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Host", href: "/host", icoon: "host" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];

// Alleen voor het testen van de frontend. Dit zijn geen echte spelresultaten.
const VOORBEELD = {
  teamNaam: "Team 1",
  rondeNummer: 1,
  aantalRondes: 4,
  rondeAfgerond: true,
  afgerondOp: "10-10-2026",
  totaalScore: 78,
  positie: 2,
  aantalTeams: 4,
  puntenDezeRonde: 4,
  kpis: [
    { naam: "Bezettingsgraad", waarde: "70%", verschil: "+5%", gestegen: true, balkProcent: 70 },
    { naam: "Gasttevredenheid", waarde: "70 / 100", verschil: "+5", gestegen: true, balkProcent: 70 },
    { naam: "Winst & Cashflow", waarde: "€ 120.000", verschil: "-€20k", gestegen: false, balkProcent: 55 },
    { naam: "Duurzaamheid", waarde: "60 / 100", verschil: "+5", gestegen: true, balkProcent: 60 },
  ],
  watGingGoed: [
    "De bezettingsgraad is gestegen.",
    "De marketingkeuze heeft meer gasten aangetrokken.",
    "De duurzaamheidsscore is verbeterd.",
  ],
  watKanBeter: [
    "De cashflow is gedaald door hoge investeringskosten.",
    "De personeelskosten zijn deze ronde gestegen.",
    "De winst had hoger kunnen zijn.",
  ],
  advies: [
    "Kies een goedkopere investering om je cashflow te beschermen.",
    "Houd marketing op peil, maar let beter op het totale budget.",
    "Zoek een betere balans tussen service, personeel en kosten.",
  ],
  beslissingen: [
    { beslissing: "Marketing verhoogd", effect: "meer gasten", positief: true },
    { beslissing: "Spa & Wellness", effect: "lagere cashflow", positief: false },
    { beslissing: "Zonnepanelen", effect: "hogere duurzaamheid", positief: true },
  ],
};

const KPI_KLEUREN = {
  Bezettingsgraad: "blauw",
  Gasttevredenheid: "roze",
  "Winst & Cashflow": "geel",
  Duurzaamheid: "groen",
};

function KpiIcoon({ naam }) {
  const standaard = { fill: "none", stroke: "currentColor", strokeWidth: "2", strokeLinecap: "round", strokeLinejoin: "round" };
  if (naam === "Bezettingsgraad") {
    return <svg viewBox="0 0 24 24" {...standaard}><path d="M3 21V8l9-5 9 5v13M9 21v-6h6v6" /></svg>;
  }
  if (naam === "Gasttevredenheid") {
    return <svg viewBox="0 0 24 24" {...standaard}><circle cx="12" cy="12" r="9" /><path d="M8 14s1.5 2 4 2 4-2 4-2M9 9h.01M15 9h.01" /></svg>;
  }
  if (naam === "Winst & Cashflow") {
    return <svg viewBox="0 0 24 24" {...standaard}><path d="M12 2v20M17 5H9.5a3.5 3.5 0 000 7h5a3.5 3.5 0 010 7H6" /></svg>;
  }
  return <svg viewBox="0 0 24 24" {...standaard}><path d="M11 20A7 7 0 014 13c0-6 7-10 16-10 0 9-4 16-10 16zM4 21c3-6 6-9 11-12" /></svg>;
}

export default function Feedback() {
  const [feedback, setFeedback] = useState(VOORBEELD);
  const [voorbeelddata, setVoorbeelddata] = useState(true);
  const [melding, setMelding] = useState("Voorbeeldgegevens — de Feedback API is nog niet gekoppeld.");

  // Feedback van de laatste ronde ophalen bij de backend.
  // Lukt dat niet (bijv. nog geen ronde gespeeld), dan blijven de voorbeeldgegevens staan.
  useEffect(() => {
    const controller = new AbortController();

    getFeedback(controller.signal)
      .then((gegevens) => {
        setFeedback(gegevens);
        setVoorbeelddata(false);
        setMelding("");
      })
      .catch((err) => {
        if (err.name === "AbortError") return;
        setVoorbeelddata(true);
        setMelding(`Voorbeeldgegevens — ${err.message}`);
      });

    return () => controller.abort();
  }, []);

  const ronde = feedback.rondeNummer;
  const totaalRondes = feedback.aantalRondes || 1;
  const voortgang = Math.min(1, Math.max(0, ronde / totaalRondes));
  const ringOffset = 214 * (1 - voortgang);
  const naam = feedback.teamNaam || "Team";

  return (
    <Layout
      menuItems={TEAM_MENU}
      gebruiker={{ naam, rol: "Speler" }}
      kruimel={[`Ronde ${ronde}`, "Feedback"]}
    >
      <div className="feedback-page">
        <div className="pagina">
          <div className="titel-rij animatie">
            <div>
              <span className="tag">Ronde {ronde} • Resultaten</span>
              <h1>Ronde feedback</h1>
              <p className="subtitel">Bekijk wat goed ging, wat beter kan en wat je in de volgende ronde kunt verbeteren.</p>
            </div>
            <div className="ronde-status">
              <div className="vinkje">✓</div>
              <div>
                <h3>Ronde {ronde} {feedback.rondeAfgerond ? "afgerond" : "bezig"}</h3>
                <p>{naam}{feedback.afgerondOp ? ` • ${feedback.afgerondOp}` : ""}</p>
              </div>
            </div>
          </div>

          {melding && <div className="melding zichtbaar" role="status">{melding}</div>}
          {voorbeelddata && <p className="feedback-test-label">Demo: de getoonde scores zijn niet echt opgeslagen.</p>}

          <div className="score-kaarten animatie">
            <div className="score-kaart blauw">
              <div>
                <p>Totaalscore</p>
                <h2>{feedback.totaalScore} <small>punten</small></h2>
              </div>
              <div className="ring">
                <svg viewBox="0 0 80 80" aria-hidden="true">
                  <circle className="ring-achter" cx="40" cy="40" r="34" />
                  <circle className="ring-voor" cx="40" cy="40" r="34" style={{ strokeDashoffset: ringOffset }} />
                </svg>
                <span>{ronde}/{totaalRondes}<small>ronde</small></span>
              </div>
            </div>
            <div className="score-kaart groen">
              <div>
                <p>Positie</p>
                <h2>{feedback.positie}e <small>plaats</small></h2>
              </div>
              <div className="podium" aria-label={`Positie ${feedback.positie}`}>
                {[2, 1, 3].map((plaats) => (
                  <div key={plaats} className={`trede trede-${plaats} ${feedback.positie === plaats ? "jij" : ""}`}>
                    {plaats}
                  </div>
                ))}
              </div>
            </div>
            <div className="score-kaart paars">
              <div>
                <p>Resultaat deze ronde</p>
                <h2>{feedback.puntenDezeRonde >= 0 ? "+" : "-"}{Math.abs(feedback.puntenDezeRonde)} <small>punten</small></h2>
              </div>
              <div className="groot-icoon" aria-hidden="true">↗</div>
            </div>
          </div>

          <div className="animatie">
            <h3 className="kopje">KPI-veranderingen</h3>
            <p className="klein">Verschil ten opzichte van de vorige ronde</p>
          </div>
          <div className="kpi-kaarten animatie">
            {(feedback.kpis || []).map((kpi) => (
              <div key={kpi.naam} className={`kpi-kaart ${KPI_KLEUREN[kpi.naam] || "blauw"}`}>
                <div className="kpi-kop"><p>{kpi.naam}</p><KpiIcoon naam={kpi.naam} /></div>
                <div className="kpi-waarde">
                  <h2>{kpi.waarde}</h2>
                  <span className={`badge ${kpi.gestegen ? "omhoog" : "omlaag"}`}>
                    {kpi.gestegen ? "▲" : "▼"} {kpi.verschil}
                  </span>
                </div>
                <div className="balk"><div style={{ width: `${Math.max(0, Math.min(100, kpi.balkProcent || 0))}%` }} /></div>
              </div>
            ))}
          </div>

          <div className="twee-blokken animatie">
            <div className="blok goed">
              <h3><span className="blok-icoon">✓</span> Wat ging goed</h3>
              <ul>{(feedback.watGingGoed || []).map((zin, index) => <li key={index}>{zin}</li>)}</ul>
            </div>
            <div className="blok beter">
              <h3><span className="blok-icoon">!</span> Wat kan beter</h3>
              <ul>{(feedback.watKanBeter || []).map((zin, index) => <li key={index}>{zin}</li>)}</ul>
            </div>
          </div>

          <div className="onderste-rij animatie">
            <div className="blok advies">
              <h3><span className="blok-icoon">→</span> Advies voor de volgende ronde</h3>
              <ol>{(feedback.advies || []).map((zin, index) => (
                <li key={index}><span className="nummer">{index + 1}</span>{zin}</li>
              ))}</ol>
            </div>
            <div className="blok invloed">
              <h3>Invloed van beslissingen</h3>
              {(feedback.beslissingen || []).length === 0 ? (
                <p className="klein">Er zijn deze ronde geen beslissingen genomen.</p>
              ) : (feedback.beslissingen || []).map((beslissing, index) => (
                <div className="invloed-rij" key={index}>
                  <span className="beslissing">{beslissing.beslissing}</span>
                  <span className={`label ${beslissing.positief ? "positief" : "negatief"}`}>
                    {beslissing.positief ? "↗" : "↘"} {beslissing.effect}
                  </span>
                </div>
              ))}
              <a href="/overzicht" className="knop">Terug naar overzicht →</a>
            </div>
          </div>
        </div>
      </div>
    </Layout>
  );
}
