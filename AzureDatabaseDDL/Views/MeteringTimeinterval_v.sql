create view [dbo].[MeteringTimeinterval_v] as 
SELECT  cast([start] AT TIME ZONE 'UTC' AT TIME ZONE 'Central Europe Standard Time' as datetime)  as [start]
      , cast([end] AT TIME ZONE 'UTC' AT TIME ZONE 'Central Europe Standard Time' as datetime)as [end]
      ,[Period_Id]
  FROM [dbo].[MeteringtimeInterval]
