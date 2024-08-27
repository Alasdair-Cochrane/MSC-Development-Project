using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace APIBenchmarker
{
    public class Benchmarker
    {
        private HttpClient _httpClient;
        private MultipartFormDataContent _itemPost;



        [GlobalSetup]
        public async Task Setup()
        {
            var client = new HttpClient();
            User u = new User();
            await u.Login();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", u.AccessToken);
            client.BaseAddress = new Uri("https://localhost:32768/api/");
            _httpClient = client;

            _itemPost = new MultipartFormDataContent();
            _itemPost.Add(new StringContent("POST_TEST"),"SerialNumber");
            _itemPost.Add(new StringContent("Active"),"CurrentStatus");
            _itemPost.Add(new StringContent("TEST_MODEL"),"ModelNumber");
            _itemPost.Add(new StringContent("TEST_MODEL"),"ModelName");
            _itemPost.Add(new StringContent("TEST_MODEL"),"Manufacturer");
            _itemPost.Add(new StringContent("TEST"),"Category");
            _itemPost.Add(new StringContent("3751"),"UnitId");
        }


       // [Benchmark]
        public async Task QuerySingleRootItem()
        {
           var response =  await _httpClient.GetAsync("items?serialNumber=TEST_ROOT_ITEM");

        }
        public async Task QueryRootPreTest()
        {
            var response = await _httpClient.GetAsync("items?serialNumber=TEST_ROOT_ITEM");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

        }

       // [Benchmark]
        public async Task QuerySingleChildItem()
        {
            var response = await _httpClient.GetAsync("items?serialNumber=TEST_CHILD_ITEM");

        }
        public async Task QuerySinglePreTest()
        {
            var response = await _httpClient.GetAsync("items?serialNumber=TEST_CHILD_ITEM");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }

        [Benchmark]
        public async Task GetItemFromFullDatabase()
        {
            var result = await _httpClient.GetAsync("test/items/super");
        }
        public async Task GetItemFromFullPreTest()
        {
            var response = await _httpClient.GetAsync("test/items/super");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

        }

       // [Benchmark]
        public async Task  QueryMultipleItems()
        {
            var response = await _httpClient.GetAsync("items?category=Freezer");

        }
        public async Task QueryMultiplePreTest()
        {
            var response = await _httpClient.GetAsync("items?category=Freezer");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }

       // [Benchmark]
        public async Task GetAllItems()
        {
            var response = await _httpClient.GetAsync("items");
        }
        public async Task GetAllItemsPreTest()
        {
            var response = await _httpClient.GetAsync("items");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }

       // [Benchmark]
        public async Task GetOrgStructure()
        {
            await _httpClient.GetAsync("units?flat=false&adminOnly=true");
        }
        public async Task GetOrgStructurePreTest()
        {
            var response = await _httpClient.GetAsync("units?flat=false&adminOnly=true");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }
        //[Benchmark]
        public async Task GetUnitsFlatStructure()
        {
            await _httpClient.GetAsync("units?flat=true");
        }
        public async Task GetUnitsFlatStructurePreTest()
        {
           var response = await _httpClient.GetAsync("units?flat=true");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
        }

        //[Benchmark]
        public async Task PostItem()
        { 
            var response = await _httpClient.PostAsync("items", _itemPost);

        }
        public async Task PostItemPreTest()
        {
            var response = await _httpClient.PostAsync("items", _itemPost);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

        }

        public async Task PreTest()
        {
            //Console.WriteLine("----ROOT PRE TEST----");
            //await QueryRootPreTest();
            //Console.WriteLine("----SINGLE CHILD PRE TEST----");
            //await QuerySinglePreTest();
            Console.WriteLine("----SUPER PRE TEST----");
            await GetItemFromFullPreTest();
            //Console.WriteLine("----MULTIPLE PRE TEST----");
            //await QueryMultiplePreTest();
            //Console.WriteLine("----GET ALL PRE TEST----");
            //await GetAllItemsPreTest();
            //Console.WriteLine("----GET ORG PRE TEST----");
            //await GetOrgStructurePreTest();
            //Console.WriteLine("----GET FLAT PRE TEST----");
            //await GetUnitsFlatStructurePreTest();
            //Console.WriteLine("----POST ITEM PRE TEST----");
            //await PostItemPreTest();




        }


        //[Benchmark]
        //public async Task GetCSVFile()
        //{
        //    var result = await _httpClient.GetAsync("items/csv");
        //}


    }
}
