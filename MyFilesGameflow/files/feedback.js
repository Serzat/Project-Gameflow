// Adres van de backend (zie backend/Gameflow.Api/Properties/launchSettings.json)
const API_URL = "http://localhost:5063";

// Welke feedback we laten zien, bijvoorbeeld feedback.html?competitie=1&team=1&ronde=2
const parameters = new URLSearchParams(window.location.search);
const competitieId = parameters.get("competitie") || 1;
const teamId = parameters.get("team") || 1;
const rondeNummer = parameters.get("ronde") || 2;

// Kleur en icoon per KPI-kaart
const kpiStijl = {
    "Bezettingsgraad": {
        kleur: "blauw",
        icoon: '<path d="M3 21V8l9-5 9 5v13M9 21v-6h6v6"/>'
    },
    "Gasttevredenheid": {
        kleur: "roze",
        icoon: '<circle cx="12" cy="12" r="9"/><path d="M8 14s1.5 2 4 2 4-2 4-2M9 9h.01M15 9h.01"/>'
    },
    "Winst & Cashflow": {
        kleur: "geel",
        icoon: '<path d="M12 2v20M17 5H9.5a3.5 3.5 0 000 7h5a3.5 3.5 0 010 7H6"/>'
    },
    "Duurzaamheid": {
        kleur: "groen",
        icoon: '<path d="M11 20A7 7 0 014 13c0-6 7-10 16-10 0 9-4 16-10 16zM4 21c3-6 6-9 11-12"/>'
    }
};

laadFeedback();

async function laadFeedback() {
    const url = `${API_URL}/api/competities/${competitieId}/teams/${teamId}/rondes/${rondeNummer}/feedback`;

    try {
        const antwoord = await fetch(url);

        if (!antwoord.ok) {
            const foutmelding = await antwoord.text();
            toonMelding(foutmelding || "De feedback kon niet worden opgehaald.");
            telAlleTellersOp();
            return;
        }

        const feedback = await antwoord.json();
        toonFeedback(feedback);
    } catch (fout) {
        toonMelding("Geen verbinding met de backend. Start de backend en ververs de pagina. Je ziet nu voorbeelddata.");
        telAlleTellersOp();
    }
}

function toonFeedback(feedback) {
    // Team en ronde
    document.getElementById("team-naam").textContent = feedback.teamNaam;
    document.getElementById("team-afkorting").textContent = maakAfkorting(feedback.teamNaam);
    document.getElementById("ronde-tag").textContent = `Ronde ${feedback.rondeNummer} • Resultaten`;

    document.getElementById("ronde-titel").textContent = feedback.rondeAfgerond
        ? `Ronde ${feedback.rondeNummer} afgerond`
        : `Ronde ${feedback.rondeNummer} bezig`;

    document.getElementById("ronde-info").textContent = feedback.afgerondOp
        ? `${feedback.teamNaam} • ${feedback.afgerondOp}`
        : feedback.teamNaam;

    // Totaalscore en ring met de voortgang van de rondes
    document.getElementById("totaal-score").dataset.getal = feedback.totaalScore;
    toonRing(feedback.rondeNummer, feedback.aantalRondes);

    // Positie en podium
    document.getElementById("positie").textContent = feedback.positie + "e";

    for (let plek = 1; plek <= 3; plek++) {
        document.getElementById("trede-" + plek).classList.toggle("jij", plek === feedback.positie);
    }

    // Punten van deze ronde
    document.getElementById("punten-teken").textContent = feedback.puntenDezeRonde < 0 ? "-" : "+";
    document.getElementById("punten-ronde").dataset.getal = Math.abs(feedback.puntenDezeRonde);

    // KPI-kaarten, lijstjes en beslissingen
    toonKpis(feedback.kpis);
    toonLijst("lijst-goed", feedback.watGingGoed);
    toonLijst("lijst-beter", feedback.watKanBeter);
    toonAdvies(feedback.advies);
    toonBeslissingen(feedback.beslissingen);

    telAlleTellersOp();
}

function toonRing(ronde, aantalRondes) {
    const omtrek = 214;
    const deel = aantalRondes > 0 ? ronde / aantalRondes : 0;

    document.getElementById("ring-voor").style.strokeDashoffset = omtrek - omtrek * deel;
    document.getElementById("ring-tekst").innerHTML = `${ronde}/${aantalRondes}<small>ronde</small>`;
}

function toonKpis(kpis) {
    const container = document.getElementById("kpi-kaarten");
    container.innerHTML = "";

    kpis.forEach(function (kpi) {
        const stijl = kpiStijl[kpi.naam] || { kleur: "blauw", icoon: "" };
        const pijl = kpi.gestegen ? "▲" : "▼";
        const richting = kpi.gestegen ? "omhoog" : "omlaag";

        container.innerHTML += `
            <div class="kpi-kaart ${stijl.kleur}">
                <div class="kpi-kop">
                    <p>${veilig(kpi.naam)}</p>
                    <svg viewBox="0 0 24 24">${stijl.icoon}</svg>
                </div>
                <div class="kpi-waarde">
                    <h2>${veilig(kpi.waarde)}</h2>
                    <span class="badge ${richting}">${pijl} ${veilig(kpi.verschil)}</span>
                </div>
                <div class="balk"><div style="width: ${kpi.balkProcent}%"></div></div>
            </div>`;
    });
}

function toonLijst(id, zinnen) {
    const lijst = document.getElementById(id);
    lijst.innerHTML = "";

    zinnen.forEach(function (zin) {
        lijst.innerHTML += `<li>${veilig(zin)}</li>`;
    });
}

function toonAdvies(adviezen) {
    const lijst = document.getElementById("lijst-advies");
    lijst.innerHTML = "";

    adviezen.forEach(function (advies, index) {
        lijst.innerHTML += `<li><span class="nummer">${index + 1}</span> ${veilig(advies)}</li>`;
    });
}

function toonBeslissingen(beslissingen) {
    const container = document.getElementById("invloed-lijst");
    container.innerHTML = "";

    if (beslissingen.length === 0) {
        container.innerHTML = `<p class="klein">Er zijn deze ronde geen beslissingen genomen.</p>`;
        return;
    }

    beslissingen.forEach(function (b) {
        const soort = b.positief ? "positief" : "negatief";
        const pijl = b.positief ? "↗" : "↘";

        container.innerHTML += `
            <div class="invloed-rij">
                <span class="beslissing">${veilig(b.beslissing)}</span>
                <span class="label ${soort}">${pijl} ${veilig(b.effect)}</span>
            </div>`;
    });
}

// Getallen laten optellen van 0 tot de echte waarde
function telAlleTellersOp() {
    document.querySelectorAll(".teller").forEach(function (teller) {
        const doel = parseInt(teller.dataset.getal) || 0;
        const stap = Math.max(1, Math.ceil(doel / 40));
        let huidig = 0;

        const interval = setInterval(function () {
            huidig += stap;

            if (huidig >= doel) {
                huidig = doel;
                clearInterval(interval);
            }

            teller.textContent = huidig;
        }, 25);
    });
}

function toonMelding(tekst) {
    const melding = document.getElementById("melding");
    melding.textContent = tekst;
    melding.classList.add("zichtbaar");
}

// "Team 1" wordt "T1"
function maakAfkorting(naam) {
    return naam
        .split(" ")
        .map(function (woord) { return woord.charAt(0); })
        .join("")
        .substring(0, 2)
        .toUpperCase();
}

// Voorkomt dat tekst uit de backend als HTML wordt uitgevoerd
function veilig(tekst) {
    const div = document.createElement("div");
    div.textContent = tekst;
    return div.innerHTML;
}