using System.IO;
using System.Numerics;

using NumericalLib;

using static System.Math;


namespace Projection {

  class Program {
    private class Projection : OdeModel<double> {
      private const double G = 9.80665;

      public Projection(double v0 = 5.0, double theta = 0.25 * PI)
        : base(v0 * Cos(theta), v0 * Sin(theta), 0.0, 0.0)
          => TimeEnd = 0.5;

      public override Vector<double> Function(double t, Vector<double> x)
        => new Vector<double>(new[] {0.0, -G, x[0], x[1]});

      public (double x, double y) ExactSolutions(double t) {
        var v0X = InitialValues[0];
        var v0Y = InitialValues[1];
        
        return (v0X * t, -0.5 * G * t * t + v0Y * t);
      }
    }
    
    static void Main() {
      using (var simFile = new StreamWriter(@"sim.csv")) {
        var model = new Projection();
        var t = model.TimeStart;
        var te = model.TimeEnd;
        var xE = model.InitialValues;
        var xH = model.InitialValues;
        var xR = model.InitialValues;
        
        Solver<double>.Delta = 0.01;
        simFile.WriteLine("time,x_e,y_e,x_h,y_h,x_r,y_r,x,y");
        var pos = model.ExactSolutions(t);
        simFile.WriteLine($"{t},{xE[2]},{xE[3]},{xH[2]},{xH[3]},{xR[2]},{xR[3]},{pos.x},{pos.y}");
        while (t + Solver<double>.Delta <= te) {
          xE += Solver<double>.EulerUpdate(model, t, xE);
          xH += Solver<double>.HeunUpdate(model, t, xH);
          xR += Solver<double>.Rk4Update(model, t, xR);
          t += Solver<double>.Delta;
          pos = model.ExactSolutions(t);
          simFile.WriteLine($"{t},{xE[2]},{xE[3]},{xH[2]},{xH[3]},{xR[2]},{xR[3]},{pos.x},{pos.y}");
        }
      }
    }
  }

}