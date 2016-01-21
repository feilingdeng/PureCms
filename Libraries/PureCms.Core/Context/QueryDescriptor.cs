using PureCms.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PureCms.Core.Context
{

    public abstract class QueryDescriptor<T, TChild>
        where T : class
        where TChild : class, new()
    {
        /// <summary>
        /// 设置接收条件值的对象
        /// </summary>
        /// <returns></returns>
        public abstract object GetConditionContainer();
        private int _topCount;
        /// <summary>
        /// 查询前N条记录
        /// </summary>
        public int TopCount { get { return _topCount; } set { this._topCount = value; } }

        private PageDescriptor _pagingDescriptor;
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageDescriptor PagingDescriptor
        {
            get
            {
                return _pagingDescriptor;
            }
            set
            {
                this._pagingDescriptor = value;
            }
        }

        private List<SortDescriptor<T>> _sortingDescriptor;
        /// <summary>
        /// 排序信息
        /// </summary>
        public List<SortDescriptor<T>> SortingDescriptor
        {
            get
            {
                return _sortingDescriptor;
            }
            set
            {
                this._sortingDescriptor = value;
            }
        }
        /// <summary>
        /// 设置读取记录数
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public TChild Take(int take)
        {
            _topCount = take;
            return this as TChild;
        }
        /// <summary>
        /// 设置分页信息
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public TChild Page(int currentPage, int pageSize)
        {
            if (_pagingDescriptor == null)
            {
                _pagingDescriptor = new PageDescriptor(currentPage, pageSize);
            }
            else
            {
                _pagingDescriptor.PageNumber = currentPage;
                _pagingDescriptor.PageSize = pageSize;
            }
            return this as TChild;
        }
        /// <summary>
        /// 设置排序信息
        /// </summary>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public TChild Sort(params SortDescriptor<T>[] sorts)
        {
            if (this._sortingDescriptor == null)
            {
                this._sortingDescriptor = new List<SortDescriptor<T>>();
            }
            foreach (var item in sorts)
            {
                if (!this._sortingDescriptor.Contains(item))
                {
                    this._sortingDescriptor.Add(item);
                }
            }
            return this as TChild;
        }
        /// <summary>
        /// 设置排序信息
        /// </summary>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public TChild Sort(params Func<SortDescriptor<T>, SortDescriptor<T>>[] sorts)
        {
            if (this._sortingDescriptor == null)
            {
                this._sortingDescriptor = new List<SortDescriptor<T>>();
            }
            foreach (var item in sorts)
            {
                var sd = item(new SortDescriptor<T>());

                if (!this._sortingDescriptor.Contains(sd))
                {
                    this._sortingDescriptor.Add(sd);
                }
            }
            return this as TChild;
        }
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TChild Where(Expression<Func<TChild, object>> field, object value)
        {
            var name = ExpressionHelper.GetPropertyName<TChild>(field);
            Type t = typeof(TChild);
            t.SetPropertyValue(GetConditionContainer(), name, value);
            
            return this as TChild;
        }
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public TChild Conditions(Expression<Func<T, bool>> conditions)
        {

            return this as TChild;
        }

        private List<string> _columns;
        public List<string> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }
        public TChild Select(params Expression<Func<T, object>>[] fields)
        {
            if (_columns == null)
            {
                _columns = new List<string>();
            }
            foreach (var item in fields)
            {
                _columns.Add(ExpressionHelper.GetPropertyName<T>(item));
            }
            return this as TChild;
        }
    }
}
