import { Biblioteka } from "./Biblioteka.js";

let listaBiblioteki = [];

fetch("https://localhost:5001/Biblioteka/PreuzmiDveBiblioteke")
.then(p => {
    p.json().then(biblioteke => {
        biblioteke.forEach(biblioteka => 
        {
            listaBiblioteki.push(new Biblioteka(biblioteka.naziv, biblioteka.adresa, biblioteka.kontakt));
        })

        // listaBiblioteki.forEach(b => 
        // {
        //     b.pribaviKorisnike()
        //     .then(() => 
        //     {
        //         b.pribaviKnjige()
        //         .then(() => b.crtaj(document.body))
        //     })
        // })

        listaBiblioteki[0].pribaviKorisnike().then(() =>
            listaBiblioteki[0].pribaviKnjige().then(() =>
                listaBiblioteki[0].crtaj(document.body).then(() =>
                    listaBiblioteki[1].pribaviKorisnike().then(() =>
                        listaBiblioteki[1].pribaviKnjige().then(() =>
                            listaBiblioteki[1].crtaj(document.body))))))
    })
})

