namespace Domain.DTOs.Google
{
    public class UserByGoogleDTO
    {
        public string GivenName { get; set; } = null!;
        public string FamilyName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Picture { get; set; }
        public bool EmailVerified { get; set; } = false;

    }
}