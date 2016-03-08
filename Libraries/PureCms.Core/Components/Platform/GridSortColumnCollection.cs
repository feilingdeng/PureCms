namespace PureCms.Core.Components.Platform
{
    public sealed class GridSortColumnCollection : System.Collections.Generic.List<QueryColumnSortInfo>
    {
        private const char ColumnInfoSeperator = ';';
        private const char ColumnDataSeperator = ':';
        public QueryColumnSortInfo this[string name]
        {
            get
            {
                foreach (QueryColumnSortInfo current in this)
                {
                    if (current.Name == name)
                    {
                        return current;
                    }
                }
                return null;
            }
            set
            {
                for (int i = 0; i < base.Count; i++)
                {
                    if (base[i].Name == name)
                    {
                        base[i] = value;
                        return;
                    }
                }
                base.Add(value);
            }
        }
        public void DeserializeFromGridParameter(string parameter)
        {
            string[] array = parameter.Split(new char[]
            {
                ';'
            });
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(new char[]
                {
                    ':'
                });
                if (array2.Length == 2)
                {
                    base.Add(new QueryColumnSortInfo(array2[0], array2[1] == "1"));
                }
            }
        }
        public string SerializeToGridParameter()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(256);
            bool flag = false;
            foreach (QueryColumnSortInfo current in this)
            {
                if (flag)
                {
                    stringBuilder.Append(';');
                }
                stringBuilder.Append(current.Name);
                stringBuilder.Append(':');
                stringBuilder.Append(current.SortAscending ? "1" : "0");
                flag = true;
            }
            return stringBuilder.ToString();
        }
    }
}
