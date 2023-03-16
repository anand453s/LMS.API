using Microsoft.EntityFrameworkCore;

namespace LMS.Repository.Interfaces;

public interface IUnitOfWork : IDisposable
{
    DbContext Db { get; }
    void Save();
}


    

