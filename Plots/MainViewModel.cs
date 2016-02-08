using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;

using Probability;

namespace Plots
{
  public class MainViewModel
  {
    public MainViewModel()
    {
      this.MyModel = new PlotModel("Example 1");
      var scatterseries = new ScatterSeries();
      var prnd = new Random();
      var i = 0;
      while (i < 10000)
      {
        var pair = Distribution.GaussianSamplePair(0.0, 0.1, prnd);
        scatterseries.Points.Add(new DataPoint(pair.Key, pair.Value));
        i++;
      }
      MyModel.Series.Add(scatterseries);
    }
    public PlotModel MyModel { get; private set; }
  }
}
