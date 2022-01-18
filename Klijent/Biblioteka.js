import { Korisnik } from "./Korisnik.js";
import { Knjiga } from "./Knjiga.js";
import { Top5Knjiga } from "./Top5Knjiga.js";

export class Biblioteka
{
    constructor(naziv, adresa, kontakt)
    {
        this.naziv = naziv;
        this.adresa = adresa;
        this.kontakt = kontakt;
        this.kontejner = null;
        this.korisnici = [];
        this.knjige = [];
        this.dugmad = [];
        this.pritisnutaKnjiga = null;
        this.dugmadOcena = [];
    }

    crtaj(host)
    {
        return new Promise((resolve, reject) =>
        {
            this.kontejner = document.createElement("div");
            this.kontejner.className = "glavniKontejner";
            host.appendChild(this.kontejner);
    
            let labelaNaziv = document.createElement("label");
            labelaNaziv.className = "labelaNaslov";
            labelaNaziv.innerHTML = "Biblioteka '" + this.naziv + "'";
            this.kontejner.appendChild(labelaNaziv);
    
            let divZaFormu = document.createElement("div");
            divZaFormu.className = "divZaFormu";
            this.kontejner.appendChild(divZaFormu);
    
            this.crtajTelo(divZaFormu);
    
            let labelaAdresa = document.createElement("label");
            labelaAdresa.innerHTML = "Adresa: " + this.adresa;
            this.kontejner.appendChild(labelaAdresa);
    
            let labelaKontakt = document.createElement("label");
            labelaKontakt.innerHTML = "Kontakt: " + this.kontakt;
            this.kontejner.appendChild(labelaKontakt); 
            resolve();
        })
    }

    crtajTelo(host)
    {
        this.crtajPrviRed(host);

        this.crtajDrugiRed(host);

        this.crtajTreciRed(host);
    }

    crtajPrviRed(host)
    {
        let prviRed = this.crtajRed(host);
        prviRed.classList.add("redUFormi");

        let divDodajKorisnika = document.createElement("div");
        divDodajKorisnika.classList.add("divDodajKorisnika", "divMiniForma");
        this.crtajDodajKorisnika(divDodajKorisnika);
        prviRed.appendChild(divDodajKorisnika);

        let divIznajmiKnjigu = document.createElement("div");
        divIznajmiKnjigu.classList.add("divIznajmiKnjigu", "divMiniForma");
        this.crtajIznajmiKnjigu(divIznajmiKnjigu);
        prviRed.appendChild(divIznajmiKnjigu);
    }

    crtajDrugiRed(host)
    {
        let drugiRed = document.createElement("div");
        drugiRed.classList.add("staviOkvir");
        host.appendChild(drugiRed);

        let divKnjige = document.createElement("div");
        divKnjige.classList.add("red");
        this.crtajDivKnjige(divKnjige);
        drugiRed.appendChild(divKnjige);

        let redDugmad = this.crtajRed(drugiRed);

        let dugmeObrisi = this.crtajDugme(redDugmad, "Obrisi");
        dugmeObrisi.onclick = (ev) => this.obrisi();

        let dugmeIzmeni = this.crtajDugme(redDugmad, "Izmeni");
        dugmeIzmeni.onclick = (ev) => this.izmeni();

        let dugmeDodaj = this.crtajDugme(redDugmad, "Dodaj");
        dugmeDodaj.onclick = (ev) => this.dodaj();
    }

    crtajTreciRed(host)
    {
        let treciRed = this.crtajRed(host);
        treciRed.classList.add("redUFormi");

        let divVratiKnjigu = document.createElement("div");
        divVratiKnjigu.classList.add("vratiKnjigu", "divMiniForma");
        this.crtajVratiKnjigu(divVratiKnjigu);
        treciRed.appendChild(divVratiKnjigu);

        let divTop5 = document.createElement("div");
        divTop5.classList.add("divTop5");
        this.crtajTop5(divTop5);
        treciRed.appendChild(divTop5);
    }

