select * from Courses;
select * from Tracks;
select * from Quizzes;
select * from Projects;
select * from CourseTracks;
select * from CourseMaterials;
select * from Enrollments;
select * from Recommendations;
select * from LearningMaterials;
select * from AspNetUsers;
select * from AspNetRoles;
select * from CourseTracks;
select * from AspNetUserLogins;
select * from AspNetUserTokens;
select * from AspNetUserClaims;
select * from AspNetRoleClaims;
select * from AspNetUserRoles where RoleId = '637f289d-d239-476b-bbc3-d0e1fa93b3df';
select * from AspNetUserRoles where RoleId = '10e3105a-c63f-469f-85b9-67641a366675';

delete from Courses;
delete from Tracks;
delete from Quizzes;
delete from Recommendations;
delete from Projects;
delete from CourseTracks;
delete from CourseMaterials;
delete from Enrollments;
delete from LearningMaterials;


CREATE TABLE AspNetUsers_Staging (
    Id NVARCHAR(450),
    Fullname NVARCHAR(MAX),
    ProfilePicture NVARCHAR(255),
    UserName NVARCHAR(256),
    NormalizedUserName NVARCHAR(256),
    Email NVARCHAR(256),
    NormalizedEmail NVARCHAR(256),
    EmailConfirmed BIT,
    PasswordHash NVARCHAR(MAX),
    SecurityStamp NVARCHAR(MAX),
    ConcurrencyStamp NVARCHAR(MAX),
    PhoneNumber NVARCHAR(20),
    PhoneNumberConfirmed BIT,
    TwoFactorEnabled BIT,
    LockoutEnd DATETIMEOFFSET,
    LockoutEnabled BIT,
    AccessFailedCount INT
);

BULK INSERT AspNetUsers_Staging
FROM 'E:\Graduation-Project\Graduation Project\Data\aspnetusers.csv'
WITH (
    FIRSTROW = 2,  -- skip header
    DATAFILETYPE = 'widechar',
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n'
);

INSERT INTO AspNetUsers (
    Id, FullName, ProfilePicture, UserName, NormalizedUserName, Email, NormalizedEmail,
    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
    PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
    LockoutEnd, LockoutEnabled, AccessFailedCount
)
SELECT
    Id, FullName, ProfilePicture, UserName, NormalizedUserName, Email, NormalizedEmail,
    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
    PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled,
    LockoutEnd, LockoutEnabled, AccessFailedCount
FROM AspNetUsers_Staging;

drop table AspNetUsers_Staging;


CREATE TABLE r_Staging (
    Id NVARCHAR(450),
    Name NVARCHAR(256),
    NormalizedName NVARCHAR(256),
    ConcurrencyStamp NVARCHAR(MAX)
);

BULK INSERT r_Staging
FROM 'E:\Graduation-Project\Graduation Project\Data\aspnetroles.csv'
WITH (
    FIRSTROW = 2,  -- skip header
    DATAFILETYPE = 'widechar',
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n'
);

INSERT INTO AspNetRoles (
    Id, Name, NormalizedName, ConcurrencyStamp
)
SELECT
    Id, Name, NormalizedName, ConcurrencyStamp
FROM r_Staging;

drop table r_Staging;


CREATE TABLE ur_Staging (
    UserId NVARCHAR(256),
    RoleId NVARCHAR(256)
);

BULK INSERT ur_Staging
FROM 'E:\Graduation-Project\Graduation Project\Data\aspnetuserroles.csv'
WITH (
    FIRSTROW = 2,  -- skip header
    DATAFILETYPE = 'widechar',
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n'
);

INSERT INTO AspNetUserRoles (
    UserId, RoleId
)
SELECT
    UserId, RoleId
FROM ur_Staging;

drop table ur_Staging;


create table course_temp (
	ID INT,
	Title NVARCHAR(255),
	Description NVARCHAR(255),
	DifficultyLevel NVARCHAR(255),
	InstructorID NVARCHAR(255),
	Duration NVARCHAR(30),
	ImagePath NVARCHAR(255)
);

BULK INSERT course_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\courses.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert Courses ON;

