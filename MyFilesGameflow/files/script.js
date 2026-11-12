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