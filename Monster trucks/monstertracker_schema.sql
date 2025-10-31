-- Monster table
CREATE TABLE IF NOT EXISTS Monster (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Species TEXT NOT NULL,
    DangerRating INTEGER NOT NULL CHECK (DangerRating BETWEEN 1 AND 5)
);

-- Location table
CREATE TABLE IF NOT EXISTS Location (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Description TEXT NOT NULL,
    DangerLevel INTEGER NOT NULL CHECK (DangerLevel BETWEEN 1 AND 5)
);

-- Hunter table
CREATE TABLE IF NOT EXISTS Hunter (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    ExperienceLevel INTEGER NOT NULL CHECK (ExperienceLevel BETWEEN 1 AND 10),
    ContactInfo TEXT
);

-- Observation table
CREATE TABLE IF NOT EXISTS Observation (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MonsterId INTEGER NOT NULL,
    LocationId INTEGER NOT NULL,
    HunterId INTEGER NOT NULL,
    ObservedAt TEXT NOT NULL,
    Notes TEXT,
    FOREIGN KEY (MonsterId) REFERENCES Monster(Id),
    FOREIGN KEY (LocationId) REFERENCES Location(Id),
    FOREIGN KEY (HunterId) REFERENCES Hunter(Id)
);

-- ObservationAudit table
CREATE TABLE IF NOT EXISTS AuditLog (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ObservationId INTEGER NOT NULL,
    OldLocationId INTEGER NOT NULL,
    NewLocationId INTEGER NOT NULL,
    ChangedAt TEXT NOT NULL,
    FOREIGN KEY (ObservationId) REFERENCES Observation(Id)
);
