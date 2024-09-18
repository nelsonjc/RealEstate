namespace RealEstate.Core.Interfaces.Utils
{
    public interface IPasswordService
    {
        bool CheckPassword(string hash, string password);
        string GenerateHash(string password);
    }
}
