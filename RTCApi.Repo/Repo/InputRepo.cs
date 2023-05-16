using ConnectSQL19.Model.Common;
using ConnectSQL19.Model.Context;
using ConnectSQL19.Model.DTO;
using ConnectSQL19.Model.Entities;
using ConnectSQL19.Repo.GenericRepo;
using StockManagementCSPart.Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConnectSQL19.Model.Common.VariableHelper;

namespace ConnectSQL19.Repo.Repo
{
    public class InputRepo : GenericRepo<ImportWarehouse>
    {
        public ImportWarehouseModel GetByCode(string code)
        {
            DataTable dt = TextUtils.GetDataSetSP(StoreName.spGetInputAPI, new string[] { "@Code" }, new object[] { code }).Tables[3];
            ImportWarehouseModel input = TextUtils.ConvertDataTable<ImportWarehouseModel>(dt).FirstOrDefault();

            //ImportWarehouse input = GetAll().Where(x => x.Code == code).FirstOrDefault();
            return input;
        }

        public Tuple<List<InputMaterialDTO>, List<AreaDTO>, List<ListSerialInputAreaDTO>> GetListInput(string code)
        {
            List<InputMaterialDTO> list = new List<InputMaterialDTO>();
            List<AreaDTO> listAreaMaterial = new List<AreaDTO>();
            List<ListSerialInputAreaDTO> listSerial = new List<ListSerialInputAreaDTO>();

            DataSet ds = TextUtils.GetDataSetSP(StoreName.spGetInputAPI, new string[] { "@Code" }, new object[] { code });

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    InputMaterialDTO input = new InputMaterialDTO();
                    input.orderNumber = TextUtils.ToInt(ds.Tables[0].Rows[i]["STT"]);
                    input.materialID = TextUtils.ToInt(ds.Tables[0].Rows[i]["MaterialID"]);
                    input.materialCode = TextUtils.ToString(ds.Tables[0].Rows[i]["MaterialCode"]);
                    input.materialName = TextUtils.ToString(ds.Tables[0].Rows[i]["MaterialName"]);
                    input.typeMaterial = TextUtils.ToBoolean(ds.Tables[0].Rows[i]["TypeMaterial"]);
                    input.typeMaterialText = TextUtils.ToString(ds.Tables[0].Rows[i]["TypeMaterialText"]);
                    input.quantityRequest = TextUtils.ToInt(ds.Tables[0].Rows[i]["QuantityRequest"]);
                    input.quantityInput = TextUtils.ToInt(ds.Tables[0].Rows[i]["QuantityInput"]);
                    input.serialNumber = ListSerial(ds.Tables[2], input.materialID);
                    
                    list.Add(input);
                }
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    AreaDTO areaDTO = new AreaDTO();
                    areaDTO.materialCode = TextUtils.ToString(ds.Tables[1].Rows[i]["MaterialCode"]);
                    areaDTO.areaCode = TextUtils.ToString(ds.Tables[1].Rows[i]["AreaCode"]);
                    areaDTO.areaName = TextUtils.ToString(ds.Tables[1].Rows[i]["AreaName"]);
                    areaDTO.totalQuantity = TextUtils.ToInt(ds.Tables[1].Rows[i]["TotalQuantity"]);

                    listAreaMaterial.Add(areaDTO);
                }
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ListSerialInputAreaDTO serial = new ListSerialInputAreaDTO();
                    serial.materialCode = TextUtils.ToString(ds.Tables[2].Rows[i]["MaterialCode"]);
                    serial.areaCode = TextUtils.ToString(ds.Tables[2].Rows[i]["AreaCode"]);
                    serial.areaName = TextUtils.ToString(ds.Tables[2].Rows[i]["AreaName"]);
                    serial.serialNumber = TextUtils.ToString(ds.Tables[2].Rows[i]["SerialNumber"]);

                    listSerial.Add(serial);
                }
            }

            return new Tuple<List<InputMaterialDTO>, List<AreaDTO>, List<ListSerialInputAreaDTO>>(list, listAreaMaterial, listSerial);
        }


        public List<string> CheckExist(string serialNumber)
        {
            List<string> list = new List<string>();
            DataTable dt = TextUtils.GetDataTableSP(StoreName.spCheckExistUpdateInputAPI,
                            new string[] { "@SerialNumber" },
                            new object[] { serialNumber });

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
