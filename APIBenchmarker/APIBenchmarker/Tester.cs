using BenchmarkDotNet.Attributes;
using CsvHelper;
using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace APIBenchmarker
{
    public class Tester
    {
        private HttpClient _httpClient;
        private byte[] _data;
        private string baseRoute = "C:\\Users\\alasd\\source\\repos\\APIBenchmarker\\APIBenchmarker\\Results\\ImageTest\\";



        public async Task Setup()
        {
            var client = new HttpClient();
            User u = new User();
            await u.Login();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", u.AccessToken);
            client.BaseAddress = new Uri("https://localhost:32768/api/");
            _httpClient = client;
        }

        public async Task TestImages(int iterations, bool isTest)
        {
            ImageDetails image1 = new ImageDetails
            {
                serialNumber = "905TRVM18271",
                modelNumber = "LBNC15231V/00",
                width = 701.675,
                length = 701.675,
                height = 1720.85,
                manufacturer = "LG",
                category = "refrigerator"
            };
            ImageDetails image2 = new ImageDetails
            {
                serialNumber = "2332620060889",
                modelNumber = "Q33326",
                modelName = "qubit flex fluorometer",
                category = "fluorometer",
                manufacturer = "thermo fisher scientific",
                width = 0,
                height = 0,
                length = 0,
            };
            ImageDetails image3 = new ImageDetails
            {
                serialNumber = "502BS4789",
                modelNumber = "248009",
                modelName = "BACT/ALERT 3D60",
                manufacturer = "bioMérieux,Inc",
                width = 0,
                height = 0,
                length = 0,
            };

            await LogTest(image1, "test_1.jpg", "test1", iterations, isTest);
            await LogTest(image2, "test_2.jpg", "test2", iterations, isTest);
            await LogTest(image3,"test_3.jpg","test3",iterations, isTest);
        }

        private async Task LogTest(ImageDetails image, string imageName, string testName,int iterations, bool isTest)
        {
            ImageAccuracyTest test1 = new ImageAccuracyTest(1, _httpClient, image, imageName);
            List<ImageAccuracyResult> tests = new List<ImageAccuracyResult>();
            var testResult = await test1.RunTest(iterations, isTest);
            tests.AddRange(testResult.Results);

            DateTime time = DateTime.Now;
            string timeString = time.DayOfYear.ToString() + "_" + time.Hour.ToString() + "_" + time.Minute.ToString() + "_" + time.Second.ToString();

            using (var writer = new StreamWriter(baseRoute + timeString +  "_" + testName+ ".csv"))
            using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(tests);
            }
        }

    }

    

}
