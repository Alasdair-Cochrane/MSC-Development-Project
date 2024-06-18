using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using Xunit;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers.Tests
{
    public class ModelsControllerTests
    {
        private IEquipmentModelService _modelService;
        private readonly ModelsController sut;

        public ModelsControllerTests()
        {
            _modelService = Substitute.For<IEquipmentModelService>();
            sut = new ModelsController(_modelService);
        }


    }
}
