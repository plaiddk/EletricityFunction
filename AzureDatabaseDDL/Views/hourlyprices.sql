CREATE view [dm].[hourlyprices] as 

with dataset as (
Select SportPriceEur, HourUTC,HourDK,HourTime,DataStartPrice,[_full_text],[_id],PriceArea,SpotPriceDKK
      ,PriceDKR =     cast(replace([SpotPriceDKK],',','.') as decimal(18,2)) / 1000 
	  ,PriceDKRMoms = cast(replace([SpotPriceDKK],',','.') as decimal(18,2)) / 1000 * 1.25
	  from (
SELECT [SportPriceEur]
      ,[HourUTC]
      ,[HourDK]
	  ,substring(convert(varchar(50),[HourDK],114),1,5) as HourTime
	  ,cast(HourDK as date) as DataStartPrice
      ,[_full_text]
      ,[_id]
      ,[PriceArea]
	-- , cast(replace(SportPriceEur,',','.') as decimal(18,2))*7.4 
      ,case when cast(replace([SpotPriceDKK],',','.') as decimal(18,2)) is null then cast(replace(SportPriceEur,',','.') as decimal(18,2))*7.4 else cast(replace([SpotPriceDKK],',','.') as decimal(18,2)) end as SpotPriceDKK
	    FROM [dm].SpotpricesHistory
	  ) x

	  )
--	  select * from dataset

	  Select a.*,b.AfgifterInklMoms,a.PriceDKRMoms+AfgifterInklMoms as SpotPriceAfgiftInklMoms from dataset a

	  cross apply (select sum(PricesMoms) as AfgifterInklMoms from [dm].[PriceHistory] where [Name] not in ('Refusion')
	  ) b
