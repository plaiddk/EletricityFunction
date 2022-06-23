using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eletricity.Helper
{
    internal class InsertData
    {
        public static void InsertDataSQL(DataSet _dataset, string option)
        {
            try
            {
                var conn = SqlConnecter.SqlConn();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {

                    foreach (DataTable item in _dataset.Tables)
                    {

                        if (option.Equals("insertmetering"))
                        {

                            if (item.TableName.Equals("sender_MarketParticipant.mRID"))
                            {
                                bulk.DestinationTableName = "MeteringMarketParticipantmRID";
                                conn.Open();

                                bulk.WriteToServer(item);
                                conn.Close();

                            }

                            if (item.TableName.Equals("period.timeInterval"))
                            {
                                bulk.DestinationTableName = "MeteringperiodtimeInterval";
                                conn.Open();

                                bulk.WriteToServer(item);
                                conn.Close();

                            }

                            else
                            {
                                if (!item.TableName.Equals("sender_MarketParticipant.mRID") && !item.TableName.Equals("period.timeInterval"))
                                {

                                    bulk.DestinationTableName = @$"Metering{item.TableName}";
                                    conn.Open();

                                    bulk.WriteToServer(item);
                                    conn.Close();
                                }
                            }



                        }

                        if (option.Equals("insertprices"))
                        {
                            if (item.TableName.Equals("prices"))
                            {
                                bulk.DestinationTableName = "Prices";
                                conn.Open();

                                bulk.WriteToServer(item);
                                conn.Close();

                            }
                            else
                            {
                                bulk.DestinationTableName = $"Prices{item.TableName}";
                                conn.Open();

                                bulk.WriteToServer(item);
                                conn.Close();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //log.LogInformation(ex.Message);
            }



        }
    }
}