    crtajTop5(host)
    {
        let redNaslov = this.crtajRed(host);
        this.crtajLabelu(redNaslov, "Najbolje ocenjene knjige", "labelaNaslov");
        let divPrikaz = document.createElement("div");
        divPrikaz.classList.add("divPrikaz");
        host.appendChild(divPrikaz);
        let najboljeKnjige = [];
        this.pribaviNajboljeKnjige(najboljeKnjige).then(() => 
        {
            najboljeKnjige.forEach(k => k.crtaj(divPrikaz))
        })
    }

    crtajVratiKnjigu(host)
    {
        let redVratiKnjigu = this.crtajRed(host);
        this.crtajLabelu(redVratiKnjigu, "Vrati knjigu", "labelaNaslov");

        let redKorisnik = this.crtajRed(host);
        redKorisnik.classList.add("vratiKnjiguRedKorisnik");
        this.crtajLabelu(redKorisnik, "Korisnik:", "labela");
        let selectKorisnik = document.createElement("select");
        selectKorisnik.classList.add("vratiKnjiguSelectKorisnik");
        redKorisnik.appendChild(selectKorisnik);

        let op;
        this.korisnici.forEach(k => 
        {
            op = document.createElement("option");
            op.innerHTML = k.naziv;
            op.value = k.naziv;
            selectKorisnik.appendChild(op);
        })

        let redKnjiga = this.crtajRed(host);
        redKnjiga.classList.add("vratiKnjiguRedKnjiga");
        this.crtajLabelu(redKnjiga, "Knjiga:", "labela");
        let selectKnjiga = document.createElement("select");
        selectKnjiga.classList.add("vratiKnjiguSelectKnjiga");
        redKnjiga.appendChild(selectKnjiga);

        this.knjige.forEach(k =>
        {
            if (k.preostalo < k.kolicina)
            {
                op = document.createElement("option");
                op.innerHTML = k.prikaz;
                op.value = k.prikaz;
                selectKnjiga.appendChild(op);
            }
        })

        let redOcena = this.crtajRed(host);
        let radio;
        this.crtajLabelu(redOcena, "Ocena: ", "labela");
        for (let i = 1; i <= 5; i++)
        {
            radio = document.createElement("input");
            radio.type = "radio";
            radio.name = "name";
            radio.value = i;

            redOcena.appendChild(radio);
            this.crtajLabelu(redOcena, i, "labela");
        }

        let redDugme = this.crtajRed(host);
        let dugmeVrati = this.crtajDugme(redDugme, "Vrati");
        redDugme.appendChild(dugmeVrati);
        dugmeVrati.onclick = (ev) =>
        {
            let korisnik = this.kontejner.querySelector(".vratiKnjiguSelectKorisnik");
            let knjiga = this.kontejner.querySelector(".vratiKnjiguSelectKnjiga");
            let ocena = this.kontejner.querySelector("input[name = 'name']:checked");
            if (ocena == null)
            {
                alert("Morate uneti ocenu!");
                return;
            }
            if (korisnik.options[korisnik.selectedIndex] === undefined)
            {
                alert("Morate uneti korisnika!");
                return;
            }
            if (knjiga.options[knjiga.selectedIndex] === undefined)
            {
                alert("Morate uneti knjigu!");
                return;
            }
            this.vratiKnjigu(korisnik.options[korisnik.selectedIndex].value, knjiga.options[knjiga.selectedIndex].value, ocena.value);
        }

    }

