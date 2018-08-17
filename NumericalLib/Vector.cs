namespace NumericalLib {

  public struct Vector {
    private double[] e;
    public int Length => e.Length;

    public Vector(int size) => e = new double[size];
    public Vector(params double[] elements) => e = elements;

    public double this[int index] {
      get => e[index];
      set => e[index] = value;
    }

    public static Vector operator -(Vector v) {
      var res = new Vector(v.Length);
      for (var i = 0; i < res.Length; i++) res[i] = -v[i];
      return res;
    }
    
    public static Vector operator +(Vector v1, Vector v2) {
      var res = new Vector(v1.Length);
      for (var i = 0; i < res.Length; i++) res[i] = v1[i] + v2[i];
      return res;
    }

    public static Vector operator -(Vector v1, Vector v2) {
      var res = new Vector(v1.Length);
      for (var i = 0; i < res.Length; i++) res[i] = v1[i] - v2[i];
      return res;
    }

    public static Vector operator *(double k, Vector v) {
      var res = new Vector(v.Length);
      for (var i = 0; i < res.Length; i++) res[i] = k * v[i];
      return res;
    }

    public static Vector operator *(Vector v, double k) => k * v;

    public static Vector operator /(Vector v, double k) {
      var res = new Vector(v.Length);
      for (var i = 0; i < res.Length; i++) res[i] = v[i] / k;
      return res;
    }

    public override string ToString() => ToString(' ');
    public string ToString(char delimiter) => string.Join(delimiter, e);
  }

}