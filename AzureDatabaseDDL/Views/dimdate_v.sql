
CREATE view [dbo].[dimdate_v] as 
SELECT  [DateKey]
      ,[Date_Clean]
      ,[PK_Date]
      ,[Date_Name]
      ,[Year]
      ,[Year_Name]
      ,[Year_Int]
      ,[Fiscal_Year]
      ,[Fiscal_Year_Name]
      ,[Fiscal_Year_Int]
      ,[Quarter]
      ,[Quarter_Name]
      ,[Month]
      ,[Month_Name]
      ,[Week]
      ,[Week_Name]
      ,[Day_Of_Week]
      ,[Day_Of_Year]
      ,[Day_Of_Year_Name]
      ,[Day_Of_Quarter]
      ,[Day_Of_Quarter_Name]
      ,[Day_Of_Month]
      ,[Day_Of_Month_Name]
      ,[Month_Of_Year]
      ,[Month_Of_Year_Name]
      ,[Month_Of_Quarter]
      ,[Month_Of_Quarter_Name]
      ,[Quarter_Of_Year]
      ,[Quarter_Of_Year_Name]
      ,[Fiscal_Quarter]
      ,[Fiscal_Month]
      ,[Fiscal_Month_Name]
      ,[Fiscal_Quarter_Name]
	  ,UPPER(LEFT(FORMAT(Date_Clean,'dddd','da-DK'),1))+LOWER(substring(FORMAT(Date_Clean,'dddd','da-DK'),2,len(FORMAT(Date_Clean,'dddd','da-DK')))) as DagNavn
	  ,UPPER(LEFT(FORMAT(Date_Clean,'MMMM','da-DK'),1))+LOWER(substring(FORMAT(Date_Clean,'MMMM','da-DK'),2,len(FORMAT(Date_Clean,'dddd','da-DK')))) as MånedNavn
	  ,Last7DaysFlag
  FROM [dbo].[DimDate]


  where datekey >= 20170101
  and Date_Clean <= cast(getdate()+2 as date)
