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
	parent_id bigint 
)
go
