using System.IO;

using NumericalLib;

using static System.Math;

namespace EulerMethod {

  class Program {
    private class Wave : NumericalModelWithODE {
      public Wave() : base(0.0, 1.0) => TimeEnd = 2.0 * PI;
      
      public override Vector Function(double t, Vector x) => new Vector(x[1], -x[0]);
    }

    private static void Main() {
      Solver.Delta = 0.01;
      using (var sw = new StreamWriter("wave.csv")) {
        var model = new Wave();
        sw.WriteLine("time,x1_e,x2_e,x1_h,x2_h,x1_r,x2_r,sin,cos");
        var t = model.TimeStart;
        var te = model.TimeEnd;
        var x_e = model.InitialValues;
        var x_h = model.InitialValues;
        var x_r = model.InitialValues;
        
        sw.WriteLine("time,x1_e,x2_e,x1_h,x2_h,x1_r,x2_r");
        sw.WriteLine($"{t},{x_e.ToString(',')},{x_h.ToString(',')},{x_r.ToString(',')},{Sin(t)},{Cos(t)}");
        while (t <= te) {
          x_e += Solver.EulerUpdate(model, t, x_e);
          x_h += Solver.HeunUpdate(model, t, x_h);
          x_r += Solver.Rk4Update(model, t, x_r);
          t += Solver.Delta;
          sw.WriteLine($"{t},{x_e.ToString(',')},{x_h.ToString(',')},{x_r.ToString(',')},{Sin(t)},{Cos(t)}");
        }
      }
    }
  }

}