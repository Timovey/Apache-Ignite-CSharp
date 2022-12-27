using Apache.Ignite.Core.Cluster;
using Apache.Ignite.Core.Compute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeNode.Main
{
    public class CharCountTask : IComputeTask<string, int, int>
    {
        public IDictionary<IComputeJob<int>, IClusterNode> Map(IList<IClusterNode> subgrid, string arg)
        {
            var map = new Dictionary<IComputeJob<int>, IClusterNode>();
            using (var enumerator = subgrid.GetEnumerator())
            {
                foreach (var s in arg.Split(" "))
                {
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Reset();
                        enumerator.MoveNext();
                    }

                    map.Add(new CharCountComputeJob(s), enumerator.Current);
                }
            }

            return map;
        }

        public ComputeJobResultPolicy OnResult(IComputeJobResult<int> res, IList<IComputeJobResult<int>> rcvd)
        {
            // If there is no exception, wait for all job results.
            return res.Exception != null ? ComputeJobResultPolicy.Failover : ComputeJobResultPolicy.Wait;
        }

        public int Reduce(IList<IComputeJobResult<int>> results)
        {
            return results.Select(res => res.Data).Sum();
        }
    }
}
