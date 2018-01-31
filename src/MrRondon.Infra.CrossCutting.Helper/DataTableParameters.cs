using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace MrRondon.Infra.CrossCutting.Helper
{
    public class DataTableParameters
    {
        public List<DataTableColumn> Columns { get; set; }
        public int Draw { get; set; }
        public int Length { get; set; }
        public List<DataOrder> Order { get; set; }
        public Search Search { get; set; }
        public int Start { get; set; }
    }

    public class Search
    {
        public bool Regex { get; set; }
        public string Value { get; set; }
    }

    public class DataTableColumn
    {
        public int Data { get; set; }
        public string Name { get; set; }
        public bool Orderable { get; set; }
        public bool Searchable { get; set; }
        public Search Search { get; set; }
    }

    public class DataOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DataTableResultSet
    {
        public int draw;

        public int recordsTotal;

        public int recordsFiltered;

        public List<string[]> data = new List<string[]>();

        public DataTableResultSet(int draw, int recordsTotal)
        {
            this.draw = draw;
            recordsFiltered = recordsTotal;
            this.recordsTotal = recordsTotal;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}