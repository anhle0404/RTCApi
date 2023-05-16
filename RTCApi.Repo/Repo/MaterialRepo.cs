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
    public class MaterialRepo : GenericRepo<Material>
    {
        public MaterialModel GetMaterialByCode(string code)
        {
            //Material material = GetAll().Where(x => x.MaterialCode == code).FirstOrDefault();

            //DataTable dt = TextUtils.Select($"SELECT * FROM Material WHERE MaterialCode = '{code}'");
            //MaterialModel material = TextUtils.ConvertDataTable<MaterialModel>(dt).FirstOrDefault();

            MaterialModel material = SQLHelper<MaterialModel>.SqlToModel($"SELECT * FROM Material WHERE MaterialCode = '{code}'");
            return material;
        }
    }
}
