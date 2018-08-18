using System.IO;
using System.Linq;

using NumericalLib;

using static System.Math;

namespace DoublePendulum {

  internal class Program {
    private class DoublePendulum : NumericalModelWithODE {
      private const double g = 9.80665;

      private const double m1 = 1.0;
      private const double m2 = 1.0;
      private const double m12 = m1 + m2;
      public const double l1 = 1.0;
      public const double l2 = 1.0;

      public DoublePendulum(double theta1 = 90.0, double theta2 = 90.0)
        : base(theta1 * PI / 180.0, theta2 * PI / 180.0, 0.0, 0.0)
          => TimeEnd = 60.0;
      
      public override Vector Function(double t, Vector x) {
        var sin = Sin(x[0] - x[1]);
        var cos = Cos(x[0] - x[1]);

        return new Vector(x[2], x[3], DTheta1(), DTheta2());
        
        double DTheta1()
          => (g * (Sin(x[1]) * cos - m12 / m2 * Sin(x[0])) - (l1 * x[2] * x[2] * cos + l2 * x[3] * x[3]) * sin) /
             (l1 * (m12 / m2 - cos * cos));

        double DTheta2()
          => (g * m12 / m2 * (cos   * Sin(x[0]) - Sin(x[1]))
             + (m12 / m2 * l1 * x[2] * x[2] + l2 * x[3] * x[3] * cos) * sin)
             / (l2 * (m12 / m2 - cos * cos));
      }
    }
    static void Main(string[] args) {
      var model = new DoublePendulum();
      using (var sw = new StreamWriter("dp.csv")) Solver.Rk4(model, sw, ',');
      using (var sr = new StreamReader("dp.csv"))
      using (var sw = new StreamWriter("dp_c.csv"))
        ConvertToPositions(sr, sw);

      void ConvertToPositions(StreamReader input, StreamWriter output) {
        const double l1 = DoublePendulum.l1;
        const double l2 = DoublePendulum.l2;
        var sin = Sin(-0.5 * PI);
        var cos = Cos(-0.5 * PI);
        output.WriteLine("time,x1,y1,x2,y2");
        while (!input.EndOfStream) {
          var data = input.ReadLine().Split(',').Select(double.Parse).ToArray();
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