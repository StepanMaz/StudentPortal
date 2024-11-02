namespace StudentPortalServer.Models.DTO.Auth;

public record StudentRegisterDTO(string Name, string Email);

public record TeacherRegisterDTO(string Name, string Email);

public record StudentLoginDTO(string Name, string Email);

public record TeacherLoginDTO(string Name, string Email);