# Who am I?

Ein narratives 2D-Spiel im Pixel-Art-Stil, inspiriert von *OMORI*, mit einem verzweigten Dialogsystem (Ink), interaktiven Szenen und eigenem Storytelling-Framework. Entwickelt mit Unity und einem Fokus auf Modularität und Erweiterbarkeit.

## 🎮 Gameplay und Idee

Der Spieler erkundet handgebaute Szenen, spricht mit Objekten und Charakteren und beeinflusst den Spielverlauf durch Dialogentscheidungen. Atmosphäre, Dialogtiefe und Interaktion stehen im Fokus.

## ⚙️ Technische Umsetzung

### Komponentenbasiertes System

- Alle Spielfunktionen folgen dem komponentenorientierten Unity-Paradigma (Trennung von Objekten und Funktionalität)
- Saubere Trennung von Spiellogik, Darstellung und Interaktion

### Tileset & Leveldesign

- Verwendung von Unity Tilemaps für die Szenengestaltung
- Zwei Layer: begehbare Fläche (z. B. Boden) und Kollisionen (z. B. Wände)
- Erstellung eigener Tileset-Paletten zur Raumgestaltung

### Player & Animationen

- 4-Wege-Bewegung mit Unity Animator
- Idle-Frames speichern Bewegungsrichtung für natürliche Standanimation
- Übergänge zwischen Animationen mit State-Machine

### Interaktive Objekte

- Objekte mit Tags & Eigenschaften (beweglich, interaktiv, starr etc.)
- Umsetzung als Prefabs für einfache Wiederverwendung

### Shader: Outline-System

- Sichtbare Hervorhebung interaktiver Objekte via Shadergraph
- Effekt durch mehrfach versetzte Sprite-Kopien mit weißer Farbe

### Dialogsystem mit Ink

- Integration des Ink-Frameworks für verzweigte Dialoge
- Dialogdaten in `.ink`-Dateien, bearbeitet mit Inky Editor
- Eigene Tags für:
  - Darstellung von Charakterporträts
  - Fade-Effekte
  - Trigger für Szenen-Events
- Dialoge beeinflussen Spielzustände und freischaltbare Wege

### Interaktion & Eventsystem

- Spieler besitzt Collider zur Objekterkennung
- Interaktion startet Dialoge oder Events (z. B. Öffnen von Wegen)
- Coroutine-gesteuertes „Director“-Skript zur Sequenzsteuerung von Szenen und Cutscenes

### Audio

- Dynamisches „Dialog-Gebrabbel“ mit 3 Basis-Sounds
- Pitch wird per Buchstaben-Wert variiert, um den Eindruck von stimmhaftem Sprechen zu erzeugen

### UI & Menüs

- Pause-Menü mit Lautstärkeregler und Navigation
- Einfache Umsetzung mit Unity UI-System
- Fade-In/Out beim Szenenwechsel zur Unterstützung der Atmosphäre

## 🛠 Technologien

- Unity (C#)
- Ink + Inky (Narrative Dialoge)
- Unity Tilemap
- Shadergraph
- Unity Animator
- TextMeshPro
- AudioMixer für dynamische Lautstärke

## 🔧 Projektstatus

🧪 In Entwicklung  
Derzeitige Schwerpunkte:
- Story-Ausbau
- Verbesserung des Directors und Level-Transitions
- Erweiterung der Dialogstruktur

## 📚 Lernziele

- Komplexe Spielstruktur mit Unity umsetzen
- Zusammenspiel von Shader, Animation und UI
- Integration externer Narrative-Tools (Ink)
- Testgetriebene Entwicklung und modulare Projektstruktur

## 📸 Screenshots
