import { useState, useEffect } from 'react';
import './beslissingen.css';
import Layout from '../components/layout';
const TEAM_MENU = [
  { label: "Overzicht", href: "/overzicht", icoon: "overzicht" },
  { label: "Beslissingen", href: "/beslissingen", icoon: "beslissingen" },
  { label: "Ranglijst", href: "/ranglijst", icoon: "ranglijst" },
  { label: "Host", href: "/host", icoon: "host" },
  { label: "Feedback", href: "/feedback", icoon: "feedback" },
];


export default function ExtraBeslissingen() {

    const [opslaanTekst, setOpslaanTekst] = useState("► Beslissingen opslaan");
    const [doelGroep, setDoelGroep] = useState("Geen");
    const [marketingKanaal, setMarketingKanaal] = useState("Geen");
    const [personeelTraining, setPersoneelTraining] = useState("Geen");
    const [korting, setKorting] = useState("Geen");

    const [teamStats, setTeamStats] = useState({
        teamNummer: 1,
        rondeNummer: 1,
        startProfiel: "-",
        resortNaam: "-",
        locatie: "-",
        huidigBudget: 0,
        aantalKamers: 0,
        aantalMedewerkers: 0
    });

    const teamsNummer = 1;
    const rondeNummer = 1;

    useEffect(() => {
        async function laadTeamStats() {
            try {
                const response = await fetch(`/api/teamstats/${teamsNummer}/${rondeNummer}`);
                if (!response.ok) throw new Error("Kon de teamstats niet ophalen.");
                const data = await response.json();
                setTeamStats(data);
            } catch (error) {
                console.error("Fout bij het laden van stats:", error);
            }
        }
        laadTeamStats();
    }, []);
    
    const handleReset = (e) => {
        e.preventDefault();
        setDoelGroep("Geen");
        setMarketingKanaal("Geen");
        setPersoneelTraining("Geen");
        setKorting("Geen");
    };

    const handleOpslaan = async (e) => {
        e.preventDefault();

        const extraBeslissingData = {
            teamNummer: teamsNummer,
            rondeNummer: rondeNummer,
            doelGroep: doelGroep === "Geen" ? null : doelGroep,
            marketingKanaal: marketingKanaal === "Geen" ? null : marketingKanaal,
            personeelTraining: personeelTraining === "Geen" ? null : personeelTraining,
            korting: korting === "Geen" ? null : parseInt(korting)
        };

        try {
            const response = await fetch('/api/Extrabeslissingen', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(extraBeslissingData)
            });

            const resultText = await response.text();

            if (!response.ok) {
                alert(resultText);
                return;
            }

            console.log("Extra beslissingen opgeslagen:", resultText);

            setOpslaanTekst("Opgeslagen!");
            setTimeout(() => {
                setOpslaanTekst("► Beslissingen opslaan");
            }, 2000);

        } catch (error) {
            console.error("Fout bij opslaan extra beslissingen:", error);
            alert("Fout details: " + error.message);
        }
    };

    return (
        <Layout
            menuItems={TEAM_MENU}
            gebruiker={{ naam: "Team", rol: "Speler" }}
            kruimel={["Ronde", "Extra beslissingen"]}
        >
            <div className="dec-page">

                <div className="h1-ronde-container">
                    <div className="h1-container">
                        <h1>Extra beslissingen</h1>
                        <p>Maak aanvullende keuzes voor deze ronde.</p>
                    </div>
                    <div className="ronde-card">
                        <h2>Ronde {teamStats.rondeNummer}/4</h2>
                        <p>Deadline: nog niet ingesteld</p>
                    </div>
                </div>

                <div className="stats-container">
                    <h2>Team {teamStats.teamNummer}</h2>
                    <div className="card-container-stats">
                        <div className="stats-cards" data-card="1">
                            <p><span>Startprofiel:</span> <span>{teamStats.startProfiel}</span></p>
                            <p><span>Resort naam:</span> <span>{teamStats.resortNaam}</span></p>
                            <p><span>Locatie:</span> <span>{teamStats.locatie}</span></p>
                        </div>
                        <div className="stats-cards" data-card="2">
                            <p><span>Huidig budget:</span> <span>€ {teamStats.huidigBudget?.toLocaleString('nl-NL')}</span></p>
                            <p><span>Aantal kamers:</span> <span>{teamStats.aantalKamers}</span></p>
                            <p><span>Aantal medewerkers:</span> <span>{teamStats.aantalMedewerkers}</span></p>
                        </div>
                        <div className="stats-cards" data-card="3">
                            <p><span>Bezettingsgraad:</span> 70%</p>
                            <p><span>Gasttevredenheid:</span> 7/10</p>
                            <p><span>Duurzaamheidsscore:</span> 60/100</p>
                        </div>
                    </div>
                </div>

                <div className="cards-container">
                    {/* Doelgroep */}
                    <div className="card" data-card="doelgroep">
                        <div>
                            <h2>Doelgroep</h2>
                            <p>Kies op welke soort gasten je vooral wilt richten.</p>
                        </div>
                        <div>
                            <label>Keuze</label><br />
                            <select value={doelGroep} onChange={(e) => setDoelGroep(e.target.value)}>
                                <option value="Geen">Geen</option>
                                <option value="gezinnen">gezinnen</option>
                                <option value="Studenten">Studenten</option>
                                <option value="Particulieren">Particulieren</option>
                            </select>
                        </div>
                    </div>

                    {/* Marketingkanaal */}
                    <div className="card" data-card="marketingkanaal">
                        <div>
                            <h2>Marketingkanaal</h2>
                            <p>Kies waar je jouw resort vooral wilt promoten.</p>
                        </div><br />
                        <div>
                            <label>Keuze</label><br />
                            <select value={marketingKanaal} onChange={(e) => setMarketingKanaal(e.target.value)}>
                                <option value="Geen">Geen</option>
                                <option value="Instagram">Instagram</option>
                                <option value="Facebook">Facebook</option>
                                <option value="X">X</option>
                                <option value="Threads">Threads</option>
                            </select>
                        </div>
                    </div>

                    {/* Personeeltraining */}
                    <div className="card" data-card="personeeltraining">
                        <div>
                            <h2>Personeeltraining</h2>
                            <p>Bepaal hoeveel training je personeel krijgt.</p>
                        </div><br />
                        <div>
                            <label>Keuze</label><br />
                            <select value={personeelTraining} onChange={(e) => setPersoneelTraining(e.target.value)}>
                                <option value="Geen">Geen</option>
                                <option value="Basis">Basis</option>
                                <option value="Gemiddeld">Gemiddeld</option>
                                <option value="Hoog">Hoog</option>
                            </select>
                        </div>
                    </div>

                    {/* Korting */}
                    <div className="card" data-card="korting">
                        <div>
                            <h2>Korting</h2>
                            <p>Kies hoeveel korting je deze ronde aanbiedt.</p>
                        </div><br />
                        <div>
                            <label>Keuze</label><br />
                            <select value={korting} onChange={(e) => setKorting(e.target.value)}>
                                <option value="Geen">Geen</option>
                                <option value="10">10%</option>
                                <option value="15">15%</option>
                                <option value="20">20%</option>
                                <option value="25">25%</option>
                            </select>
                        </div>
                    </div>
                </div>

                <div className="info-card-extra-beslissingen-container">
                    <h3>Deze extra beslissingen zijn optioneel.</h3>
                    <p>Ze kunnen invloed hebben op bezettingsgraad, gasttevredenheid, personeelsresultaten en winst.</p>
                </div>

                <div className="button-container-extra-beslissingen">
                    <div className="button-container-left">
                        <button data-card="meer-beslissingen" onClick={() => { window.location.href = "/beslissingen"; }}>⬅ Standaard beslissingen</button>
                    </div>
                    <div className="button-container-right">
                        <button data-card="resetten" onClick={handleReset}>Resetten</button>
                        <button data-card="extra-beslissingen-opslaan" onClick={handleOpslaan}>{opslaanTekst}</button>
                    </div>
                </div>
            </div>
        </Layout>
    );
}