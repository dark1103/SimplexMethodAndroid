using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexMethod
{
    public static class OutputFormat
    {
        public static bool rawFormat = true;
        public static int spaceFilling = 5;
        public static int zeroCount = 2;
        public static bool fullRoots = true;
        public static int aroundValue = 5;
        public static int rootsAroundValue = 0;
    }
    public struct Point
    {
        public int x, y;
        public Point(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Point Zero() { return new Point(0, 0); }
        public override string ToString()
        {
            return $"({x} {y})";
        }
    }
    public class Matrix
    {
        public class Row:IList<double>
        {
            protected List<double> columns;
            #region IList
            public double this[int index]
            {
                get
                {
                    return ((IList<double>)columns)[index];
                }

                set
                {
                    ((IList<double>)columns)[index] = value;
                }
            }

            public int Count
            {
                get
                {
                    return ((IList<double>)columns).Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return ((IList<double>)columns).IsReadOnly;
                }
            }

            public void Add(double item)
            {
                ((IList<double>)columns).Add(item);
            }

            public void Clear()
            {
                ((IList<double>)columns).Clear();
            }

            public bool Contains(double item)
            {
                return ((IList<double>)columns).Contains(item);
            }

            public void CopyTo(double[] array, int arrayIndex)
            {
                ((IList<double>)columns).CopyTo(array, arrayIndex);
            }

            public IEnumerator<double> GetEnumerator()
            {
                return ((IList<double>)columns).GetEnumerator();
            }

            public int IndexOf(double item)
            {
                return ((IList<double>)columns).IndexOf(item);
            }

            public void Insert(int index, double item)
            {
                ((IList<double>)columns).Insert(index, item);
            }

            public bool Remove(double item)
            {
                return ((IList<double>)columns).Remove(item);
            }

            public void RemoveAt(int index)
            {
                ((IList<double>)columns).RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IList<double>)columns).GetEnumerator();
            }
            #endregion
            #region Initialisators
            public Row()
            {
                columns = new List<double>();
            }
            public Row(int capacity)
            {
                columns = new List<double>(capacity);
                columns.AddRange(new double[capacity]);
            }
            public Row(IEnumerable<double> data)
            {
                columns = new List<double>(data);
            }
            #endregion
            #region operators
            public static Row operator + (Row r1,Row r2)
            {
                if (r1.Count != r2.Count) return null;
                Row result = new Row(r1.Count);
                for (int i = 0; i < r1.Count; i++)
                {
                    result[i] = r1[i] + r2[i];
                }
                return result;
            }
            public static Row operator - (Row r1, Row r2)
            {
                if (r1.Count != r2.Count) return null;
                Row result = new Row(r1.Count);
                for (int i = 0; i < r1.Count; i++)
                {
                    result[i] = r1[i] - r2[i];
                }
                return result;
            }
            public static Row operator / (Row r1, double value)
            {
                Row result = new Row(r1.Count);
                for (int i = 0; i < r1.Count; i++)
                {
                    result[i] = r1[i] / value;
                }
                return result;
            }
            public static Row operator * (Row r1, double value)
            {
                Row result = new Row(r1.Count);
                for (int i = 0; i < r1.Count; i++)
                {
                    result[i] = r1[i] * value;
                }
                return result;
            }
            #endregion
            public override string ToString()
            {
                if (OutputFormat.rawFormat)
                {
                    return string.Join(" ", columns);
                }
                else
                {
                    return string.Join(" ", columns.ConvertAll(c => String.Format("{0," + OutputFormat.spaceFilling + ":#0." + "0".Repeat(OutputFormat.zeroCount) + "}", c)));
                }
            }
            public string ToStringWithSelection(int selectedIndex)
            {
                if (OutputFormat.rawFormat)
                {
                    List<string> s = columns.ConvertAll(c=>c.ToString());
                    s[selectedIndex] = "["+s[selectedIndex]+"]";
                    return string.Join(" ", s);
                }
                else
                {
                    List<string> s = columns.ConvertAll(c => String.Format("{0," + OutputFormat.spaceFilling + ":#0." + "0".Repeat(OutputFormat.zeroCount) + "}", c));
                    s[selectedIndex] = String.Format("{0," + OutputFormat.spaceFilling + ":[#0." + "0".Repeat(OutputFormat.zeroCount) + "]}", columns[selectedIndex]);
                    return string.Join(" ", s);
                }
            }
            public bool ContainsPositive {
                get
                {
                    foreach (var item in columns)
                    {
                        if (item > 0) return true;
                    }
                    return false;
                }
            }
            public void Round(int aroundValue)
            {
                for (int i = 0; i < Count; i++)
                {
                    columns[i] = Math.Round(columns[i], aroundValue);
                }
            }
        }
        protected List<Row> rows;
        public double this[Point index]
        {
            get
            {
                return rows[index.y][index.x];
            }

            set
            {
                rows[index.y][index.x] = value;
            }
        }
        public double this[int y, int x]
        {
            get
            {
                return rows[y][x];
            }

            set
            {
                rows[y][x] = value;
            }
        }
        public Matrix()
        {
            rows = new List<Row>();
        }
        public Matrix(int xCapacity,int yCapacity)
        {
            rows = new List<Row>();
            for (int i = 0; i < yCapacity; i++)
            {
                rows.Add(new Row(xCapacity));
            }
        }
        public Matrix(double[,] source)
        {
            rows = new List<Row>(source.GetLength(0));
            for (int y = 0; y < source.GetLength(0); y++)
            {
                rows.Add(new Row(source.GetLength(1)));
                for (int x = 0; x < source.GetLength(1); x++)
                {
                    rows[y][x] = Math.Round(source[y, x],OutputFormat.aroundValue);
                }
            }
        }
        public int RowsCount { get { return rows.Count; } }
        public int ColumnsCount { get { return rows[0].Count; } }
        public double[,] ToArray()
        {
            double[,] array = new double[RowsCount, ColumnsCount];
            for (int y = 0; y < RowsCount; y++)
            {
                for (int x = 0; x < ColumnsCount; x++)
                {
                    array[y, x] = rows[y][x];
                }
            }
            return array;
        }
        public override string ToString()
        {
            return string.Join(Environment.NewLine, rows.ConvertAll(r => r.ToString()));
        }

    }
    public class SimplexMatrix : Matrix
    {
        public SimplexMatrix() : base()
        {
        }
        public SimplexMatrix(int xCapacity, int yCapacity) : base(xCapacity, yCapacity)
        {
        }
        public SimplexMatrix(double[,] source) : base(source)
        {
        }

        public double GetB(int y)
        {
            return rows[y][ColumnsCount.Reduce()];
        }
        public double GetC(int x)
        {
            return rows[RowsCount.Reduce()][x];
        }
        //public void SetB(int y,double value)
        //{
        //    rows[y][ColumnsCount.Reduce()] = value;
        //}
        //public void SetC(int x,double value)
        //{
        //    rows[RowsCount.Reduce()][x] = value;
        //}
        public Row RowC { get { return rows[RowsCount.Reduce()]; } }
        public bool IsFinal { get { return !RowC.ContainsPositive; } }
        public double CalculateMatrixValue(Point point)
        {
            return GetC(point.x) * CalculateColumnValue(point);
        }
        public double CalculateColumnValue(Point point)
        {
            return GetB(point.y) / rows[point.y][point.x];
        }
        public Point GetGeneralValuePoint()//Позиция минимально-максимального числа...что-то там
        {
            Point? matchedPoint = null;
            //for (int y = 0; y < RowsCount - 1; y++)
            //{
            //    Point? matchedInRowPoint = null;
            //    for (int x = 0; x < ColumnsCount - 1; x++)
            //    {
            //        if (rows[y][x] > 0 && RowC[x] > 0)
            //        {
            //            if (!matchedInRowPoint.HasValue || CalculateRowValue(new Point(x, y)) < CalculateRowValue(matchedInRowPoint.Value))
            //            {
            //                matchedInRowPoint = new Point(x, y);
            //            }
            //        }
            //    }
            //    if (matchedInRowPoint.HasValue)
            //    {
            //        if (!matchedPoint.HasValue || CalculateMatrixValue(matchedInRowPoint.Value) > CalculateMatrixValue(matchedPoint.Value))
            //        {
            //            matchedPoint = matchedInRowPoint;
            //        }
            //    }
            //}
            //Console.WriteLine("Value:" + rows[matchedPoint.Value.y][matchedPoint.Value.x]);
            //Console.WriteLine("Point:" + matchedPoint.Value);

            for (int x = 0; x < ColumnsCount - 1; x++)
            {
                if (RowC[x] > 0)
                {
                    Point? matchedInColumnPoint = null;
                    for (int y = 0; y < RowsCount - 1; y++)
                    {
                        if (rows[y][x] > 0)
                        {
                            if (!matchedInColumnPoint.HasValue || CalculateColumnValue(new Point(x, y)) < CalculateColumnValue(matchedInColumnPoint.Value))
                            {
                                matchedInColumnPoint = new Point(x, y);
                            }
                        }
                    }
                    if (matchedInColumnPoint.HasValue)
                    {
                        if (!matchedPoint.HasValue || CalculateMatrixValue(matchedInColumnPoint.Value) > CalculateMatrixValue(matchedPoint.Value))
                        {
                            matchedPoint = matchedInColumnPoint;
                        }
                    }
                }
            }
            if (!matchedPoint.HasValue)
            {
                throw new NullSimplexGeneralValueException();
            }
            return matchedPoint.Value;
        }

        public override string ToString()
        {
            Point global;
            try
            {
                global = GetGeneralValuePoint();
            }
            catch
            {
                return string.Join(Environment.NewLine, rows.ConvertAll(r => r.ToString()));
            }
            List <string> s = rows.ConvertAll(r => r.ToString());
            s[global.y] = rows[global.y].ToStringWithSelection(global.x);
            return string.Join(Environment.NewLine, s);
        }

        public SimplexMatrix Simplify()
        {
            Point gvPoint = GetGeneralValuePoint();
            double gvValue = rows[gvPoint.y][gvPoint.x];
            SimplexMatrix newMatrix = new SimplexMatrix(this.ToArray());
            for (int y = 0; y < newMatrix.RowsCount; y++)
            {
                if (y != gvPoint.y)
                {
                    for (int x = 0; x < newMatrix.ColumnsCount; x++)
                    {
                        newMatrix.rows[y][x] = Math.Round(newMatrix.rows[y][x] - rows[gvPoint.y][x] * rows[y][gvPoint.x] / gvValue, OutputFormat.aroundValue);
                    }
                }
                else
                {
                    newMatrix.rows[y] = newMatrix.rows[y]/gvValue;
                    newMatrix.rows[y].Round(OutputFormat.aroundValue);
                }
            }
            newMatrix.RowC[newMatrix.ColumnsCount.Reduce()] = 0;
            return newMatrix;
        }
        public static Row SimplifyToEnd(SimplexMatrix matrix,Action<SimplexMatrix> output)
        {
            int rootsCount = matrix.RowC.Where(r => r != 0).Count();
            output?.Invoke(matrix);
            while (!matrix.IsFinal)
            {
                matrix = matrix.Simplify();
                output?.Invoke(matrix);
            }
            return GetRoots(matrix,OutputFormat.fullRoots ? matrix.ColumnsCount.Reduce() : rootsCount);
        }
        private static Row GetRoots(SimplexMatrix matrix,int rootsCount)
        {
            Row row = new Row();
            for (int i = 0; i < rootsCount; i++)
            {
                if (matrix.RowC[i] < 0)
                {
                    row.Add(0);
                }
                else if (matrix.RowC[i] == 0)
                {
                    for (int j = 0; j < matrix.RowsCount - 1; j++)
                    {
                        if (matrix.rows[j][i] == 1)
                        {
                            row.Add(Math.Round(matrix.GetB(j),OutputFormat.rootsAroundValue));
                            break;
                        }
                    }
                }
                else
                {
                    throw new Exception("Simplex Final Calculation Error");
                }
            }
            return row;
        }
        public class NullSimplexGeneralValueException : Exception
        {
            public NullSimplexGeneralValueException():base()
            {
            }
            public NullSimplexGeneralValueException(SimplexMatrix matrix)
            {
                Data.Add("Matrix", matrix);
            }
            public override string Message
            {
                get
                {
                    return "Невозможно получить генеральное значение,последующие вычисления невозможны";
                }
            }
        }
    }
}
