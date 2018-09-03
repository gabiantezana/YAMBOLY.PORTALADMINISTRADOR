CREATE TABLE Roles
(
RolId int not null primary key identity
,RolName nvarchar(max)
,RolDescription nvarchar(max)
)
--drop table Users
CREATE TABLE Users
(
UserId int not null primary key identity
,UserName nvarchar(max)
,Pass varbinary(max)
,isActive bit
,RolId int foreign key references Roles(RolId)
)

create table ErrorLog
(
	ErrorLogId int not null primary key identity
	,internalException nvarchar(max)
	,errorMessage nvarchar(max)
	,browser nvarchar(max)
	,stackTrace nvarchar(max)
	,errorType nvarchar(max)
	,sessionKeys nvarchar (max)
	,errorTimeStamp nvarchar(max)
)

CREATE TABLE ViewGroup
(
ViewGroupId int not null primary key identity
,ViewGroupName nvarchar (max)
,ViewGroupCode nvarchar(max)
)


CREATE TABLE Views
(
ViewId int not null primary key identity
,ViewName nvarchar(max)
,ViewValue nvarchar(max)
,ViewGroupId int not null foreign key references ViewGroup(ViewGroupId)
)

CREATE TABLE RolesViews
(
RolViewId int not null primary key identity
,RolId int not null foreign key references Roles(RolId)
,ViewId int not null foreign key references Views(ViewId)
,RolesViewsState bit
)

INSERT INTO Roles VALUES ('SUPERADMINISTRATOR','SUPERADMINISTRATOR')
INSERT INTO Roles VALUES ('ADMINISTRATOR	','ADMINISTRATOR')
INSERT INTO Users VALUES ('ADMIN',NULL,1, 1) 
INSERT INTO Users VALUES ('SUPERADMIN',NULL,1, 1) 

INSERT INTO ViewGroup VALUES ( 'ADMINISTRATION.USER', 'ADMINISTRATION.USER')
INSERT INTO Views VALUES ('ADMINISTRATION.USER.LIST','Usuarios',  1)
INSERT INTO Views VALUES ('ADMINISTRATION.USER.ADD_UPDATE','Usuarios',  1)
