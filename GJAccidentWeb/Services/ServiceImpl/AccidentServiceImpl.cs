using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GJAccidentWeb.Dao;
using GJAccidentWeb.Entity;
using GJAccidentWeb.Infrastructure;
using GJAccidentWeb.Models;
using log4net;
using Unity;

namespace GJAccidentWeb.Services.ServiceImpl
{
    public class AccidentServiceImpl : IAccidentService
    {
        [Dependency]
        public IAccidentDao dao { get; set; }
        public async Task<Result<AccidentModel>> add(AccidentModel model)
        {
            Result<AccidentModel> result = new Result<AccidentModel>();
            if (model != null)
            {
                result = await Task.Factory.StartNew(() => dao.add(model));
            }
            else
            {
                result.addError("无法插入空数据");
            }
            return result;
        }

        public async Task<Result<Accident>> delete(string id)
        {
            Result<Accident> result = new Result<Accident>();
            if (!string.IsNullOrEmpty(id))
            {
                result = await Task.Factory.StartNew(() => dao.delete(id));
            }
            else
            {
                result.addError("id为空");
            }
            return result;
        }

        public async Task<Result<AccidentModel>> findById(string id)
        {
            Result<AccidentModel> result = new Result<AccidentModel>();
            if (string.IsNullOrEmpty(id))
            {
                result.addError("无效值，请重新选择");
            }
            else
            {
                var result_dao = await Task.Factory.StartNew(() => dao.findById(id));
                if (result_dao.success)
                {
                    AccidentModel model = new AccidentModel();
                    BeanHelper.CopyBean(ref model, result_dao.data);
                    result.data = model;
                }
            }
            return result;
        }

        public async Task<List<AccidentExportModel>> getListByCondition(AccidentQueryModel condition)
        {
            List<AccidentExportModel> result = new List<AccidentExportModel>();
            List<Accident> dataList = await Task.Factory.StartNew(() => dao.getListByCondition(condition));
            foreach (var item in dataList)
            {
                AccidentExportModel model = new AccidentExportModel();
                BeanHelper.CopyBean(ref model, item);
                result.Add(model);
            }
            return result;

        }

        public async Task<Result<Pager<List<Accident>>>> search(Pager<List<Accident>> pager,AccidentQueryModel condition)
        {
            Result<Pager<List<Accident>>> result = new Result<Pager<List<Accident>>>();
            var result_data = await Task.Factory.StartNew(() => dao.search(pager,condition));
            if (result_data.success)
            {
                pager.data = result_data.data;
                pager.recTotal = await Task.Factory.StartNew(() => dao.searchCount(condition));
            }
            else
            {
                result.addError(result_data.message);
            }
            return result;
        }

        public async Task<Result<AccidentModel>> update(AccidentModel model)
        {
            Result<AccidentModel> result = new Result<AccidentModel>();
            if (model != null)
            {
                result = await Task.Factory.StartNew(() => dao.update(model));
            }
            else
            {
                result.addError("无法修改空值");
            }
            return result;
        }


    }
}