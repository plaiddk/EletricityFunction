CREATE view [dbo].[nettarifdynamic] as
WITH TimeTally (n) AS
(
    SELECT top(24)
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1  n                 -- zero-based
    -- Returns exactly 86400 rows (number of seconds in a day)
    FROM       (VALUES(0),(0),(0),(0),(0),(0))                         a(n) -- 6 rows 
    CROSS JOIN (VALUES(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0)) b(n) -- x12 rows
    CROSS JOIN (VALUES(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0),(0)) c(n) -- x12 rows
    CROSS JOIN (VALUES(0),(0),(0),(0),(0),(0),(0),(0),(0),(0))         d(n) -- x10 rows
    CROSS JOIN (VALUES(0),(0),(0),(0),(0),(0),(0),(0),(0),(0))         e(n) -- x10 rows
) ,
ten AS(
    SELECT n FROM (VALUES(1),(1),(1),(1),(1),(1),(1),(1),(1),(1)) t(n)
), millions AS (
    SELECT
    TOP(DATEDIFF(day,'2017-01-01','2022-04-01'))
    ROW_NUMBER() OVER(ORDER BY (SELECT 'no specific order')) AS n 
    FROM ten t1
    CROSS JOIN ten t2
    CROSS JOIN ten t3 
    CROSS JOIN ten t4 
    CROSS JOIN ten t5 
    CROSS JOIN ten t6
    ORDER BY n
)
,HoursDay as 
(
SELECT TimeSlot=DATEADD(hour, n, CAST('00:00' AS TIME))
FROM TimeTally
)


select cast(CONCAT(VALIDFROM,+' ' + HourTime)  as datetime) as ValidFrom,ValidTo,Price, HourTime from (
SELECT -- [ChargeOwner]
    --  ,[GLN_Number]
  
     -- ,[Note]
     -- ,[Description]
      cast(convert(datetime,convert(varchar(50),[ValidFrom],5),103) as date)  as ValidFrom
	  --,ValidFrom
	  --,ISDATE(ValidFrom) ad
    --  ,[ValidTo]
	 , cast(convert(datetime,convert(varchar(50),[ValidTo],5),101) as date)  as ValidTo
   --   ,[VATClass]
      ,[Price1]														as '00:00'
      ,case when [Price2]  is null then [Price1] else [Price2] 	end as '01:00'
      ,case when [Price3]  is null then [Price1] else [Price3] 	end as '02:00'
      ,case when [Price4]  is null then [Price1] else [Price4] 	end as '03:00'
      ,case when [Price5]  is null then [Price1] else [Price5] 	end as '04:00'
      ,case when [Price6]  is null then [Price1] else [Price6] 	end as '05:00'
      ,case when [Price7]  is null then [Price1] else [Price7] 	end as '06:00'
      ,case when [Price8]  is null then [Price1] else [Price8] 	end as '07:00'
      ,case when [Price9]  is null then [Price1] else [Price9] 	end as '08:00'
      ,case when [Price10] is null then [Price1] else [Price10]	end as '09:00'
      ,case when [Price11] is null then [Price1] else [Price11]	end as '10:00'
      ,case when [Price12] is null then [Price1] else [Price12]	end as '11:00'
      ,case when [Price13] is null then [Price1] else [Price13]	end as '12:00'
      ,case when [Price14] is null then [Price1] else [Price14]	end as '13:00'
      ,case when [Price15] is null then [Price1] else [Price15]	end as '14:00'
      ,case when [Price16] is null then [Price1] else [Price16]	end as '15:00'
      ,case when [Price17] is null then [Price1] else [Price17]	end as '16:00'
      ,case when [Price18] is null then [Price1] else [Price18]	end as '17:00'
      ,case when [Price19] is null then [Price1] else [Price19]	end as '18:00'
      ,case when [Price20] is null then [Price1] else [Price20]	end as '19:00'
      ,case when [Price21] is null then [Price1] else [Price21]	end as '20:00'
      ,case when [Price22] is null then [Price1] else [Price22]	end as '21:00'
      ,case when [Price23] is null then [Price1] else [Price23]	end as '22:00'
      ,case when [Price24] is null then [Price1] else [Price24]	end as '23:00'
     
  FROM 
  
  [dbo].[DatahubPricelist]


 where note = 'Nettarif C forbrug' and ValidTo is not null
 and ResolutionDuration = 'PT1H'
 ) x

 UNPIVOT
 (PRICE for HourTime in (
  [00:00]
 ,[01:00]
 ,[02:00]
 ,[03:00]
 ,[04:00]
 ,[05:00]
 ,[06:00]
 ,[07:00]
 ,[08:00]
 ,[09:00]
 ,[10:00]
 ,[11:00]
 ,[12:00]
 ,[13:00]
 ,[14:00]
 ,[15:00]
 ,[16:00]
 ,[17:00]
 ,[18:00]
 ,[19:00]
 ,[20:00]
 ,[21:00]
 ,[22:00]
 ,[23:00]
) 
)
as unpiv


union all


select Timeslotstart + timeslot as ValidFrom, '2022-04-01', 0.1725 as Price,substring(convert(varchar(50),Timeslotstart + timeslot,114),1,5) as HourTime from (
SELECT
    cast(DATEADD(day, (m.n-1),'2017-01-01') as datetime) AS TimeSlotStart , cast(g.timeslot as datetime) as timeslot
FROM millions m

cross apply (
select TimeSlot from HoursDay
)g
)Z
