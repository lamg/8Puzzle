using System;

namespace Puzzle
{
	public class Arbol
	{
		Matriz val;
		Matriz[] hijos;

		public Arbol (Matriz val)
		{
			this.val = val;
		}


		public Matriz[] Hijos {
			get {
				return hijos;
			}
			set {
				hijos = value;
			}
		}
	}
}

