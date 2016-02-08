using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Numerics;
using Probability;

namespace ProbabilityTests
{
  [TestFixture]
  class UniformTests
  {
    // uniform Double sequence tests
    [Test]
    public void UniformSequence_returns_seq_of_uniform_random_numbers()
    {
      IEnumerable<double> test = Distribution.UniformSequence(0.0, 1.0).Take(10);
      Assert.IsTrue(test.Count().Equals(10));
    }

    [Test]
    public void UniformSequence_returns_values_less_than_end()
    {
      var start = 0.0;
      var end = 1.0;
      var test = Distribution.UniformSequence(start, end).Take(10).Max() < end;
      Assert.IsTrue(test);
    }

    [Test]
    public void UniformSequence_returns_values_greater_than_start()
    {
      var start = 0.0;
      var end = 1.0;
      var test = Distribution.UniformSequence(start, end).Take(10).Min() > start;
      Assert.IsTrue(test);
      start = -10.0;
      end = 10.0;
      test = Distribution.UniformSequence(start, end).Take(10).Min() > start;
      Assert.IsTrue(test);
    }

    // Uniform integer sequence tests
    [Test]
    public void UniformSequence_returns_integer_seq_of_uniform_random_numbers()
    {
      IEnumerable<int> test = Distribution.UniformSequence(0, 100).Take(10);
      Assert.IsTrue(test.Count().Equals(10));
    }

    [Test]
    public void UniformSequence_returns_integer_values_less_than_end()
    {
      var start = 0;
      var end = 2;
      var test = Distribution.UniformSequence(start, end).Take(10).Max() < end;
      Assert.IsTrue(test);
    }

    [Test]
    public void UniformSequence_returns_int_values_greater_than_or_equal_to_start()
    {
      var start = 0;
      var end = 10;
      var test = Distribution.UniformSequence(start, end).Take(10).Min() >= start;
      Assert.IsTrue(test);
      start = -10;
      end = 10;
      test = Distribution.UniformSequence(start, end).Take(10).Min() >= start;
      Assert.IsTrue(test);
    }

    // uniform sampling testing
    [Test]
    public void double_UniformSample_returns_sample_between_two_values()
    {
      var start = 0.0;
      var end = 1.0;
      var result = Distribution.UniformSample(start, end);
      Assert.That(result, Is.TypeOf<double>());
      Assert.That(result, Is.LessThan(end));
      Assert.That(result, Is.GreaterThan(start));
    }

    [Test]
    public void double_UniformSample_throws_ArgumentOutOfRangeException_when_start_greater_than_end()
    {
      var start = 10.0;
      var end = 1.0;
      Assert.That(() => Distribution.UniformSample(start, end), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void integer_UniformSample_returns_sample_between_two_values()
    {
      var start = 0;
      var end = 1;
      var result = Distribution.UniformSample(start, end);
      Assert.That(result, Is.TypeOf<int>());
      Assert.That(result, Is.LessThan(end));
      Assert.That(result, Is.GreaterThanOrEqualTo(start));
    }

    [Test]
    public void integer_UniformSample_throws_ArgumentOutOfRangeException_when_start_equals_end()
    {
      var start = 1;
      var end = 1;
      Assert.That(() => Distribution.UniformSample(start, end), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void integer_UniformSample_throws_ArgumentOutOfRangeException_when_start_greater_than_end()
    {
      var start = 10;
      var end = 1;
      Assert.That(() => Distribution.UniformSample(start, end), Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
    }

    // Choosing elements randomly and permutations
    [Test]
    public void choose_a_random_element_from_a_list_returns_a_list_element()
    {
      var theList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

      Assert.That(Distribution.Choice(theList), Is.TypeOf<int>());
    }

    [Test]
    public void permute_returns_permutation_of_a_list_of_items()
    {
      var theList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
      var result = theList.Permute(new List<int>(){1,2}).ToArray();
      Assert.That(result.Count(), Is.EqualTo(9));
      Assert.That(result[1], Is.EqualTo(theList.ToArray()[2]));
      Assert.That(result[2], Is.EqualTo(theList.ToArray()[1]));
      Assert.That(result[0], Is.EqualTo(theList.ToArray()[0]));
      Assert.That(result[8], Is.EqualTo(theList.ToArray()[8]));
    }
  }
}
