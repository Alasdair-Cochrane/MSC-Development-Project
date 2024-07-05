using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;
using Xunit;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.Tests
{
    public class ModelServiceTests
    {
        private IEquipmentModelRepository _equipmentModelRepository;
        private ICategoryRepository<EquipmentModelCategory> _categoryRepository;
        private EquipmentModel _testModel;
        private IEnumerable<EquipmentModel> _testModels;
        private EquipmentModelCategory _testCategory;
        private EquipmentModelsService sut;

        public ModelServiceTests()
        {
            _equipmentModelRepository = Substitute.For<IEquipmentModelRepository>();
            _categoryRepository = Substitute.For<ICategoryRepository<EquipmentModelCategory>>();
            sut = new EquipmentModelsService(_equipmentModelRepository, _categoryRepository);
            _testCategory = new EquipmentModelCategory { Id = -1, Name = "TEST CATEGORY" };

            _testModel = new EquipmentModel
            {
                ModelNumber = "TEST_MODEL_NUMBER",
                ModelName = "TEST NAME",
                Manufacturer = "TEST MANUFACTURER",
                Category = _testCategory
            };

            _testModels = new List<EquipmentModel> { _testModel ,
            new EquipmentModel
            {
                ModelName = "Model 2",
                ModelNumber = "Model 2",
                Manufacturer = "Model 2",
                Category = _testCategory
            } };
        }

        [Fact]

        public async Task Get_ReturnsNotNull()
        {
            //Arrange
            _equipmentModelRepository.GetAsync(Arg.Any<int>()).Returns(_testModel);

            //Act 
            var result = await sut.GetByiDAsync(1);

            //Assert
            result.Should().BeEquivalentTo(_testModel.ToDTO());
        }

        [Fact]

        public async Task Get_ReturnsNull()
        {
            _equipmentModelRepository.GetAsync(Arg.Any<int>()).ReturnsNull();

            var result = await sut.GetByiDAsync(1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAll_ReturnsList()
        {
            _equipmentModelRepository.GetAllAsync().Returns(_testModels);

            var result = await sut.GetAllAsync();

            var modelsDTO = _testModels.Select(x => x.ToDTO());

            result.Should().BeEquivalentTo(modelsDTO);
        }

        [Fact]
        public async Task Update_ReturnsSuccessfully()
        {
            _categoryRepository.FindOrCreateByName(Arg.Any<string>()).Returns(_testCategory);
            _equipmentModelRepository.UpdateAsync(Arg.Any<EquipmentModel>()).Returns(_testModel);
            _equipmentModelRepository.SaveAsync().Returns(Task.CompletedTask);

            var result = await sut.UpdateAsync(_testModel.ToDTO());

            result.Should().BeEquivalentTo(_testModel.ToDTO());

        }

        [Fact]
        public async Task Update_ReturnsUnsuccessfully()
        {
            _equipmentModelRepository.UpdateAsync(Arg.Any<EquipmentModel>()).ReturnsNull();
            _categoryRepository.FindOrCreateByName(Arg.Any<string>()).Returns(_testCategory);
            _equipmentModelRepository.SaveAsync().Returns(Task.CompletedTask);

            var result = await sut.UpdateAsync(_testModel.ToDTO());

            result.Should().BeNull();

        }


    }
}
