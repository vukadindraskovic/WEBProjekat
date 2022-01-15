--BIBLIOTEKE

SET IDENTITY_INSERT Biblioteke ON

INSERT INTO Biblioteke
(ID, Naziv, Adresa, Kontakt) 
VALUES (1, 'Stevan Sremac, Niš', 'Pobedina 13, Niš', '067 235192');

INSERT INTO Biblioteke
(ID, Naziv, Adresa, Kontakt) 
VALUES (2, 'Bora Stanković, Niš', 'Branka Miljkovića 34, Niš', '067 234193');

SET IDENTITY_INSERT Biblioteke OFF

-- KNJIGE

SET IDENTITY_INSERT Knjige ON

INSERT INTO Knjige
(ID, Autor, Naslov, BrojOcenjivanja, Ocena) 
VALUES (1, 'Fjodor M. Dostojevski', 'Zločin i kazna', 0, 0);

INSERT INTO Knjige
(ID, Autor, Naslov, BrojOcenjivanja, Ocena) 
VALUES (2, 'Fjodor M. Dostojevski', 'Braća Karamazovi', 0, 0);

INSERT INTO Knjige
(ID, Autor, Naslov, BrojOcenjivanja, Ocena) 
VALUES (3, 'Agata Kristi', 'Ubistvo u Orijent ekspresu', 0, 0);

INSERT INTO Knjige
(ID, Autor, Naslov, BrojOcenjivanja, Ocena) 
VALUES (4, 'Agata Kristi', 'Ubistva po abecedi', 0, 0);

SET IDENTITY_INSERT Knjige OFF

--KNJIGEBIBLIOTEKE

SET IDENTITY_INSERT KnjigeBiblioteke ON

INSERT INTO KnjigeBiblioteke
(ID, KnjigaID, BibliotekaID, Kolicina, Preostalo) 
VALUES (1, 1, 1, 2, 2);

INSERT INTO KnjigeBiblioteke
(ID, KnjigaID, BibliotekaID, Kolicina, Preostalo) 
VALUES (2, 2, 1, 2, 2);

INSERT INTO KnjigeBiblioteke
(ID, KnjigaID, BibliotekaID, Kolicina, Preostalo) 
VALUES (3, 3, 2, 2, 2);

INSERT INTO KnjigeBiblioteke
(ID, KnjigaID, BibliotekaID, Kolicina, Preostalo) 
VALUES (4, 4, 2, 2, 2);

SET IDENTITY_INSERT KnjigeBiblioteke OFF