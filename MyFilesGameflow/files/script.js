document.addEventListener("DOMContentLoaded", function() {

    const resortStats = {
        startprofiel: "A",
        resortNaam: "Sunny Beach Resort",
        locatie: "Tropisch Eiland",
        budget: "€ 500.000",
        aantalKamers: 80,
        aantalMedewerkers: 30,
        bezettingsgraad: "70%",
        gasttevredenheid: "7/10",
        duurzaamheidsscore: "60/100"
    };
    
    const teamsNummer = 1;
    const rondeNummer = 1;

    laadTeamStats(teamsNummer, rondeNummer);
    async function laadTeamStats(teamNummer, rondeNummer) {
        try {
            const response = await fetch(`http://localhost:5050/api/teamstats/${teamNummer}/${rondeNummer}`);

            if (!response.ok) {
                throw new Error("Kon de teamstats niet ophalen van de server.");
            }

            const data = await response.json();
            console.log("Ontvangen data van API:", data); 

            document.getElementById("team-titel").innerText = `Team ${data.teamNummer}`;
            document.getElementById("stat-startprofiel").innerText = data.startProfiel;
            document.getElementById("stat-resortnaam").innerText = data.resortNaam;
            document.getElementById("stat-locatie").innerText = data.locatie;
            
            document.getElementById("stat-budget").innerText = `€ ${data.huidigBudget.toLocaleString('nl-NL')}`;

            document.getElementById("stat-kamers").innerText = data.aantalKamers;
            document.getElementById("stat-medewerkers").innerText = data.aantalMedewerkers;

            const rondeElement = document.getElementById("ronde");
            if (rondeElement) {
                rondeElement.innerText = `Ronde ${data.rondeNummer}/4`;
            }

        } catch (error) {
            console.error("Fout bij het laden van stats:", error);
        }
    }
    
    const resetBtn = document.querySelector('button[data-card="resetten"]');
    if (resetBtn) {
        resetBtn.addEventListener("click", function(e) {
            e.preventDefault();
            
            document.querySelectorAll("select").forEach(select => {
                select.selectedIndex = 0;
            });

            document.querySelectorAll("input[type='text']").forEach(input => {
                input.value = input.defaultValue;
            });

            const origineleTekst = resetBtn.innerText;
            resetBtn.innerText = "Gereset!";

            setTimeout(function() {
                resetBtn.innerText = origineleTekst;
            }, 2000);
        });
    }
    
    const opslaanBtn = document.querySelector('button[data-card="beslissingen-opslaan"]');
    if (opslaanBtn) {
        opslaanBtn.addEventListener("click", async function(e) {
            e.preventDefault();

            console.log("Opslaan knop is aangeklikt!");
            
            const beslissingData = {
                teamNummer: teamsNummer,
                rondeNummer: rondeNummer,
                kamerprijs: parseFloat(document.getElementById("kamerprijs-select").value) || 0,
                marketingBudget: parseFloat(document.getElementById("marketingbudget-select").value) || 0,
                marketingFocus: document.getElementById("marketingfocus-select").value,
                aantalMedewerkers: parseInt(document.getElementById("aantalmedewerkers-input").value) || 0,
                typeInvestering: document.getElementById("typeinvestering-select").value,
                investeringKosten: parseFloat(document.getElementById("investeringkosten-input").value) || 0,
                duurzaamheidMaatregel: document.getElementById("duurzaamheidmaatregel-select").value,
                duurzaamheidKosten: parseFloat(document.getElementById("duurzaamheidkosten-input").value) || 0,
                onderhoudsNiveau: document.getElementById("onderhoudsniveau-select").value
            };

            try {
                const response = await fetch('http://localhost:5050/api/Beslissingen', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(beslissingData)
                });

                if (!response.ok) {
                    throw new Error("Kon de beslissingen niet opslaan.");
                }

                const result = await response.json();
                console.log("Succesvol opgeslagen:", result);
                
                const origineleTekst = opslaanBtn.innerText;
                opslaanBtn.innerText = "Opgeslagen!";

                setTimeout(function() {
                    opslaanBtn.innerText = origineleTekst;
                }, 2000);

            } catch (error) {
                console.error("Fout bij opslaan van beslissingen:", error);
                alert("Fout details: " + error.message);
            }
        });
    }

    const opslaanExtraBtn = document.querySelector('button[data-card="extra-beslissingen-opslaan"]');
    if (opslaanExtraBtn) {
        opslaanExtraBtn.addEventListener("click", async function (e) {
            e.preventDefault();

            const kortingVal = document.getElementById("korting-select").value;
            const kortingNummer = kortingVal === "Geen" ? null : parseInt(kortingVal);

            const extraBeslissingData = {
                teamNummer: teamsNummer,
                rondeNummer: rondeNummer,
                doelGroep: document.getElementById("doelgroep-select").value,
                marketingKanaal: document.getElementById("marketingkanaal-select").value,
                personeelTraining: document.getElementById("personeeltraining-select").value,
                korting: kortingNummer
            };

            try {
                const response = await fetch('http://localhost:5050/api/Beslissingen/extrabeslissingen', {
                    method: 'POST',
                    headers: {'Content-Type': 'application/json'},
                    body: JSON.stringify(extraBeslissingData)
                });

                if (!response.ok) throw new Error("Kon de extra beslissingen niet opslaan.");

                const result = await response.json();
                console.log("Extra beslissingen opgeslagen:", result);
                toonSuccesFeedback(opslaanExtraBtn);

            } catch (error) {
                console.error("Fout bij opslaan extra beslissingen:", error);
                alert("Er is iets misgegaan bij het opslaan van de extra beslissingen.");
            }
        });  
    }

});