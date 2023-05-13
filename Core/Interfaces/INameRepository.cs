using Core.Entities;

namespace Core.Interfaces;

public interface INameRepository
{
    Task<Name> GetNameByIdWithIncludes(int nameId);
}