export class Knjiga
{
    constructor(autor, naslov, prikaz, ocena, kolicina, preostalo)
    {
        this.autor = autor;
        this.naslov = naslov;
        this.prikaz = prikaz;
        this.ocena = ocena;
        this.kolicina = kolicina;
        this.preostalo = preostalo;
    }

    crtajKnjigu(host)
    {
        let dugme = document.createElement("button");
        dugme.innerHTML = this.prikaz;
        dugme.value = this.prikaz;
        if (this.preostalo > 0)
            dugme.classList.add("rotate", "dostupno");
        else
            dugme.classList.add("rotate", "nedostupno");

        host.appendChild(dugme);

        return dugme;
    }
}