- [ERD (Lucidchart)](https://lucid.app/lucidchart/f25db3e7-9bcf-4475-9461-9dd03ea9e1cc/view)

# Webbshoppen

## Beskrivning
Ni ska bygga en “webb”-shop som säljer ett valfritt sortiment. Webbshoppen ska ha följande funktionalitet:

### Webbshoppen
- **Startsida**
  - Välkomst-text
  - Tre utvalda produkter ska visas på sidan. Detta ska kunna anges i admin.
- **Shopsida**
  - Minst tre kategorier (t.ex. Tröjor, Byxor och Skor), cirka 5 produkter av varje.
  - Möjlighet att fritextsöka
  - Varje produkt ska kunna väljas för att få veta mer om produkten
  - Varje produkt ska ha ett val för “köp” som adderar produkten till kundkorg
  - Övrig info: Kort text om produkten, Pris
- **Varukorg**
  - Valda produkter visas i lista
  - Möjligt att ändra antal
  - Möjlighet att ta bort produkt
  - Priset visas och summan av produkterna visas längst ner
- **Frakt-vy**
  - Namn, adress m.m. ska gå att fylla i via 

Console.ReadLine()


  - Val av frakt med minst två alternativ, med olika pris
- **Betala-vy**
  - Produkterna visas, med pris
  - Pris med frakt, samt moms visas
  - Val av betalningsmetod, minst två. Betalningen behöver självklart inte vara på riktigt
  - När varan är betald så töms varukorgen

### Webbshoppen - Admin
- **Admin**
  - Produkter – kunna lägga till nya, ta bort och ändra
    - Produktnamn
    - Infotext
    - Pris
    - Produktkategori
    - Leverantör
    - Lagersaldo
  - Produktkategorier (t.ex. byxor, tröjor osv)
  - Kunder
    - Ändra uppgifter om kunden
    - Beställningshistorik

### Förslag på datafält
- Produktnamn
- Pris
- Detaljerad information
- Land
- Telefon
- Mejl
- Ålder
- Produktkategori
- Produktleverantör
- Utvald produkt (bool)
- Leveransalternativ
- Kund
  - Namn
  - Adress
  - Gata
  - Stad

### Förslag på querys
- Bäst säljande produkter
- Populäraste kategorin
- Populäraste produkt för herr/dam-sortiment
- Flest beställningar per stad
- Försäljning sorterat på leverantörer

### Teknik
- Fokus ligger på databasdelen, men programmeringen ska vara av god kvalitet (DRY)
- En viss del av uppgiften ska vara utförd med ren SQL, t.ex. via Dapper
- En betydande del ska göras med hjälp av Entity Framework, inkl constraints
- LINQ ska huvudsakligen användas, men mer komplexa querys kan göras med SQL/Dapper
- Databasen ska ligga på Azure
- Dokumentdatabas (MongoDB) samt transaktioner behöver inte vara med i G-delen
- Övrig Teknik
  - Asynkrona anrop till databasen, inkl tidtagning
  - Moment som Enums, try/catch ska finnas med

### Redovisning och inlämning
- Redovisning sker kursens sista fredag, muntligen, där funktion och struktur beskrivs
- Butiken ska vara fylld med produkter, tidigare kunder och inköp
- Inlämning sker söndagen samma vecka
  - Inlämningen består av:
    - ER-diagram (Lucidcharts)
    - GIT-länk till projektet (Console-appen)
    - Testdata ska finnas inlagt
    - Databasen ska finnas upplagd på Azure

### Arbetsgång
- Bestäm butiksnamn och sortiment
- Skissa på papper eller ritprogram
- Rita ER-diagram i Lucidchart (kommer att behövas ändras efter lovet)
- Skapa klasser – Skapa DB med EF Code first
- Skapa GUI
- Programmera funktionalitet
- Testa (Funktionstest)
- Planera redovisning

### VG-del
- Målet med VG-uppgiften är att: “Den studerande visar förmågan att lösa programmeringsrelaterade uppgifter på ett genomtänkt sätt som påvisar djupare förståelse för kodens uppbyggnad. Den studerande kan planera sitt arbete så att leverans sker enligt deadline.”
- Extra features + högre krav på användning av databaser och kodning
- Du väljer själv vilka features du vill visa upp, och väljer teknik efter vad som är lämpligt (kommentera i koden hur du tänkt)
- Viktigt är att bara göra VG-uppgiften om du själv känner att du uppfyller kursens krav på VG. Satsa hellre på G, och ge dig på VG när du är klar med detta.

### VG-delen - Krav
- Inloggning av kunder och admin, med olika användarnamn/lösenord (I G-delen räcker om du väljer olika användare via menyval)
- Mer befintliga kunder/beställningar/ordrar
- Statistik(Querys) som visar demografisk information, exempelvis:
  - Vad vill personer i olika åldrar, kön m.m. köpa?
  - Vilka typer av kunder har shoppen?
  - Vilka delar av landet har bäst kunder: södra/mellersta/norra Sverige
- Loggning av transaktioner via MongoDb
  - Kunder
    - Inloggningar
    - Registrering
    - Köp
  - Admin
    - Inloggningar
    - Tilläggning av produkter
- Leverans enligt deadline!
- Stor möjlighet att själv bestämma vad du tror bäst visar din kompetens...

### VG-delen
- Större vikt på komplexitet/constraints/normalisering osv
- Korrekta namn på databaser/klasser/properties/listor osv
- Välj att göra koden självförklarande, men kommentera där det behövs
- Se till att applikationen är extra enkel och logisk att använda
- Skapa intressanta och givande queries

### Slutord om uppgifterna
- En uppgift
  - G-uppgift Webbshoppen
  - VG-uppgift Webbshoppen på VG-nivå
- Webbshoppens G-del är den viktigaste inlämningen, och där avgörs om det är G på kursen
- Mycket viktigt att informera om du har för avsikt att lämna in uppgiften på VG-nivå, då det innebär strängare krav på helheten.