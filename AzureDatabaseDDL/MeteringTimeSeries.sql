CREATE TABLE [dbo].[MeteringTimeSeries](
	[mRID] [nvarchar](50) NULL,
	[businessType] [nvarchar](50) NULL,
	[curveType] [nvarchar](50) NULL,
	[measurement_Unit.name] [nvarchar](10) NULL,
	[TimeSeries_Id] [int] NULL,
	[MyEnergyData_MarketDocument_Id] [int] NULL
) ON [PRIMARY]