    crtajDivKnjige(host)
    {
        let polica = document.createElement("div");
        polica.classList.add("polica");
        this.knjige.forEach(k => 
            {
                this.dugmad.push(k.crtajKnjigu(polica));
            });
        this.dugmad.forEach(d => d.onclick=(ev) =>
        {
            this.pritisnutaKnjiga = this.knjige.find(k => k.prikaz === d.value);
            this.napuniKontrole();
        })
        host.appendChild(polica);

        let formaKnjiga = document.createElement("div");
        formaKnjiga.classList.add("divFormaKnjiga");
        this.crtajFormuKnjiga(formaKnjiga);
        host.appendChild(formaKnjiga);
    }

    crtajFormuKnjiga(host)
    {
        let redAutor = this.crtajRed(host);
        this.crtajLabelu(redAutor, "Autor:", "labela");
        let autorInput = this.crtajInput(redAutor);
        autorInput.className = "autorInput";

        let redNaslov = this.crtajRed(host);
        this.crtajLabelu(redNaslov, "Naslov:", "labela");
        let naslovInput = this.crtajInput(redNaslov);
        naslovInput.className = "naslovInput";

        let redKolicina = this.crtajRed(host);
        this.crtajLabelu(redKolicina, "Kolicina:", "labela");
        let kolicinaInput = this.crtajInput(redKolicina);
        kolicinaInput.className = "kolicinaInput";

        let redPreostalo = this.crtajRed(host);
        this.crtajLabelu(redPreostalo, "Preostalo:", "labela");
        let labelaPreostalo = document.createElement("label");
        labelaPreostalo.innerHTML = "";
        labelaPreostalo.className = "labelaPreostalo";
        redPreostalo.appendChild(labelaPreostalo);

        let redOcena = this.crtajRed(host);
        this.crtajLabelu(redOcena, "Ocena:", "labela");
        let labelaOcena = document.createElement("label");
        labelaOcena.innerHTML = "";
        labelaOcena.className = "labelaOcena";
        redOcena.appendChild(labelaOcena);
    }

    crtajRed(host)
    {
        let red = document.createElement("div");
        red.className = "red";
        host.appendChild(red);
        return red;
    }

    crtajDodajKorisnika(host)
    {
        let redDodajKorisnika = this.crtajRed(host);
        this.crtajLabelu(redDodajKorisnika, "Dodaj korisnika", "labelaNaslov");

        let redIme = this.crtajRed(host);
        this.crtajLabelu(redIme, "Ime:", "labela");
        let imeInput = this.crtajInput(redIme);
        imeInput.className = ".imeInput";

        let redPrezime = this.crtajRed(host);
        this.crtajLabelu(redPrezime, "Prezime:", "labela");
        let prezimeInput = this.crtajInput(redPrezime);
        prezimeInput.className = ".prezimeInput";

        let redJMBG = this.crtajRed(host);
        this.crtajLabelu(redJMBG, "JMBG:", "labela");
        let JMBGInput = this.crtajInput(redJMBG);
        JMBGInput.className = ".JMBGInput";

        let redButton = this.crtajRed(host);
        let dugmeDodajKorisnika = this.crtajDugme(redButton, "Dodaj");
        // dugmeDodaj.className = "dugmeDodajKorisnika";
        dugmeDodajKorisnika.onclick = (ev) => 
        {
            this.dodajKorisnika(imeInput.value, prezimeInput.value, JMBGInput.value).then(() =>
            {
                imeInput.value = "";
                prezimeInput.value = "";
                JMBGInput.value = "";
            });
        }
    }

