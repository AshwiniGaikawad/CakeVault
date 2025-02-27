using CakeVault.Models;

namespace CakeVault.Repositories
{
    public interface ICakeVaultRepository
    {
        Task<IEnumerable<Cake>> GetAllCakesAsync();
        Task<Cake?> GetCakeByIdAsync(int id);
        Task<bool> CakeExistsByNameAsync(string name);
        Task AddCakeAsync(Cake cake);
        Task UpdateCakeAsync(Cake cake);
        Task DeleteCakeAsync(Cake cake);
        Task<bool> SaveChangesAsync();
    }
}
