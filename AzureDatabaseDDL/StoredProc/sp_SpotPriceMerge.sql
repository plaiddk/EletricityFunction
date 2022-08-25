  CREATE proc [dbo].[sp_Spotpricemerge] as

  begin


  MERGE dm.SpotPricesHistory as targ
USING [dbo].SpotPrices AS src
    ON src.HourUTC = targ.HourUTC
WHEN MATCHED
    AND EXISTS (
        SELECT src.SportPriceEUR,src.HourDK,src._full_text,src._id,src.PriceArea,src.SpotPriceDKK
        EXCEPT
        SELECT targ.SportPriceEUR,targ.HourDK,targ._full_text,targ._id,targ.PriceArea,targ.SpotPriceDKK
        )
    THEN
        UPDATE
        SET 
		SportPriceEUR = src.SportPriceEUR
		,HourDK = src.HourDK
		,_full_text = src._full_text
		,_id = src._id
		,PriceArea = src.PriceArea
		,SpotPriceDKK = src.SpotPriceDKK
WHEN NOT MATCHED BY TARGET
    THEN
        INSERT (
            [SportPriceEur]
      ,[HourUTC]
      ,[HourDK]
      ,[_full_text]
      ,[_id]
      ,[PriceArea]
      ,[SpotPriceDKK]
            )
        VALUES (
          src.[SportPriceEur]
      ,src.[HourUTC]
      ,src.[HourDK]
      ,src.[_full_text]
      ,src.[_id]
      ,src.[PriceArea]
      ,src.[SpotPriceDKK]
            )
;
