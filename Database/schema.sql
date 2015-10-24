/*SCHEMA DEFINITIONS */
go
if exists( select * from sys.objects where name = 'ScanObject')
drop table ScanObject 
go
create table ScanObject
(
	id bigint identity(1,1),
	name nvarchar(256),
	description nvarchar(max),
	url nvarchar(max),
	page_status nvarchar(max),
	page_source nvarchar(max), 
	crawl_depth int, 
	screenshot binary,
	parent_id bigint,
	guid uniqueidentifier
)
go
if exists(select * from sys.objects where name = 'NarrativeObject')
drop table NarrativeObject 
go
create table NarrativeObject
(
	id bigint identity(1,1),
	guid uniqueidentifier,
	scan_object_id bigint,
	created datetime,

)
go
if exists(select * from sys.objects where name = 'NarrativeMessage')
drop table NarrativeMessage
go
create table NarrativeMessage
(
	id bigint identity(1,1), 
	contents nvarchar(max),
	created datetime,
	guid uniqueidentifier,
	errors bit,
	narrative_id bigint 
)
go


