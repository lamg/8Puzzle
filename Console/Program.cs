using System;
using Puzzle;

namespace Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var m = Inicial (4, 4);
			Imprimir (m);
			System.Console.WriteLine (Puzzle.Solver.Ordenada(m));
			bool ok;
			var n = Puzzle.Solver.Regar(m, out ok);
			System.Console.WriteLine (ok);
			if (ok) {
				System.Console.WriteLine (Puzzle.Solver.Ordenada(n));
				Imprimir (n);
				var cam = Puzzle.Solver.SolDFS (n);
				for (int i = 0; cam != null && i < cam.Length; i++) {
					Imprimir (cam[i]);
				}
			}
		}

		static void Imprimir(Matriz m){
			for (int i = 0; i < m.GetLength(0); i++) {
				for (int j = 0; j < m.GetLength(1); j++) {
					System.Console.Write (" {0}", m[i,j]);
				}
				System.Console.WriteLine ();
			}
			System.Console.WriteLine ();
		}

		static Matriz Inicial(int m, int n){
			var r = new Matriz (new int[n, m], 3, 3, 4);
			for (int i = 0; i < m; i++) {
				for (int j = 0; j < n; j++) {
					r [i, j] = m*i + j;
				}
			}
			return r;
		}
	}
}
