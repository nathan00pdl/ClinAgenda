USE clinagenda_database;

ALTER TABLE doctor 
ADD CONSTRAINT fk_doctor_status 
FOREIGN KEY (statusId) REFERENCES status(id);

ALTER TABLE patient 
ADD CONSTRAINT fk_patient_status 
FOREIGN KEY (statusId) REFERENCES status(id);

ALTER TABLE appointment 
ADD CONSTRAINT fk_patient FOREIGN KEY (patientId) REFERENCES patient(id),
ADD CONSTRAINT fk_doctor FOREIGN KEY (doctorId) REFERENCES doctor(id),
ADD CONSTRAINT fk_specialty FOREIGN KEY (specialtyId) REFERENCES specialty(id);

ALTER TABLE doctor_specialty 
ADD CONSTRAINT fk_doctorspecialty_doctor 
FOREIGN KEY (doctorId) REFERENCES doctor(id) 
ON DELETE CASCADE ON UPDATE CASCADE,
ADD CONSTRAINT fk_doctorspecialty_specialty 
FOREIGN KEY (specialtyId) REFERENCES specialty(id) 
ON DELETE RESTRICT ON UPDATE CASCADE;
