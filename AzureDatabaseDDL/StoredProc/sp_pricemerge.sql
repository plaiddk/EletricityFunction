create proc [dbo].[sp_pricemerge] as

  begin


  Insert into dm.PriceHistory
([Name]
      ,[description]
      ,[Owner]
      ,[periodType]
      ,[ValidFromDate]
      ,[ValidToDate]
      ,[tariffs_Id]
      ,[prices]

	  ,IsRowCurrent
)
Select [Name]
      ,[description]
      ,[Owner]
      ,[periodType]
      ,getdate()
      ,'9999-12-31'
      ,[tariffs_Id]
      ,[prices]
	  ,1 from (

  MERGE INTO dm.PriceHistory        AS t
  USING (
  SELECT distinct
      [name]
      ,[description]
      ,[owner]
      ,[periodType]  
      ,a.[tariffs_Id]    
	  ,b.prices
  FROM [dbo].[Pricestariffs] a inner join [dbo].[Prices] b on a.tariffs_Id = b.tariffs_Id

  )  AS s ON t.tariffs_id        = s.tariffs_id
	
  WHEN NOT MATCHED BY TARGET
      THEN INSERT (
            [Name]
      ,[description]
      ,[Owner]
      ,[periodType]
      ,[ValidFromDate]
      ,[ValidToDate]
      ,[tariffs_Id]
      ,[prices]
      ,[PricesMoms]
	  ,IsRowCurrent
		
       
        ) VALUES (
          s.[name]
         ,s.[description]
         ,s.[owner]
         ,s.periodType        
         ,getdate() --s.ValidfromDate   
         ,'9999-12-31'   --ValidTo
		 ,s.tariffs_id
		 ,s.prices
		 ,null
		 ,1 --ISRowCurrent
		
        )	

	  WHEN MATCHED AND EXISTS 
      ( 
      Select 
			t.prices
		EXCEPT 
		SELECT
		s.prices
      )
      THEN UPDATE SET
      IsRowCurrent = 0
	  ,t.ValidToDate = getdate()	 

	  OUTPUT S.[Name]
      ,s.[description]
      ,s.[Owner]
      ,s.[periodType]
     
      ,s.[tariffs_Id]
      ,s.[prices]
	  ,$Action as MergeAction
	  ) as MRG

	  where MRG.MergeAction ='UPDATE'
	
  

    update [dm].[PriceHistory]
  set PricesMoms = prices*1.25
end