    crtajIznajmiKnjigu(host)
    {
        let redIznajmiKnjigu = this.crtajRed(host);
        this.crtajLabelu(redIznajmiKnjigu, "Iznajmi knjigu", "labelaNaslov");

        let redKorisnik = this.crtajRed(host);
        redKorisnik.classList.add("iznajmiKnjiguRedKorisnik");
        this.crtajLabelu(redKorisnik, "Korisnik:", "labela");
        let selectKorisnik = document.createElement("select");
        selectKorisnik.classList.add("iznajmiKnjiguSelectKorisnik");
        redKorisnik.appendChild(selectKorisnik);

        let op;
        this.korisnici.forEach(k => 
        {
            op = document.createElement("option");
            op.innerHTML = k.naziv;
            op.value = k.naziv;
            selectKorisnik.appendChild(op);
        })

        let redKnjiga = this.crtajRed(host);
        redKnjiga.classList.add("iznajmiKnjiguRedKnjiga");
        this.crtajLabelu(redKnjiga, "Knjiga:", "labela");
        let selectKnjiga = document.createElement("select");
        selectKnjiga.classList.add("iznajmiKnjiguSelectKnjiga");
        redKnjiga.appendChild(selectKnjiga);

        this.knjige.forEach(k =>
        {
            if (k.preostalo > 0)
            {
                op = document.createElement("option");
                op.innerHTML = k.prikaz;
                op.value = k.prikaz;
                selectKnjiga.appendChild(op);
            }
        })

        let redDugme = this.crtajRed(host);
        let dugmeIznajmi = this.crtajDugme(redDugme, "Iznajmi");
        redDugme.appendChild(dugmeIznajmi); 
        dugmeIznajmi.onclick = (ev) => 
        {
            let korisnik = this.kontejner.querySelector(".iznajmiKnjiguSelectKorisnik");
            let knjiga = this.kontejner.querySelector(".iznajmiKnjiguSelectKnjiga");
            if (korisnik.options[korisnik.selectedIndex] == undefined)
            {
                alert("Morate izabrati korisnika!");
                return;
            }
            if (knjiga.options[knjiga.selectedIndex] == undefined)
            {
                alert("Morate izabrati knjigu!");
                return;
            }
            this.iznajmiKnjigu(korisnik.options[korisnik.selectedIndex].value, knjiga.options[knjiga.selectedIndex].value);
        }       
    }

    crtajLabelu(host, tekst, klasa)
    {
        let labela = document.createElement("label");
        labela.innerHTML = tekst;
        labela.className = klasa;
        host.appendChild(labela);

        return labela;
    }

    crtajInput(host, klasa)
    {
        let input = document.createElement("input");
        input.className = klasa;
        host.appendChild(input);

        return input;
    }

    crtajDugme(host, tekst, klasa)
    {
        let dugme = document.createElement("button");
        dugme.innerHTML = tekst;
        dugme.className = klasa;
        host.appendChild(dugme);

        return dugme;
    }

    vratiKnjigu(korisnik, knjiga, ocena)
    {
        fetch("https://localhost:5001/Iznajmljivanje/VratiKnjigu?kontaktBiblioteke=" + encodeURIComponent(this.kontakt)
        + "&nazivKorisnika=" + encodeURIComponent(korisnik) + "&nazivKnjige=" + encodeURIComponent(knjiga) + "&ocenaKorisnika=" + ocena,
        {
            method: "DELETE"
        }).then(p =>{
            if (p.ok)
            {
                p.json().then(k => {
                    let pomKnjiga = this.knjige.find(p => p.prikaz == knjiga);
                    pomKnjiga.preostalo++;
                    pomKnjiga.ocena = k.ocena;
                    this.updateKnjige(pomKnjiga);
                    this.napuniKontrole();
                    this.updateTop5().then(() => alert(k.poruka));
                })
            }
            else
            {
                p.json().then(k => alert(k.poruka))
            }
        })
    }

    updateTop5()
    {
        return new Promise((resolve) => 
        {
            let najboljeKnjige = [];
            let divovi = this.kontejner.querySelectorAll(".prikazTop5");
            let roditelj = divovi[0].parentNode;
            divovi.forEach(d =>
            {
                    roditelj.removeChild(d);
            })
    
            this.pribaviNajboljeKnjige(najboljeKnjige).then(() =>
            {
                najboljeKnjige.forEach(k => k.crtaj(roditelj))
                resolve();
            });
        })
    }

