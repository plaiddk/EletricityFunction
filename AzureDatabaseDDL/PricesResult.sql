CREATE TABLE [dbo].[PricesResult](
	[meteringPointId] [nvarchar](50) NULL,
	[result_Id] [int] NULL,
	[success] [bit] NULL,
	[errorCode] [nvarchar](100) NULL,
	[errorCodeEnum] [nvarchar](100) NULL,
	[errorText] [nvarchar](50) NULL,
	[id] [nvarchar](50) NULL,
	[stackTrace] [nvarchar](50) NULL,
	[result_id_0] [int] NULL
) ON [PRIMARY]