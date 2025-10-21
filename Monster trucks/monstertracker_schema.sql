-- ============================================
-- monstertracker_schema.sql
-- Creates all tables for MonsterTracker
-- ============================================

-- Monster table
CREATE TABLE IF NOT EXISTS Monsters (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Species TEXT NOT NULL,
    DangerRating INTEGER NOT NULL CHECK (DangerRating BETWEEN 1 AND 5)
);

-- Location table
CREATE TABLE IF NOT EXISTS Locations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    Description TEXT NOT NULL,
    DangerLevel INTEGER NOT NULL CHECK (DangerLevel BETWEEN 1 AND 5)
);

-- Hunter table
CREATE TABLE IF NOT EXISTS Hunters (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL UNIQUE,
    ExperienceLevel INTEGER NOT NULL CHECK (ExperienceLevel BETWEEN 1 AND 10),
    ContactInfo TEXT
);

-- Observation table
CREATE TABLE IF NOT EXISTS Observations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MonsterId INTEGER NOT NULL,
    LocationId INTEGER NOT NULL,
    HunterId INTEGER NOT NULL,
    ObservedAt TEXT NOT NULL, -- Stored as TEXT in SQLite
    Notes TEXT,
    FOREIGN KEY (MonsterId) REFERENCES Monsters(Id),
    FOREIGN KEY (LocationId) REFERENCES Locations(Id),
    FOREIGN KEY (HunterId) REFERENCES Hunters(Id)
);

-- ObservationAudit table (Optional feature)
CREATE TABLE IF NOT EXISTS ObservationAudit (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ObservationId INTEGER NOT NULL,
    OldLocationId INTEGER NOT NULL,
    NewLocationId INTEGER NOT NULL,
    ChangedAt TEXT NOT NULL,
    FOREIGN KEY (ObservationId) REFERENCES Observations(Id)
);
