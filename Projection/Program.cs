using System.IO;

using NumericalLib;

using static System.Math;

namespace Projection {

  class Program {
    private class Projection : NumericalModelWithODE {
      private const double g = 9.80665;

      public Projection(double v0 = 5.0, double theta = 0.25 * PI)
        : base(v0 * Cos(theta), v0 * Sin(theta), 0.0, 0.0)
          => TimeEnd = 0.5;
      public override Vector Function(double t, Vector x) => new Vector(0.0, -g, x[0], x[1]);

      public (double x, double y) ExactSolutions(double t) {
        var v0X = InitialValues[0];
        var v0Y = InitialValues[1];
        
        return (v0X * t, -0.5 * g * t * t + v0Y * t);
      }
    }
    
    static void Main() {
      using (var simFile = new StreamWriter(@"sim.csv")) {
        var model = new Projection();
        var t = model.TimeStart;
        var te = model.TimeEnd;
        var x_e = model.InitialValues;
        var x_h = model.InitialValues;
        var x_r = model.InitialValues;
        
        Solver.Delta = 0.01;
        simFile.WriteLine("time,x_e,y_e,x_h,y_h,x_r,y_r,x,y");
        var pos = model.ExactSolutions(t);
        simFile.WriteLine($"{t},{x_e[2]},{x_e[3]},{x_h[2]},{x_h[3]},{x_r[2]},{x_r[3]},{pos.x},{pos.y}");
        while (t + Solver.Delta <= te) {
          x_e += Solver.EulerUpdate(model, t, x_e);
          x_h += Solver.HeunUpdate(model, t, x_h);
          x_r += Solver.Rk4Update(model, t, x_r);
          t += Solver.Delta;
          pos = model.ExactSolutions(t);
          simFile.WriteLine($"{t},{x_e[2]},{x_e[3]},{x_h[2]},{x_h[3]},{x_r[2]},{x_r[3]},{pos.x},{pos.y}");
        }
      }
    }
  }

}