using System;
using System.Collections.Generic;
using System.Linq;
using Probability;

namespace Probabilities_tester
{
  class Program
  {
    static void Main(string[] args)
    {
      var list1 = Distribution.UniformSequence(0, 10).Take(10);
      var list2 = list1.UniformRandomPermute();
      foreach (var item in list1.Zip(list2, (a, b) => new KeyValuePair<int, int>(a, b)))
      {
        Console.WriteLine(String.Format(@"{0} => {1}", item.Key, item.Value));
      }

      //var list1 = new List<int>() { 0, 1, 2, 3 };
      //var indexes = new List<int>() { 1, 2 };
      //foreach (var value in list1.Permute(indexes))
      //{
      //  Console.WriteLine(value);
      //}
    }
  }
}
