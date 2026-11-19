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


export default function Beslissingen() {

    const [opslaanTekst, setOpslaanTekst] = useState("► Beslissingen opslaan");
    const [kamerprijs, setKamerprijs] = useState("40");
    const [marketingBudget, setMarketingBudget] = useState("5000");
    const [marketingFocus, setMarketingFocus] = useState("Alle doelgroepen");
    const [aantalMedewerkers, setAantalMedewerkers] = useState("10");
    const [typeInvestering, setTypeInvestering] = useState("Spa & Welness");
    const [investeringKosten, setInvesteringKosten] = useState("100000");
    const [duurzaamheidMaatregel, setDuurzaamheidMaatregel] = useState("Zonnepanelen installeren");
    const [duurzaamheidKosten, setDuurzaamheidKosten] = useState("150000");
    const [onderhoudsNiveau, setOnderhoudsNiveau] = useState("Minimaal");

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
        setKamerprijs("40");
        setMarketingBudget("5000");
        setMarketingFocus("Alle doelgroepen");
        setAantalMedewerkers("10");
        setTypeInvestering("Spa & Welness");
        setInvesteringKosten("100000");
        setDuurzaamheidMaatregel("Zonnepanelen installeren");
        setDuurzaamheidKosten("150000");
        setOnderhoudsNiveau("Minimaal");
    };

    const handleOpslaan = async (e) => {
        e.preventDefault();
        const beslissingData = {
            teamNummer: teamsNummer,
            rondeNummer: rondeNummer,
            kamerprijs: parseFloat(kamerprijs) || 0,
            marketingBudget: parseFloat(marketingBudget) || 0,
            marketingFocus: marketingFocus,
            aantalMedewerkers: parseInt(aantalMedewerkers) || 0,
            typeInvestering: typeInvestering,
            investeringKosten: parseFloat(investeringKosten) || 0,
            duurzaamheidMaatregel: duurzaamheidMaatregel,
            duurzaamheidKosten: parseFloat(duurzaamheidKosten) || 0,
            onderhoudsNiveau: onderhoudsNiveau
        };

        try {
            const response = await fetch('/api/Beslissingen', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(beslissingData)
            });

            if (!response.ok) throw new Error("Kon de beslissingen niet opslaan.");
            const result = await response.json();
            console.log("Succesvol opgeslagen:", result);

            // Tekst tijdelijk veranderen naar Opgeslagen!
            setOpslaanTekst("Opgeslagen!");
            setTimeout(() => {
                setOpslaanTekst("► Beslissingen opslaan");
            }, 2000);

        } catch (error) {
            console.error("Fout:", error);
            alert("Fout bij opslaan: " + error.message);
        }
    };

    return (
        <Layout
            menuItems={TEAM_MENU}
            gebruiker={{ naam: "Team", rol: "Speler" }}
            kruimel={["Ronde", "Beslissingen"]}
        >
            <div className="dec-page">

                <div className="h1-ronde-container">
                    <div className="h1-container">
                        <h1>Beslissingen</h1>
                        <p>Maak hier de beslissingen voor de huidige ronde.</p>
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
                    {/* Kamerprijs */}
                    <div className="card" data-card="kamerprijs">
                        <div>
                            <h2>Kamerprijs</h2>
                            <p>Bepaal de gemiddelde prijs per kamer per nacht.</p>
                        </div>
                        <div>
                            <label>Prijs per nacht (€)</label><br />
                            <select value={kamerprijs} onChange={(e) => setKamerprijs(e.target.value)}>
                                <option value="40">40</option>
                                <option value="80">80</option>
                                <option value="120">120</option>
                            </select>
                        </div>
                    </div>

                    {/* Marketing */}
                    <div className="card" data-card="marketing">
                        <div>
                            <h2>Marketing</h2>
                            <p>Stel het marketingbudget in voor deze ronde.</p>
                        </div><br />
                        <div className="field-container">
                            <div>
                                <label>Marketingbudget (€)</label><br />
                                <select value={marketingBudget} onChange={(e) => setMarketingBudget(e.target.value)}>
                                    <option value="5000">5000</option>
                                    <option value="10000">10000</option>
                                    <option value="15000">15000</option>
                                    <option value="20000">20000</option>
                                </select>
                            </div>
                            <div>
                                <label>Marketing focus.</label><br />
                                <select value={marketingFocus} onChange={(e) => setMarketingFocus(e.target.value)}>
                                    <option value="Alle doelgroepen">Alle Doelgroepen</option>
                                    <option value="Jong volwassenen">Jong volwassenen</option>
                                    <option value="Ouderen">Ouderen</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    {/* Personeel */}
                    <div className="card" data-card="personeel">
                        <div>
                            <h2>Personeel</h2>
                            <p>Pas het aantal medewerkers aan.</p>
                        </div><br />
                        <div>
                            <label>Aantal medewerkers.</label><br />
                            <input
                                type="text"
                                value={aantalMedewerkers}
                                onChange={(e) => setAantalMedewerkers(e.target.value)}
                            />
                        </div>
                    </div>

                    {/* Investeringen */}
                    <div className="card" data-card="investering">
                        <div>
                            <h2>Investeringen</h2>
                            <p>Investeer in nieuwe faciliteiten of uitbreidingen.</p>
                        </div><br />
                        <div className="field-container">
                            <div>
                                <label>Type investering</label><br />
                                <select value={typeInvestering} onChange={(e) => setTypeInvestering(e.target.value)}>
                                    <option value="Spa & Welness">Spa & Welness</option>
                                    <option value="Bar">Bar</option>
                                    <option value="Casino">Casino</option>
                                    <option value="Gym">Gym</option>
                                </select>
                            </div>
                            <div>
                                <label>Kosten (€)</label><br />
                                <input
                                    type="text"
                                    value={investeringKosten}
                                    onChange={(e) => setInvesteringKosten(e.target.value)}
                                />
                            </div>
                        </div>
                    </div>

                    {/* Duurzaamheid */}
                    <div className="card" data-card="duurzaamheid">
                        <div>
                            <h2>Duurzaamheid</h2>
                            <p>Kies een duurzaamheidsmaatregel.</p>
                        </div><br />
                        <div className="field-container">
                            <div>
                                <label>Maatregel</label><br />
                                <select value={duurzaamheidMaatregel} onChange={(e) => setDuurzaamheidMaatregel(e.target.value)}>
                                    <option value="Zonnepanelen installeren">Zonnepanelen installeren</option>
                                    <option value="Isolatie">Isolatie</option>
                                    <option value="Slimme aansturing en verlichting">Slimme aansturing en verlichting</option>
                                    <option value="Ventilatie en waterbesparing">Ventilatie en waterbesparing</option>
                                </select>
                            </div>
                            <div>
                                <label>Kosten (€)</label><br />
                                <input
                                    type="text"
                                    value={duurzaamheidKosten}
                                    onChange={(e) => setDuurzaamheidKosten(e.target.value)}
                                />
                            </div>
                        </div>
                    </div>

                    {/* Onderhoud & Service */}
                    <div className="card" data-card="onderhoud&service">
                        <div>
                            <h2>Onderhoud & Service</h2>
                            <p>Kies het onderhoudsniveau voor kamers en faciliteiten.</p>
                        </div><br />
                        <div>
                            <label>Onderhoudsniveau</label><br />
                            <select value={onderhoudsNiveau} onChange={(e) => setOnderhoudsNiveau(e.target.value)}>
                                <option value="Minimaal">Minimaal</option>
                                <option value="Normaal">Normaal</option>
                                <option value="Maximaal">Maximaal</option>
                            </select>
                        </div>
                    </div>
                </div>

                <div className="button-container">
                    <button data-card="resetten" onClick={handleReset}>Resetten</button>
                    <button data-card="beslissingen-opslaan" onClick={handleOpslaan}>{opslaanTekst}</button>
                    <button data-card="meer-beslissingen" onClick={() => { window.location.href = "/extra-beslissingen"; }}>Meer beslissingen ➜</button>
                </div>
            </div>
        </Layout>
    );
}