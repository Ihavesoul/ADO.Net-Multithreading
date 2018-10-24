CREATE TABLE Students (
StudentId int IDENTITY(1,1) NOT NULL,
FirstName varchar(255) NOT NULL,
SecondName varchar(255) NOT NULL,
AverageMark int NOT NULL,
Visiting float
CONSTRAINT [Students_PK] PRIMARY KEY NONCLUSTERED(StudentId))
GO
CREATE TABLE Lectors (
LectorId int  IDENTITY(1,1)NOT NULL,
FirstName varchar(255) NOT NULL,
SecondName varchar(255) NOT NULL
CONSTRAINT [Lectors_PK] PRIMARY KEY NONCLUSTERED(LectorId))
GO
CREATE TABLE Lection (
LectionId int IDENTITY(1,1) NOT NULL,
LectorId int NOT NULL,
LectionName varchar(255) NOT NULL,
LectionData datetime NOT NULL,
CONSTRAINT [Lection_PK] PRIMARY KEY NONCLUSTERED(LectionId),
CONSTRAINT FK_Lection_Lectors FOREIGN KEY (LectorId)
  REFERENCES dbo.Lectors(LectorId)
  ON DELETE CASCADE
  ON UPDATE CASCADE
  );
  GO
CREATE TABLE StudentAndLections(
StudentId int  NOT NULL,
LectionId int NOT NULL,
Mark int NOT NULL,
Presence bit NOT NULL,
CONSTRAINT FK_Student_Lection_Lectors FOREIGN KEY (LectionId)
  REFERENCES dbo.Lection(LectionId)
  ON DELETE CASCADE
  ON UPDATE CASCADE,
  CONSTRAINT FK_Lection_Students FOREIGN KEY (StudentId)
  REFERENCES dbo.Students(StudentId)
  ON DELETE CASCADE
  ON UPDATE CASCADE,
CONSTRAINT [StudentAndLections_PK] PRIMARY KEY CLUSTERED (StudentId ASC, LectionId ASC))

