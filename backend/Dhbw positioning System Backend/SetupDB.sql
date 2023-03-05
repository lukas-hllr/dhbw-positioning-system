/*
This Document is used to setup the SQLite Database used for the DHBWPS project.
It will delete all existing tables and replace them with new and empty tables.
Use this command in Windows Powershell to renew the DB
cat .\SetupDB.sql | sqlite3 .\DhbwPositioningSystemDB.db

Scaffolding in PM like this:
"C:\\Users\\lneumann\\source\\repos\\dhbw-positioning-system\\Dhbw positioning System Backend\\Dhbw positioning System Backend\\DhbwPositioningSystemDB.db"
Scaffold-DbContext "DataSource=C:\\Users\\lneumann\\source\\repos\\dhbw-positioning-system\\Dhbw positioning System Backend\\Dhbw positioning System Backend\\DhbwPositioningSystemDB.db" Microsoft.EntityFrameworkCore.Sqlite 
Dont forget to change Values generated never to on Add!!!

*/

DROP TABLE IF EXISTS Measurement;
CREATE TABLE Measurement(
measurement_id integer primary key autoincrement,
date text,
longitudeHighAccuracy real NOT NULL,
latitudeHighAccuracy real NOT NULL,
longitudeLowAccuracy real,
latitudeLowAccuracy real
);

DROP TABLE IF EXISTS Router_Type;
CREATE TABLE Router_Type(
router_type_id integer primary key autoincrement,
name text,
range real
);

DROP TABLE IF EXISTS Access_Point;
CREATE TABLE Access_Point(
mac_address text primary key,
longitude real NOT NULL,
latitude real NOT NULL,
room text NOT NULL,
router_type_id integer NOT NULL, 
FOREIGN KEY (router_type_id) REFERENCES Router_Type(router_type_id)
);

DROP TABLE IF EXISTS Network_Measurement;
CREATE TABLE Network_Measurement(
network_measurement_id integer primary key autoincrement,
network_SSID text,
measured_strength real,
measurement_id integer NOT NULL, 
mac_address text NOT NULL,
FOREIGN KEY (measurement_id) REFERENCES Measurement(measurement_id)
FOREIGN KEY (mac_address) REFERENCES Access_Point(mac_address)
);