INSERT INTO Courses (ID, Title, Description, DifficultyLevel, InstructorID, Duration, ImagePath)
SELECT ID, Title, Description, DifficultyLevel, InstructorID, Duration, ImagePath
FROM course_temp;

SET identity_insert Courses OFF;

drop table course_temp;


create table lm_temp (
	ID INT,
	Title NVARCHAR(255),
	Type NVARCHAR(255),
	Url NVARCHAR(512)
);

BULK INSERT lm_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\learningmaterials.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert LearningMaterials ON;

INSERT INTO LearningMaterials (ID, Title, Type, Url)
SELECT ID, Title, Type, Url
FROM lm_temp;

SET identity_insert LearningMaterials OFF;

drop table lm_temp;


create table enr_temp (
	ID INT,
	StudentID NVARCHAR(255),
	CourseID INT,
	EnrollmentDate Date
);

BULK INSERT enr_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\enrollments.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert Enrollments ON;

INSERT INTO Enrollments (ID, StudentID, CourseID, EnrollmentDate)
SELECT ID, StudentID, CourseID, EnrollmentDate
FROM enr_temp;

SET identity_insert Enrollments OFF;

drop table enr_temp;


create table tr_temp (
	ID INT,
	Name NVARCHAR(255),
	Description NVARCHAR(MAX),
	ImageUrl NVARCHAR(255)
);

BULK INSERT tr_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\tracks.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert Tracks ON;

INSERT INTO Tracks (ID, Name, Description, ImageURL)
SELECT ID, Name, Description, ImageUrl
FROM tr_temp;

SET identity_insert Tracks OFF;

drop table tr_temp;


create table ct_temp (
	ID INT,
	CourseID INT,
	TrackID INT
);

BULK INSERT ct_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\coursetrack.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert CourseTracks ON;

INSERT INTO CourseTracks (ID, CourseID, TrackID)
SELECT ID, CourseID, TrackID
FROM ct_temp;

SET identity_insert CourseTracks OFF;

drop table ct_temp;


create table q_temp (
	ID INT,
	Title NVARCHAR(255),
	DifficultyLevel NVARCHAR(30),
	CourseID INT
);

BULK INSERT q_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\quizzes.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert Quizzes ON;

INSERT INTO Quizzes (ID, Title, DifficultyLevel, CourseID)
SELECT ID, Title, DifficultyLevel, CourseID
FROM q_temp;

SET identity_insert Quizzes OFF;

drop table q_temp;


create table proj_temp (
	ID INT,
	Title NVARCHAR(255),
	Description NVARCHAR(255),
	DifficultyLevel NVARCHAR(30),
	CourseID INT
);

BULK INSERT proj_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\projects.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert Projects ON;

INSERT INTO Projects (ID, Title, Description, DifficultyLevel, CourseID)
SELECT ID, Title, Description, DifficultyLevel, CourseID
FROM proj_temp;

SET identity_insert Projects OFF;

drop table proj_temp;


create table cm_temp (
	ID INT,
	CourseID INT,
	MaterialID INT
);

BULK INSERT cm_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\coursematerials.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert CourseMaterials ON;

INSERT INTO CourseMaterials (ID, CourseID, MaterialID)
SELECT ID, CourseID, MaterialID
FROM cm_temp;

SET identity_insert CourseMaterials OFF;

drop table cm_temp;


create table rec_temp (
	ID INT,
	StudentID NVARCHAR(255),
	TrackID INT,
	Skills NVARCHAR(512),
	Feedback INT
);

BULK INSERT rec_temp
FROM 'E:\Graduation-Project\Graduation Project\Data\recommendations.csv'
WITH (
    FIELDTERMINATOR = ',',
    ROWTERMINATOR = '\n',
    FIRSTROW = 2
);

SET identity_insert Recommendations ON;

INSERT INTO Recommendations (ID, StudentID, TrackID, Skills, Feedback)
SELECT ID, StudentID, TrackID, Skills, Feedback
FROM rec_temp;

SET identity_insert Recommendations OFF;

drop table rec_temp;