using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;

namespace GJAccidentWeb.Services
{
    public interface IAccidentService
    {
        Task<Result<Pager<List<Accident>>>> search(Pager<List<Accident>> pager, AccidentQueryModel condition);
        Task<Result<AccidentModel>> findById(string id);
        Task<Result<AccidentModel>> add(AccidentModel model);
        Task<Result<AccidentModel>> update(AccidentModel model);
        Task<Result<Accident>> delete(string id);
        Task<List<AccidentExportModel>> getListByCondition(AccidentQueryModel condition);
    }
}
