using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIBenchmarker
{
    public class User
    {
        public string? AccessToken { get; set; }
        private string _username = "test@email.com";
        private string _password = "StrongPassword1234!";
        private readonly string _login;
        private readonly HttpClient _httpClient;
        private readonly string url = "https://localhost:32768/api/login";

        public User()
        {
            _httpClient = new HttpClient();
            var login = new { Email =  _username, Password = _password};
            _login =JsonSerializer.Serialize(login);
        }

        public async Task Login()
        {
            HttpContent content = new StringContent(_login,Encoding.UTF8,"application/json");
            
            var response = await _httpClient.PostAsync(url, content);
            if(response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var tokenObject = JsonDocument.Parse(token);
                Token? t = tokenObject.Deserialize<Token>();
                if(t == null)
                {
                    throw new NullReferenceException("Could not parse json");
                }
                AccessToken = t.accessToken;
            }
        }

        private class Token
        {
            public string TokenType { get; set; }
            public string accessToken { get; set; }
            public int ExpiresIn { get; set; }
            public string RefreshToken { get; set; }
            
        }
    }
}
