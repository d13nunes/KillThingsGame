using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Score
{

    

    public class Scores
    {
        private String[] columnHeader;
        private List<Object[]> table;

        public IEnumerable<Object[]> Table {
            get { return table; }
        }
        
        
        public Boolean Inicialize(String[] _columnHeader, List<Object[]> _table)
        {

            if (_columnHeader == null || _table == null || _table.First<Object[]>() == null || _columnHeader.Length != _table.First<Object[]>().Length)
                return false;

            this.columnHeader = _columnHeader;
            this.table = _table;
            return true;
        }

        public Boolean addScore(Object[] score) {
            if (table == null || table.First<Object[]>() == null || score.Length != table.First<Object[]>().Length)
                return false;
            table.Add(score);
            return true;
        }

        public Boolean SortColumnByName(String column, IComparer<Object> cmp) {
            int columnIndex;
            if ((columnIndex = SearchColumn(column)) < 0 || columnIndex >= columnHeader.Length)
                return false;
            SortColumn(columnIndex, cmp);
            return true;
        }

        private void SortColumn(int idx, IComparer<Object> cmp)
        {
            for(int i = 1; i < table.Count; i++) { 
                for(int j = i; j > 0; j--)
                {
                    if (cmp.Compare(table[j][idx], table[j - 1][idx]) > 0)
                        SwapTable(j, j - 1);
                }
            }
        }

        private void SwapTable(int a, int b) {
            var aux = table[b];
            table[b] = table[a];
            table[a] = aux;
        }



        private int SearchColumn(String columnName)
        {
            for (int i = 0; i < columnName.Length;i++)
                if (columnHeader.Equals(columnName))
                    return i;
            return -1;
        }

    }
}
