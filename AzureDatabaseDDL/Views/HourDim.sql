
  create view [dbo].[HourDim] as
  Select '00:00' as HourTime, 0 as ID
  union all
   Select '01:00' as HourTime, 1 as ID
  union all
   Select '02:00' as HourTime, 2 as ID
  union all
   Select '03:00' as HourTime, 3 as ID
  union all
   Select '04:00' as HourTime, 4 as ID
  union all
   Select '05:00' as HourTime, 5 as ID
  union all
   Select '06:00' as HourTime, 6 as ID
  union all
   Select '07:00' as HourTime, 7 as ID
  union all
   Select '08:00' as HourTime, 8 as ID
  union all
   Select '09:00' as HourTime, 9 as ID
  union all
   Select '10:00' as HourTime, 10 as ID
  union all
   Select '11:00' as HourTime, 11 as ID
  union all
   Select '12:00' as HourTime, 12 as ID
  union all
   Select '13:00' as HourTime, 13 as ID
  union all
   Select '14:00' as HourTime, 14 as ID
  union all
   Select '15:00' as HourTime, 15 as ID
  union all
   Select '16:00' as HourTime, 16 as ID
  union all
   Select '17:00' as HourTime, 17 as ID
  union all
  Select '18:00' as HourTime, 18 as ID
  union all
Select '19:00' as HourTime, 19 as ID
  union all
Select '20:00' as HourTime, 20 as ID
  union all
Select '21:00' as HourTime, 21 as ID
  union all
Select '22:00' as HourTime, 22 as ID
  union all
Select '23:00' as HourTime, 23 as ID
