
CREATE view [dbo].[HourData] as 

with hourdata as (
select 
--a.result_Id
--,a.success
--,a.errorCode
--,a.errorText
--,a.id
--,a.stacktrace
--,b.mrid
--,b.createdDatetime
--,b.MyEnergyData_MarketDocument_Id
c.[start] as DataSetStartDate
,c.[end] as DataSetEndDate
--,case when d.businessType ='a04' then 'Forbrug' else d.businessType end as businessType
--,d.curveType
,d.[measurement_Unit.name] as Unit
--,d.TimeSeries_Id
--,f.codingScheme
--,g.resolution
--,h.position as realposition
--,h.position -1 as Position
,case when (h.position-1) between 0 and 5 then 'Nat' 
when (h.position-1) between 6 and 8 then 'Morgen'
when (h.position-1)  between 9 and 12 then 'Formiddag'
when (h.position-1) between 13 and 17 then 'Eftermiddag'
when (h.position-1) between 18 and 21 then 'Aften'
when (h.position-1) between 21 and 23 then 'Sen aften' end as HourTimeStatus
,right('00'+ISNULL(cast(h.position -1 as varchar(10)),''),2)+':00' as HourTime
--,case when cast(h.[out_Quantity.quantity] as decimal(18,2)) > 5 then 'Elbil' else 'Normal' end as ForbrugsType

--,case when h.[out_Quantity.quality] ='a04' then 'Målt' else h.[out_Quantity.quality] end as MåleStatus
,cast(h.[out_Quantity.quantity] as decimal(18,2)) as KiloWatt
,cast(j.[start] as date) as  DataStart
,DATENAME(weekday,cast(j.[start] as date)) as  DataDateName
,cast(j.[end]  as date) as DataEnd
from
[dbo].[Pricesresult] a
inner join dbo.[MeteringMyEnergyData_MarketDocument] b on a.result_Id = b.result_Id
inner join dbo.MeteringperiodtimeInternal_v c on b.MyEnergyData_MarketDocument_Id = c.MyEnergyData_MarketDocument_Id
inner join dbo.MeteringTimeSeries d on b.MyEnergyData_MarketDocument_Id = d.MyEnergyData_MarketDocument_Id
inner join [dbo].[MeteringMarketEvaluationPoint] e on d.TimeSeries_Id = e.TimeSeries_Id
inner join [dbo].[MeteringmRID] f on f.MarketEvaluationPoint_Id = e.MarketEvaluationPoint_Id
inner join [dbo].[MeteringPeriod] g on d.TimeSeries_Id = g.TimeSeries_Id
inner join  [dbo].[MeteringPoint] h on g.Period_Id = h.Period_Id
inner join [dbo].MeteringTimeinterval_v j on j.Period_Id = g.Period_Id

where businessType != 'a64' and h.position != 25)

select DatasetStartDate,DatasetEndDate,Unit,HourTimeStatus,a.HourTime,a.KiloWatt,DataStart,DataDateName,DataEnd,b.Kilowatt as ElBilKwh
	,b.Kilowatt * 1.10 as Refusion
	,case when  b.Kilowatt is not null then 'ELBil' else 'Normal' end as ForbrugsType from hourdata a
left join dm.monta_v b on a.DAtaStart = b.StartTid and a.HourTime = b.HourTime
