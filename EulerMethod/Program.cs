using System.Globalization;
using System.IO;
using System.Numerics;

using NumericalLib;

using static System.Math;

namespace EulerMethod {

  class Program {
    private class Wave : OdeModel<double> {
      public Wave() : base(0.0, 1.0) => TimeEnd = 2.0 * PI;

      public override Vector<double> Function(double t, Vector<double> x) => new Vector<double>(new[] {x[1], -x[0]});
    }

    private static void Main() {
      Solver<double>.Delta = 0.01;
      using (var sw = new StreamWriter("wave.csv")) {
        var model = new Wave();
        sw.WriteLine("time,x1_e,x2_e,x1_h,x2_h,x1_r,x2_r,sin,cos");
        var t = model.TimeStart;
        var te = model.TimeEnd;
        var xE = model.InitialValues;
        var xH = model.InitialValues;
        var xR = model.InitialValues;
        
        sw.WriteLine("time,x1_e,x2_e,x1_h,x2_h,x1_r,x2_r");
        sw.WriteLine($"{t},{xE.ToString(",")},{xH.ToString(",")},{xR.ToString(",")},{Sin(t).ToString(CultureInfo.CurrentCulture)},{Cos(t).ToString(CultureInfo.CurrentCulture)}");
        while (t <= te) {
          xE += Solver<double>.EulerUpdate(model, t, xE);
          xH += Solver<double>.HeunUpdate(model, t, xH);
          xR += Solver<double>.Rk4Update(model, t, xR);
          t += Solver<double>.Delta;
          sw.WriteLine($"{t},{xE.ToString(",")},{xH.ToString(",")},{xR.ToString(",")},{Sin(t).ToString(CultureInfo.CurrentCulture)},{Cos(t).ToString(CultureInfo.CurrentCulture)}");
        }
      }
    }
  }

}