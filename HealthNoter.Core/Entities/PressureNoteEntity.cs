namespace HealthNoter.Core.Entities;

public class PressureNoteEntity
{
    public static PressureNoteEntity Create(int sys, int dia, int pulse, Guid userId)
    {
        if (sys < 0 || dia < 0 || pulse < 0)
            throw new ArgumentOutOfRangeException("Кардио параметры не могут быть неположительными");

        return new PressureNoteEntity()
        {
            Id = Guid.NewGuid(),
            Sys = sys,
            Dia = dia,
            Pulse = pulse,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };
    }

    public Guid Id { get; set; }
    public int Sys { get; set; }
    public int Dia { get; set; }
    public int Pulse { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;
}