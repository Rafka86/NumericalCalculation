using System.Numerics;

namespace NumericalLib {

  public struct Vector {
    internal Vector<double> Elements;
    public int Length { get; }
    private readonly double[] _pArray;

    public Vector(int size) {
      Elements = new Vector<double>(new double[size]);
      Length = size;
      _pArray = new double[Length];
    }

    public Vector(params double[] elements) {
      Elements = new Vector<double>(elements);
      Length = elements.Length;
      _pArray = new double[Length];
    }

    internal Vector(in Vector<double> vector, int size) {
      Elements = vector;
      Length = size;
      _pArray = new double[Length];
    }

    public double this[int index] => Elements[index];

    public static Vector operator -(Vector v) => new Vector(-v.Elements, v.Length);

    public static Vector operator +(Vector v1, Vector v2) => new Vector(v1.Elements + v2.Elements, v1.Length);

    public static Vector operator -(Vector v1, Vector v2) => new Vector(v1.Elements - v2.Elements, v1.Length);

    public static Vector operator *(double k, Vector v) => new Vector(k * v.Elements, v.Length);

    public static Vector operator *(Vector v, double k) => new Vector(v.Elements * k, v.Length);

    public static Vector operator /(Vector v, double k) => new Vector(v.Elements * (1.0 / k), v.Length);

    public override string ToString() => ToString(' ');
    public string ToString(char delimiter) {
      Elements.CopyTo(_pArray);
      return string.Join(delimiter, _pArray);
    }
    public string ToString(string delimiter) {
      Elements.CopyTo(_pArray);
      return string.Join(delimiter, _pArray);
    }
  }

}