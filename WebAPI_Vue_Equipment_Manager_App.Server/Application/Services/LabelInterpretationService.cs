namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Services
{
    public class GoogleLabelInterpretationService : ILabelInterpretationService

    {



    }

    public interface ILabelInterpretationService {
    
        public Task<string> InterpretLabelTextAsync(string text);
    
    }

}
