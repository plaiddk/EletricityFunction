  create view [dbo].[Prognose]
  as

  SELECT  [Time]
      ,([Price] * 7.43) / 1000 as PriceDKR --Husk at hente valutakurser dynamisk
	  
	  ,cast(time as date) as Dato

	  ,substring(convert(varchar(50),[Time],114),1,5) as HourTime
  FROM dm.[PrognoseSpotpriceHistory]

  where  cast(time as date) >= cast(Getdate() as date)
