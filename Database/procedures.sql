/*SPROCS */
if exists(select * from sys.objects where name = 'ScanObjectById')
drop procedure ScanObjectById
go
create procedure ScanObjectById
(
	@id bigint 
)
as 
begin
	select 
	id,
	name,
	description,
	url,
	page_status,
	page_source,
	crawl_depth,
	screenshot,
	parent_id,
	guid
	from ScanObject 
	where Id = @id 
end
go
if exists(select * from sys.objects where name = 'ScanObjectByUrl')
drop procedure ScanObjectByUrl 
go
create procedure ScanObjectByUrl 
(
	@url nvarchar(max)
)
as 
begin
	select 
	id,
	name,
	description,
	url,
	page_status,
	page_source,
	crawl_depth,
	screenshot,
	parent_id,
	guid
	from ScanObject 
	where Url = @url
end
go
if exists(select * from sys.objects where name = 'InsertScanObject')
drop procedure InsertScanObject
go
create procedure InsertScanObject 
(
	@name nvarchar(256),
	@description nvarchar(max) = null,
	@url nvarchar(max),
	@page_status nvarchar(max) = null,
	@page_source nvarchar(max) = null,
	@crawl_depth int = null,
	@screenshot binary = null,
	@parent_id bigint = null,
	@guid uniqueidentifier = null
)
as
begin
	if (select count(*) from ScanObject where Url = @url) = 0
	begin 
		insert into ScanObject
		(
			name,
			description,
			url,
			page_status,
			page_source,
			crawl_depth,
			screenshot,
			parent_id,
			guid
		)
		select 
		@name,
		@description,
		@url,
		@page_status,
		@page_source,
		@crawl_depth,
		@screenshot,
		@parent_id,
		@guid
	end 
end
go
if exists(select * from sys.objects where name = 'UpdateScanObject')
drop procedure UpdateScanObject 
go
create procedure UpdateScanObject 
(
	@id bigint,
	@name nvarchar(256),
	@description nvarchar(max) = null,
	@url nvarchar(max),
	@page_status nvarchar(max) = null,
	@page_source nvarchar(max) = null,
	@crawl_depth int = null,
	@screenshot binary = null,
	@parent_id bigint = null 
)
as
begin 
	update ScanObject 
	set name = @name,
	description = @description,
	url = @url,
	page_status = @page_status,
	page_source = @page_source,
	crawl_depth = @crawl_depth,
	screenshot = @screenshot,
	parent_id = @parent_id
	where Id = @id 
end 
go
if exists(select * from sys.objects where name = 'InsertNarrativeObject')
drop procedure InsertNarrativeObject
go
create procedure InsertNarrativeObject
(
	@guid uniqueidentifier,
	@scan_object_id bigint,
	@created datetime 
)	
as
begin
if not exists(select * from NarrativeObject where guid = @guid and scan_object_id = @scan_object_id)
	begin 
		insert into NarrativeObject
		(
			guid,
			scan_object_id,
			created
		)
		select 
		@guid,
		@scan_object_id,
		@created
	end 
end
go
if exists(select * from sys.objects where name = 'GetNarrativeObjectId')
drop procedure GetNarrativeObjectId 
go
create procedure GetNarrativeObjectId
(
	@guid uniqueidentifier,
	@scan_object_id bigint 
)	
as
begin
	select 
	Id 
	from NarrativeObject 
	where guid = @guid 
	and scan_object_id = @scan_object_id 

end
go
if exists(select * from sys.objects where name = 'InsertNarrativeMessage')
drop procedure InsertNarrativeMessage 
go
create procedure InsertNarrativeMessage
(
	@contents nvarchar(max),
	@created datetime,
	@guid uniqueidentifier,
	@errors bit,
	@narrative_id bigint 
)
as 
begin 
	insert into NarrativeMessage
	(
		contents,
		created,
		guid,
		errors,
		narrative_id 
	)
	select 
	@contents,
	@created,
	@guid,
	@errors,
	@narrative_id
end 
go
if exists(select * from sys.objects where name = 'NextScanObject')
drop procedure NextScanObject
go
create procedure NextScanObject
as
begin
declare @id bigint = (	select top 1 id 
						from ScanObject 
						where page_status is null and 
						substring(url, 8, 22) not in (select distinct substring(url, 8, 22) from ScanObject where page_status = 'PROCESSING')
						order by id)
	select top 1 
	id,
	name,
	description,
	url,
	page_status,
	page_source,
	crawl_depth,
	screenshot,
	parent_id,
	guid
	from ScanObject with(nolock)
	where id = @id 
update ScanObject set page_status = 'PROCESSING' where id = @id
end
go



