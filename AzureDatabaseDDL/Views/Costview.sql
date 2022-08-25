

CREATE view [dm].[costview] as 
 WITH rawdata
AS (
	SELECT a.*
		,b.[name] AS PriceName
		,b.[description] AS PriceDescription
		,b.prices as PriceDKR	
		
	FROM dm.HourDataHistory a
	LEFT JOIN [dm].[PriceHistory] b ON a.DataStart BETWEEN b.ValidFromDate AND b.ValidToDate and tariffs_Id != 4
	--left join dbo.nettarifdynamic c on a.DAtaStart = cast(c.ValidFrom as date) and a.HourTime = c.HourTime
	--182,668
	union all
		SELECT a.*
		,'Nettarif C forbrug' AS PriceName
		,'Nettarif C forbrug' AS PriceDescription
		,b.PRICE	 as PriceDKR
	FROM dm.HourDataHistory a
	LEFT JOIN [dbo].nettarifdynamic b ON a.DataStart = cast(b.ValidFrom as date)
		AND a.HourTime = b.HourTime
	
	UNION ALL
	
	SELECT a.*
		,'FlexEL' AS PriceName
		,'Variable ELPris' AS PriceDescription
		,b.PriceDKR		
	FROM dm.HourDataHistory a
	LEFT JOIN [dm].hourlyprices b ON a.DataStart = b.DataStartPrice
		AND a.HourTime = b.HourTime

		--order by DAtaStart,HourTime
	)
SELECT DataSetStartDate
	,DatasetEndDate
	,Unit
	,HourTimeStatus

	,a.HourTime
	,a.KiloWatt
	,DAtaStart
	,DataDateName
	,DataEnd
	,PriceName
	,PriceDescription 
	,PriceDKR
	,case when PriceName = 'Refusion' then 0 else (PriceDKR * 1.25) *a.KiloWatt end AS CostsMoms
	,case when PriceName = 'Refusion' then 0 else (a.KiloWatt * PriceDKR) end AS CostExMoms
	--,ElCosts = (KiloWatt * ElPriceDKR) * 1.25
	,b.Kilowatt as ElBilKwh
	,case when PriceName = 'Refusion' then 1.10 * b.KiloWatt end as Refusion
	,case when  b.Kilowatt is not null then 'ELBil' else 'Normal' end as ForbrugsType
	
FROM rawdata a

left join dm.monta_v b on a.DAtaStart = b.StartTid and a.HourTime = b.HourTime
cross apply ( select * from dm.PriceHistory  x where tariffs_Id = 6-- and a.DAtaStart > x.ValidFromDate
) u
--where DAtaStart >= '2022-06-09'
--where b.Kilowatt is not null
--order by DAtaStart,HourTime
