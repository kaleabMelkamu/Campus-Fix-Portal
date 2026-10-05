# Team Name: The Avengers  

## Team Members & Responsibilities

- Kaleab Melkamu — Team Lead & Pitch
- Rahmet Mekbib — Frontend Developer
- Zerubabel Firew — Backend Developer
- Surafel Mengistu — Database & Full-Stack Developer
- Yosef Worku & Kashay Arefayne — QA & Testing

## Tech Stack Used

- Frontend: Angular 19
- Backend: ASP.NET Core Web API (.NET 10)
- Database: EF Core with PostgreSQL

## How to Run Locally

### Backend Setup

1. cd src/FixMyCampus.API
2. dotnet restore
3. dotnet ef database update --project ../FixMyCampus.Infrastructure/FixMyCampus.Infrastructure.csproj
4. dotnet run

### Frontend Setup

1. cd fix-my-campus
2. npm install
3. ng serve

## Test Accounts & Demo Credentials

- Admin / Staff User: admin@hackathon.local / Admin123!
- Standard User: user@hackathon.local / User123!
- Technician User: tech@hackathon.local / Tech123!

## Working Features

- **Authentication & Roles**
- **Ticket Submission**
- **Campus Feed**
- **Reporter "My Tickets"**
- **Admin Dashboard UI**
- **Clean Architecture & Standards**

## Known Limitations & Bugs

- Real-time updates currently require page refresh (live push via SignalR is planned for post-MVP).
- Photo attachments / image uploads for damaged equipment are not yet implemented.
