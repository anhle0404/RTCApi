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
    public class MaterialSerialRepo : GenericRepo<RequestExportWarehouseSerial>
    {
        public object Create(List<string> list, int exportWarehouseId, int materialId, string loginName, int quantity, bool typeMaterial)
        {
            foreach (var item in list)
            {
                //RequestExportWarehouseSerial materialSerial = new RequestExportWarehouseSerial();
                RequestExportWarehouseSerialModel materialSerial = new RequestExportWarehouseSerialModel();
                if (!string.IsNullOrEmpty(item))
                {
                    //materialSerial = GetAll().Where(x => x.SerialNumber == item).FirstOrDefault();

                    //
                    //
                    //Create(materialSerial);

                    if (typeMaterial)
                    {
                        //materialSerial = materialSerial == null ? new RequestExportWarehouseSerialModel() : materialSerial;
                        materialSerial.ExportWarehouseID = exportWarehouseId;
                        materialSerial.MaterialID = materialId;
                        materialSerial.STT = item.Split('@').Length < 2 ? 0 : TextUtils.ToInt(item.Split('@')[1]);
                        materialSerial.SerialNumber = item;
                        materialSerial.Quantity = typeMaterial == true ? quantity : 1;

                        materialSerial.CreatedDate = materialSerial.UpdatedDate = DateTime.Now;
                        materialSerial.CreatedBy = materialSerial.UpdatedBy = loginName;

                        //TextUtils.ExcuteProcedure("spInsertRequestWarehouse",
                        //    new string[] { "@ID", "@STT", "@ExportWarehouseID", "@MaterialID", "@Quantity", "@SerialNumber", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                        //    new object[] { materialSerial.ID, materialSerial.STT, materialSerial.ExportWarehouseID, materialSerial.MaterialID, materialSerial.Quantity, materialSerial.SerialNumber, materialSerial.CreatedBy, materialSerial.UpdatedDate, materialSerial.UpdatedBy });



                        string sql = $"EXEC dbo.spInsertRequestWarehouse {materialSerial.STT},{materialSerial.ExportWarehouseID},{materialSerial.MaterialID}," +
                            $"{ materialSerial.Quantity},'{ materialSerial.SerialNumber}',{materialSerial.ID}, N'{materialSerial.CreatedBy}', '{ materialSerial.UpdatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}',N'{materialSerial.UpdatedBy}'";

                        string value = TextUtils.ToString(TextUtils.ExcuteScalar(sql));

                        if (!string.IsNullOrEmpty(value))
                        {
                            if (materialSerial.Quantity > TextUtils.ToInt(value.Split(';')[0]))
                            {
                                return value.Split(';')[1];
                            }
                        }
                    }
                    else
                    {
                        DataTable dt = TextUtils.Select($"SELECT * FROM RequestExportWarehouseSerial WHERE SerialNumber = '{item}'");
                        materialSerial = TextUtils.ConvertDataTable<RequestExportWarehouseSerialModel>(dt).FirstOrDefault();

                        materialSerial = materialSerial == null ? new RequestExportWarehouseSerialModel() : materialSerial;
                        materialSerial.ExportWarehouseID = exportWarehouseId;
                        materialSerial.MaterialID = materialId;
                        materialSerial.STT = item.Split('@').Length < 2 ? 0 : TextUtils.ToInt(item.Split('@')[1]);
                        materialSerial.SerialNumber = item;
                        materialSerial.Quantity = typeMaterial == true ? quantity : 1;

                        if (materialSerial.ID > 0)
                        {
                            materialSerial.UpdatedDate = DateTime.Now;
                            materialSerial.UpdatedBy = loginName;
                        }
                        else
                        {
                            materialSerial.CreatedDate = materialSerial.UpdatedDate = DateTime.Now;
                            materialSerial.CreatedBy = materialSerial.UpdatedBy = loginName;
                        }

                        TextUtils.ExcuteProcedure("spInsertRequestWarehouse",
                            new string[] { "@ID", "@STT", "@ExportWarehouseID", "@MaterialID", "@Quantity", "@SerialNumber", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                            new object[] { materialSerial.ID, materialSerial.STT, materialSerial.ExportWarehouseID, materialSerial.MaterialID, materialSerial.Quantity, materialSerial.SerialNumber, materialSerial.CreatedBy, materialSerial.UpdatedDate, materialSerial.UpdatedBy });


                        //string sql = $"EXEC dbo.spInsertRequestWarehouse {materialSerial.STT},{materialSerial.ExportWarehouseID},{materialSerial.MaterialID}," +
                        //    $"{ materialSerial.Quantity},'{ materialSerial.SerialNumber}',{materialSerial.ID}, N'{materialSerial.CreatedBy}', '{ materialSerial.UpdatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}',N'{materialSerial.UpdatedBy}'";

                        //var value = TextUtils.ExcuteScalar(sql);

                    }
                }

                //materialSerial = materialSerial == null ? new RequestExportWarehouseSerial() : materialSerial;
                //materialSerial = materialSerial == null ? new RequestExportWarehouseSerialModel() : materialSerial;
                //materialSerial.ExportWarehouseID = exportWarehouseId;
                //materialSerial.MaterialID = materialId;
                //materialSerial.STT = item.Split('@').Length < 2 ? 0 : TextUtils.ToInt(item.Split('@')[1]);
                //materialSerial.SerialNumber = item;
                //materialSerial.Quantity = typeMaterial == true ? quantity : 1;
                //materialSerial.CreatedDate = materialSerial.UpdatedDate = DateTime.Now;
                //materialSerial.CreatedBy = materialSerial.UpdatedBy = loginName;

                //if (materialSerial.ID > 0)
                //{
                //    materialSerial.UpdatedBy = loginName;
                //    materialSerial.UpdatedDate = DateTime.Now;

                //    TextUtils.ExcuteProcedure("spInsertRequestWarehouse",
                //        new string[] { "@ID","@STT", "@ExportWarehouseID", "@MaterialID", "@Quantity", "@SerialNumber", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                //        new object[] { materialSerial.ID, materialSerial.STT, materialSerial.ExportWarehouseID, materialSerial.MaterialID, materialSerial.Quantity, materialSerial.SerialNumber, materialSerial.CreatedBy, materialSerial.UpdatedDate, materialSerial.UpdatedBy });
                //    //Update(materialSerial);
                //}
                //else
                //{
                //    materialSerial.CreatedDate = materialSerial.UpdatedDate = DateTime.Now;
                //    materialSerial.CreatedBy = materialSerial.UpdatedBy = loginName;

                //    TextUtils.ExcuteProcedure("spInsertRequestWarehouse",
                //        new string[] { "@ID", "@STT", "@ExportWarehouseID", "@MaterialID", "@Quantity", "@SerialNumber", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                //        new object[] { materialSerial.ID, materialSerial.STT, materialSerial.ExportWarehouseID, materialSerial.MaterialID, materialSerial.Quantity, materialSerial.SerialNumber, materialSerial.CreatedBy, materialSerial.UpdatedDate, materialSerial.UpdatedBy });
                //    //Create(materialSerial);
                //}

            }

            return 1;
        }
    }
}
