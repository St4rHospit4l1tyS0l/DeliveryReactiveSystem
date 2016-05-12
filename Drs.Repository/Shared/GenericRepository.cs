using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Web.Script.Serialization;
using Drs.Infrastructure.JqGrid;
using Drs.Infrastructure.JqGrid.Model;
using Drs.Repository.Entities;
using Drs.Repository.Log;

namespace Drs.Repository.Shared
{
    public class GenericRepository<TEntity> : BaseRepository where TEntity : class
    {
        private readonly DbSet _dbSet;
        public GenericRepository(CallCenterEntities dbConnP)
            : base(dbConnP)
        {
            _dbSet = DbConn.Set(typeof(TEntity));
        }

        public GenericRepository()
            : base(new CallCenterEntities())
        {
            _dbSet = DbConn.Set(typeof(TEntity));
        }

        public TEntity FindById(object id)
        {
            try
            {
                return (TEntity) _dbSet.Find(id);
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
                return default(TEntity);
            }
        }

        public void Add(TEntity model, bool bSaveChanges = true)
        {
            _dbSet.Add(model);
            if (bSaveChanges)
                DbConn.SaveChanges();
        }

        public void Update(TEntity model, bool bSaveChanges = true)
        {
            _dbSet.Attach(model);
            DbConn.Entry(model).State = EntityState.Modified;
            if (bSaveChanges)
                DbConn.SaveChanges();
        }

        public void Delete(object id)
        {
            dynamic model = _dbSet.Find(id);

            if (model != null)
            {
                Delete(model);
            }
        }

        public void Delete(TEntity model, bool bSaveChanges = true)
        {
            _dbSet.Attach(model);
            _dbSet.Remove(model);
            if (bSaveChanges)
                DbConn.SaveChanges();
        }

        public JqGridResultModel JqGridFindBy<TEntityDto>(JqGridFilterModel opts, string sKey, string sColumns, Expression<Func<TEntity, bool>> extraFilter = null,
            Func<dynamic, TEntityDto> dtoFunc = null)
        {

            try
            {
                var result = new JqGridResultModel();
                var query = _dbSet as IQueryable<TEntity>;

                if(query == null)
                    return null;

                if (opts._search)
                {
                    var filterDyn = new JavaScriptSerializer().Deserialize<JqGridMultipleFilterModel>(opts.filters);
                    JqGridQueryWhere whereOpts = JqGridClause.ExpresionToExec(filterDyn);
                    query = query.Where(whereOpts.Query, whereOpts.ArrParams);
                }

                if (extraFilter != null)
                {
                    query = query.Where(extraFilter);
                }

                if (string.IsNullOrWhiteSpace(opts.sidx) == false && string.IsNullOrWhiteSpace(opts.sord) == false)
                {
                    query = query.OrderBy(JqGridClause.OrderBy(opts));
                }

                result.total = 0;
                result.page = 1;
                result.records = query.Count();
                if (opts.page.HasValue && opts.rows.HasValue)
                {
                    query = query.Skip((opts.page.Value - 1) * opts.rows.Value).Take(opts.rows.Value);
                    result.page = opts.page.Value;
                    result.total = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(result.records) / opts.rows.Value));
                }

                IEnumerable qSel = query.Select(string.Format("New({0})", sColumns));

                if (qSel == null)
                    return null;

                var lstObj = qSel.Cast<dynamic>().ToList();


                if (lstObj.Count == 0)
                {
                    result.rows = new List<JqGridRowsModel>();
                }
                else
                {
                    var prop = lstObj[0].GetType().GetProperty(sKey);
                    result.rows = lstObj.Select(e => new JqGridRowsModel
                    {
                        id = prop.GetValue(e),
                        cell = dtoFunc == null ? e : dtoFunc(e)
                    }).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, opts, sKey, sColumns);
                return null;
            }

        }

        public List<TEntity> FindBy(Expression<Func<TEntity, bool>> exp)
        {
            var query = _dbSet as IQueryable<TEntity>;

            if (query == null)
                return null;

            return query.Where(exp).ToList();
        }
    }
}
