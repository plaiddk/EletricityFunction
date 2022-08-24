create proc [dbo].[sp_truncateall]
as

begin

truncate table [dbo].[MeteringMarketEvaluationPoint]
truncate table [dbo].[MeteringMarketParticipantmRID]
truncate table [dbo].[MeteringmRID]
truncate table [dbo].[MeteringMyEnergyData_MarketDocument]
truncate table [dbo].[MeteringPeriod]
truncate table [dbo].[MeteringperiodtimeInterval]
truncate table [dbo].[MeteringPoint]
truncate table [dbo].[MeteringResult]
truncate table [dbo].[MeteringtimeInterval]
truncate table [dbo].[MeteringTimeSeries]
truncate table [dbo].[Prices]
truncate table [dbo].[PricesResult]
truncate table [dbo].[PricesSubscriptions]
truncate table [dbo].[PricesTariffs]
truncate table [dbo].[Spotprices]


--Set last 7 days flag and 1 day ahead
drop table if exists #test
SELECT * into #test FROM dimdate_v  WHERE Date_Clean BETWEEN 

CONVERT(VARCHAR, getdate()-7,7)AND
CONVERT(VARCHAR, getdate()+1,7)



--Reset columns
Update dbo.DimDate
set Last7DaysFlag = 0;

--Set last 7 days flag
update t1
set t1.Last7DaysFlag = 1
 from dbo.DimDate t1
 inner join #test b on t1.Date_Clean = b.Date_Clean



end