    dodajKnjiguNaPolicu(knjiga)
    {
        let polica = this.kontejner.querySelector(".polica");
        let dugme = knjiga.crtajKnjigu(polica);
        dugme.onclick = (ev) =>
        {
            this.pritisnutaKnjiga = this.knjige.find(k => k.prikaz === dugme.value);
            this.napuniKontrole();
        }
        this.dugmad.push(dugme);
    }

    napuniKontrole()
    {
        if (this.pritisnutaKnjiga == null)
        {
            this.ocistiKontrole();
            return;
        }

        let autorInput = this.kontejner.querySelector(".autorInput");
        autorInput.value = this.pritisnutaKnjiga.autor;

        let naslovInput = this.kontejner.querySelector(".naslovInput");
        naslovInput.value = this.pritisnutaKnjiga.naslov;

        let kolicinaInput = this.kontejner.querySelector(".kolicinaInput");
        kolicinaInput.value = this.pritisnutaKnjiga.kolicina;

        let labelaPreostalo = this.kontejner.querySelector(".labelaPreostalo");
        labelaPreostalo.innerHTML = this.pritisnutaKnjiga.preostalo;

        let labelaOcena = this.kontejner.querySelector(".labelaOcena");
        labelaOcena.innerHTML = this.pritisnutaKnjiga.ocena;
    }

    ocistiKontrole()
    {
        let autorInput = this.kontejner.querySelector(".autorInput");
        autorInput.value = "";

        let naslovInput = this.kontejner.querySelector(".naslovInput");
        naslovInput.value = "";

        let kolicinaInput = this.kontejner.querySelector(".kolicinaInput");
        kolicinaInput.value = "";

        let labelaPreostalo = this.kontejner.querySelector(".labelaPreostalo");
        labelaPreostalo.innerHTML = "";

        let labelaOcena = this.kontejner.querySelector(".labelaOcena");
        labelaOcena.innerHTML = "";
    }

    dodajKorisnika(ime, prezime, JMBG)
    {
        return new Promise( (resolve, reject) => {
            if (ime === "")
            {
                alert("Morate uneti ime!");
                reject;
                return;
            }
    
            if (prezime === "")
            {
                alert("Morate uneti prezime!");
                reject;
                return;
            }
    
            if (JMBG === "")
            {
                alert("Morate uneti JMBG!");
                reject;
                return;
            }

            if (JMBG.length != 13)
            {
                alert("JMBG mora imati 13 cifara!");
                reject;
                return;
            }
    
            fetch("https://localhost:5001/Korisnik/DodajKorisnika?kontaktBiblioteke=" + encodeURIComponent(this.kontakt)
            + "&ime=" + encodeURIComponent(ime) + "&prezime=" + encodeURIComponent(prezime) + "&JMBG=" + encodeURIComponent(JMBG),
            {
                method: "POST"
            })
            .then(p => {
                if (p.ok)
                {
                    p.json().then(msg =>
                    {
                        this.korisnici.push(new Korisnik(ime + " " + prezime + " " + JMBG));
                        this.updateKorisnike();
                        alert(msg.poruka);
                        resolve();
                    })
                }
                else
                {
                    p.json().then(msg => alert(msg.poruka))
                    reject();
                }  
            })
        }) 
    }

