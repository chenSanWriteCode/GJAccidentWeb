using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Models;

namespace GJAccidentWeb.Dao
{
    public interface IAccidentDao
    {
        Result<List<Accident>> search(Pager<List<Accident>> pager, AccidentQueryModel condition);
        int searchCount(AccidentQueryModel condition);
        Result<Accident> findById(string id);
        Result<AccidentModel> add(AccidentModel model);
        Result<AccidentModel> update(AccidentModel model);
        Result<Accident> delete(string id);
        List<Accident> getListByCondition(AccidentQueryModel condition);
    }
}
