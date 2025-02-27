using CakeVault.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CakeVault.Repositories
{
    public class CakeVaultRepository : ICakeVaultRepository
    {
        private readonly CakeVaultDBContext _context;
        private readonly ILogger<CakeVaultRepository> _logger;

        public CakeVaultRepository(CakeVaultDBContext context, ILogger<CakeVaultRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Cake>> GetAllCakesAsync()
        {
            try
            {
                return await _context.Cakes.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all cakes.");
                throw new Exception("Database error occurred while fetching cakes.");
            }
        }

        public async Task<Cake?> GetCakeByIdAsync(int id)
        {
            try
            {
                var cake = await _context.Cakes.AsNoTracking().FirstOrDefaultAsync(i => id == id);
                return cake ?? throw new KeyNotFoundException($"Cake with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching cake with ID {id}.");
                throw new Exception("Database error occurred while fetching the cake.");
            }
        }

        public async Task<bool> CakeExistsByNameAsync(string name)
        {
            try
            {
                return await _context.Cakes.AnyAsync(c => EF.Functions.Like(c.Name, name));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if cake exists by name.");
                throw new Exception("Database error occurred while checking cake existence.");
            }
        }

        public async Task AddCakeAsync(Cake cake)
        {
            try
            {
                await _context.Cakes.AddAsync(cake);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new cake.");
                throw new Exception("Database error occurred while adding a new cake.");
            }
        }

        public async Task UpdateCakeAsync(Cake cake)
        {
            try
            {
                _context.Cakes.Update(cake);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cake.");
                throw new Exception("Database error occurred while updating the cake.");
            }
        }

        public async Task DeleteCakeAsync(Cake cake)
        {
            try
            {
                _context.Cakes.Remove(cake);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting cake.");
                throw new Exception("Database error occurred while deleting the cake.");
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update exception occurred.");
                throw new Exception("A database update error occurred. Please check constraints and relations.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error occurred while saving changes.");
                throw new Exception("An unknown error occurred while saving changes.");
            }
        }
    }
}
