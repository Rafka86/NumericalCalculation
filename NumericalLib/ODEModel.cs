using System.Numerics;

namespace NumericalLib {

  public abstract class OdeModel<T> where T : struct {
    public Vector<T> InitialValues { get; }
    public double    TimeStart     { get; set; }
    public double    TimeEnd       { get; set; } = 1.0;

    public OdeModel(params T[] initialValues) => InitialValues = new Vector<T>(initialValues);
    public abstract Vector<T> Function(double t, Vector<T> x);
  }

}