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

    laadTeamStats()
    async function laadTeamStats() {
        try {
            const response = await fetch(`http://localhost:5050/api/teamstats?teamnummer=1&rondenummer=1`);

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
        opslaanBtn.addEventListener("click", function(e) {
            e.preventDefault()

            const origineleTekst = opslaanBtn.innerText;
            opslaanBtn.innerText = "Opgeslagen!";

            setTimeout(function() {
                opslaanBtn.innerText = origineleTekst;
            }, 2000);
        });
    }

});