create view [dm].[MaxDate] as

Select cast(getdate() as date) as MaxDate
