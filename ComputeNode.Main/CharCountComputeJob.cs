using Apache.Ignite.Core.Compute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeNode.Main
{
    public class CharCountComputeJob : IComputeJob<int>
    {
        private readonly string _arg;

        public CharCountComputeJob(string arg)
        {
            Console.WriteLine(">>> Printing '" + arg + "' from compute job.");
            this._arg = arg;
        }

        public int Execute()
        {
            return _arg.Length;
        }

        public void Cancel()
        {
            throw new System.NotImplementedException();
        }
    }

}
