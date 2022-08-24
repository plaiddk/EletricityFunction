
  CREATE proc [dbo].[sp_HourDatamerge] as

  begin


  MERGE dm.HourDataHistory as targ
USING [dbo].HourData AS src
    ON src.DataStart = targ.DataStart and src.DataEnd = targ.DataEnd and src.HourTime = targ.HourTime
WHEN MATCHED
    AND EXISTS (
        SELECT src.DataSetStartDate,src.DataSetEndDate,src.unit,src.HourTimeStatus,src.KiloWatt,src.DataDateName
		 ,src.[ElBilKwh]
      ,src.[Refusion]
      ,src.[ForbrugsType]
        EXCEPT
        SELECT targ.DataSetStartDate,targ.DataSetEndDate,targ.unit,targ.HourTimeStatus,targ.KiloWatt,targ.DataDateName
		 ,targ.[ElBilKwh]
      ,targ.[Refusion]
      ,targ.[ForbrugsType]
        )
    THEN
        UPDATE
        SET 
		DataSetStartDate = src.DataSetStartDate
		,DataSetEndDate = src.DataSetEndDate
		,unit = src.unit
		,HourTimeStatus = src.HourTimeStatus
		,KiloWatt = src.KiloWatt
		,DataDateName = src.DataDateName
		 ,[ElBilKwh] = src.ElBilKwh
      ,[Refusion] = src.Refusion
      ,[ForbrugsType] = src.ForbrugsType
		
WHEN NOT MATCHED BY TARGET
    THEN
        INSERT (
             [DataSetStartDate]
      ,[DataSetEndDate]
      ,[Unit]
      ,[HourTimeStatus]
      ,[HourTime]
      ,[KiloWatt]
      ,[DataStart]
      ,[DataDateName]
      ,[DataEnd]
	   ,[ElBilKwh]
      ,[Refusion]
      ,[ForbrugsType]
	
            )
        VALUES (
          src.[DataSetStartDate]
      ,src.[DataSetEndDate]
      ,src.[Unit]
      ,src.[HourTimeStatus]
      ,src.[HourTime]
      ,src.[KiloWatt]
      ,src.[DataStart]
      ,src.[DataDateName]
      ,src.[DataEnd]
	   ,src.[ElBilKwh]
      ,src.[Refusion]
      ,src.[ForbrugsType]
	 
            )
;



end
