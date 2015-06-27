﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DReporting.Core;
using DReporting.Models;

namespace DReporting.Services
{
    [Export(typeof(IReportDatas))]
    public class DefaultReportDatas : IReportDatas
    {
        public IEnumerable<DataSourceModel> QueryDataSources(int? skip = null, int? take = null)
        {
            var metas = InjectContainer.Instance.ExportMetas();

            var objs = InjectContainer.Instance.GetExports<IDataSource>();

            var query = objs.Select(x => new DataSourceModel
            {
                DataSourceId = metas.First(m => m.ComponentType == x.GetType()).ContractName,
                DataSourceName = x.DataSourceName,
                DataSource = x
            });

            query = query.OrderBy(x => x.DataSourceId);

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }

        public DataSourceModel GetDataSource(string dataSourceId)
        {
            var dataSource = InjectContainer.Instance.GetExport<IDataSource>(dataSourceId);

            return new DataSourceModel
            {
                DataSourceId = dataSourceId,
                DataSourceName = dataSource.DataSourceName,
                DataSource = dataSource
            };
        }
    }
}
