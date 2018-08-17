namespace NumericalLib {

  public abstract class NumericalModelWithODE {
    public Vector InitialValues { get; }
    public double TimeStart { get; set; } = 0.0;
    public double TimeEnd { get; set; } = 1.0;
    
    public NumericalModelWithODE(params double[] initialValues) => InitialValues = new Vector(initialValues);
    public abstract Vector Function(double t, Vector x);
  }

}