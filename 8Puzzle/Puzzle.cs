using System;
using System.Collections.Generic;

namespace Puzzle
{
	public class Solver
	{

		const int NHijos = 4, Profundidad = 31;

		static bool DFS(Matriz raiz, ref List<Matriz> sol, int depth, int prof){
			if (depth == prof || Ordenada(raiz)) {
				return depth != prof;
			}
			var hs = Hijos (raiz);
			foreach (var item in hs) {			
				if (sol.Count < 2 || !Iguales(item, sol[sol.Count-2])) {
					sol.Add (item);
					if (DFS (item, ref sol, depth + 1, prof)) {
						return true;
					}
					sol.RemoveAt (sol.Count - 1);
				}
			}
			return false;
		}

		public static Matriz[] SolDFS(Matriz m){
			var ls = new List<Matriz>(Profundidad);
			bool sol = false;
			ls.Add (m);
			for (int i = 0; !sol && i < Profundidad; i++) {
				sol = DFS (m, ref ls, 0, i);
			}
			return ls.ToArray ();
		}

		static Matriz[] HastaRaiz(Matriz m){
			var r = new List<Matriz> ();
			bool ok = true;
			while (m != null && ok) {
				r.Add (Copiar(m,out ok));
				m = m.Padre;
			}
			return r.ToArray ();
		}

		public static Matriz[] SolBFS(Matriz m){
			var q = new Queue<Matriz> ();
			//var st = new Stack<Matriz> ();
			q.Enqueue (m);
			bool ord = false;
			Matriz t = null;
			while (!ord && q.Count != 0) {
				if (t != null) {
					t.Dispose ();
					t = null;
				}
				t = q.Dequeue ();
				var hs = Hijos (t);
				foreach (var item in hs) {
					item.Padre = t;
					if (item.Padre.Padre == null || !Iguales(item.Padre.Padre, item)) {
						q.Enqueue (item);
					}
				}
				ord = Ordenada (t);
			}

			var r = HastaRaiz(t);
			Array.Reverse (r);
			// limpiando basura
			foreach (var item in q) {
				item.Dispose ();
			}


			t.Dispose ();
			GC.Collect (GC.GetGeneration (t), GCCollectionMode.Forced);
			t = null;
			// fin de la limpia
			return r;
		}

		static Matriz[] Hijos(Matriz m) {
			var r = new List<Matriz> ();
			bool ok = true;
			for (int i = 0; ok && i < NHijos; i++) {
				var n = Movimiento (m, i, out ok);
				if (ok) {
					r.Add (n);
					i = n.NHijo;
				}
			}
			return r.ToArray ();
		}

		public static Matriz Movimiento(Matriz m, int i, out bool ok){
			Matriz r = null;
			int[,] movs = new int[NHijos,2]{{-1,0},{0,-1},{0,1},{1,0}};
			ok = false;
			if (m != null) {
				while (!ok && 0 <= i && i < movs.GetLength (0)) {
					r = IntercambiaHueco (m, m.Hi + movs [i, 0], m.Hj + movs [i, 1], out ok);
					if (!ok) {
						i++;
					} else {
						r.NHijo = i;
					}
				}
			}
			return r;
		}

		static Matriz IntercambiaHueco(Matriz m, int k, int l, out bool ok){
			Matriz r = null;
			ok = 0 <= k && k < m.GetLength (0) && 0 <= l && l < m.GetLength (1) && m != null;
			if (ok) {
				r = Copiar (m, out ok);
				var t = r [m.Hi, m.Hj];
				r [m.Hi, m.Hj] = r [k, l];
				r [k, l] = t;
				r.Hi = k;
				r.Hj = l;
			}
			return r;
		}

		static Matriz Copiar(Matriz m, out bool ok){
			Matriz r = null;
			ok = m != null;
			if (ok) {
				var a = new int[m.GetLength (0), m.GetLength (1)];
				for (int i = 0; i < m.GetLength(0); i++) {
					for (int j = 0; j < m.GetLength(1); j++) {
						a [i, j] = m [i, j];
					}
				}
				r = new Matriz (a, m.Hi, m.Hj, m.NHijo);
			}
			return r;
		}

		public static bool Ordenada(Matriz m){
			bool ord = true && m != null;
			int ant = 0;
			for (int i = 0; ord && i < m.GetLength(0)*m.GetLength(1); i++) {
				ord = ord && (ant <= m [i / m.GetLength (0), i % m.GetLength (1)]);
				ant = m [i / m.GetLength (0), i % m.GetLength (1)];
			}
			return ord;
		}

		public static bool Iguales(Matriz m, Matriz n){
			bool eq = true && m != null && n != null;
			for (int i = 0; eq && i < m.GetLength(0); i++) {
				for (int j = 0; eq && j < m.GetLength(1); j++) {
					eq = eq && m [i, j] == n [i, j];
				}
			}
			return eq;
		}

		public static Matriz Regar(Matriz m, out bool ok){
			var dm = new Random ();
			Matriz r = m;
			ok = true;
			r = Movimiento (r, 0, out ok);

			for (int i = 0; i < Profundidad; i++) {
				var rm = dm.Next (4);
				var x = Movimiento (r, rm, out ok);
				if (ok)
					r = x;
			}
			ok = r != null;
			return r;
		}

		public static Matriz Inicial(int m, int n){
			var r = new Matriz (new int[m, n], m-1, n-1, 4);
			for (int i = 0; i < m; i++) {
				for (int j = 0; j < n; j++) {
					r [i, j] = m*i + j;
				}
			}
			return r;
		}
	}
}

