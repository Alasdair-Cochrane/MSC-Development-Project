using WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs;
using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Data.Repositories
{
    public interface INoteRepository
    {
        Task<ItemNoteDTO> AddNoteAsync(ItemNote note);
        Task<IEnumerable<ItemNoteDTO>> AssignUserCanDeleteAsync(IEnumerable<ItemNoteDTO> notes, int userId);
        Task<bool> CheckUserCanDeleteAsync(int noteId, int userId);
        Task DeleteNoteAsync(int noteId);
        Task<ItemNoteDTO?> GetNoteDTO(int noteId);
        Task<IEnumerable<ItemNoteDTO>> GetNoteDTOsForItemAsync(int itemId);
        Task<IEnumerable<ItemNote>> GetNotesForItemAsync(int itemId);
        Task<IEnumerable<ItemNoteDTO>> GetNotesInTimePeriodAsync(int daysBefore, IEnumerable<int> unitIds);
    }
}