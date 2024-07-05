using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class ItemPostDTO
    {
        public required ItemDTO Item { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFileCollection? Files { get; set; }
    }
}

public class ItemPostBinder : IModelBinder
{
    public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
    {
        throw new NotImplementedException();
    }
}
