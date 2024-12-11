-- Crear la tabla Actividades
CREATE TABLE Actividades (
    ActividadId INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreActividad TEXT NOT NULL,
    Fecha DATE NOT NULL
);

-- Crear índice para búsquedas rápidas de actividades por fecha
CREATE INDEX idx_Actividades_Fecha ON Actividades (Fecha);

-- Crear la tabla Tareas
CREATE TABLE Tareas (
    TareaId INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreTarea TEXT NOT NULL,
    ResponsableId INTEGER,  -- Campo para el responsable
    FOREIGN KEY (ResponsableId) REFERENCES Personas(PersonaId) ON DELETE SET NULL
);

-- Crear índice para búsquedas rápidas de tareas por nombre
CREATE INDEX idx_Tareas_NombreTarea ON Tareas (NombreTarea);

-- Crear la tabla Personas
CREATE TABLE Personas (
    PersonaId INTEGER PRIMARY KEY AUTOINCREMENT,
    NombrePersona TEXT NOT NULL
);

-- Tabla intermedia para relacionar Actividades, Tareas y Personas
CREATE TABLE ActividadTareaPersonas (
    ActividadId INTEGER NOT NULL,
    TareaId INTEGER NOT NULL,
    PersonaId INTEGER,
    PRIMARY KEY (ActividadId, TareaId, PersonaId),
    FOREIGN KEY (ActividadId) REFERENCES Actividades (ActividadId) ON DELETE CASCADE,
    FOREIGN KEY (TareaId) REFERENCES Tareas (TareaId) ON DELETE CASCADE,
    FOREIGN KEY (PersonaId) REFERENCES Personas (PersonaId) ON DELETE SET NULL
);

-- Tabla para tareas que no pueden realizar ciertas personas
CREATE TABLE TareaPersonas (
    TareaId INTEGER NOT NULL,
    PersonaId INTEGER NOT NULL,
    PRIMARY KEY (TareaId, PersonaId),
    FOREIGN KEY (TareaId) REFERENCES Tareas (TareaId) ON DELETE CASCADE,
    FOREIGN KEY (PersonaId) REFERENCES Personas (PersonaId) ON DELETE CASCADE
);

-- Insertar valores predeterminados en Actividades
INSERT INTO Actividades (NombreActividad, Fecha) VALUES
('Cena navideña de trabajo', '2024-12-11'),
('Cena familiar', '2024-12-01'),
('Constancia IA', '2024-11-23');

-- Insertar valores predeterminados en Tareas
INSERT INTO Tareas (NombreTarea) VALUES
('Colocar sillas'),
('Hacer ensalada'),
('Hornear pierna'),
('Lavar platos'),
('Lavar baños');

-- Insertar valores predeterminados en Personas
INSERT INTO Personas (NombrePersona) VALUES
('Mark Z.'),
('Warm B.'),
('Anna T.');

-- Insertar relaciones iniciales en ActividadTareaPersonas
INSERT INTO ActividadTareaPersonas (ActividadId, TareaId, PersonaId) VALUES
(1, 1, 1), -- Mark Z. coloca sillas en la Cena navideña de trabajo
(1, 2, 2), -- Warm B. hace ensalada en la Cena navideña de trabajo
(2, 3, NULL); -- Hornear pierna para la Cena familiar, sin responsable

-- Insertar restricciones en TareaPersonas
INSERT INTO TareaPersonas (TareaId, PersonaId) VALUES
(3, 1), -- Mark Z. no puede hornear pierna
(4, 2); -- Warm B. no puede lavar platos


