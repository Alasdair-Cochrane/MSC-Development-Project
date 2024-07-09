using Google.Api.Gax.Grpc;
using Google.Cloud.AIPlatform.V1;
using Google.Protobuf;
using Grpc.Core;
using System.Text;


namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    //https://cloud.google.com/vertex-ai/generative-ai/docs/start/quickstarts/quickstart-multimodal#set-up-your-environment
    //https://cloud.google.com/dotnet/docs/reference/Google.Protobuf/latest/Google.Protobuf.ByteString
    //https://cloud.google.com/vertex-ai/generative-ai/docs/start/quickstarts/quickstart-multimodal
    public class GoogleLabelInterpretationService : ILabelInterpretationService

    {
        private readonly IConfiguration _config;
        private readonly string projectID = "fine-sublime-428810-m0";
        private readonly string location = "us-central1";
        private readonly string publisher = "google";
        private readonly string model = "gemini-1.5-flash-001";

        private readonly string _testPromp = "this is a test of the api. Please tell me something interesting.";

        private readonly string _prompt = "The attached photo is of a piece of equipment and is likely to be it's label. Can you extract the details from this photo and return in a json format as follows " +
             "{ query: {serialNumber: str," +
            " modelNumber: str, " +
            "modelName: str," +
            " manufacturer: str, " +
            "category: string, " +
            "weight: number kilograms," +
            " height: number millimeters," +
            " length: number milimeters, width: " +
            "number milimeters}," +
            "isValidLabel : boolean } " +
            "Instructions: Only assign a value if the confidence is very high, otherwise assign null. The serial number is a unique identifier for a single piece of equipment and so cannot be shared by any other field. The 'isValidLabel' shoudl be false if all fields are null and or the picture is not discenerable as an equipment label.";

        public GoogleLabelInterpretationService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> InterpretLabelAsync(IFormFile image)
        {

            Byte[] bytes;

            using (var stream = new MemoryStream())
            {
                await image.CopyToAsync(stream);
                bytes = stream.ToArray();
                if (bytes == null || bytes?.Length == 0) { throw new Exception("Image could not be read"); }
            }

            ByteString imageContent = ByteString.CopyFrom(bytes);

            var predictionServiceClient = new PredictionServiceClientBuilder
            {
                Endpoint = $"{location}-aiplatform.googleapis.com"
            }.Build();

            var generateContentRequest = new GenerateContentRequest
            {
                Model = $"projects/{projectID}/locations/{location}/publishers/{publisher}/models/{model}",
                Contents =
            {
                new Content
                {
                    Role = "USER",
                    Parts =
                    {
                        new Part { Text = _prompt },
                        new Part { InlineData = new() { MimeType = "image/jpeg", Data = imageContent } }
                    }
                }
            }
            };

            GenerateContentResponse response = await predictionServiceClient.GenerateContentAsync(generateContentRequest);

            string responseText = response.Candidates[0].Content.Parts[0].Text;
            Console.WriteLine(responseText);

            return responseText;
        }
    }

    public interface ILabelInterpretationService {
    
        public Task<string> InterpretLabelAsync(IFormFile image);
    
    }

}
