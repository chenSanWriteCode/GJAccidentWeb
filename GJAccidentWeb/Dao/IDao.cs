using GJAccidentWeb.Entity;

namespace GJAccidentWeb.Dao
{
    public interface IDao<T>
    {
        Result<T> update(T t);
        Result<int> delete(string id);
        Result<T> add(T t);
    }
}
