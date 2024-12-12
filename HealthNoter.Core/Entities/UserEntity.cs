namespace HealthNoter.Core.Entities;

public class UserEntity
{
    private const int UsernameMinSize = 3;
    private const int UsernameMaxSize = 3;


    public static UserEntity Create(string username, string password)
    {
        if (username.Length is < UsernameMinSize or > UsernameMaxSize)
            throw new ArgumentException(
                $"Имя пользователя должно быть в пределах от {UsernameMinSize} до {UsernameMaxSize}");

        return new UserEntity()
        {
            Id = Guid.NewGuid(),
            Username = username,
            Password = password,
            RegisteredAt = DateTime.UtcNow
        };
    }

    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public DateTime RegisteredAt { get; set; }
    public virtual ICollection<PressureNoteEntity> PressureNotes { get; set; } = new List<PressureNoteEntity>();
    
    
}