using System;
using System.IO;

namespace NumericalLib {

  public static class Solver {
    private enum NumericalMethod {
      Euler,
      Heun,
      Rk4
    }

    private const double Frac6 = 1.0 / 6.0;
    private static double _delta = 0.1;
    private static double _halfDelta = _delta * 0.5;
    public static double Delta {
      get => _delta;
      set {
        _delta = value;
        _halfDelta = value * 0.5;
      }
    }

    public static void Euler(NumericalModelWithODE model)
      => SolveAndPrint(NumericalMethod.Euler, model, Console.Out);
    public static void Euler(NumericalModelWithODE model, TextWriter file)
      => SolveAndPrint(NumericalMethod.Euler, model, file);

    public static void Heun(NumericalModelWithODE model)
      => SolveAndPrint(NumericalMethod.Heun, model, Console.Out);
    public static void Heun(NumericalModelWithODE model, TextWriter file)
      => SolveAndPrint(NumericalMethod.Heun, model, file);

    public static void Rk4(NumericalModelWithODE model)
      => SolveAndPrint(NumericalMethod.Rk4, model, Console.Out);
    public static void Rk4(NumericalModelWithODE model, TextWriter file)
      => SolveAndPrint(NumericalMethod.Rk4, model, file);

    private delegate Vector UpdateFunction(NumericalModelWithODE model, double t, Vector x);
    
    public static Vector EulerUpdate(NumericalModelWithODE model, double t, Vector x)
      => model.Function(t, x) * _delta;

    public static Vector HeunUpdate(NumericalModelWithODE model, double t, Vector x) {
      var k1 = model.Function(t, x);
      var k2 = model.Function(t + _delta, x + k1 * _delta);
      return (k1 + k2) * _delta * 0.5;
    }

    public static Vector Rk4Update(NumericalModelWithODE model, double t, Vector x) {
      var k1 = model.Function(t, x);
      var k2 = model.Function(t + _halfDelta, x + _halfDelta * k1);
      var k3 = model.Function(t + _halfDelta, x + _halfDelta * k2);
      var k4 = model.Function(t + _delta, x + _delta * k3);
      return (k1 + 2.0 * k2 + 2.0 * k3 + k4) * _delta * Frac6;
    }
    
    private static void SolveAndPrint(NumericalMethod method, NumericalModelWithODE model, TextWriter file) {
      var t = model.TimeStart;
      var te = model.TimeEnd;
      var x = model.InitialValues;

      UpdateFunction updFunc = EulerUpdate;
      if (method == NumericalMethod.Heun) updFunc = HeunUpdate;
      if (method == NumericalMethod.Rk4)  updFunc = Rk4Update;

      file.WriteLine($"{t} {x}");
      while (t + _delta <= te) {
        x += updFunc(model, t, x);
        t += _delta;
        file.WriteLine($"{t} {x}");
      }
    }
  }

}