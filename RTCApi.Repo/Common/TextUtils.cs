using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RTCApi.Repo.Common
{
    public class TextUtils
    {
        private static string connectionString = Config.Connection();

        public static object ExcuteScalar(string strSQL)
        {
            object value = null;
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                
                SqlCommand cmd = new SqlCommand(strSQL, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cn.Open();
                cmd.CommandText = strSQL;
                value = cmd.ExecuteScalar();
                cn.Close();
            }
            catch(SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();   
            }
            return value;
        }

        /// <summary>
        /// Thực thi một câu lệnh Command
        /// </summary>
        /// <param name="strSQL">Chuỗi command</param>
        public static void ExcuteSQL(string strSQL)
        {
            SqlConnection cn = new SqlConnection(connectionString);

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 0;
                cn.Open();
                cmd.CommandText = strSQL;
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (SqlException ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            
        }

        /// <summary>
        /// Load DataTable từ StoreProcedure
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="valueParam"></param>
        /// <returns></returns>
        public static DataTable GetDataTableSP(string commandText, string[] param, object[] valueParam)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                var dt = new DataTable();
                SqlParameter sqlParam;
                SqlCommand cmd = new SqlCommand(commandText, conn);
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        sqlParam = new SqlParameter(param[i], valueParam[i]);
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Load DataSet từ StoreProcedure
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="param"></param>
        /// <param name="valueParam"></param>
        /// <returns></returns>
        public static DataSet GetDataSetSP(string commandText, string[] param, object[] valueParam)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                DataSet dataSet = new DataSet();
                SqlParameter sqlParam;
                SqlCommand cmd = new SqlCommand(commandText, conn);
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        sqlParam = new SqlParameter(param[i], valueParam[i]);
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
                conn.Close();
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

            }
        }


        public static void ExcuteProcedure(string storeProcedureName, string[] paramName, object[] paramValue)
        {
            SqlConnection cn = new SqlConnection(connectionString);
            try
            {
                cn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(storeProcedureName, cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                SqlParameter sqlParam;
                cn.Open();
                if (paramName != null)
                {
                    for (int i = 0; i < paramName.Length; i++)
                    {
                        sqlParam = new SqlParameter(paramName[i], paramValue[i]);
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }

        /// <summary>
        /// Convert object to string
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToString(object x)
        {
            try
            {
                return Convert.ToString(x);
            }
            catch
            {

                return "";
            }
        }


        /// <summary>
        /// Convert object to int
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int ToInt(object x)
        {
            try
            {
                return Convert.ToInt32(x);
            }
            catch
            {

                return 0;
            }
        }


        /// <summary>
        /// Convert object to float
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float ToFloat(object x)
        {
            try
            {
                return Convert.ToSingle(x);
            }
            catch
            {

                return 0;
            }
        }

        /// <summary>
        /// Convert object to decimal
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object x)
        {
            try
            {
                return Convert.ToDecimal(x);
            }
            catch
            {

                return 0;
            }
        }

        /// <summary>
        /// Convert object to bool
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool ToBoolean(object x)
        {
            try
            {
                return Convert.ToBoolean(x);
            }
            catch
            {

                return false;
            }
        }

        /// <summary>
        /// ngày giờ vn
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DateTime? ToDate(object x)
        {
            string date = "";
            if (x != null)
            {
                date = x.ToString();
            }
            try
            {
                try
                {
                    return DateTime.Parse(date, new CultureInfo("vi", true));
                }
                catch
                {
                    return DateTime.Parse(date, new CultureInfo("fr-FR", true));

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        static public DataTable Select(string strComm)
        {
            SqlConnection cnn = new SqlConnection(Config.Connection());
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                cnn.Open();
                cmd = new SqlCommand("spSearchAllForTrans", cnn);
                cmd.CommandTimeout = 6000;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@sqlCommand", strComm));
                //cmd.ExecuteNonQuery();

                da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds.Tables[0];
            }
            catch (SqlException se)
            {
                return new DataTable();
                //throw new Exception("Sellect error :" + se.Message);
            }
            finally
            {
                cnn.Close();
            }
        }

        /// <summary>
        /// Convert DataTable to List object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            try
            {
                List<T> data = new List<T>();
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static T GetItem<T>(DataRow dr)
        {
            try
            {
                Type temp = typeof(T);
                T obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in dr.Table.Columns)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                        {
                            var value = Convert.IsDBNull(dr[column.ColumnName]) ? null : dr[column.ColumnName];
                            pro.SetValue(obj, value, null);
                        }

                        else
                        {
                            continue;
                        }

                    }
                }
                return obj;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
