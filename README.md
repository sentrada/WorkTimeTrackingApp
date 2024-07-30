# WorkTimeTrackingApp
Work Time Tracking Application  This application is designed using microservices architecture and is suitable for tracking work hours. The backend is built using .NET 8, and the frontend will use Blazor. The first iteration focuses on core functionalities without user roles.

# Munkaidő Nyilvántartó Alkalmazás

Ez a rendszerterv egy munkaidő nyilvántartó alkalmazás fejlesztéséhez készült, amely mikroszolgáltatás-alapú architektúrát használ. Az alkalmazás célja a munkaidő nyilvántartása és kezelése, projekt- és feladatkezelés, valamint felhasználói hitelesítés és profilkezelés.

## Funkcionális Követelmények

1. **Felhasználói hitelesítés**
   - Regisztráció
   - Bejelentkezés
   - Jelszó visszaállítás

2. **Felhasználói profilkezelés**
   - Profil megtekintése
   - Profil szerkesztése

3. **Projektkezelés**
   - Projekt létrehozása
   - Projekt megtekintése
   - Projekt szerkesztése
   - Projekt törlése

4. **Feladatkezelés**
   - Feladat létrehozása
   - Feladat megtekintése
   - Feladat szerkesztése
   - Feladat törlése

5. **Munkaidő nyilvántartás**
   - Munkaidő rögzítése
   - Munkaidő megtekintése
   - Munkaidő szerkesztése
   - Munkaidő törlése

## Nem-funkcionális Követelmények

1. **Skálázhatóság**
   - A rendszer képes legyen növekedni a felhasználók és projektek számának növekedésével.

2. **Biztonság**
   - Biztonságos hitelesítés és adatkezelés (pl. jelszavak hash-elése, SSL használata).

3. **Teljesítmény**
   - Gyors válaszidők és hatékony adatkezelés.

4. **Karbantarthatóság**
   - Kód könnyű karbantarthatósága és bővíthetősége.

## Architektúra

Az alkalmazás mikroszolgáltatás-alapú architektúrára épül, és a következő komponensekből áll:

1. **Felhasználói Szolgáltatás**
   - Regisztráció, bejelentkezés, profilkezelés
   - Technológia: .NET 8, Entity Framework Core, SQL Server (vagy PostgreSQL)

2. **Projekt Szolgáltatás**
   - Projektkezelés (létrehozás, megtekintés, szerkesztés, törlés)
   - Technológia: .NET 8, Entity Framework Core, SQL Server (vagy PostgreSQL)

3. **Feladat Szolgáltatás**
   - Feladatkezelés (létrehozás, megtekintés, szerkesztés, törlés)
   - Technológia: .NET 8, Entity Framework Core, SQL Server (vagy PostgreSQL)

4. **Munkaidő Szolgáltatás**
   - Munkaidő nyilvántartás (rögzítés, megtekintés, szerkesztés, törlés)
   - Technológia: .NET 8, Entity Framework Core, SQL Server (vagy PostgreSQL)

5. **Auth Szolgáltatás**
   - JWT tokenek kezelése, hitelesítés
   - Technológia: .NET 8, IdentityServer4 (vagy hasonló .NET alapú megoldás)

## Adatbázis Tervezés

### Felhasználók Tábla
| Oszlop Név       | Típus        | Leírás                          |
|------------------|--------------|---------------------------------|
| ID               | INT          | Primary Key                     |
| Név              | NVARCHAR(100)| Felhasználó neve                |
| E-mail           | NVARCHAR(100)| Felhasználó e-mail címe         |
| Jelszó           | NVARCHAR(255)| Hash-elt jelszó                 |
| Telefonszám      | NVARCHAR(20) | (opcionális) Telefonszám        |
| Cím              | NVARCHAR(255)| (opcionális) Cím                |
| LétrehozásDátuma | DATETIME     | Regisztráció dátuma             |

### Projektek Tábla
| Oszlop Név       | Típus        | Leírás                          |
|------------------|--------------|---------------------------------|
| ID               | INT          | Primary Key                     |
| Név              | NVARCHAR(100)| Projekt neve                    |
| Leírás           | NVARCHAR(255)| Projekt leírása                 |
| LétrehozásDátuma | DATETIME     | Projekt létrehozásának dátuma   |
| TulajdonosID     | INT          | Felhasználó ID (Foreign Key)    |

### Feladatok Tábla
| Oszlop Név       | Típus        | Leírás                          |
|------------------|--------------|---------------------------------|
| ID               | INT          | Primary Key                     |
| ProjektID        | INT          | Projekt ID (Foreign Key)        |
| Név              | NVARCHAR(100)| Feladat neve                    |
| Leírás           | NVARCHAR(255)| Feladat leírása                 |
| Határidő         | DATETIME     | Feladat határideje              |
| Állapot          | NVARCHAR(20) | Feladat állapota (pl. "Folyamatban", "Befejezett") |

### Munkaidő Tábla
| Oszlop Név       | Típus        | Leírás                          |
|------------------|--------------|---------------------------------|
| ID               | INT          | Primary Key                     |
| FeladatID        | INT          | Feladat ID (Foreign Key)        |
| FelhasználóID    | INT          | Felhasználó ID (Foreign Key)    |
| Munkaóra         | DECIMAL(5,2) | Munkaórák száma                 |
| Dátum            | DATETIME     | Munkaidő dátuma                 |

