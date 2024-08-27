using Google.Cloud.AIPlatform.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs.Mappings;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Services.Entity_Services;
using WebAPI_Vue_Equipment_Manager_App.Server.Data;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Controllers
{
    [ApiController]

    //Used for populating test databses with data and performin specific test operations

    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IEquipmentModelService _modelService;
        private readonly MainDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ICategoryRepository<ItemStatusCategory> _categoryRepository;

        public TestController(IItemService itemService, 
            IEquipmentModelService mService,
            MainDbContext context, 
            UserManager<User> uManager,
            ICategoryRepository<ItemStatusCategory> icRepo)
        {
            _itemService = itemService;
            _modelService = mService;
            _context = context;
            _userManager = uManager;
            _categoryRepository = icRepo;
        }

        [HttpPost("items")]
        public async  Task<IActionResult> AddManyItems(IEnumerable<TestItem> itemsDTOs)
        {


           var statuses = await _categoryRepository.GetAllAsync();
           var statusIds = statuses.Select(x => x.Id).ToList();
           var modelIds = await _context.Models.Select(x => x.Id).ToListAsync();
           var units = await _context.Units.Select(x => x.Id).ToListAsync();

            var randStatus = new Random();
            var randModel = new Random();
            var randUnit = new Random();

            var items = itemsDTOs.Select(x => new Item
            {
                SerialNumber = x.SerialNumber,
                LocalName = x.LocalName,
                Barcode = x.Barcode,
                Condition_On_Reciept = x.Condition_On_Reciept,
                ModelId = modelIds[randModel.Next(modelIds.Count)],
                UnitId = units[randUnit.Next(units.Count)],
                ItemStatusCategoryId = statusIds[randStatus.Next(statusIds.Count)]
        });

          
            _context.Items.AddRange(items);
            await _context.SaveChangesAsync();
            return Ok(items);           
        }

        //just used for adding a lot of entities to the db for testing purposes
        [HttpPost ("models")]
        public async Task<IActionResult> AddManyModels(IEnumerable<EquipmentModelDTO> models)
        {
            await _modelService.AddMany(models);
            return NoContent();
        }

        [HttpPost("units")]
        public async Task<IActionResult> AddManyUnits(IEnumerable<UnitDTO> unitDTOs)
        {
            var units = unitDTOs.Select(x => x.ToEntity()).ToList();
           _context.Units.AddRange(units);
           await _context.SaveChangesAsync();
            var addedUnits = await _context.Units.ToListAsync();
            addedUnits = addedUnits.ToList();
            var tenth = addedUnits.Count / 10;

            for(var i = 0; i < addedUnits.Count; i += tenth)
            {
               var rand = new Random();
              var seletedUnits = addedUnits.Skip(i).Take(tenth).ToList();
                CreateUnitTree(seletedUnits, 0, seletedUnits.Count - 1);
                _context.UpdateRange(seletedUnits);
            }
           await _context.SaveChangesAsync();

           return Ok();          

        }

        private void CreateUnitTree(List<Unit> units, int l, int r, Unit? parent = null)
        {
            if (l > r) return;
            int mid = (l + r) / 2;
            var midUnit = units[mid];
            if(parent!= null)
            {
                midUnit.ParentId = parent.Id;
            }
            CreateUnitTree(units, mid + 1, r, midUnit);
            CreateUnitTree(units,l, mid - 1, midUnit);
            return;
        }

        [HttpPost("users")]
        public async Task<IActionResult> AddManyUsers(IEnumerable<UserRegisterDTO> users)
        {
            var toRegister = users.Select(newUser => new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                UserName = newUser.Email,
            }).ToList();

            var tasks = new List<Task>();
            foreach (var user in toRegister)
            {
                tasks.Add(_userManager.CreateAsync(user, "StrongPassword1234!"));
            }

            await Task.WhenAll(tasks);
            return Ok();
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRandomAll()
        {
            var unitIds = await _context.Units.Select(u => u.Id).ToListAsync();
            var userIds = await _context.Users.Select(u =>u.Id).ToListAsync();
            int[] roles = [1,2,3];
            

            var assignments = new List<UserAssignment>();

            var unitRandom = new Random();
            var roleRandom = new Random();

            for(int i = 0; i < userIds.Count -1; i++)
            {
                assignments.Add(new UserAssignment {
                    UnitId = unitIds[unitRandom.Next(unitIds.Count)], 
                    RoleId = roles[roleRandom.Next(roles.Length)], 
                    UserId = userIds[i] });
            }
            var existing = await _context.Assignments.AsNoTracking().ToListAsync();
            var exclusive = assignments.Where(x => !existing.
                    Any(a => a.UnitId == x.UnitId && a.UserId == x.UserId)).
                    ToList();
            _context.Assignments.AddRange(exclusive);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("items/super")]
        public async Task<IActionResult> SearchFullItemsDatabase()
        {
            var query  = _context.Items.AsQueryable();
            List<Item> r = await query.Where(x => EF.Functions.ILike(x.SerialNumber, $"%{"TEST_ROOT_ITEM"}%")).
            Include(x => x.Unit).
            Include(x => x.EquipmentModel).
            ThenInclude(x => x.Category).
            Include(x => x.Notes).
            Include(x => x.Documents).
            ThenInclude(x => x.Document).
            Include(x => x.Maintenances).
            ThenInclude(x => x.Documents).
            Include(x => x.Maintenances).
            ThenInclude(x => x.Category).
            AsSplitQuery().
            AsNoTracking().
            ToListAsync();
            return Ok(r);
        }
    }

    public class TestItem
    {
        public required string SerialNumber { get; set; }
        public required string Barcode { get; set; }
        public required string LocalName { get; set; }
        public required string Condition_On_Reciept { get; set; }

    }
}
