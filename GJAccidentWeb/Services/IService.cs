using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;

namespace GJAccidentWeb.Services
{
    public interface IService<T>
    {
        Task<Result<T>> update(T t, string userName);
        Task<Result<int>> delete(string id);
        Task<Result<T>> add(T t,string userName);
    }
}
