using System;
using System.Collections.Generic;
using System.Linq;
using Numerics;

namespace Probability
{
    public static class Distribution
    {
      public static double UniformSample(double start, double end, Random prnd = null)
      {
        if (start > end) throw new ArgumentOutOfRangeException("start", @"the ValueType of 'start' must be less than the valuetype 'end'");
        if (prnd == null)
          prnd = new Random();
        return start + prnd.NextDouble() * (end - start);
      }

      public static int UniformSample(int start, int end, Random prnd = null)
      {
        if (start >= end) throw new ArgumentOutOfRangeException("start", @"the ValueType of 'start' must be less than the valuetype 'end'");
        if (prnd == null)
          prnd = new Random();
        return (int)Math.Floor(UniformSample((double)start, (double)end, prnd));
      }

      /// <summary>
      /// Return an enumerable of uniformly distributed random doubles or integers.
      /// The sequence is infinite, so the caller is responsible for stopping
      /// </summary>
      /// <param name="start">uniform lower limit</param>
      /// <param name="end">uniform upper limit</param>
      /// <returns>enumerable of samples from the specified distribution</returns>
      public static IEnumerable<double> UniformSequence(double start, double end)
      {
        var prnd = new Random();
        while (true)
        {
          var x = UniformSample(start, end, prnd);
          yield return x;
        }
      }

      public static IEnumerable<int> UniformSequence(int start, int end)
      {
        var prnd = new Random();
        while (true)
        {
          var x = UniformSample(start, end, prnd);
          yield return x;
        }
      }

      public static T Choice<T>(this IEnumerable<T> source)
      {
        if (source == null) throw new ArgumentNullException("source");

        var step = 0;
        var value = UniformSample(0, source.Count());
        using (var iterator = source.GetEnumerator())
        {
          while (step != value)
          {
            if (!iterator.MoveNext())
              throw new IndexOutOfRangeException("index not found in source container");
            step++;
          }
          return iterator.Current;
        }
      }

      public static IEnumerable<int> UniformRandomPermuteIndexes<T>(this IEnumerable<T> source)
      {
        if (source == null) throw new ArgumentNullException("source");

        var size = source.Count();

        // choose a random number of swaps within the range.
        var num_swaps = UniformSample(2, size);
        return UniformSequence(0, num_swaps).Take(num_swaps);
      }

      public static IEnumerable<T> UniformRandomPermute<T>(this IEnumerable<T> source)
      {
        if (source == null) throw new ArgumentNullException("source");
        return source.Permute(source.UniformRandomPermuteIndexes());
      }

      // Gaussian distribution support
      /// <summary>
      /// Gausian random sample via Box-Muller method.  This method always 
      /// produces a pair of samples from a single uniform sample.
      /// </summary>
      /// <param name="mean"></param>
      /// <param name="sd">standard deviation</param>
      /// <returns>two samples for the distribution</returns>
      public static KeyValuePair<double,double> GaussianSamplePair(double mean = 0.0, double sd = 1.0, Random prnd = null)
      {
        if (prnd == null) prnd = new Random();
        var x = 0.0;
        var y = 0.0;
        var upsilon = -1.0;
        while (true)
        {
          x = UniformSample(-1.0, 1.0, prnd);
          y = UniformSample(-1.0, 1.0, prnd);

          upsilon = x * x + y * y;
          if (upsilon < 1.0 && upsilon != 0.0)
            break;
        }
        var upsilonp = sd * Math.Sqrt(-2 * Math.Log(upsilon) / upsilon);
        return new KeyValuePair<double, double>(upsilonp*x + mean, upsilonp*y + mean);
      }

      /// <summary>
      /// Return an infinite list of gausian random samples generated with the specified mean and variance.
      /// </summary>
      /// <param name="mean"></param>
      /// <param name="sd">standard deviation</param>
      /// <returns>An infinite list of samples.</returns>
      public static IEnumerable<double> GaussianSequence(double mean = 0.0, double sd = 1.0)
      {
        var prnd = new Random();
        while (true)
        {
          var pair = GaussianSamplePair(mean, sd, prnd);
          yield return pair.Key;
          yield return pair.Value;
        }
      }

      public static int BernoulliSample(double p, Random prnd = null)
      {
        if (prnd == null) prnd = new Random();

        if (UniformSample(0.0, 1.0, prnd) <= p)
          return 1;
        return 0;
      }

      /// <summary>
      /// Return an infinite list of bernoulli random samples generated with 
      /// the specified threshold.
      /// </summary>
      /// <param name="p">the threshold of the sample.</param>
      /// <returns>An infinite list of samples.</returns>
      public static IEnumerable<int> BernoulliSequence(double p)
      {
        var prnd = new Random();
        while (true)
        {
          yield return BernoulliSample(p, prnd);
        }
      }

      /// <summary>
      /// Use a list of samples to train a gaussian distribution
      /// </summary>
      /// <typeparam name="T">int or double</typeparam>
      /// <param name="source">enumerable container of type T</param>
      /// <returns>a Gausian distribution object derived from source</returns>
      public static Gaussian Train(this IEnumerable<double> source)
      {
        if (source == null) throw new ArgumentNullException("source");

        var c = source.Count();
        if (c == 0) return new Gaussian(0, 0, 0);

        double m1 = source.Aggregate((x,y) => x+y)/(double)c;
        double m2 = source.Select(x => x*x - m1*m1).Aggregate((x,y) => x+y);
        return new Gaussian(c, m1, m2);
      }

      /// <summary>
      /// Given two gaussian distributions, combine them into one gaussian distribution.
      /// This is an O(1) addition method for such distributions.  
      /// </summary>
      /// <param name="source">A gaussian distribution</param>
      /// <param name="addend">A second gaussian distribution</param>
      /// <returns>A new distribution which is the sum of the two distributions</returns>
      public static Gaussian Add(this Gaussian source, Gaussian addend)
      {
        if (source == null || addend == null) throw new ArgumentNullException("source or addend distribution is null");

        var samples = source.Samples + addend.Samples;
        if (samples == 0)
          return new Gaussian(0, 0, 0);

        var mean = (source.Samples * source.Mean + addend.Samples * addend.Mean) / samples;

        var moment2 = source.Variance * (source.Samples-1) + source.Samples * Math.Pow(source.Mean, 2)
            + addend.Variance * (addend.Samples - 1) + addend.Samples * Math.Pow(addend.Mean, 2)
            - samples * Math.Pow(mean, 2);

        return new Gaussian(samples, mean, moment2);
      }

      public static Gaussian Invert(this Gaussian source)
      {
        return new Gaussian(-source.Samples, source.Mean, -source.WeightedVariance);
      }
    }
}
