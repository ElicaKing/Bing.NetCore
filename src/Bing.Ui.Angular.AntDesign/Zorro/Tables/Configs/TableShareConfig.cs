﻿using System.Collections.Generic;

namespace Bing.Ui.Zorro.Tables.Configs
{
    /// <summary>
    /// 表格共享配置
    /// </summary>
    public class TableShareConfig
    {
        /// <summary>
        /// 表格包装器标识
        /// </summary>
        public string TableWrapperId => $"{TableId}_wrapper";

        /// <summary>
        /// 表格标识
        /// </summary>
        public string TableId { get; }

        /// <summary>
        /// 列信息集合
        /// </summary>
        public List<ColumnInfo> Columns { get; }

        /// <summary>
        /// 是否自动创建行
        /// </summary>
        public bool AutoCreateRow { get; set; }

        /// <summary>
        /// 是否自动创建表头
        /// </summary>
        public bool AutoCreateHead { get; set; }

        /// <summary>
        /// 是否自动创建排序列
        /// </summary>
        public bool AutoCreateSort { get; set; }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool IsSort { get; set; }

        /// <summary>
        /// 初始化一个<see cref="TableShareConfig"/>类型的实例
        /// </summary>
        /// <param name="tableId">表格标识</param>
        public TableShareConfig(string tableId)
        {
            TableId = tableId;
            Columns = new List<ColumnInfo>();
            AutoCreateRow = true;
            AutoCreateHead = true;
            AutoCreateSort = true;
        }
    }
}