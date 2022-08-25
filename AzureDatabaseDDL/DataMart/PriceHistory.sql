CREATE TABLE [dm].[PriceHistory](
	[Name] [nvarchar](50) NULL,
	[description] [nvarchar](1000) NULL,
	[Owner] [nvarchar](50) NULL,
	[periodType] [nvarchar](50) NULL,
	[ValidFromDate] [date] NULL,
	[ValidToDate] [date] NULL,
	[tariffs_Id] [int] NULL,
	[prices] [numeric](18, 6) NULL,
	[PricesMoms] [numeric](18, 6) NULL,
	[IsRowCurrent] [int] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL
) ON [PRIMARY]
