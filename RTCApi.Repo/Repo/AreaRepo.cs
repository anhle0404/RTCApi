using ConnectSQL19.Model.Common;
using ConnectSQL19.Model.Context;
using ConnectSQL19.Model.Entities;
using ConnectSQL19.Repo.GenericRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSQL19.Repo.Repo
{
    public class AreaRepo:GenericRepo<Area>
    {
        public AreaModel GetAreaByCode(string code)
        {
            //Area area = GetAll().Where(x => x.AreaCode == code).FirstOrDefault();

            //DataTable dt = TextUtils.Select($"SELECT * FROM Area WHERE AreaCode = '{code}'");
            //AreaModel area = TextUtils.ConvertDataTable<AreaModel>(dt).FirstOrDefault();

            AreaModel area = SQLHelper<AreaModel>.SqlToModel($"SELECT * FROM Area WHERE AreaCode = '{code}'");

            return area;
        }
    }
}
