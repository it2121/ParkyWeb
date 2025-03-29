using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;
using System.Net.Http.Headers;
using System.Text;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class


    {

        private readonly IHttpClientFactory _httpClientFactory;


        public Repository (IHttpClientFactory clientFactory)
        {


            _httpClientFactory = clientFactory;
        }

         async Task<bool> IRepository<T>.CreateAsync(string url, T objToCreate, string token ="")
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if(objToCreate != null)
            {

                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8,"application/json");

            }
            else { 

            return false;
            }

            var client = _httpClientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            }



            HttpResponseMessage response = await client.SendAsync(request);

            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {



                return true;
            }
            else
            {

                return false;
            }

        }

        async Task<bool> IRepository<T>.DeleteAsync(string url, int Id, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+Id);



            var client = _httpClientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            }

            HttpResponseMessage response = await client.SendAsync(request);


            if (response.StatusCode == System.Net.HttpStatusCode. NoContent)
            {



                return true;
            }
            else
            {

                return false;
            }


        }

        async Task<IEnumerable<T>> IRepository<T>.GetAllAsync(string url, string token="")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url );



            var client = _httpClientFactory.CreateClient();

            if (token!=null&& token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            }

            HttpResponseMessage response = await client.SendAsync(request);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {



               var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
           
            
            }
            else
            {

                return null;
            }
        }

        async Task<T> IRepository<T>.GetAsync(string url, int Id ,string token = "")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+Id);



            var client = _httpClientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            }

            HttpResponseMessage response = await client.SendAsync(request);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {



                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {

                return null;
            }
        }

        async Task<bool> IRepository<T>.UpdateAsync(string url, T objToUpdate,string token ="")
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objToUpdate != null)
            {

                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");

            }
            else
            {

                return false;
            }

            var client = _httpClientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            }
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {



                return true;
            }
            else
            {

                return false;
            }
        }
    }
}
