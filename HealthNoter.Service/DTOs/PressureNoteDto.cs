namespace HealthNoter.Service.DTOs;

public record PressureNoteDto(Guid Id, int Sys, int Dia, int Pulse, DateTime CreatedAt, Guid UserId, string Username);