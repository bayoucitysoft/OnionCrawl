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
	parent_id
	from ScanObject 
	where Id = @id 
end
go
