# Kanban Board (WPF Desktop App)

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)
![License](https://img.shields.io/badge/license-MIT-green)
![SQLite](https://img.shields.io/badge/Database-Sqlite-39477F?logo=sqlite&logoColor=white)

Eine Desktop-Kanban-Anwendung, entwickelt mit C#, .NET 8 und WPF.

Das Projekt dient dazu, zentrale Konzepte moderner Desktop-Anwendungsentwicklung praktisch umzusetzen – darunter MVVM, Datenpersistenz mit SQLite, Repository Pattern, Zustandsverwaltung, Drag & Drop sowie eine anpassbare Benutzeroberfläche mit Light- und Dark-Theme.

![KanbanBoard Screenshot](docs/screenshots/main-window_5.png)

## Funktionen

- Karten erstellen und löschen
- Details einer Karte ändern (Titel, Status, Beschreibung, Fälligkeitsdatum)
- MVVM Architektur
- Repository Pattern für Datenpersistenz
- Beschreibung für Karten erstellbar
- Tags können Karten hinzugefügt werden
- Lokale Datenspeicherung (in JSON)
- Kanban Layout (ToDo / Doing / Done)
- Drag & Drop Bedienung der Karten
- Menübar für aktuelle und neue Buttons
- seperates Fenster zur Bearbeitung von Karten
- Sortierung der Karten in den Spalten nach CreatedAt / UpdatedAt / DueDate

![DragDrop Screenshot](docs/screenshots/DragDrop_cards.png)

## Projektstruktur

Das Projekt ist in mehrere Schichten unterteilt:

- **App** → WPF UI, ViewModels and Commands
- **Core**  → Domain models and repository interfaces
- **Infrastructure** → Data persistence implementations
- **Tests** → Unit tests für zentrale Funktionen
  
## Technologien

- C#
- .NET 8
- WPF

## Geplante Erweiterungen / Updates
   
- UI
  - Darkmode
  - Hover-Effekte für Karten + Cursor Änderung
  - Spalten bei DragOver hervorheben

<br>
<br>

#### Hinweis
Icons by Bootstrap Icons
<br>
https://icons.getbootstrap.com/