## Mikroszolgáltatások Részletezése

### 1. Felhasználói Szolgáltatás
- **Funkciók:**
  - Felhasználó regisztráció
  - Felhasználó bejelentkezés
  - Profil megtekintése és szerkesztése

- **API Végpontok:**
  - POST `/api/users/register`: Új felhasználó regisztrálása
  - POST `/api/users/login`: Felhasználó bejelentkezése
  - GET `/api/users/profile`: Felhasználói profil megtekintése
  - PUT `/api/users/profile`: Felhasználói profil szerkesztése

- **Tesztelés:**
  - Unit és integrációs tesztek a regisztráció, bejelentkezés és profilkezelés funkciókra.

### 2. Projekt Szolgáltatás
- **Funkciók:**
  - Projekt létrehozása
  - Projektek megtekintése
  - Projektek szerkesztése
  - Projektek törlése

- **API Végpontok:**
  - POST `/api/projects`: Új projekt létrehozása
  - GET `/api/projects`: Projektek listázása
  - GET `/api/projects/{id}`: Projekt megtekintése
  - PUT `/api/projects/{id}`: Projekt szerkesztése
  - DELETE `/api/projects/{id}`: Projekt törlése

- **Tesztelés:**
  - Unit és integrációs tesztek a projekt létrehozására, megtekintésére, szerkesztésére és törlésére.

### 3. Feladat Szolgáltatás
- **Funkciók:**
  - Feladat létrehozása
  - Feladatok megtekintése
  - Feladatok szerkesztése
  - Feladatok törlése

- **API Végpontok:**
  - POST `/api/tasks`: Új feladat létrehozása
  - GET `/api/tasks`: Feladatok listázása
  - GET `/api/tasks/{id}`: Feladat megtekintése
  - PUT `/api/tasks/{id}`: Feladat szerkesztése
  - DELETE `/api/tasks/{id}`: Feladat törlése

- **Tesztelés:**
  - Unit és integrációs tesztek a feladat létrehozására, megtekintésére, szerkesztésére és törlésére.

### 4. Munkaidő Szolgáltatás
- **Funkciók:**
  - Munkaidő rögzítése
  - Munkaidő megtekintése
  - Munkaidő szerkesztése
  - Munkaidő törlése

- **API Végpontok:**
  - POST `/api/worktimes`: Új munkaidő rögzítése
  - GET `/api/worktimes`: Munkaidők listázása
  - GET `/api/worktimes/{id}`: Munkaidő megtekintése
  - PUT `/api/worktimes/{id}`: Munkaidő szerkesztése
  - DELETE `/api/worktimes/{id}`: Munkaidő törlése

- **Tesztelés:**
  - Unit és integrációs tesztek a munkaidő rögzítésére, megtekintésére, szerkesztésére és törlésére.

### 5. Auth Szolgáltatás
- **Funkciók:**
  - JWT tokenek kezelése
  - Hitelesítés

- **API Végpontok:**
  - POST `/api/auth/token`: JWT token generálása bejelentkezéskor
  - GET `/api/auth/validate`: Token érvényességének ellenőrzése

- **Tesztelés:**
  - Unit és integrációs tesztek a token generálásra és érvényesség ellenőrzésére.

## Tesztelési Stratégia

### Unit Tesztek
- Minden mikroszolgáltatás tartalmazza az egyes funkciókhoz tartozó unit teszteket.
- Minden végpont működésének ellenőrzése különböző bemeneti adatokkal és várt kimenetekkel.

### Integrációs Tesztek
- Minden mikroszolgáltatás esetén a különböző komponensek közötti együttműködés tesztelése.
- A mikroszolgáltatások közötti kommunikáció és adatátvitel tesztelése.

### End-to-End (E2E) Tesztek
- Az egész rendszer működésének tesztelése valós felhasználói szcenáriók alapján.
- Felhasználói regisztrációtól kezdve a munkaidő rögzítéséig minden folyamat tesztelése.

## Fejlesztési Eszközök és Technológiák

- **Backend:** .NET 8
- **Adatbázis:** SQL Server vagy PostgreSQL
- **Frontend:** Blazor
- **Auth:** IdentityServer4
- **Tesztelés:** xUnit, NUnit vagy MSTest

## Ütemterv

### Első Iteráció (1-2 hónap)
- Felhasználói hitelesítés
- Felhasználói profilkezelés
- Projektkezelés
- Feladatkezelés
- Munkaidő nyilvántartás
- Alapvető tesztelési keretrendszer kiépítése

### Második Iteráció (2-3 hónap)
- Felhasználói jogosultságok és szerepkörök
- Adminisztrációs felület
- Jelentéskészítés és statisztikák
- Fejlettebb tesztelési eszközök integrálása

### Harmadik Iteráció (1-2 hónap)
- Teljesítmény optimalizálás
- Felhasználói visszajelzések alapján történő módosítások
- Hibajavítások és kisebb fejlesztések
