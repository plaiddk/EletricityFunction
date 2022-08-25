create proc [dbo].[sp_PrognoseSpotpricemerge] as

  begin


  MERGE [dm].[PrognoseSpotpriceHistory] as targ
USING [dbo].[PrognoseSpotprice] AS src
    ON src.Time = targ.Time
WHEN MATCHED
    AND EXISTS (
        SELECT src.Price
        EXCEPT
        SELECT targ.Price
        )
    THEN
        UPDATE
        SET 
		Price = src.Price

WHEN NOT MATCHED BY TARGET
    THEN
        INSERT (
     Time,Price
            )
        VALUES (
          src.Time
      ,src.Price

            )
;


end
