export class Top5Knjiga
{
    constructor(prikaz, ocena)
    {
        this.prikaz = prikaz;
        this.ocena = ocena;
    }

    crtaj(host)
    {
        let pom = document.createElement("div");
        pom.classList.add("rotate", "prikazTop5");
        let sirina = (this.ocena / 5) * 150;
        pom.style.height = (sirina + "px");
        pom.innerHTML = this.prikaz + " - " + this.ocena;
        host.appendChild(pom);
    }
}