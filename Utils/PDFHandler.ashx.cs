using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PPCP07302018.Utils
{
    /// <summary>
    /// Summary description for PDFHandler
    /// </summary>
    public class PDFHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //Added by sridevi ,if we passed one parameters File path 

                context.Response.ContentType = "application/pdf";

                // Reading the image data from the given Physicial path
                Stream strm = new MemoryStream(File.ReadAllBytes(context.Request.QueryString["FilePath"]));
                var buffer = new byte[4096];
                int byteSeq = strm.Read(buffer, 0, 4096);
                while (byteSeq > 0)
                {
                    context.Response.OutputStream.Write(buffer, 0, byteSeq);
                    byteSeq = strm.Read(buffer, 0, 4096);
                }

            }
            catch (Exception ex)
            { }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}