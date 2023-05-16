using ConnectSQL19.Model.Common;
using ConnectSQL19.Model.Context;
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
using static StockManagementCSPart.Model.DTO.InventoryWarehouseDTO;

namespace ConnectSQL19.Repo.Repo
{
    public class InventoryWarehouseRepo : GenericRepo<InventoryWarehouse>
    {
        InventoryWarehouseSerialRepo inventorySerialRepo = new InventoryWarehouseSerialRepo();
        //Get biên bản kiểm kho
        public InventoryWarehouseDTO GetInventoryWarehouse(string code)
        {
            InventoryWarehouseDTO inventory = new InventoryWarehouseDTO();
            List<MaterialInventoryWarehouse> list = new List<MaterialInventoryWarehouse>();
            
            DataSet ds = TextUtils.GetDataSetSP(StoreName.spGetInventoryWarehouseAPI, new string[] { "@Number" }, new object[] { code });

            DataTable dtMaster = ds.Tables[0];
            DataTable dtArea = ds.Tables[1];
            DataTable dtSerial = ds.Tables[2];

            if (dtMaster.Rows.Count > 0)
            {
                inventory.id = TextUtils.ToInt(dtMaster.Rows[0]["ID"]);
                inventory.code = TextUtils.ToString(dtMaster.Rows[0]["Number"]);
                //inventory.areaCode = TextUtils.ToString(dt.Rows[0]["AreaCode"]);
                //inventory.areaName = TextUtils.ToString(dt.Rows[0]["AreaName"]);
                inventory.date = TextUtils.ToDate(dtMaster.Rows[0]["DateCheck"]).Value.ToString("dd/MM/yyyy");
                inventory.purpose = TextUtils.ToString(dtMaster.Rows[0]["Purpose"]);


                //inventory.material = list;

                for (int i = 0; i < dtArea.Rows.Count; i++)
                {
                    MaterialInventoryWarehouse area = new MaterialInventoryWarehouse();

                    area.areaId = TextUtils.ToInt(dtArea.Rows[i]["AreaID"]);
                    area.areaCode = TextUtils.ToString(dtArea.Rows[i]["AreaCode"]);
                    area.areaName = TextUtils.ToString(dtArea.Rows[i]["AreaName"]);

                    area.materialCode = TextUtils.ToString(dtArea.Rows[i]["MaterialCode"]);
                    area.materialName = TextUtils.ToString(dtArea.Rows[i]["MaterialName"]);
                    area.typeMaterial = TextUtils.ToBoolean(dtArea.Rows[i]["TypeMaterial"]);
                    area.typeMaterialText = TextUtils.ToString(dtArea.Rows[i]["TypeMaterialText"]);
                    area.quantity = TextUtils.ToInt(dtArea.Rows[i]["Qty"]);
                    area.quantityReal = TextUtils.ToInt(dtArea.Rows[i]["QtyReal"]);
                    area.serialNumber = ListSerial(dtSerial, TextUtils.ToInt(dtArea.Rows[i]["MaterialID"]));

                    

                    list.Add(area);
                }

                inventory.material = list;
            }

            

            return inventory;
        }

