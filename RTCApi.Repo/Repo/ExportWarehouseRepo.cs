using ConnectSQL19.Model.Common;
using ConnectSQL19.Model.Context;
using ConnectSQL19.Model.Entities;
using ConnectSQL19.Repo.GenericRepo;
using ConnectSQL19.Repo.Repo;
using StockManagementCSPart.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConnectSQL19.Model.Common.VariableHelper;

namespace ConnectSQL19.Repo
{
    public class ExportWarehouseRepo: GenericRepo<RequestExportWarehouse>
    {
        DataSet dataSet = new DataSet();

        ImageRepo imageRepo = new ImageRepo();

        /// <summary>
        /// Get phiếu yêu cầu xuất kho
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public List<ExportWarehouseDTO> GetExportWareHouse(string resquestCode)
        {
            List<ExportWarehouseDTO> list = new List<ExportWarehouseDTO>();
            dataSet = TextUtils.GetDataSetSP(StoreName.spGetMaterialExportWareHouseAPI, new string[] { "@RequestCode" }, new object[] { resquestCode });
            DataTable dt = dataSet.Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ExportWarehouseDTO export = new ExportWarehouseDTO();
                    export.MaterialCode = TextUtils.ToString(dt.Rows[i]["MaterialCode"]);
                    export.MaterialName = TextUtils.ToString(dt.Rows[i]["MaterialName"]);

                    list.Add(export);
                }
            }

            return list;
        }

        public List<ExportWarehouseDetailDTO> GetMaterialDetailExportWarehouse(string resquestCode)
        {
            List<ExportWarehouseDetailDTO> list = new List<ExportWarehouseDetailDTO>();
            DataTable dt = TextUtils.GetDataSetSP(StoreName.spGetMaterialExportWareHouseAPI, new string[] { "@RequestCode" }, new object[] { resquestCode }).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ExportWarehouseDetailDTO export = new ExportWarehouseDetailDTO();
                    export.materialId = TextUtils.ToInt(dt.Rows[i]["MaterialID"]);
                    export.materialCode = TextUtils.ToString(dt.Rows[i]["MaterialCode"]);
                    export.materialName = TextUtils.ToString(dt.Rows[i]["MaterialName"]);
                    export.typeMaterial = TextUtils.ToBoolean(dt.Rows[i]["TypeMaterial"]);
                    export.typeMaterialText = TextUtils.ToString(dt.Rows[i]["TypeMaterialText"]);
                    export.quantityRequest = TextUtils.ToInt(dt.Rows[i]["QuantityRequest"]);
                    export.quantityGet = TextUtils.ToInt(dt.Rows[i]["QuantityGet"]);
                    export.stt = TextUtils.ToInt(dt.Rows[i]["STT"]);
                    export.orderNumber = TextUtils.ToInt(dt.Rows[i]["OrderNumber"]);

                    export.serialCode = export.typeMaterial == false ? ListMaterialSirial(export.stt, (export.quantityRequest - export.quantityGet), export.materialCode) : new List<string>() { export.materialCode };
                    export.materialImage = ListMaterialImage(TextUtils.ToInt(dt.Rows[i]["MaterialID"]));
                    list.Add(export);
                }
            }


            return list;
        }

        public List<string> ListMaterialSirial(int stt, int quantity, string materialCode)
        {
            List<string> list = new List<string>();
            for (int i = (stt + 1); i <= (quantity + stt); i++)
            {
                string number = i < 10 ? $"0{i}" : $"{i}";
                string serial = $"{materialCode}@{number}";

                list.Add(serial);
            }

            return list;
        }

        public List<string> CheckExist(string requestCode, string serialNumber)
        {
            List<string> list = new List<string>();
            DataTable dt = TextUtils.GetDataTableSP(StoreName.spCheckExistUpdateExportWarehouse,
                            new string[] { "@RequestCode", "@SerialNumber" },
                            new object[] { requestCode, serialNumber });

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string serial = TextUtils.ToString(dt.Rows[i]["SerialNumber"]);
                    list.Add(serial);
                }
            }

            return list;
        }

        private List<MaterialImageDTO> ListMaterialImage(int materialId)
        {
            //List<Image> listImage = imageRepo.GetAll().Where(x => x.MaterialID == materialId).ToList();
            DataTable dt = TextUtils.Select($"SELECT * FROM Image WHERE MaterialID = {materialId}");
            List<ImageModel> listImage = TextUtils.ConvertDataTable<ImageModel>(dt);

            List<MaterialImageDTO> list = new List<MaterialImageDTO>();

            foreach (var item in listImage)
            {
                if (!string.IsNullOrEmpty(item.ImageName))
                {
                    MaterialImageDTO image = new MaterialImageDTO();
                    image.ImageName = item.ImageName;
                    image.ImageLink = Config._baseImageUrl + item.ImageName.Replace(" ","%20");

                    list.Add(image);
                }
            }

            return list;
        }
    }
}
