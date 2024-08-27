using BenchmarkDotNet.Toolchains.InProcess.Emit.Implementation;

namespace APIBenchmarker
{
    public class ImageAccuracyResult
    {
        public int Attempt { get; set; }
        public int StringFieldsMatch { get; set; }
        public int StringFieldsTotal { get; set; }
        public int NumberFieldsMatch { get; set; }
        public int NumberFieldsTotal { get; set; }
        public ResultType SerialNumber { get => serialNumber; set => serialNumber = value; }
        public ResultType ModelName { get => modelName; set => modelName = value; }
        public ResultType ModelNumber { get => modelNumber; set => modelNumber = value; }
        public ResultType Manufacturer { get => manufacturer; set => manufacturer = value; }
        public ResultType Category { get => category; set => category = value; }
        public ResultType Width { get => width; set => width = value; }
        public ResultType Height { get => height; set => height = value; }
        public ResultType Length { get => length; set => length = value; }

        public string SerialNoValue { get; set; }
        public string ModelNoValue { get; set; }
        public string ModelNameValue { get; set; }
        public string ManufacturerValue { get; set; }
        public string CategoryValue { get; set; }
        public double WidthValue { get; set; }
        public double HeightValue { get; set; }
        public double LengthValue { get; set; } 
        public int TimeTaken { get; set; }

        private ResultType serialNumber;

        private ResultType modelName;
        private ResultType modelNumber;
        private ResultType manufacturer;
        private ResultType category;
        private ResultType width;
        private ResultType height;
        private ResultType length;

        private double marginForNumbers = 1;

        private ImageDetails _correctLabel;
        private ImageDetails _otherLabel;


        public ImageAccuracyResult(ImageDetails correctImage, ImageDetails interpretedImage, int counter)
        {
            _correctLabel = correctImage;
            Attempt = counter;
            SerialNoValue = interpretedImage.serialNumber;
            ModelNoValue = interpretedImage.modelNumber;
            ModelNameValue = interpretedImage.modelName;
            ManufacturerValue = interpretedImage.manufacturer;
            CategoryValue = interpretedImage.category;
            WidthValue = interpretedImage.width ?? 0;
            HeightValue = interpretedImage.height ?? 0;
            LengthValue = interpretedImage.length ?? 0;
               
            _otherLabel = interpretedImage;
            CompareLabels();
        }

        private bool WithinMargin(double correct, double sample)
        {
            double marginBottom = correct- marginForNumbers;
            double marginTop = correct + marginForNumbers;

            if(sample > marginBottom && sample < marginTop ) {
                return true;
            }
            return false;
        }

        private void CompareLabels()
        {
            CompareStringField(_correctLabel.serialNumber, _otherLabel.serialNumber, ref serialNumber);            
            CompareStringField(_correctLabel.modelNumber, _otherLabel.modelNumber, ref modelNumber);
            CompareStringField(_correctLabel.manufacturer, _otherLabel.manufacturer, ref manufacturer);
            CompareStringField(_correctLabel.modelName, _otherLabel.modelName, ref modelName);
            CompareStringField(_correctLabel.category, _otherLabel.category, ref category);   
            CompareNumberFields(_correctLabel.width ?? 0, _otherLabel.width ?? 0,ref width);
            CompareNumberFields(_correctLabel.height ?? 0, _otherLabel.height ?? 0, ref height);
            CompareNumberFields(_correctLabel.length ?? 0, _otherLabel.length ?? 0, ref length);
          
        }

        private void CompareStringField(string correct, string other, ref ResultType field)
        {
            if (correct == null) { field = ResultType.Unassigned;  return; }
            else if (other != null && correct.Trim().ToLower() == other.Trim().ToLower()){
                field = ResultType.Success;
                StringFieldsMatch++;
            }
            else
            {
                field = ResultType.Failure;
            }
            StringFieldsTotal++;
        }
        private void CompareNumberFields(double correct, double other, ref ResultType field)
        {
            if (correct == 0) { field = ResultType.Unassigned; return; }
            else if (WithinMargin(correct, other))
            {
                field = ResultType.Success;
                NumberFieldsMatch++;

            }
            else
            {
                field = ResultType.Failure;

            }
            NumberFieldsTotal++;
        }

    }
    public class ImageDetails
    {
        public string serialNumber {  get; set; }
        public string modelName { get; set; }
        public string modelNumber { get; set; }
        public string manufacturer { get; set; }
        public string category { get; set; }
        public double? width { get; set; }
        public double? length { get; set; }
        public double? height { get; set; }

    }

    public class APIResponse
    {
        public ImageDetails query { get; set; }
    }


    public enum ResultType
    {
        Unassigned,
        Success,
        Failure
    }
}
