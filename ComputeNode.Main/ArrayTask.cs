using Apache.Ignite.Core.Cluster;
using Apache.Ignite.Core.Compute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeNode.Main
{
    public class ArrayTask : IComputeTask<int[][], (int,int), int[][]>
    {
        private int[][] _arg;
        public IDictionary<IComputeJob<(int,int)>, IClusterNode> Map(IList<IClusterNode> subgrid, int[][] arg)
        {
            _arg = Transpon(arg);
            Console.WriteLine("Transpon: ");
            OutputHelper.Write(_arg);
            var map = new Dictionary<IComputeJob<(int,int)>, IClusterNode>();
            using (var enumerator = subgrid.GetEnumerator())
            {
                int index = 0;
                foreach (var arr in _arg)
                {
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Reset();
                        enumerator.MoveNext();
                    }

                    map.Add(new ArrayComputeJob(arr,index), enumerator.Current);
                    index++;
                }
            }

            return map;
        }
        private int[][] Transpon(int[][] arr)
        {
            int[][] res = new int[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
            {
                res[i] = new int[arr.Length];
            }
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    res[j][i] = arr[i][j];
                }
            }
            return res;
        }

        private void Replace(int column1, int column2)
        {
            var t = _arg[column1];
            _arg[column1] = _arg[column2];
            _arg[column2] = t;
        }
        public ComputeJobResultPolicy OnResult(IComputeJobResult<(int,int)> res, IList<IComputeJobResult<(int,int)>> rcvd)
        {
            // If there is no exception, wait for all job results.
            return res.Exception != null ? ComputeJobResultPolicy.Failover : ComputeJobResultPolicy.Wait;
        }

        public int[][] Reduce(IList<IComputeJobResult<(int,int)>> results)
        {
            //return results.Select(res => res.Data.Item1).Sum();
            var res = results.Select(item => item.Data).ToList();
            res.Sort((pair1, pair2) => pair1.Item2.CompareTo(pair2.Item2));  
            for(int m = _arg.Length - 1; m >= 1; m--)
            {
                for (int j = 0; j < m; j++)
                {
                    if (res[j + 1].Item1 > res[j].Item1)
                    {
                        Replace(j, j+1);
                        var t = res[j + 1];
                        res[j + 1] = res[j];
                        res[j] = t;
                    }
                }
            }

            return Transpon(_arg);
        }

    }
}
