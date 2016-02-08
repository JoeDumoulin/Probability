using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using Numerics;
using Probability;

namespace ProbabilityTests
{
  [TestFixture]
  public class GaussianTests
  {
    [Test]
    public void gaussian_parameters_represent_the_data()
    {
      var data = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };

      var expected = 3;

      var dist = data.Train();

      Assert.That(dist.Mean, Is.EqualTo(expected));
    }

    [Test]
    public void generating_a_container_of_gaussian_reproduces_a_gausian_distribution()
    {
      var dist = Distribution.GaussianSequence().Take(1000000).Train();
      var tolerance = 0.01;

      var actualsd = Math.Abs(dist.Sdev - 1.0);

      Assert.That(Math.Abs(dist.Mean), Is.LessThan(tolerance));
      Assert.That(actualsd, Is.LessThan(tolerance));
    }

    [Test]
    public void gaussian_can_be_created_directly()
    {
      var dist = new Gaussian(0, 0, 0);

      Assert.That(dist.Samples, Is.EqualTo(0));
      Assert.That(dist.Mean, Is.EqualTo(0));
      Assert.That(dist.Variance, Is.EqualTo(0));
      Assert.That(dist.Sdev, Is.EqualTo(0));
    }

    [Test]
    public void add_gaussian_is_homomorphic_with_list_append()
    {
      var data1 = new[] { -1.0, -2.0, -3.0, -4.0, -5.0 };
      var data2 = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
      var data3 = data1.Concat(data2);

      var expected = data3.Train();

      var dist1 = data1.Train();
      var dist2 = data2.Train();

      var actual = dist1.Add(dist2);

      Assert.That(actual.Samples, Is.EqualTo(expected.Samples));
      Assert.That(actual.Mean, Is.EqualTo(expected.Mean));
      Assert.That(actual.Variance, Is.EqualTo(expected.Variance));
      Assert.That(actual.Sdev, Is.EqualTo(expected.Sdev));
    }

    [Test]
    public void gaussian_inverse_subtracts_from_a_distribution()
    {
      var data1 = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
      var dist1 = data1.Train();

      var data2 = new[] { 4.0, 5.0 };
      var dist2 = data2.Train();

      var expected = (new[] { 1.0, 2.0, 3.0 }).Train();

      var actual = dist1.Add(dist2.Invert());

      Assert.That(actual.Samples, Is.EqualTo(expected.Samples));
      Assert.That(actual.Mean, Is.EqualTo(expected.Mean));
      Assert.That(actual.Variance, Is.EqualTo(expected.Variance));
      Assert.That(actual.Sdev, Is.EqualTo(expected.Sdev));
    }
  }
}
