using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Probability
{
  /// <summary>
  /// Gassian is a template for immutable objects which describe gaussian distributions.
  /// </summary>
  public class Gaussian
  {
    private int _samples;
    private double _moment1;
    private double _moment2;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="samples">number of samples used for the estimated distribution</param>
    /// <param name="moment1">first moment (mean)</param>
    /// <param name="moment2">second moment (variance)</param>
    public Gaussian(int samples, double moment1, double moment2)
    {
      _samples = samples;
      _moment1 = moment1;
      _moment2 = moment2;
    }

    public int Samples
    {
      get { return _samples; }
    }

    public double Mean
    {
      get { return _moment1; }
    }

    public double Variance
    {
      get { return _moment2/(double)(Samples-1); }
    }

    public double Sdev
    {
      get { return Math.Sqrt(Variance); }
    }

    public double WeightedVariance
    {
      get { return _moment2; }
    }
  }
}
