using RTCApi.Model.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RTCApi.Controllers
{
    [RoutePrefix("api/Home")]
    public class DefaultController : ApiController
    {

        [HttpGet]
        [Route("getall")]
        public IHttpActionResult GetAll()
        {
            try
            {
                List<string> listConnection = new List<string>() { "ư ư","ikuiku" };

                return Ok(new
                {
                    status = 1,
                    message = "Upload thành công!",
                    data = listConnection
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getbyid")]
        public IHttpActionResult GetByID(int id)
        {
            try
            {
                List<string> listConnection = new List<string>() { "ư ư", "ikuiku" };

                return Ok(new
                {
                    status = id,
                    message = "Upload thành công!",
                    data = listConnection
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("uploadfiledemo")]
        public async Task<IHttpActionResult> UploadFile([FromBody] FileUploadInfo info)
        {
            try
            {
                //string path = @"E:\project\12. ScadaPana - 0905223 ver 1.2\BMS\bin\x86\Debug\Image\";
                string path = @"D:\Image\";
                //string path = @"\\192.168.1.2\ftp\Upload\FileDemo\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] imageBytes = Convert.FromBase64String(info.content);

                // Convert byte[] to Image
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {

                    ms.Flush();

                    string filePath = path + info.name + ".jpg";

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ms.CopyToAsync(stream);
                    }
                }

                return Ok(new
                {
                    status = 1,
                    message = "Upload thành công!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}