using System.Runtime.Intrinsics.Arm;

namespace HealthNoter.Service.Hash;

public interface IHashService
{
    string ComputeHash(string message);
}