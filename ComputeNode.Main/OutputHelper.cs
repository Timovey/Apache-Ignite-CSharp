using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputeNode.Main
{
    public static class OutputHelper
    {
        public static void Write(int[][] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[0].Length; j++)
                {
                    Console.Write(arr[i][j].ToString() + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
