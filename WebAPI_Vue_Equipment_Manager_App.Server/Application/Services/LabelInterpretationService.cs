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
        private readonly string projectID;
        private readonly string location = "us-central1";
        private readonly string publisher = "google";
        private readonly string model = "gemini-1.5-flash-001";

        private readonly string _testPromp = "this is a test of the api. Please tell me something interesting.";

        private readonly string _prompt = "The attached photo should be a piece of equipment and is likely to be it's label or something that otherwise details it's properties. Can you extract the details from this photo and return in a json format as follows " +
             "{ query: {serialNumber: str," +
            " modelNumber: str, " +
            "modelName: str," +
            " manufacturer: str, " +
            "category: string, " +
            "weight: number kilograms," +
            " height: number millimeters," +
            " length: number milimeters, width: " +
            "number milimeters}," +
            "isValidLabel : boolean," +
            "errors: [] " +
            "Instructions: Only assign a value if the confidence is extremely high, otherwise assign null. " +
            "Only assign the serial number value to the serialNumber property and not any other. It represents a unique identifier for a single piece of equipment.  " +
            "The 'isValidLabel' shoudl be false if all fields are null and or the picture is not discenerable as an equipment label." +
            "The 'errors' property should list any reasons why the assigned values may not be accruate or explain why the image as a whole is invalid. Do not count as an error if a particular property is not present. These should be short and formal." +
            "Only include the Json response and no other syntax. Never include any prefix or postfix to the json object." +
            "If the weight or dimensions are listed in another unit of measurement please convert to kiligrams and milimeters respectively. ";

        public GoogleLabelInterpretationService()
        {
            projectID = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID")!;
            if(projectID == null)
            {
                throw new Exception("Google Project ID could not be found");
            }

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
