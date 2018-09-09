using System.IO;
using System.Linq;

using NumericalLib;

using static System.Math;

namespace DoublePendulum {

  internal class Program {
    private class DoublePendulum : OdeModel {
      private const double G = 9.80665;

      private const double M1 = 1.0;
      private const double M2 = 1.0;
      private const double M12 = M1 + M2;
      public const double L1 = 1.0;
      public const double L2 = 1.0;

      public DoublePendulum(double theta1 = 90.0, double theta2 = 90.0)
        : base(theta1 * PI / 180.0, theta2 * PI / 180.0, 0.0, 0.0)
          => TimeEnd = 60.0;
      
      public override Vector Function(double t, Vector x) {
        var sin = Sin(x[0] - x[1]);
        var cos = Cos(x[0] - x[1]);

        return new Vector(x[2], x[3], DTheta1(), DTheta2());
        
        double DTheta1()
          => (G * (Sin(x[1]) * cos - M12 / M2 * Sin(x[0])) - (L1 * x[2] * x[2] * cos + L2 * x[3] * x[3]) * sin) /
             (L1 * (M12 / M2 - cos * cos));

        double DTheta2()
          => (G * M12 / M2 * (cos   * Sin(x[0]) - Sin(x[1]))
             + (M12 / M2 * L1 * x[2] * x[2] + L2 * x[3] * x[3] * cos) * sin)
             / (L2 * (M12 / M2 - cos * cos));
      }
    }
    static void Main() {
      Solver.Delta = 0.015625;
      var model = new DoublePendulum();
      using (var sw = new StreamWriter("dp.csv")) Solver.Rk4(model, sw, ',');
      using (var sr = new StreamReader("dp.csv"))
      using (var sw = new StreamWriter("dp_c.csv"))
        ConvertToPositions(sr, sw);

      void ConvertToPositions(StreamReader input, TextWriter output) {
        const double l1 = DoublePendulum.L1;
        const double l2 = DoublePendulum.L2;
        var sin = Sin(-0.5 * PI);
        var cos = Cos(-0.5 * PI);
        output.WriteLine("time,x1,y1,x2,y2");
        while (!input.EndOfStream) {
          var data = input.ReadLine()?.Split(',').Select(double.Parse).ToArray();
          if (data == null) break;
          var x1 = l1 * Cos(data[1]);
          var y1 = l2 * Sin(data[1]);
          var x2 = l2 * Cos(data[2]) + x1;
          var y2 = l2 * Sin(data[2]) + y1;
          (x1, y1) = (x1 * cos - y1 * sin, x1 * sin + y1 * cos);
          (x2, y2) = (x2 * cos - y2 * sin, x2 * sin + y2 * cos);
          output.WriteLine($"{data[0]},{x1},{y1},{x2},{y2}");
        }
      }
    }
  }

}