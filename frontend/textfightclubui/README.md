```markdown
# TextFightClub App

This project is a hackathon app featuring a **.NET backend** and a **React frontend**. It simulates a roast battle between bots using AI-generated responses.

## Folder Structure

```

TextFightClubApp/
├── backend/           # .NET Web API backend
└── frontend/          # React frontend
└── textfightclubui

````

---

## Prerequisites

Make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v16+ recommended)
- npm (comes with Node.js)

---

## Backend Setup (.NET)

1. Navigate to the backend folder:

```bash
cd backend/TextFightClub
````

2. Restore dependencies:

```bash
dotnet restore
```

3. Run the backend API:

```bash
dotnet run
```

By default, the API will run on `http://localhost:5175`.

---

## Frontend Setup (React)

1. Navigate to the frontend folder:

```bash
cd frontend/textfightclubui
```

2. Install dependencies:

```bash
npm install
```

3. Start the development server:

```bash
npm start
```

The React app will run on `http://localhost:3000` and should connect to the backend API.

---

## Build for Production

**Frontend:**

```bash
npm run build
```

This will create an optimized production build in the `build/` folder.

**Backend:**

```bash
dotnet publish -c Release
```

This will publish the backend API in `bin/Release/net8.0/publish`.

---

## Notes

* Ensure the backend is running before starting the frontend.
* API key for Gemini AI should be set in `appsettings.json` or environment variables for the backend.

---

## Learn More

* [React Documentation](https://reactjs.org/docs/getting-started.html)
* [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)

```

You can just **replace your current README.md with this**.  

If you want, I can also **add instructions for setting the backend API URL in the React app** so it connects automatically without editing files. Do you want me to do that too?
```
