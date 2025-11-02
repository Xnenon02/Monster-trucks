 Monster Tracker  
### Ett databasprojekt av Tom Ekstrand  

---

## 📖 Översikt  

**Monster Tracker** är en C#-applikation som hanterar observationer av monster, deras platser och jägare.  
Projektet är byggt med **SQLite** som databas och implementerar **Repository Pattern** samt **Facade Pattern** för tydlig kodstruktur.  

Syftet är att demonstrera god databasdesign (3NF), säker SQL-hantering, och en robust arkitektur som separerar logik, datalager och användargränssnitt.

---

## 🧩 Funktioner  

- Skapa, läsa, uppdatera och ta bort **monster, platser, jägare och observationer**  
- **Automatisk seeding** av testdata med \`DatabaseSeeder\`  
- **Rapporter** över observationer och statistik  
- **Säkra SQL-frågor** med parametrisering (förhindrar SQL-injektion)  
- **Transaktioner** vid databasuppdateringar för att skydda dataintegritet  
- **AuditLog** för att registrera ändringar av observationer  

---

## 🗂️ Databasstruktur  

Projektet består av följande tabeller:

| Tabell | Beskrivning |
|--------|--------------|
| **Monster** | Lagrar information om olika monster (namn, art, farlighetsnivå) |
| **Location** | Platsdata (namn, beskrivning, farlighetsnivå) |
| **Hunter** | Jägare som gjort observationer |
| **Observation** | Relationstabell mellan Monster, Location och Hunter |
| **AuditLog** | Loggar ändringar av observationer |

Alla tabeller följer **tredje normalformen (3NF)** och använder främmande nycklar med ON DELETE-skydd.

---

## 🧱 Arkitektur  

Projektet är uppdelat i flera lager för att följa **SRP (Single Responsibility Principle)**:

\`\`\`
Monster_trucks/
│
├── Data/
│   ├── DatabaseConnection.cs
│   ├── MonsterRepository.cs
│   ├── LocationRepository.cs
│   ├── HunterRepository.cs
│   ├── ObservationRepository.cs
│
├── Models/
│   ├── Monster.cs
│   ├── Location.cs
│   ├── Hunter.cs
│   ├── Observation.cs
│
├── Services/
│   ├── DatabaseSeeder.cs
│
├── Program.cs
└── monstertracker_schema.sql
\`\`\`

- **Repositories** – ansvarar för alla SQL-operationer.  
- **DatabaseConnection** – laddar och initierar databasen.  
- **DatabaseSeeder** – fyller databasen med testdata.  
- **Facade** – hanterar interaktion mellan flera repositories.  

---

## ⚙️ Installation & Körning  

### Förutsättningar  
- .NET 8.0 SDK eller senare  
- Visual Studio / VS Code  
- SQLite installerat eller medföljer som inbäddad databas  

### Steg för steg  

1. Klona projektet eller kopiera koden till en lokal mapp.  
   \`\`\`bash
   git clone https://github.com/[ditt-repo-namn]/Monster_Tracker.git
   cd Monster_Tracker
   \`\`\`

2. Kör programmet (via terminal eller Visual Studio):
   \`\`\`bash
   dotnet run
   \`\`\`

3. Vid första körningen initieras databasen automatiskt via \`DatabaseSeeder\`.  
   Testdata läggs till om tabellerna är tomma.

4. I konsolen kan du:
   - Lista monster och platser  
   - Lägga till nya observationer  
   - Se rapporter och loggar  

---

## 🔒 Säkerhet  

Alla SQL-kommandon använder **parametriserade queries**:
\`\`\`csharp
cmd.CommandText = "SELECT * FROM Monster WHERE Name = @name";
cmd.Parameters.AddWithValue("@name", name);
\`\`\`
Detta skyddar mot **SQL-injektion**, även om användaren försöker mata in skadlig kod.  

Transaktioner används också för att säkerställa att flera databasoperationer antingen lyckas helt eller rullas tillbaka vid fel.

---

## 🧠 Lärdomar  

Projektet demonstrerar praktisk tillämpning av:  
- Databasdesign i 3NF  
- SQL och ADO.NET med SQLite  
- Designmönster (Repository + Facade)  
- SRP och tydlig separering mellan lager  
- Felhantering och robust kodstruktur  

---

## 📸 Skärmdumpar

1. **Programstart & meny**
![Screenshot 1](./Monster%20trucks/screenshots/1-startmenu.png)

2. **Databas seeding och exempeldata**
![Screenshot 2](./Monster%20trucks/screenshots/2-seeding.png)

3. **Observationer och rapportvy**
![Screenshot 3](./Monster%20trucks/screenshots/3-report.png)

4. **Felhantering vid försök att radera monster med observationer**
![Screenshot 4](./Monster%20trucks/screenshots/4-error.png)


---

## 👨‍💻 Utvecklare  

**Tom Ekstrand**  
2025-11-02
" > README.md
