namespace Domain.DTOs.Google
{
    public class UserWithGoogleDTO
    {
        public string Name { get; set; } = null!;
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string? Picture { get; set; }
        public bool ValidatedEmail { get; set; } = false;
    }
}