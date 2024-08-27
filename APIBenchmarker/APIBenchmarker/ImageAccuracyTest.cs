using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIBenchmarker
{
    public class ImageAccuracyTest
    {

        private HttpClient _client;
        private byte[] _image;
        public double Attempts { get; private set; }
        public double StringScore { get; private set; }
        public double NumberScore { get; private set; }

        public double StringScorePercentage { get { return StringScore / Attempts; } }
        public double NumberScorePercentage { get { return NumberScore / Attempts; } }  
        public int SerialNumber { get; private set; }
        public int ModelName { get; private set; }
        public int ModelNumber { get; private set; }
        public int Manufacturer { get; private set; }
        public int Category { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Length { get; private set; }
        public List<int> Timeings { get; private set; }
        [JsonIgnore]
        public IEnumerable<ImageAccuracyResult> Results { get; private set; }

        private readonly ImageDetails correctImage;
           


        public ImageAccuracyTest(int numberOfAttempts, HttpClient client, ImageDetails correctDetails, string imageFileName) { 
            this.Attempts = numberOfAttempts;
            this._client = client;
            Timeings = new List<int>();

            var filepath = ($"C:\\Users\\alasd\\source\\repos\\APIBenchmarker\\APIBenchmarker\\images\\{imageFileName}");
            Console.WriteLine(filepath);
            _image = File.ReadAllBytes(filepath);
            correctImage = correctDetails;

        }

        private async Task<ImageDetails> GetImageFromAi(int counter)
        {
            var bytes = new ByteArrayContent(_image);
            bytes.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            var content = new MultipartFormDataContent();
            content.Add(bytes, "image", "test_image.jpg");

            var stopwatch = Stopwatch.StartNew();
            var result = await _client.PostAsync("ai", content);
            stopwatch.Stop();

            Timeings.Add(stopwatch.Elapsed.Milliseconds);
            var r = await result.Content.ReadAsStringAsync();
            Console.WriteLine($"AI Result {counter} : {r}");
            var json = JsonDocument.Parse(r);

            var returnedDetails = json.Deserialize<APIResponse>();

            if(returnedDetails == null || returnedDetails.query == null ) {
                throw new ArgumentException("Could not parse image details");
            }
            await Task.Delay(21000);

            return returnedDetails.query;
        }

        private async Task<ImageDetails> GetImageFromAi(int counter, bool Test)
        {
            var watch = Stopwatch.StartNew();
            await Task.Delay(50);
            watch.Stop();
            Timeings.Add(watch.Elapsed.Milliseconds);

            return new ImageDetails
            {
                serialNumber = "905TRVM18271",
                modelNumber = "LBNCxxxxxxx/00",
                width = 701.675,
                length = 701.675,
                height = 1720.85,
                manufacturer = "LG",
                category = "refrigerator"
            }; ;
        }


        public async Task<ImageAccuracyTest> RunTest(int tries, bool test)
        {

                List<ImageAccuracyResult> results = new List<ImageAccuracyResult>();
                for (int i = 0; i < tries; i++)
                {
                ImageDetails aiImage;
                    try
                    {
                         aiImage = test ? await GetImageFromAi(i + 1, true) : await GetImageFromAi(i + 1);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }

                    var testResult = new ImageAccuracyResult(correctImage, aiImage, i);
                    testResult.TimeTaken = Timeings[i];
                    results.Add(testResult);
                }

                Attempts = tries;
                StringScore = results.Select(x => (double)x.StringFieldsMatch / (double)x.StringFieldsTotal).Sum();

                NumberScore = results.Select(x => (double)x.NumberFieldsMatch / (double)x.NumberFieldsTotal).Sum();

                //Aggregate the results fore each field
                SerialNumber = results.Where(x => x.SerialNumber == ResultType.Success).Count();
                ModelName = results.Where(x => x.ModelName == ResultType.Success).Count();
                ModelNumber = results.Where(x => x.ModelNumber == ResultType.Success).Count();
                Category = results.Where(x => x.Category == ResultType.Success).Count();
                Manufacturer = results.Where(x => x.Manufacturer == ResultType.Success).Count();
                Width = results.Where(x => x.Width == ResultType.Success).Count();
                Length = results.Where(x => x.Length == ResultType.Success).Count();
                Height = results.Where(x => x.Height == ResultType.Success).Count();
                Results = results;


            
            
            return this;
        }

       




    }
}
