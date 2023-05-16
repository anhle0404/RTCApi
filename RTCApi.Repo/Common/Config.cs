using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTCApi.Repo.Common
{
    public class Config
    {
        /// <summary>
        /// Môi trường chạy
        /// 1: Môi trường Publish lên server
        /// 0: Môi trường Test trên local
        /// </summary>
        public static int _environment = 1;

        public static string _pathUpload = @"\\192.168.1.2\ftp\Upload\StockManagement\Images\";
        public static string _baseImageUrl = "http://192.168.1.2:8085/api/Upload/StockManagement/Images?filename=";
        //public static string _baseImageUrl = "https://localhost:44381/api/Upload/StockManagement/Images?filename=";

        public static string Connection()
        {
            string conn = "";
            if (_environment == 0)
            {
                conn = @"Data Source=DESKTOP-40H717B\SQLEXPRESS;Initial Catalog=StockManagement;User ID=sa; Password = 123456a@";

                //conn = @"Data Source=DESKTOP-UUBJ3SI;Initial Catalog=StockManagement;User ID=sa; Password = 123456a@";

            }
            else
            {
                //conn = @"Data Source=JP27011-2-DT104\SQLEXPRESS;Initial Catalog=StockManagement;User ID=sa; Password = Pap@123cs";

            }

            return conn;
        }
    }
}
