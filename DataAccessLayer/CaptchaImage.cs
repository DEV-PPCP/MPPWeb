using PPCP07302018.Models.Member;
using PPCP07302018.Models.Organization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Net.Http;

namespace PPCP07302018.DataAccessLayer
{
    public class CaptchaImage
    {

    }
    class ServiceCall<T>
    {
        static string ServiceUrl = System.Configuration.ConfigurationSettings.AppSettings["ServiceUrl"].ToString();
        public List<T> CallService(int TestID, string Methodname, ServiceData data)
        {
            List<T> List = new List<T>();
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(ServiceUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text");
               
                var urls = "Member/" + Methodname;
                var o = data;
                string T = JsonConvert.SerializeObject(o);
                var content = new StringContent(T, System.Text.Encoding.UTF8, "application/json");
                var responseTask = client.PostAsJsonAsync(urls, o);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = result.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var objects = Newtonsoft.Json.Linq.JArray.Parse(jsonString.Result);

                    var studentsa = JsonConvert.DeserializeObject<List<T>>(objects[0].ToString());
                    List<T> student111 = new List<T>();

                    student111.AddRange(studentsa);

                    List = student111;
                }
                else
                {
                    List = new List<T>();
                }
            }
            return List;
        }
        public List<T> CallServices(int TestID, string Methodname, ServiceData data)
        {
            List<T> List = new List<T>();
            try
            {

                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri(ServiceUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text");
                    var urls = "OrganizationServices/" + Methodname;
                    var o = data;
                    string T = JsonConvert.SerializeObject(o);
                    var content = new StringContent(T, System.Text.Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsJsonAsync(urls, o);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        var objects = Newtonsoft.Json.Linq.JArray.Parse(jsonString.Result);

                        var studentsa = JsonConvert.DeserializeObject<List<T>>(objects[0].ToString());
                        List<T> student111 = new List<T>();

                        student111.AddRange(studentsa);

                        List = student111;
                    }
                    else
                    {
                        List = new List<T>();
                    }
                }
            }catch(Exception ex)
            {
                return List;
            }
            return List;
        }
        public List<T> CallServiceTestLKP(int TestID, string Methodname, ServiceData data)
        {
            List<T> List = new List<T>();
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(ServiceUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text");
                var urls = "DefaultService/" + Methodname;
                var o = data;
                string T = JsonConvert.SerializeObject(o);
                var content = new StringContent(T, System.Text.Encoding.UTF8, "application/json");
                var responseTask = client.PostAsJsonAsync(urls, o);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = result.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    //if (!string.IsNullOrEmpty(jsonString.Result))
                    //{
                    //    System.Web.Script.Serialization.JavaScriptSerializer json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //    List<T> obj = json_serializer.Deserialize<List<T>>(jsonString.Result);                        
                    //}

                    var objects = Newtonsoft.Json.Linq.JArray.Parse(jsonString.Result);

                    var studentsa = JsonConvert.DeserializeObject<List<T>>(objects.ToString());
                    List<T> student111 = new List<T>();

                    student111.AddRange(studentsa);

                    List = student111;
                }
                else
                {
                    List = new List<T>();
                }
            }
            return List;
        }


       

       
        public List<T> CallServicePro(int TestID, string Methodname, ServiceData data)
        {
            List<T> List = new List<T>();
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(ServiceUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text");
                var urls = "Provider/" + Methodname;
                var o = data;
                string T = JsonConvert.SerializeObject(o);
                var content = new StringContent(T, System.Text.Encoding.UTF8, "application/json");
                var responseTask = client.PostAsJsonAsync(urls, o);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = result.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var objects = Newtonsoft.Json.Linq.JArray.Parse(jsonString.Result);

                    var studentsa = JsonConvert.DeserializeObject<List<T>>(objects[0].ToString());
                    List<T> student111 = new List<T>();

                    student111.AddRange(studentsa);

                    List = student111;
                }
                else
                {
                    List = new List<T>();
                }
            }
            return List;
        }
        public List<T> CallServicesAdmin(int TestID, string Methodname, ServiceData data)
        {
            List<T> List = new List<T>();
            try
            {

                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri(ServiceUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text");
                    var urls = "DefaultService/" + Methodname;
                    var o = data;
                    string T = JsonConvert.SerializeObject(o);
                    var content = new StringContent(T, System.Text.Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsJsonAsync(urls, o);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        var objects = Newtonsoft.Json.Linq.JArray.Parse(jsonString.Result);

                        var studentsa = JsonConvert.DeserializeObject<List<T>>(objects[0].ToString());
                        List<T> student111 = new List<T>();

                        student111.AddRange(studentsa);

                        List = student111;
                    }
                    else
                    {
                        List = new List<T>();
                    }
                }
            }
            catch (Exception ex)
            {
                return List;
            }
            return List;
        }
    }
}