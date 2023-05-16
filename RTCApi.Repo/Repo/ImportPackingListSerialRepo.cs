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

namespace ConnectSQL19.Repo.Repo
{
    public class ImportPackingListSerialRepo : GenericRepo<ImportPackingListSerial>
    {
        MaterialRepo materialRepo = new MaterialRepo();

        public ImportPackingListSerialModel GetByCode(string code)
        {
            //ImportPackingListSerial materialSerialInput = GetAll().Where(x => x.SerialNumber == code).FirstOrDefault();

            DataTable dt = TextUtils.Select($"SELECT * FROM ImportPackingListSerial WHERE SerialNumber = '{code}'");
            ImportPackingListSerialModel materialSerialInput = TextUtils.ConvertDataTable<ImportPackingListSerialModel>(dt).FirstOrDefault();
            return materialSerialInput;
        }

        public object Create(InfoUpdateInput info, int inputId, int areaId, int materialId)
        {
            try
            {
                DataTable dt = TextUtils.Select($"SELECT * FROM Material WHERE ID = {materialId}");
                //MaterialModel material = TextUtils.ConvertDataTable<MaterialModel>(dt).FirstOrDefault();
                MaterialModel material = SQLHelper<MaterialModel>.SqlToModel($"SELECT * FROM Material WHERE ID = {materialId}");

                foreach (var item in info.serialCode)
                {
                   
                    ImportPackingListSerialModel serialInput = new ImportPackingListSerialModel();
                    if (!string.IsNullOrEmpty(item.Trim()))
                    {

                        if (material.TypeMaterial == true)
                        {
                            serialInput.ImportWarehouseID = inputId;
                            serialInput.MaterialID = materialId;
                            serialInput.STT = item.Split('@').Length < 2 ? 0 : TextUtils.ToInt(item.Split('@')[1]);
                            serialInput.SerialNumber = item.Trim();
                            serialInput.QuantityOld = material.TypeMaterial == false ? 0 : TextUtils.ToInt(serialInput.Quantity);
                            serialInput.Quantity = material.TypeMaterial == false ? 1 : info.quantity;
                            serialInput.Status = 1;
                            serialInput.PackingListID = 0;
                            serialInput.QuantityExport = serialInput.QuantityExportOld = 0;

                            serialInput.AreaID = areaId;
                            serialInput.CreatedDate = DateTime.Now;
                            serialInput.CreatedBy = info.loginName;
                            serialInput.UpdatedDate = DateTime.Now;
                            serialInput.UpdatedBy = info.loginName;

                            string sql = $"EXEC dbo.spInsertUpdateImportPackingListSerial @ID = {serialInput.ID}, @STT = {serialInput.STT},@ImportWarehouseID = {serialInput.ImportWarehouseID}, " +
                                $"@PackingListID = {serialInput.PackingListID}, @MaterialID = {serialInput.MaterialID}, @AreaID = {serialInput.AreaID}," +
                                $"@SerialNumber = '{serialInput.SerialNumber.Trim()}', @Quantity = {serialInput.Quantity}, @QuantityOld = {serialInput.QuantityOld}," +
                                $"@AreaIDPackingList = 0, @QuantityExport = {serialInput.QuantityExport}, @QuantityExportOld = {serialInput.QuantityExportOld}, @Status = {serialInput.Status}, " +
                                $"@CreatedBy = N'{serialInput.CreatedBy}', @UpdatedDate = '{serialInput.UpdatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}',@UpdatedBy = N'{serialInput.UpdatedBy}'";

                            string value = TextUtils.ToString(TextUtils.ExcuteScalar(sql));

                            if (!string.IsNullOrEmpty(value))
                            {
                                if (serialInput.Quantity > TextUtils.ToInt(value.Split(';')[0]))
                                {
                                    return value.Split(';')[1];
                                }
                            }

                            //TextUtils.ExcuteProcedure("spInsertUpdateImportPackingListSerial",
                            //    new string[] { "@ID", "@STT", "@ImportWarehouseID", "@PackingListID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@QuantityOld", "@QuantityExport", "@QuantityExportOld", "@Status", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                            //    new object[] { serialInput.ID, serialInput.STT, serialInput.ImportWarehouseID, serialInput.PackingListID, serialInput.MaterialID, serialInput.AreaID, serialInput.SerialNumber, serialInput.Quantity, serialInput.QuantityOld, serialInput.QuantityExport, serialInput.QuantityExportOld, serialInput.Status, serialInput.CreatedBy, serialInput.UpdatedDate, serialInput.UpdatedBy });
                        }
                        else
                        {
                            serialInput = GetByCode(item.Trim()) == null ? new ImportPackingListSerialModel(): GetByCode(item.Trim());
                          

                            serialInput.ImportWarehouseID = inputId;
                            serialInput.MaterialID = materialId;
                            serialInput.STT = item.Split('@').Length < 2 ? 0 : TextUtils.ToInt(item.Split('@')[1]);
                            serialInput.SerialNumber = item.Trim();
                            serialInput.QuantityOld = material.TypeMaterial == false ? 0 : TextUtils.ToInt(serialInput.Quantity);
                            serialInput.Quantity = material.TypeMaterial == false ? 1 : info.quantity;
                            serialInput.Status = 1;
                            serialInput.PackingListID = 0;
                            serialInput.QuantityExport = serialInput.QuantityExportOld = 0;
                            serialInput.AreaID = areaId;
                            serialInput.UpdatedDate = DateTime.Now;
                            serialInput.UpdatedBy = info.loginName;

                            if (serialInput.ID > 0)
                            {
                                serialInput.UpdatedDate = DateTime.Now;
                                serialInput.UpdatedBy = info.loginName;
                            }
                            else
                            {
                                serialInput.CreatedDate = serialInput.UpdatedDate = DateTime.Now;
                                serialInput.CreatedBy = serialInput.UpdatedBy = info.loginName;
                            }

                            TextUtils.ExcuteProcedure("spInsertUpdateImportPackingListSerial",
                                new string[] { "@ID", "@STT", "@ImportWarehouseID", "@PackingListID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@QuantityOld", "@QuantityExport", "@QuantityExportOld", "@Status", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                                new object[] { serialInput.ID, serialInput.STT, serialInput.ImportWarehouseID, serialInput.PackingListID, serialInput.MaterialID, serialInput.AreaID, serialInput.SerialNumber, serialInput.Quantity, serialInput.QuantityOld, serialInput.QuantityExport, serialInput.QuantityExportOld, serialInput.Status, serialInput.CreatedBy, serialInput.UpdatedDate, serialInput.UpdatedBy });

                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public object Update(List<string> list, int packinglistId, bool typeMaterial, int quantity, string loginName,int areaId)
        {
            List<string> listCode = new List<string>();
            try
            {
                
                //DataTable dt = TextUtils.Select($"SELECT * FROM PackingList WHERE ID = {packinglistId}");
                //PackingListModel packing = TextUtils.ConvertDataTable<PackingListModel>(dt).FirstOrDefault();

                foreach (var item in list)
                {
                    //ImportPackingListSerial materialSerial = GetByCode(item.Trim());
                    //ImportPackingListSerialModel materialSerial = GetByCode(item.Trim());

                    //Tìm xem đã có qrcode chưa
                    ImportPackingListSerialModel materialSerial = GetByCode(item.Trim());
                    if (materialSerial == null)
                    {
                        listCode.Add(item.Trim());
                    }

                    
                    if (typeMaterial)
                    {
                        ImportPackingListSerialModel materialSmall = new ImportPackingListSerialModel();

                        materialSmall.ImportWarehouseID = materialSerial.ImportWarehouseID;
                        materialSmall.PackingListID = packinglistId;
                        materialSmall.MaterialID = materialSerial.MaterialID;
                        materialSmall.AreaID = areaId;
                        materialSmall.SerialNumber = item;
                        materialSmall.QuantityExport = quantity;
                        materialSmall.Status = 2;
                        materialSmall.CreatedBy = materialSmall.UpdatedBy = loginName;
                        materialSmall.CreatedDate = materialSmall.UpdatedDate = DateTime.Now;
                        materialSmall.Quantity = materialSerial.Quantity;
                        materialSmall.QuantityOld = materialSerial.QuantityOld;
                        materialSmall.STT = item.Split('@').Length < 2 ? 0 : TextUtils.ToInt(item.Split('@')[1]);
                        materialSmall.QuantityExportOld = TextUtils.ToInt(materialSmall.QuantityExport);


                        string sql = $"EXEC dbo.spInsertUpdateImportPackingListSerial @ID = {materialSmall.ID}, @STT = {materialSmall.STT},@ImportWarehouseID = {materialSmall.ImportWarehouseID}, " +
                                $"@PackingListID = {materialSmall.PackingListID}, @MaterialID = {materialSmall.MaterialID}, @AreaID = {materialSmall.AreaID}," +
                                $"@SerialNumber = '{materialSmall.SerialNumber.Trim()}', @Quantity = {materialSmall.Quantity}, @QuantityOld = {materialSmall.QuantityOld}," +
                                $"@AreaIDPackingList = 0, @QuantityExport = {materialSmall.QuantityExport}, @QuantityExportOld = {materialSmall.QuantityExportOld}, @Status = {materialSmall.Status}, " +
                                $"@CreatedBy = N'{materialSmall.CreatedBy}', @UpdatedDate = '{materialSmall.UpdatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}',@UpdatedBy = N'{materialSmall.UpdatedBy}'";

                        string value = TextUtils.ToString(TextUtils.ExcuteScalar(sql));

                        if (!string.IsNullOrEmpty(value))
                        {
                            if (materialSmall.Quantity > TextUtils.ToInt(value.Split(';')[0]))
                            {
                                return value.Split(';')[1];
                            }
                        }

                        //TextUtils.ExcuteProcedure("spInsertUpdateImportPackingListSerial",
                        //new string[] { "@ID", "@STT", "@ImportWarehouseID", "@PackingListID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@QuantityOld", "@AreaIDPackingList", "@QuantityExport", "@QuantityExportOld", "@Status", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                        //new object[] { materialSmall.ID, 0, materialSmall.ImportWarehouseID, materialSmall.PackingListID, materialSmall.MaterialID, materialSmall.AreaID, materialSmall.SerialNumber, materialSmall.Quantity, materialSmall.QuantityOld, materialSmall.AreaIDPackingList, materialSmall.QuantityExport, 0, materialSmall.Status, materialSmall.CreatedBy, materialSmall.UpdatedDate, materialSmall.UpdatedBy });

                    }
                    else
                    {
                        materialSerial.PackingListID = packinglistId;
                        materialSerial.QuantityExportOld = typeMaterial == false ? 0 : materialSerial.QuantityExport;
                        materialSerial.QuantityExport = typeMaterial == false ? 1 : quantity;
                        materialSerial.Status = 2;
                        materialSerial.UpdatedDate = DateTime.Now;
                        materialSerial.UpdatedBy = loginName;

                        TextUtils.ExcuteProcedure("spInsertUpdateImportPackingListSerial",
                            new string[] { "@ID", "@STT", "@ImportWarehouseID", "@PackingListID", "@MaterialID", "@AreaID", "@SerialNumber", "@Quantity", "@QuantityOld", "@AreaIDPackingList", "@QuantityExport", "@QuantityExportOld", "@Status", "@CreatedBy", "@UpdatedDate", "@UpdatedBy" },
                            new object[] { materialSerial.ID, materialSerial.STT, materialSerial.ImportWarehouseID, materialSerial.PackingListID, materialSerial.MaterialID, materialSerial.AreaID, materialSerial.SerialNumber, materialSerial.Quantity, materialSerial.QuantityOld, materialSerial.AreaIDPackingList, materialSerial.QuantityExport, materialSerial.QuantityExportOld, materialSerial.Status, materialSerial.CreatedBy, materialSerial.UpdatedDate, materialSerial.UpdatedBy });

                    }

                }

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
