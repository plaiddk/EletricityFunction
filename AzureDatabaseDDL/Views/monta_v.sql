CREATE view [dm].[monta_v] as 
with elbil as (
SELECT id,kwhs,time,value
  FROM [dbo].[monta]
  cross apply openjson(kwhs)
	with ([time] datetime ,
			[value] decimal(18,6) 
			)

  where kwh is not null

  )

  Select ID,Cast([time] as date) as StartTid,substring(convert(varchar(20),[time],120),12,5) as HourTime, [value] as Kilowatt,time 
  from(
  Select id,dateadd(hour,2,time) as time,value from  elbil

  where [value] > 0
  )x
  --order by time
  
