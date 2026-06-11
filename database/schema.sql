-- SQL-схема SQLite для проекта Interactive Voting API.
-- Файл нужен для проверки структуры базы данных без запуска приложения.

CREATE TABLE IF NOT EXISTS Voters (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    DateOfBirth TEXT NOT NULL,
    PassportNumber TEXT NOT NULL UNIQUE,
    Email TEXT NOT NULL UNIQUE,
    PhoneNumber TEXT NOT NULL,
    IsVerified INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS Candidates (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    DateOfBirth TEXT NOT NULL,
    Biography TEXT NOT NULL DEFAULT '',
    Program TEXT NOT NULL DEFAULT '',
    Slogan TEXT NOT NULL DEFAULT '',
    IsApproved INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS Elections (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL DEFAULT '',
    StartUtc TEXT NOT NULL,
    EndUtc TEXT NOT NULL,
    IsClosed INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS Votes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ElectionId INTEGER NOT NULL,
    VoterId INTEGER NOT NULL,
    CandidateId INTEGER NOT NULL,
    CreatedUtc TEXT NOT NULL,
    CONSTRAINT FK_Votes_Elections FOREIGN KEY (ElectionId) REFERENCES Elections(Id),
    CONSTRAINT FK_Votes_Voters FOREIGN KEY (VoterId) REFERENCES Voters(Id),
    CONSTRAINT FK_Votes_Candidates FOREIGN KEY (CandidateId) REFERENCES Candidates(Id),
    CONSTRAINT UQ_Votes_Election_Voter UNIQUE (ElectionId, VoterId)
);

CREATE TABLE IF NOT EXISTS CandidateApplications (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CandidateId INTEGER NOT NULL,
    SubmittedUtc TEXT NOT NULL,
    Status INTEGER NOT NULL DEFAULT 0,
    Comment TEXT NOT NULL DEFAULT '',
    ReviewedUtc TEXT NULL,
    CONSTRAINT FK_CandidateApplications_Candidates FOREIGN KEY (CandidateId) REFERENCES Candidates(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS AuditLogs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Action TEXT NOT NULL,
    EntityName TEXT NOT NULL,
    EntityId INTEGER NOT NULL,
    Details TEXT NOT NULL DEFAULT '',
    CreatedUtc TEXT NOT NULL
);