    updateKorisnike()
    {
        let iznajmiKnjiguSelectKorisnik = this.kontejner.querySelector(".iznajmiKnjiguSelectKorisnik");
        let roditelj = iznajmiKnjiguSelectKorisnik.parentNode;
        roditelj.removeChild(iznajmiKnjiguSelectKorisnik);

        iznajmiKnjiguSelectKorisnik = document.createElement("select");
        iznajmiKnjiguSelectKorisnik.classList.add("iznajmiKnjiguSelectKorisnik");
        let op;
        this.korisnici.forEach(k => 
        {
            op = document.createElement("option");
            op.innerHTML = k.naziv;
            op.value = k.naziv;
            iznajmiKnjiguSelectKorisnik.appendChild(op);
        })
        roditelj.appendChild(iznajmiKnjiguSelectKorisnik);

        let vratiKnjiguSelectKorisnik = this.kontejner.querySelector(".vratiKnjiguSelectKorisnik");
        roditelj = vratiKnjiguSelectKorisnik.parentNode;
        roditelj.removeChild(vratiKnjiguSelectKorisnik);

        vratiKnjiguSelectKorisnik = document.createElement("select");
        vratiKnjiguSelectKorisnik.classList.add("vratiKnjiguSelectKorisnik");
        this.korisnici.forEach(k => 
        {
            op = document.createElement("option");
            op.innerHTML = k.naziv;
            op.value = k.naziv;
            vratiKnjiguSelectKorisnik.appendChild(op);
        })
        roditelj.appendChild(vratiKnjiguSelectKorisnik);
    }

    iznajmiKnjigu(korisnik, knjiga)
    {
        if (korisnik === "")
        {
            alert("Morate izabrati korisnika");
            return;
        }

        if (knjiga === "")
        {
            alert("Morate izabrati knjigu!");
            return;
        }

        fetch("https://localhost:5001/Iznajmljivanje/IznajmiKnjigu?kontaktBiblioteke=" + encodeURIComponent(this.kontakt)
        + "&nazivKorisnika=" + encodeURIComponent(korisnik) + "&nazivKnjige=" + encodeURIComponent(knjiga),
        {
            method: "POST"
        }).then(p =>{
                if (p.ok)
                {
                    p.json().then(msg =>
                    {
                        let pomKnjiga = this.knjige.find(k => k.prikaz == knjiga);
                        pomKnjiga.preostalo--;
                        this.updateKnjige(pomKnjiga);
                        this.napuniKontrole();
                        alert(msg.poruka);
                    })
                }
                else
                {
                    p.json().then(msg => alert(p.status))
                }
            })
    }

    updateKnjige(knjiga)
    {
        let iznajmiKnjiguSelectKnjiga = this.kontejner.querySelector(".iznajmiKnjiguSelectKnjiga");
        let roditelj = iznajmiKnjiguSelectKnjiga.parentNode;
        roditelj.removeChild(iznajmiKnjiguSelectKnjiga);

        iznajmiKnjiguSelectKnjiga = document.createElement("select");
        iznajmiKnjiguSelectKnjiga.classList.add("iznajmiKnjiguSelectKnjiga");
        let op;
        this.knjige.forEach(k =>
        {
            if (k.preostalo > 0)
            {
                op = document.createElement("option");
                op.innerHTML = k.prikaz;
                op.value = k.prikaz;
                iznajmiKnjiguSelectKnjiga.appendChild(op);
            }
        })
        roditelj.appendChild(iznajmiKnjiguSelectKnjiga);

        let vratiKnjiguSelectKnjiga = this.kontejner.querySelector(".vratiKnjiguSelectKnjiga");
        roditelj = vratiKnjiguSelectKnjiga.parentNode;
        roditelj.removeChild(vratiKnjiguSelectKnjiga);

        vratiKnjiguSelectKnjiga = document.createElement("select");
        vratiKnjiguSelectKnjiga.classList.add("vratiKnjiguSelectKnjiga");
        this.knjige.forEach(k =>
        {
            if (k.preostalo < k.kolicina)
            {
                op = document.createElement("option");
                op.innerHTML = k.prikaz;
                op.value = k.prikaz;
                vratiKnjiguSelectKnjiga.appendChild(op);
            }
        })
        roditelj.appendChild(vratiKnjiguSelectKnjiga);

        if (knjiga === null) return;

        let dugme = this.dugmad.find(k => k.value === knjiga.prikaz);
        if (knjiga.preostalo > 0)
        {
            dugme.classList.remove("nedostupno");
            dugme.classList.add("dostupno");
        }
        else
        {
            dugme.classList.remove("dostupno");            
            dugme.classList.add("nedostupno");
        }
    }

