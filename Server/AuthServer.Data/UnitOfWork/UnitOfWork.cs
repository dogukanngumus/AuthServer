using AuthServer.Core.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.UnitOfWork;

public class UnitOfWork:IUnitOfWork
{
    private readonly DbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
       await _dbContext.SaveChangesAsync();
    }

    public void Commit()
    {
        _dbContext.SaveChanges();
    }
}