using Apache.Ignite.Core.Compute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeNode.Main
{
    public class ArrayComputeJob : IComputeJob<(int,int)>
    {
        private readonly int[]  _arg;
        private readonly int _index;

        public ArrayComputeJob(int[] arg, int index)
        {
            //Console.WriteLine(">>> Printing '" + arg + "' from compute job.");
            this._arg = arg;
            this._index = index;
        }
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public (int,int) Execute()
        {
            return (_arg.Sum(), _index);
        }
    }
}
