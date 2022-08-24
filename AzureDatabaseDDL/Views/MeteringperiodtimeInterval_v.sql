create view [dbo].[MeteringperiodtimeInternal_v] as 
SELECT  cast([start] AT TIME ZONE 'UTC' AT TIME ZONE 'Central Europe Standard Time' as datetime)  as [start]
      ,cast([end] AT TIME ZONE 'UTC' AT TIME ZONE 'Central Europe Standard Time' as datetime)  as [end]
      ,[MyEnergyData_MarketDocument_Id]
  FROM [dbo].[MeteringperiodtimeInterval]
