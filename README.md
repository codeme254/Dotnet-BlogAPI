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
Replace USERNAME with your PostgreSQL username and PASSWORD with your PostgreSQL password.

## Email Credentials
In this project, I used Gmail's SMTP server to send emails.

Any SMTP server (SendGrid, Brevo e.t.c.,) will work just fine provided you have valid credentials.

Before setting up the Gmail credentials, you need to generate a **Gmail App Password** &mdash; Gmail does not allow you to use your regular account password for SMTP.

To generate a Gmail App Password:
1. Go to your [Google Account](https://myaccount.google.com)
1. Navigate to **Security** and make sure **2-Step Verification** is enabled (App Passwords require this)
1. Search for **"App Passwords"** in the search bar at the top
1. Give it a name (e.g. `BlogAPI`) and click **Create**
1. Copy the generated 16-character password &mdash; you will only see it only once

### Setting up Credentials
Now set each value using the .NET Secret Manager:

**SMTP Server:**
```bash
dotnet user-secrets set "EmailSettings:SmtpServer" "smtp.gmail.com"
```

**Port:**
```bash
dotnet user-secrets set "EmailSettings:Port" "587"
```

**Sender email** *(your Gmail address)*:
```bash
dotnet user-secrets set "EmailSettings:SenderEmail" "you@gmail.com"
```

**Sender username** *(the display name recipients will see)*:
```bash
dotnet user-secrets set "EmailSettings:Username" "Blog Auth Team"
```

**Sender password** *(the 16-character App Password you generated above)*:
```bash
dotnet user-secrets set "EmailSettings:Password" "abcd efgh ijkl mnop"
```

### Verify
To confirm all secrets are set correctly, run:

```bash
dotnet user-secrets list
```

> **Note:** App Passwords are tied to your Google account. If you ever revoke the App Password or disable 2-Step Verification, you will need to generate a new one and update the secret.

## JWT
In this app, I am not validating the issuer and audience, so the only configurations needed are Key, and expiry in minutes:

```bash
dotnet user-secrets set "Jwt:Key" "YOUR-SUPER-SECRET-KEY-AT-LEAST-32-CHARACTERS"
```

Use [this website](https://jwtsecrets.com/#generator) to generate a secret key.

```bash
dotnet user-secrets set "Jwt:ExpiryMinutes" 15
```