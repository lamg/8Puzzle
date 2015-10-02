using System;

namespace Puzzle
{
	public class Matriz:IDisposable
	{
		int[,] m;
		int hi, hj, nHijo;
		Matriz[] hijos;
		Matriz padre;

		public Matriz (int[,] m, int i, int j, int nHijo)
		{
			this.m = m;
			this.hi = i;
			this.hj = j;
			this.nHijo = nHijo;
		}

		public Matriz Padre {
			get {
				return padre;
			}
			set {
				padre = value;
			}
		}

		public Matriz[] Hijos {
			get {
				return hijos;
			}
			set {
				hijos = value;
			}
		}

		public int GetLength(int d){
			return m.GetLength (d);
		}

		public int Hi{
			get{ return hi;}
			set { hi = value;}
		}

		public int Hj{
			get { return hj;}
			set { hj = value;}
		}

		public int NHijo {
			get {
				return nHijo;
			}
			set {
				nHijo = value;
			}
		}

		public int this[int i, int j]{
			get{ return m [i, j];}
			set { m [i, j] = value;}
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			if (hijos != null) {
				foreach (var i in Hijos) {
					i.Padre = null;
					i.Dispose ();
				}
				hijos = null;
			}

		}

		#endregion
	}
}

