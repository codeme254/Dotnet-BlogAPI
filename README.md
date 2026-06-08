# Dotnet Blog API

Clone this repository:

```bash
git clone https://github.com/codeme254/Dotnet-BlogAPI.git
```

## Set User Secrets
Run:
```bash
dotnet user-secrets init
```

Postgresql DB Connection String:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=BlogDB;Username=USERNAME;Password=PASSWORD;"
```