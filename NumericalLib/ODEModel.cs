namespace NumericalLib {

  public abstract class OdeModel {
    public Vector InitialValues { get; }
    public double TimeStart     { get; set; }
    public double TimeEnd       { get; set; } = 1.0;

    public OdeModel(params double[] initialValues) => InitialValues = new Vector(initialValues);
    public abstract Vector Function(double t, Vector x);
  }

}