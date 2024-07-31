using WebAPI_Vue_Equipment_Manager_App.Server.Data.Entities;

namespace WebAPI_Vue_Equipment_Manager_App.Server.Application.DTOs
{
    public class ItemNoteDTO
    {
        public int Id { get; set; }
        public string? UserEmail { get; set; }
        public string? UserName { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? DatePosted { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public bool userCanDelete { get; set; }

    }

    public class ItemNotePostDTO
    {
        public int ItemId { get; set; }
        public required string Text { get; set; }
        public required string Title { get; set; }

    }
    public static class ItemNoteMappingExentsions
    {
        public static ItemNote ToEntity(this ItemNotePostDTO post, User user)
        {
            var entity = new ItemNote
            {
                Created = DateTime.UtcNow,
                UserId = user.Id,
                ItemId = post.ItemId,
                Text = post.Text,
                Title = post.Title
            };
            return entity;
        }
    }
}
