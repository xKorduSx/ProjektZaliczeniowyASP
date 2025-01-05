Temat projektu: System Zarządzania Wypożyczalnią Sprzętu Sportowego

Krótki opis działania projektu:
 Projekt ma na celu stworzenie systemu zarządzania wypożyczalnią sprzętu sportowego. Aplikacja umożliwia zarządzanie dostępnością sprzętu, rezerwacjami oraz historią wypożyczeń. Użytkownicy systemu (klienci i administratorzy) mogą wykonywać różne działania, takie jak przeglądanie dostępnego sprzętu, rezerwowanie go, zwracanie sprzętu, a także zarządzanie listą sprzętu (dla administratorów).
Autorzy projektu:
•	Jakub Kordus
•	Sandra Gołembska
Informacje użytkownika:
•	Zalogowani użytkownicy mają dostęp do swoich rezerwacji.
•	Goście mogą jedynie przeglądać listę sprzętu.
 System użytkowników:
•	Admin: Dodaje, edytuje, usuwa sprzęt; zarządza rezerwacjami.
•	Użytkownik: Może tworzyć i anulować rezerwacje.
 Najciekawsze funkcjonalności:
•	Wyszukiwarka sprzętu z filtrami (np. kategoria, dostępność).
•	Automatyczne kalkulacje opłat za wypożyczenie w zależności od liczby dni.
Technologie używane w projekcie:
•	Platforma: ASP.NET Core
•	Język programowania: C#
•	Baza danych: SQL Server (Entity Framework Core)
•	Zarządzanie użytkownikami: ASP.NET Core Identity
•	Localization i Globalizacja: Obsługa języków (domyślnie: polski, pl-PL)
•	Środowisko deweloperskie: Visual Studio 2022 lub inne kompatybilne środowisko wspierające .NET 6.0.
Główne funkcjonalności:
1.	Rejestracja i logowanie użytkowników:
•	Obsługa rejestracji konta.
•	Wymagane potwierdzenie konta e-mailem przed pierwszym logowaniem.
2.	Role i uprawnienia:
•	Role dla użytkowników, np. „User” oraz „Admin”
•	User – możliwość wypożyczenia sprzętu oraz przeglądanie rezerwacji
•	Admin – Administrator zarządza całym systemem
•	Gość(Niezalogowany) – Ma tylko możliwość zobaczenia oferty
3.	Wypożyczanie sprzętu:
•	Widok dostępnych przedmiotów do wypożyczenia.
•	Możliwość dodawania przedmiotów przez administratorów.
4.	Globalizacja i lokalizacja:
•	Domyślna lokalizacja ustawiona na język polski („pl-PL”).
•	Obsługa interfejsu i dat w różnych językach/kulturach.
5.	Zarządzanie aplikacją przez administratora:
•	Dodawanie i usuwanie sprzętu.
•	Zarządzanie użytkownikami i ich rolami.
Instrukcje pierwszego uruchomienia projektu:
1.	Przygotowanie środowiska:
•	Zainstaluj Visual Studio 2022 (z obsługą .NET 6.0 i ASP.NET Core).
2.	Zaktualizuj bazę danych:
•	W oknie Menedżera pakietów NuGet uruchom komendę: Update-Database
•	To zaaplikuje migracje i wypełni bazę danych danymi początkowymi.
3.	Uruchom aplikację:
•	W Visual Studio wybierz przycisk "Run" (lub naciśnij F5).

Lista kontrolerów i metod:
1.	HomeController
Metoda Index:
•	HTTP: GET
•	Parametry: brak
•	Działanie: Wyświetla stronę główną.
Metoda Error:
•	HTTP: GET
•	Parametry: brak
•	Działanie: Wyświetla stronę błędu.
2.	EquipmentController
Metoda Index:
•	HTTP: GET
•	Parametry: brak
•	Działanie: Wyświetla listę dostępnego sprzętu.
Metoda Create:
•	HTTP: POST
•	Parametry: Obiekt Equipment
•	Działanie: Dodaje nowy sprzęt do bazy danych.
Metoda Delete:
•	HTTP: POST
•	Parametry: ID sprzętu
•	Działanie: Usuwa wybrany sprzęt.
3.	AccountController
Metoda Login:
•	HTTP: GET/POST
•	Parametry: dane użytkownika
•	Działanie: Zarządza procesem logowania.

Wymagania systemowe:
•	Minimalne: System z obsługą platformy .NET 6.0+, SQL Server.
•	Zalecane: Hosting z obsługą ASP.NET Core i baz danych SQL Server.

