using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using NSubstitute.Extensions;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Error_Handling;
using WebAPI_Vue_Equipment_Manager_App.Server.Application.Repository_Interfaces;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly MainDbContext _context;
        private readonly IUnitRepository _unitRepository;

        public NoteRepository(MainDbContext context, IUnitRepository unitRepository)
        {
            _context = context;
            _unitRepository = unitRepository;
        }

        public async Task<ItemNoteDTO> AddNoteAsync(ItemNote note)
        {
            var added = _context.ItemNotes.Add(note);
            await _context.SaveChangesAsync();
            var dto = await GetNoteDTO(added.Entity.Id);
            if(dto == null) {
                throw new DataInsertionException($"Could find inserted note {added.Entity.Id}");
            }
            dto.userCanDelete = true;
            return dto;
        }
        public async Task DeleteNoteAsync(int noteId)
        {
            var deleted = await _context.ItemNotes.FindAsync(noteId);
            if (deleted == null) { return; }
            _context.ItemNotes.Remove(deleted);
            await _context.SaveChangesAsync();
        }
        public async Task<ItemNoteDTO?> GetNoteDTO(int noteId)
        {
            var note = await _context.ItemNotes.Where(x => x.Id == noteId).
                Join(_context.Items, n => n.ItemId, i => i.Id, (n, i) => new ItemNoteDTO
                {
                    Id = n.Id,
                    SerialNumber = i.SerialNumber,
                    DatePosted = n.Created,
                    UserId = n.UserId,
                    ItemId = n.ItemId,
                    Text = n.Text,
                    Title = n.Title

                }).Join(_context.Users, n => n.UserId, u => u.Id, (n, u) =>
                new ItemNoteDTO
                {
                    Id = n.Id,
                    UserEmail = u.Email,
                    UserName = $"{u.FirstName} {u.LastName}",
                    SerialNumber = n.SerialNumber,
                    DatePosted = n.DatePosted,
                    UserId = n.UserId,
                    ItemId = n.ItemId,
                    Text = n.Text,
                    Title = n.Title

                }).FirstOrDefaultAsync();
            return note;
        }

        //Gets all notes created after specified days before & checks items belong to units that the user is authorised to view
        public async Task<IEnumerable<ItemNoteDTO>> GetNotesInTimePeriodAsync(int daysBefore, IEnumerable<int> unitIds)
        {
            var earliestDate = DateTime.UtcNow.AddDays(-daysBefore);
            var notes = await _context.ItemNotes.Where(x => x.Created > earliestDate).
                Join(_context.Items, n => n.ItemId, i => i.Id, (n, i) => new 
                {
                    Id = n.Id,
                    SerialNumber = i.SerialNumber,
                    DatePosted = n.Created,
                    UserId = n.UserId,
                    ItemId = n.ItemId,
                    Text = n.Text,
                    Title = n.Title,
                    UnitId = i.UnitId

                }).
                Where(x => unitIds.Contains(x.UnitId))
                .Join(_context.Users, n => n.UserId, u => u.Id, (n, u) =>
                new ItemNoteDTO
                {
                    Id = n.Id,
                    UserEmail = u.Email,
                    UserName = $"{u.FirstName} {u.LastName}",
                    SerialNumber = n.SerialNumber,
                    DatePosted = n.DatePosted,
                    UserId = n.UserId,
                    ItemId = n.ItemId,
                    Text = n.Text,
                    Title = n.Title

                }).ToListAsync();
            return notes;     
        }


        public async Task<IEnumerable<ItemNoteDTO>> GetNoteDTOsForItemAsync(int itemId)
        {
            var notes = await _context.ItemNotes.Where(x => x.ItemId == itemId).
                    Join(_context.Items, n => n.ItemId, i => i.Id, (n, i) => new ItemNoteDTO
                    {
                        SerialNumber = i.SerialNumber,
                        DatePosted = n.Created,
                        UserId = n.UserId,
                        ItemId = n.ItemId,
                        Text = n.Text,
                        Title = n.Title,
                        Id = n.Id,

                    }).
                    Join(_context.Users, n => n.UserId, u => u.Id, (n, u) =>
                    new ItemNoteDTO
                    {
                        UserEmail = u.Email,
                        UserName = $"{u.FirstName} {u.LastName}",
                        SerialNumber = n.SerialNumber,
                        DatePosted = n.DatePosted,
                        UserId = n.UserId,
                        ItemId = n.ItemId,
                        Text = n.Text,
                        Title = n.Title,
                        Id = n.Id,

                    }).ToListAsync();
            return notes;
        }
        public async Task<IEnumerable<ItemNote>> GetNotesForItemAsync(int itemId)
        {
            var notes = await _context.ItemNotes.
                Where(x => x.ItemId == itemId).
                ToListAsync();
            return notes;
        }

        //sets whether the user can delete certain notes in the provided list
        //based on whether they created the note or whether they are admin of the unit in which the item belongs to
        public async Task<IEnumerable<ItemNoteDTO>> AssignUserCanDeleteAsync(IEnumerable<ItemNoteDTO> notes, int userId)
        {
            var units = await _unitRepository.GetAllAdminRoleUnits(userId);
            var items = await _context.Items.
                AsNoTracking().
                //get the items that match the items in the notes list
                Where(x => notes.Select(x => x.ItemId).Contains(x.Id)).
                //get those items within the notes list that also belong to units where the user is admin
                Where(x => units.Select(x => x.Id).Contains(x.UnitId)).
                Select(x => x.Id).                
                ToListAsync();
            foreach( var note in notes)
            {
                if(note.UserId == userId || items.Contains(note.ItemId)){
                    note.userCanDelete = true;
                }
            }
            return notes;
        }

        public async Task<bool> CheckUserCanDeleteAsync(int noteId, int userId)
        {
            var units = await _unitRepository.GetAllAdminRoleUnits(userId);
            var queryResult = await _context.ItemNotes.
                Where(x => x.Id == noteId).
                //get the unit of the item that the note is attached to
                Join(_context.Items, n => n.ItemId, i => i.Id, (n, i) => 
                new { UnitID = i.UnitId,
                    UserId = n.UserId
                }
                ).FirstAsync();

            if(units.Select(x => x.Id).Contains(queryResult.UnitID) || queryResult.UserId == userId){
                return true;
            }
            return false;
        }
        
    }
}
