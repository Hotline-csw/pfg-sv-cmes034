-- intelliStack table for stack structure
CREATE TABLE [cust].[StackStructures]
(
    Sequence                        bigint          not null identity(1,1),
    StackStructureCode              int             not null,                   -- Stapelstruktur
    Description                     nvarchar(256)   null,                       -- Beschreibung
    IntelliStackAlignment           int             not null,                   -- Stapeltyp für IntelliStack
                                                                                --   0 = Default ( chaotischer Stapel )
                                                                                --   1 = Cornered (wird als CorneredSouthWest bewertet)
                                                                                --   2 = CorneredSouthWest
                                                                                --   3 = CorneredSouthEast
                                                                                --   4 = CorneredNorthEast
                                                                                --   5 = CorneredNorthWest
                                                                                --   6 = Centric
    PartsInXDirection               int             not null,                   -- Anzahl der Bauteile in X-Richtung
                                                                                --   0, null = Beliebige Anzahl
                                                                                --             In der Maske wird 0 eingegeben,
                                                                                --             an IntelliStack wird "null" übergeben
                                                                                --   >= 1 als explizite Anzahl
    PartsInYDirection               int             not null,                   -- Anzahl der Bauteile in Y-Richtung
                                                                                --   0, null = Beliebige Anzahl
                                                                                --             In der Maske wird 0 eingegeben,
                                                                                --             an IntelliStack wird "null" übergeben
                                                                                --   >= 1 als explizite Anzahl
    MaxStackHeight                  numeric(18,2)   not null,                   -- Maximal zulässige Stapelhöhe
    OrderingRelevance               int             not null default 0,         -- Sortierung der Bauteile im Stapel
                                                                                --   0 = Reihenfolge muss beachtet werden
                                                                                --   1 = Reihenfolge durch Stapelmodul bestimmbar
                                                                                -- Übergabe an Stapelmodul in "AccessGroupFeature"
    StackReversable                 int             not null,                   -- Umkehrbarkeit eines Stapels
                                                                                --   0 = Umgekehrter Stapel ist nicht mehr stabil
                                                                                --   1 = Umgekehrter Stapel ist stabil
    BaseBoardList                   nvarchar(256)   not null,                   -- Liste von ein oder mehreren Schonerplatten,
                                                                                -- die für die Stapelbildung verwendet werden können
                                                                                -- Trennzeichen der Liste ist Pipe |
                                                                                -- z.B. SPL2 oder SPL1 | SPL2
    BaseBoardRule                   int             not null default 0,         -- Regel für die Beurteilung von BaseBoardList
                                                                                --   0 = nicht relevant
                                                                                --   1 = IF Bauteillänge > 1350 + Overlap(70+70)
                                                                                --          THEN 2300 (SPL2)
                                                                                --          ELSE 1350 (SPL1)
                                                                                --   2 = IF Bauteillänge > 1350 + Overlap(70+70)
                                                                                --          THEN 2300
                                                                                --          ELSE 1350
                                                                                --   3 = IF BauteilAnzahl > 8
                                                                                --          THEN 2350
                                                                                --       ELSE
                                                                                --          IF Bauteillänge > 1350 + Overlap(70+70)
                                                                                --              THEN 2300
                                                                                --          ELSE 1350
                                                                                --   4 = IF BauteilAnzahl > 8
                                                                                --          THEN 2300
                                                                                --       ELSE
                                                                                --          IF Bauteillänge > 1350 + Overlap(0+70)
                                                                                --              THEN 2300
                                                                                --          ELSE 1350
    BaseBoardXOffset2               numeric(18,2)   null,                       -- Korrekturwert Länge in X für große Schonerplatte
                                                                                -- normalerweise ein negativer Wert, z.B. -300,
                                                                                -- damit ist die Übergabe für IntelliStack = 2000 - 300
    OverlapXPlusOffset2             numeric(18,2)   null,                       -- Korrekturwert OverlapPlus in X für große Schonerplatte
                                                                                -- normalerweise ein positiver Wert, z.B. +300,
                                                                                -- damit ist die Übergabe für IntelliStack = 70 + 300
    OverlapMaxXPlusOffset2          numeric(18,2)   null,                       -- Korrekturwert OverlapPlusMaxOffset in X für große Schonerplatte
                                                                                -- normalerweise ein positiver Wert, z.B. +300,
                                                                                -- damit ist die Übergabe für IntelliStack = 500 + 300
    Orientation                     int             not null default 0,         -- Soll-Orientierung der Bauteile auf Schonerplatte
                                                                                --   0 = IntelliStack soll die Orientierung bestimmen
                                                                                --   1 = Längs
                                                                                --   2 = Quer
    OverlapXMinus                   numeric(18,2)   not null,                   -- Kleine Schonerplatte:
                                                                                --   Zulässiger Überstand in Negativ-X
    OverlapXPlus                    numeric(18,2)   not null,                   -- Kleine Schonerplatte:
                                                                                --   Zulässiger Überstand in Positiv-X
    OverlapXMinus2                  numeric(18,2)   not null,                   -- Große Schonerplatte:
                                                                                --   Zulässiger Überstand in Negativ-X
    OverlapXPlus2                   numeric(18,2)   not null,                   -- Große Schonerplatte:
                                                                                --   Zulässiger Überstand in Positiv-X
    OverlapYMinus                   numeric(18,2)   not null,                   -- Zulässiger Überstand in Negativ-Y
    OverlapYPlus                    numeric(18,2)   not null,                   -- Zulässiger Überstand in Positiv-Y
    OverlapMaxXMinus                numeric(18,2)   not null,                   -- Kleine Schonerplatte:
                                                                                --   Maximal zulässiger Überstand in Negativ-X
    OverlapMaxXPlus                 numeric(18,2)   not null,                   -- Kleine Schonerplatte:
                                                                                --   Maximal zulässiger Überstand in Positiv-X
    OverlapMaxXMinus2               numeric(18,2)   not null,                   -- Große Schonerplatte:
                                                                                --   Maximal zulässiger Überstand in Negativ-X
    OverlapMaxXPlus2                numeric(18,2)   not null,                   -- Große Schonerplatte:
                                                                                --   Maximal zulässiger Überstand in Positiv-X
    OverlapMaxYMinus                numeric(18,2)   not null,                   -- Maximal zulässiger Überstand in Negativ-Y
    OverlapMaxYPlus                 numeric(18,2)   not null,                   -- Maximal zulässiger Überstand in Positiv-Y
    DistanceBetweenParts            numeric(18,2)   not null,                   -- Bauteilabstand im Stapel
    UseAlternatingOffset            int             not null default 0,         -- Lagenversatz, "Verschränkung" der Bauteile in
                                                                                -- den Spuren und zwischen den Spuren
                                                                                --   0 = kein Lagenversatz
                                                                                --   1 = Lagenversatz
    AlternatingOffsetX              numeric(18,2)   null,                       -- Verschränkung in X-Richtung (Default 40)
    AlternatingOffsetY              numeric(18,2)   null,                       -- Verschränkung in Y-Richtung (Default 40)
    UseLaneLimits                   int             not null default 0,         -- Verwenden der Spurbreiteneinstellungen
                                                                                --   0 = nicht verwenden
                                                                                --   1 = verwenden
    LaneHeightMin                   numeric(18,2)   null,                       -- Minimale Spurbreite (Default 120)
    LaneHeightMax                   numeric(18,2)   null,                       -- Maximale Spurbreite (Default 800)
    LaneHeightTolerance             numeric(18,2)   null,                       -- Zulässige Spurbreitenabweichung (Default 150)
    CreationDate                    datetime2       not null default getdate(),
    CreationSource                  nvarchar(32)    null,
    ModificationDate                datetime2       not null default getdate(),
    ModificationSource              nvarchar(32)    null,
    Locked                          bit             not null default 0,
    LockSource                      nvarchar(32)    null,

    CONSTRAINT [PK_StackStructures] PRIMARY KEY ( StackStructureCode )
)

