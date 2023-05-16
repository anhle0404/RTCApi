using ConnectSQL19.Model.Common;
using ConnectSQL19.Model.Context;
using ConnectSQL19.Repo.GenericRepo;
using StockManagementCSPart.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConnectSQL19.Model.Common.VariableHelper;
using static StockManagementCSPart.Model.DTO.PackingListMaterialDTO;

namespace ConnectSQL19.Repo.Repo
{
    public class PackingListRepo : GenericRepo<PackingList>
    {
        public List<PackingListMaterialDTO> ListPackingList(string code)
        {
            List<PackingListMaterialDTO> list = new List<PackingListMaterialDTO>();

            DataSet dsPackingList = TextUtils.GetDataSetSP(StoreName.spGetPakingListAPI,
                            new string[] { "@Code" },
                            new object[] { code });

            DataTable dtMaterial = dsPackingList.Tables[0];
            DataTable dtMaterialDetail = dsPackingList.Tables[1];

            if (dtMaterial.Rows.Count > 0)
            {
                for (int i = 0; i < dtMaterial.Rows.Count; i++)
                {
                    PackingListMaterialDTO packingList = new PackingListMaterialDTO();
                    packingList.orderNumber = TextUtils.ToInt(dtMaterial.Rows[i]["STT"]);
                    packingList.materialID = TextUtils.ToInt(dtMaterial.Rows[i]["MaterialID"]);
                    packingList.materialCode = TextUtils.ToString(dtMaterial.Rows[i]["MaterialCode"]);
                    packingList.materialName = TextUtils.ToString(dtMaterial.Rows[i]["MaterialName"]);
                    packingList.typeMaterial = string.IsNullOrEmpty(packingList.materialCode) ? false : TextUtils.ToBoolean(dtMaterial.Rows[i]["TypeMaterial"]);
                    packingList.typeMaterialText = TextUtils.ToString(dtMaterial.Rows[i]["TypeMaterialText"]);
                    packingList.quantityRequest = TextUtils.ToInt(dtMaterial.Rows[i]["TotalQuantity"]);
                    packingList.totalQuantityExport = TextUtils.ToInt(dtMaterial.Rows[i]["TotalQuantityExport"]);
                    packingList.detail = ListDetails(dtMaterialDetail, packingList.materialID);
                    packingList.serialNumber = ListSerial(dsPackingList.Tables[2], packingList.materialID);

                    list.Add(packingList);
                }
            }

            return list;
        }

        private List<MaterialPackListDetail> ListDetails(DataTable dataTable, int materialID)
        {
            List<MaterialPackListDetail> listDetails = new List<MaterialPackListDetail>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (materialID == TextUtils.ToInt(dataTable.Rows[i]["MaterialID"]))
                {
                    MaterialPackListDetail detail = new MaterialPackListDetail();

                    detail.areaCode = TextUtils.ToString(dataTable.Rows[i]["AreaCode"]);
                    detail.areaName = TextUtils.ToString(dataTable.Rows[i]["AreaName"]);
                    detail.quantityInArea = TextUtils.ToInt(dataTable.Rows[i]["Qty"]);
                    detail.quantityExported = TextUtils.ToInt(dataTable.Rows[i]["QuantityExport"]);

                    listDetails.Add(detail);
                }
            }

            return listDetails;
        }

        public List<string> ListSerial(DataTable dt, int materialId)
        {
            List<string> listSerial = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (materialId == TextUtils.ToInt(dt.Rows[i]["MaterialID"]))
                {
                    listSerial.Add(TextUtils.ToString(dt.Rows[i]["SerialNumber"]));
                }
            }

            return listSerial;
        }
    }
}
