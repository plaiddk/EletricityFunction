create view [dm].[montastats] as 
SELECT ID, Min(StartTid) as StartDag,count(HourTime) as Timespent,sum(Kilowatt) as SumKilowatt
  FROM [dm].[monta_v]

  group by ID


--  select * from   [dm].[monta_v]
 -- order by StartTid,HourTime
