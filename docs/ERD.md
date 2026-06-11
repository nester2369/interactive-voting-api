# ERD-диаграмма базы данных

Диаграмма показывает основные таблицы SQLite-базы и связи между ними.

```mermaid
erDiagram
    Voters ||--o{ Votes : "casts"
    Elections ||--o{ Votes : "contains"
    Candidates ||--o{ Votes : "receives"
    Candidates ||--o{ CandidateApplications : "has"

    Voters {
        int Id PK
        string FullName
        datetime DateOfBirth
        string PassportNumber UK
        string Email UK
        string PhoneNumber
        bool IsVerified
    }

    Candidates {
        int Id PK
        string FullName
        datetime DateOfBirth
        string Biography
        string Program
        string Slogan
        bool IsApproved
    }

    Elections {
        int Id PK
        string Title
        string Description
        datetime StartUtc
        datetime EndUtc
        bool IsClosed
    }

    Votes {
        int Id PK
        int ElectionId FK
        int VoterId FK
        int CandidateId FK
        datetime CreatedUtc
    }

    CandidateApplications {
        int Id PK
        int CandidateId FK
        datetime SubmittedUtc
        int Status
        string Comment
        datetime ReviewedUtc
    }

    AuditLogs {
        int Id PK
        string Action
        string EntityName
        int EntityId
        string Details
        datetime CreatedUtc
    }
```

## Ограничения данных

- `Voters.PassportNumber` должен быть уникальным.
- `Voters.Email` должен быть уникальным.
- Один избиратель может проголосовать только один раз в рамках одних выборов.
- Голос связан с выборами, избирателем и кандидатом.
- Заявка кандидата связана с кандидатом.
- Журнал аудита хранит действия системы и администратора.
