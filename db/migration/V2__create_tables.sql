USE clinagenda_database;

CREATE TABLE IF NOT EXISTS status (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS specialty (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    scheduleDuration INT NOT NULL
);

CREATE TABLE IF NOT EXISTS doctor (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    statusId INT NOT NULL
);

CREATE TABLE IF NOT EXISTS patient (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    phoneNumber VARCHAR(20) NOT NULL,
    documentNumber VARCHAR(50) NOT NULL,
    statusId INT NOT NULL,
    birthDate DATE DEFAULT NULL
);

CREATE TABLE IF NOT EXISTS appointment (
    id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    patientId INT NOT NULL,
    doctorId INT NOT NULL,
    specialtyId INT NOT NULL,
    appointmentDate DATETIME NOT NULL,
    observation TEXT
);

CREATE TABLE IF NOT EXISTS doctor_specialty (
    doctorId INT NOT NULL,
    specialtyId INT NOT NULL,
    PRIMARY KEY (doctorId, specialtyId)
);
