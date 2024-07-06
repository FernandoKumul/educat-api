namespace Domain.DTOs.Instructor
{
    public class InstructorInfoDTO
    {
        public int PkInstructor { get; set; }
        public int FkUser { get; set; }
        public string? Occupation { get; set; }
        public string? FacebookUser { get; set; }
        public string? YoutubeUser { get; set; }
        public string? LinkediId { get; set; }
        public string? TwitterUser { get; set; }
        public string? EmailPaypal { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? AvatarUrl { get; set; }
        public string? Description { get; set; }
    }
}