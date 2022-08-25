
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
  , afgifter as (
  Select HourUTC,HourDK,HourTime,DataStartPrice,PriceArea,PriceDKR,PriceDKRMoms,Sum(PricesMoms) as AfgifterInklMoms
  from (
   select a.*,b.prices,b.PricesMoms,b.Name from dataset a
   left join
   (
   Select * from dm.PriceHistory
   where tariffs_Id not in (6,4) --Exclude refusion and nettarif (kommer fra dynamisk dataset fra datahubpricelist)
   ) b on a.HourDK between b.ValidFromDate and b.ValidToDate
   ) c

  group by HourUTC,HourDK,HourTime,DataStartPrice,PriceArea,PriceDKR,PriceDKRMoms
  )

  Select HourUTC,HourDK,HourTime,DataStartPrice,PriceArea,PriceDKR,PriceDKRMoms,PriceDKRMoms+AfgifterInklMoms+PriceNetTarif as SpotPriceAfgiftInklMoms,PriceNetTarif
  from (
  Select a.*
  ,case when a.HourDK <= '2022-03-31 23:00:00.000' then 0.1725 *1.25 else b.PRICE *1.25 end as PriceNetTarif ---Case when due to not having old net tarif company info - MAYBE FIX LATER
  from afgifter a
  left join dbo.nettarifdynamic b on a.HourTime = b.HourTime and a.HourDK = b.ValidFrom --and b.ValidTo

  )z