        //Insert dữ liệu vào bảng InventoryWarehouseSerial
        public int Create(InfoUpdateInventoryWarehouse info, int inventoryWarehouseId, int materialId, int areaId, bool typeMaterial)
        {
            try
            {
                foreach (var item in info.serialCode)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //InventoryWarehouseSerialModel inventorySerial = new InventoryWarehouseSerialModel();
                        InventoryWarehouseSerialModel inventorySerial = inventorySerialRepo.GetByIventoryIdAndSerial(inventoryWarehouseId, item.Trim()); //Check đã tồn tại chưa

                        //inventorySerial = inventorySerial.ID <= 0 ? new InventoryWarehouseSerialModel() : inventorySerial;

                        inventorySerial.InventoryID = inventoryWarehouseId;
                        inventorySerial.MaterialID = materialId;
                        inventorySerial.AreaID = areaId;
                        inventorySerial.SerialNumber = item.Trim();
                        inventorySerial.Quantity = typeMaterial == true ? info.quantity : 1;

                        if (inventorySerial.ID > 0)
                        {
                            inventorySerial.UpdatedDate = DateTime.Now;
                            inventorySerial.UpdatedBy = info.loginName;
                        }
                        else
                        {
                            inventorySerial.CreatedDate = inventorySerial.UpdatedDate = DateTime.Now;
                            inventorySerial.CreatedBy = inventorySerial.UpdatedBy = info.loginName;
                        }

                        TextUtils.ExcuteProcedure("spInsertUpdateInventoryWarehouseSerial",
                        new string[] { "@ID", "@InventoryID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                        new object[] { inventorySerial.ID, inventorySerial.InventoryID, inventorySerial.MaterialID, inventorySerial.AreaID, inventorySerial.SerialNumber, inventorySerial.Quantity, inventorySerial.CreatedBy, inventorySerial.UpdatedDate, inventorySerial.UpdatedBy });


                        //if (typeMaterial) //Nếu là vật tư bé --> Cứ insert
                        //{
                        //    inventorySerial.InventoryID = inventoryWarehouseId;
                        //    inventorySerial.MaterialID = materialId;
                        //    inventorySerial.AreaID = areaId;
                        //    inventorySerial.SerialNumber = item.Trim();
                        //    inventorySerial.Quantity = typeMaterial == true ? info.quantity : 1;

                        //    inventorySerial.CreatedDate = inventorySerial.UpdatedDate = DateTime.Now;
                        //    inventorySerial.CreatedBy = inventorySerial.UpdatedBy = info.loginName;

                        //    //if (true) //NẾU CHỈ ĐỂ Ở 1 vị trí và 1 túi ->Check đã có hay chưa để update
                        //    //{
                        //    //    //Nếu chưa có thì insert
                        //    //    inventorySerial = inventorySerialRepo.GetByIventoryIdAndSerial(inventoryWarehouseId, item.Trim());
                        //    //}
                        //    //else
                        //    //{
                        //    //    //Nếu đã có thì update
                        //    //}

                        //    TextUtils.ExcuteProcedure("spInsertUpdateInventoryWarehouseSerial",
                        //       new string[] { "@ID", "@InventoryID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                        //       new object[] { inventorySerial.ID, inventorySerial.InventoryID, inventorySerial.MaterialID, inventorySerial.AreaID, inventorySerial.SerialNumber, inventorySerial.Quantity, inventorySerial.CreatedBy, inventorySerial.UpdatedDate, inventorySerial.UpdatedBy });

                        //}
                        //else //Nếu là vật tư lớn
                        //{
                        //    inventorySerial = inventorySerialRepo.GetByIventoryIdAndSerial(inventoryWarehouseId, item.Trim()); //Check đã tồn tại chưa
                        //    inventorySerial = inventorySerial == null ? new InventoryWarehouseSerialModel() : inventorySerial;

                        //    inventorySerial.InventoryID = inventoryWarehouseId;
                        //    inventorySerial.MaterialID = materialId;
                        //    inventorySerial.AreaID = areaId;
                        //    inventorySerial.SerialNumber = item.Trim();
                        //    inventorySerial.Quantity = typeMaterial == true ? info.quantity : 1;

                        //    if (inventorySerial.ID > 0)
                        //    {
                        //        inventorySerial.UpdatedDate = DateTime.Now;
                        //        inventorySerial.UpdatedBy = info.loginName;
                        //    }
                        //    else
                        //    {
                        //        inventorySerial.CreatedDate = inventorySerial.UpdatedDate = DateTime.Now;
                        //        inventorySerial.CreatedBy = inventorySerial.UpdatedBy = info.loginName;
                        //    }

                        //    TextUtils.ExcuteProcedure("spInsertUpdateInventoryWarehouseSerial",
                        //       new string[] { "@ID", "@InventoryID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                        //       new object[] { inventorySerial.ID, inventorySerial.InventoryID, inventorySerial.MaterialID, inventorySerial.AreaID, inventorySerial.SerialNumber, inventorySerial.Quantity, inventorySerial.CreatedBy, inventorySerial.UpdatedDate, inventorySerial.UpdatedBy });
                        //}
                    }


                    //InventoryWarehouseSerial inventorySerial = inventorySerialRepo.GetByIventoryIdAndSerial(inventoryWarehouseId, item.Trim());
                    //InventoryWarehouseSerialModel inventorySerial = inventorySerialRepo.GetByIventoryIdAndSerial(inventoryWarehouseId, item.Trim());

                    //inventorySerial = inventorySerial == null ? new InventoryWarehouseSerial() : inventorySerial;
                    //inventorySerial = inventorySerial == null ? new InventoryWarehouseSerialModel() : inventorySerial;

                    //inventorySerial.InventoryID = inventoryWarehouseId;
                    //inventorySerial.MaterialID = materialId;
                    //inventorySerial.AreaID = areaId;
                    //inventorySerial.SerialNumber = item.Trim();
                    //inventorySerial.Quantity = typeMaterial == true ? info.quantity : 1;

                    //if (inventorySerial.ID > 0)
                    //{
                    //    inventorySerial.UpdatedDate = DateTime.Now;
                    //    inventorySerial.UpdatedBy = info.loginName;
                    //    //inventorySerialRepo.Update(inventorySerial);

                    //    TextUtils.ExcuteProcedure("spInsertUpdateInventoryWarehouseSerial",
                    //    new string[] { "@ID", "@InventoryID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                    //    new object[] { inventorySerial.ID, inventorySerial.InventoryID, inventorySerial.MaterialID, inventorySerial.AreaID, inventorySerial.SerialNumber, inventorySerial.Quantity, inventorySerial.CreatedBy, inventorySerial.UpdatedDate, inventorySerial.UpdatedBy });

                    //}
                    //else
                    //{
                    //    inventorySerial.CreatedDate = inventorySerial.UpdatedDate = DateTime.Now;
                    //    inventorySerial.CreatedBy = inventorySerial.UpdatedBy = info.loginName;
                    //    //inventorySerialRepo.Create(inventorySerial);

                    //    TextUtils.ExcuteProcedure("spInsertUpdateInventoryWarehouseSerial",
                    //    new string[] { "@ID", "@InventoryID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                    //    new object[] { inventorySerial.ID, inventorySerial.InventoryID, inventorySerial.MaterialID, inventorySerial.AreaID, inventorySerial.SerialNumber, inventorySerial.Quantity,inventorySerial.CreatedBy, inventorySerial.UpdatedDate, inventorySerial.UpdatedBy });

                    //}
                }

                return 1;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
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
