namespace HealthNoter.Service.DTOs;

public record UserDto (Guid Id, string Username, DateTime RegisteredAt);