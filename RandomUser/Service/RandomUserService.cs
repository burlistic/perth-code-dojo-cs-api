using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Service
{
    public class RandomUserService
    {
        private readonly List<User> _users;

        public RandomUserService()
        {
            _users = new List<User>();
        }

        public List<User> GetRandomUsersAsync(bool bulkLoad, int count)
        {
            if (bulkLoad)
            {
                GetUserAsync(true, count).Wait();
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    GetUserAsync(false, count).Wait();
                }
            }
            return _users;
        }

        private async Task GetUserAsync(bool bulkLoad, int count = 0)
        {
            using (var client = new HttpClient())
            {
                
                client.BaseAddress = new Uri("http://api.randomuser.me");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (bulkLoad)
                {
                    HttpResponseMessage response = await client.GetAsync("?results=" + count + "&key=8JE0-J3P4-MCWI-42H9");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<RandomUserResponse>();

                        foreach (var user in result.results)
                        {
                            _users.Add(user.user);
                        }

                        return;
                    }
                    throw new NullReferenceException("no user returned from service");


                    // todo - handle other responces
                }
                else
                {

                    HttpResponseMessage response = await client.GetAsync("");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<RandomUserResponse>();
                        var firstResult = result.results.FirstOrDefault();
                        if (firstResult != null)
                        {
                            Console.WriteLine("Random user, registered number: {0}", firstResult.user.registered);
                            _users.Add(firstResult.user);
                            return;
                        }

                        throw new NullReferenceException("no user returned from service");
                    }

                    // todo - handle other responces
                }



            }

        }
    }
}
    