    ukloniKnjiguSaPolice(knjiga)
    {
        let dugme = this.dugmad.find(k => k.value === knjiga.prikaz);
        let roditelj = dugme.parentNode;
        roditelj.removeChild(dugme);
    }

    updateKnjiguSaPolice(knjiga)
    {
        let dugme = this.dugmad.find(k => k.value === knjiga.prikaz);
        if (knjiga.preostalo > 0)
        {
            dugme.classList.remove("nedostupno");
            dugme.classList.add("dostupno");
        }
        else
        {
            dugme.classList.remove("dostupno");            
            dugme.classList.add("nedostupno");
        }
    }

    pribaviKorisnike()
    {
        return new Promise( (resolve, reject) => {
            fetch("https://localhost:5001/Korisnik/PreuzmiKorisnikeIzBiblioteke?kontaktBiblioteke=" + encodeURIComponent(this.kontakt),
            {
                method: "GET"
            })
            .then(p => {
                if (p.ok)
                {
                    p.json().then(k => {
                        k.forEach(korisnik =>
                            {
                                this.korisnici.push(new Korisnik(korisnik.naziv));
                            });
                            
                        resolve();
                    });
                }
                else
                {
                    alert(p.status);
                    reject();
                }
                    
            })
        })
        
    }

    pribaviKnjige()
    {   
        return new Promise( (resolve, reject) => {
            let fetchString = "https://localhost:5001/Knjiga/PreuzmiKnjigeIzBiblioteke?kontaktBiblioteke=" + encodeURIComponent(this.kontakt);
            fetch(fetchString,
            {
                method: "GET"
            })
            .then(p => {
                if (p.ok)
                {
                    p.json().then(k => {
                        k.forEach(knjiga =>
                            {
                                this.knjige.push(new Knjiga(knjiga.autor, knjiga.naslov, knjiga.prikaz, knjiga.ocena, knjiga.kolicina, knjiga.preostalo));
                            });
                            
                        resolve();
                    });
                }
                else
                {
                    alert(p.status);
                    reject();
                }

            })
        })
        
    }

    pribaviNajboljeKnjige(knjige)
    {   
        return new Promise( (resolve, reject) => {
            let fetchString = "https://localhost:5001/Knjiga/Top5KnjigaIzBiblioteke?kontaktBiblioteke=" + encodeURIComponent(this.kontakt);
            fetch(fetchString,
            {
                method: "GET"
            })
            .then(p => {
                if (p.ok)
                {
                    p.json().then(k => {
                        k.forEach(knjiga =>
                            {
                                knjige.push(new Top5Knjiga(knjiga.prikaz, knjiga.ocena));
                            });
                            
                        resolve();
                    });
                }
                else
                {
                    alert(p.status);
                    reject();
                }
            })
        })
    }

    dodaj()
    {
        let autor = this.kontejner.querySelector(".autorInput").value;
        let naslov = this.kontejner.querySelector(".naslovInput").value;
        let kolicina = this.kontejner.querySelector(".kolicinaInput").value;

        if (autor === "")
        {
            alert("Morate uneti autora knjige!");
            return;
        }

        if (naslov === "")
        {
            alert("Morate uneti naslov knjige!");
            return;
        }

        if (kolicina === "")
        {
            alert("Morate uneti kolicinu knjige!");
            return;
        }

        if (kolicina <= 0)
        {
            alert("Knjiga mora imati kolicinu vecu od 0!");
            return;
        }

        fetch("https://localhost:5001/Knjiga/DodajKnjiguUBiblioteku?kontaktBiblioteke=" + encodeURIComponent(this.kontakt) +
        "&autorKnjige=" + encodeURIComponent(autor) + "&naslovKnjige=" + encodeURIComponent(naslov) + "&kolicinaKnjige=" + encodeURIComponent(kolicina),
        {
            method: "POST"
        })
        .then(p => {
            if (p.ok)
            {
                p.json().then(k => 
                {
                    let novaKnjiga = new Knjiga(autor, naslov, autor + " - " + naslov, "UNK", kolicina, kolicina);
                    this.knjige.push(novaKnjiga);
                    this.pritisnutaKnjiga = novaKnjiga;
                    this.dodajKnjiguNaPolicu(novaKnjiga);
                    this.updateKnjige(novaKnjiga);
                    this.napuniKontrole();
                    alert(k.poruka);
                });
            }
            else
            {
                p.json().then(k => { alert(k.poruka) })
            }

        })
    }

