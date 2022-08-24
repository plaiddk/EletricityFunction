CREATE TABLE [dm].[HourDataHistory](
	[DataSetStartDate] [datetime] NULL,
	[DatasetEndDate] [datetime] NULL,
	[Unit] [nvarchar](10) NULL,
	[HourTimeStatus] [varchar](11) NULL,
	[HourTime] [varchar](5) NULL,
	[KiloWatt] [decimal](18, 2) NULL,
	[DAtaStart] [date] NULL,
	[DataDateName] [nvarchar](30) NULL,
	[DataEnd] [date] NULL,
	[ElBilKwh] [decimal](18, 6) NULL,
	[Refusion] [numeric](22, 8) NULL,
	[ForbrugsType] [varchar](10) NULL
) ON [PRIMARY]
