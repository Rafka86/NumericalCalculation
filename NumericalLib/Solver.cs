using System;
using System.IO;
using System.Numerics;

namespace NumericalLib {

  public static class Solver<T> where T : struct {
    private enum NumericalMethod {
      Euler,
      Heun,
      Rk4
    }

    private const double Frac6 = 1.0 / 6.0;
    private static readonly T _frac6 = (T) (object) Frac6;
    private static T _delta;
    private static T _halfDelta;
    public static double Delta {
      get => (double) (object) _delta;
      set {
        _delta = (T) (object) value;
        _halfDelta = (T) (object) (value * 0.5);
      }
    }

    public static void Euler(OdeModel<T> model, TextWriter file = null, string delimiter = " ")
      => SolveAndPrint(NumericalMethod.Euler, model, file ?? Console.Out, delimiter);

    public static void Heun(OdeModel<T> model, TextWriter file = null, string delimiter = " ")
      => SolveAndPrint(NumericalMethod.Heun, model, file ?? Console.Out, delimiter);

    public static void Rk4(OdeModel<T> model, TextWriter file = null, string delimiter = " ")
      => SolveAndPrint(NumericalMethod.Rk4, model, file ?? Console.Out, delimiter);

    private delegate Vector<T> UpdateFunction(OdeModel<T> model, double t, Vector<T> x);
    
    public static Vector<T> EulerUpdate(OdeModel<T> model, double t, Vector<T> x)
      => model.Function(t, x) * _delta;

    public static Vector<T> HeunUpdate(OdeModel<T> model, double t, Vector<T> x) {
      var delta = (double) (object) _delta;
      var half = (T) (object) 0.5;
      var k1 = model.Function(t, x);
      var k2 = model.Function(t + delta, x + k1 * _delta);
      return (k1 + k2) * _delta * half;
    }

    public static Vector<T> Rk4Update(OdeModel<T> model, double t, Vector<T> x) {
      var halfDelta = (double) (object) _halfDelta;
      var delta = (double) (object) _delta;
      var dbl = (T) (object) 2.0;
      var k1 = model.Function(t, x);
      var k2 = model.Function(t + halfDelta, x + _halfDelta * k1);
      var k3 = model.Function(t + halfDelta, x + _halfDelta * k2);
      var k4 = model.Function(t + delta, x + _delta * k3);
      return (k1 + dbl * k2 + dbl * k3 + k4) * _delta * _frac6;
    }

    private static void SolveAndPrint(NumericalMethod method, OdeModel<T> model,
                                      TextWriter file, string delimiter) {
      var t  = model.TimeStart;
      var te = model.TimeEnd;
      var x  = model.InitialValues;
      var delta = (double) (object) _delta;

      UpdateFunction updFunc                      = EulerUpdate;
      if (method == NumericalMethod.Heun) updFunc = HeunUpdate;
      if (method == NumericalMethod.Rk4) updFunc  = Rk4Update;

      var str = string.Join(delimiter, x);
      file.WriteLine($"{t}{delimiter}{str.Substring(1, str.Length - 2)}");
      while (t + delta <= te) {
        x += updFunc(model, t, x);
        t += delta;
        str = string.Join(delimiter, x);
        file.WriteLine($"{t}{delimiter}{str.Substring(1, str.Length - 2)}");
      }
    }
  }

}