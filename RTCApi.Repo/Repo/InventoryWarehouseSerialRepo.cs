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
    public class InventoryWarehouseSerialRepo : GenericRepo<InventoryWarehouseSerial>
    {
        public InventoryWarehouseSerialModel GetByIventoryIdAndSerial(int inventoryId, string serialNumber)
        {
            //InventoryWarehouseSerial inventory = GetAll().Where(x => x.InventoryID == inventoryId && x.SerialNumber == serialNumber).FirstOrDefault();

            //DataTable dt = TextUtils.Select($"SELECT * FROM InventoryWarehouseSerial WHERE InventoryID = {inventoryId} AND SerialNumber = '{serialNumber}'");
            //InventoryWarehouseSerialModel inventory = TextUtils.ConvertDataTable<InventoryWarehouseSerialModel>(dt).FirstOrDefault();
            
            InventoryWarehouseSerialModel inventory = SQLHelper<InventoryWarehouseSerialModel>.SqlToModel($"SELECT * FROM InventoryWarehouseSerial WHERE InventoryID = {inventoryId} AND SerialNumber = '{serialNumber}'");

            return inventory;
        }
    }
}
