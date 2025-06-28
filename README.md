# 🚀 Cosmos Odyssey

🔹 [About](#-description) 🔹 [Features](#-features) 🔹 [Installation](#-how-to-run-the-application)

## 📌 Description

Cosmos Odyssey is an application that displays travel deals across the solar system.
Note that for trips with layovers, only those connections are shown where the layover duration is between 30 minutes and 48 hours (inclusive).

---

## 👨‍💻 Authors

Developed by Gisela Nüüd.

---

## 🛠 Features

✅ **Select trip** – Select your trip by selecting wanted origin and destination  
✅ **Filter/sort deals** – Filter the shown selection based on the travel company name and/or sort the result based on the price, distance or travel time based on preference  
✅ **Make a reservation** – Make reservations using your first and last name.

---

## 🧰 Tech Stack

List of the technologies and languages used in this project:

- **Frontend**: CSS, Nuxt, Shadcn, Node.js
- **Backend**: .NET, C#, Swagger
- **Database**: PostgreSQL, Docker

## 🚀 How to Run the Application

Follow these steps to set up and run the application:

### 1️⃣ Install Required Dependencies

- Install **Node.js (LTS version)**: [Download Here](https://nodejs.org/en/download/)
- Install **Docker**: [Download Here](https://www.docker.com/)

### 2️⃣ Clone the Repository

```sh
 git clone https://github.com/ginuud/CosmosOdyssey.git
```

After cloning navigate to the repository:

```sh
 cd .\CosmosOdyssey\
```

### 3️⃣ Set Up Docker

- Open **Docker Desktop**
- In the terminal, run:

```sh
docker-compose up --build
```

### 4️⃣ Start the Frontend

- Navigate to the frontend directory:

```sh
cd ./FrontEnd/cosmosodyssey/
```

- Install dependencies:

```sh
npm install
```

- Run the application:

```sh
npm run dev
```

### 5️⃣ Access the Application

- Open your browser and go to: **[localhost:3001](http://localhost:3001)**

## 📝 Route finding explained

**Routes** are generated using a **Depth First Search (DFS)** algorithm in **RouteFinderService.cs**

The method ensures that when generating a route, it won't visit the same planet more than once within a single route. That ensures avoiding cycles.

**The DFS explores all possible paths** from the current planet to the destination, while keeping track of the current path and already visited planets. The search is limited by the maximum number of legs (transfers) in any route. The maximum value 4 was set by trial and error, ensuring it supports the longest viable layover.

- The algorithm explores one path completely before trying alternatives
- When it hits a dead end or completes a path, it backtracks
- This allows exploring different branches from the same starting point

After finding all possible paths the function iterates through each leg in the path, trying **every possible provider** for that leg in chronological order. For the first leg, any provider is allowed. For subsequent legs, the function checks that the next provider's FlightStart is after the previous provider's FlightEnd, and that the layover time is between 0.5 and 48 hours.

When the end of the path is reached, the provider sequence is added to the results. The process continues by removing the last provider from the current sequence and tries the next option.

Finally a list of routes (route DTO objects) is returned and displayed to the user.

## Backend additional info

- **Swagger** can be used as an information source and for backend testing. To use Swagger, open your browser and go to: **[localhost:3000/swagger](http://localhost:3000/swagger)**
- **PgAdmin** can be used for seeing PostgreSQL database values