    izmeni()
    {
        if (this.pritisnutaKnjiga === null)
        {
            alert("Morate kliknuti na knjigu koju zelite da izmenite!");
            return;
        }

        let autor = this.kontejner.querySelector(".autorInput").value;
        let naslov = this.kontejner.querySelector(".naslovInput").value;

        if (this.pritisnutaKnjiga.prikaz != autor + " - " + naslov)
        {
            alert("Morate kliknuti na knjigu, i direktno kliknuti dugme 'Izmeni', bez da menjate autora i naslov knjige!");
            return;
        }

        let kolicinaInput = this.kontejner.querySelector(".kolicinaInput");
        let novaKolicina = kolicinaInput.value;

        fetch("https://localhost:5001/Knjiga/IzmeniKnjiguUBiblioteci?kontaktBiblioteke=" + encodeURIComponent(this.kontakt) +
        "&nazivKnjige=" + encodeURIComponent(this.pritisnutaKnjiga.prikaz) + "&novaKolicinaKnjige=" + encodeURIComponent(novaKolicina),
        {
            method: "PUT"
        })
        .then(p => {
            if (p.ok)
            {
                p.json().then(k => {
                    this.pritisnutaKnjiga.preostalo += novaKolicina - this.pritisnutaKnjiga.kolicina;
                    this.pritisnutaKnjiga.kolicina = novaKolicina;
                    this.updateKnjiguSaPolice(this.pritisnutaKnjiga);
                    this.updateKnjige(this.pritisnutaKnjiga);
                    this.napuniKontrole();
                    alert(k.poruka);
                });
            }
            else
            {
                p.json().then(k => { alert(k.poruka) })
            }
        })
    }

    obrisi()
    {
        if (this.pritisnutaKnjiga === null)
        {
            alert("Morate kliknuti na knjigu koju zelite da obrisete!");
            return;
        }

        let autor = this.kontejner.querySelector(".autorInput").value;
        let naslov = this.kontejner.querySelector(".naslovInput").value;

        if (this.pritisnutaKnjiga.prikaz != autor + " - " + naslov)
        {
            alert("Morate kliknuti na knjigu, i direktno kliknuti dugme 'Obrisi', bez da menjate autora i naslov knjige!");
            return;
        }

        fetch("https://localhost:5001/Knjiga/UkloniKnjiguIzBiblioteke?kontaktBiblioteke=" + encodeURIComponent(this.kontakt) +
        "&nazivKnjige=" + encodeURIComponent(this.pritisnutaKnjiga.prikaz),
        {
            method: "DELETE"
        })
        .then(p => {
            if (p.ok)
            {
                p.json().then(k => {
                    this.ukloniKnjiguSaPolice(this.pritisnutaKnjiga);
                    this.knjige = this.knjige.filter(k => this.pritisnutaKnjiga.prikaz != k.prikaz);
                    this.updateKnjige(this.pritisnutaKnjiga);
                    this.pritisnutaKnjiga = null;
                    this.napuniKontrole();
                    this.updateTop5().then(() => alert(k.poruka))
                });
            }
            else
            {
                p.json().then(k => { alert(k.poruka) })
            }
        })
    }
}