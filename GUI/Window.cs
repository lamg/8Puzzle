using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Puzzle;

namespace GUI
{
	public class MWindow:Form
	{
		Matriz[] sol;
		int curr, rows, hsq, wsq, xlett, ylett;
		PictureBox pbox;
		Button back, next, init;
		Label info;
		Font nfont;
		ComboBox selAlg;
		string[] algs = new string[]{"DFS", "BFS"};
		NumericUpDown selSize;

		private System.ComponentModel.IContainer components = null;

		public MWindow ()
		{

			rows = 3;
			pbox = new PictureBox ();
			back = new Button ();
			next = new Button ();
			init = new Button ();
			info = new Label ();
			selAlg = new ComboBox ();
			selSize = new NumericUpDown ();
			((System.ComponentModel.ISupportInitialize)(this.pbox)).BeginInit();
			SuspendLayout ();

			selSize.Size = new Size (30, 20);
			selSize.Location = new Point (190, 330);
			selSize.Value = rows;
			selSize.ValueChanged += (sender, e) => {
				rows = (int)selSize.Value;
				hsq = pbox.Height / rows;
				wsq = pbox.Width / rows;
				xlett = wsq / 5;
				ylett = hsq / 5;
				nfont = new Font (FontFamily.GenericMonospace, hsq/2);
			};

			selAlg.Size = new Size (60, 20);
			selAlg.Location = new Point (230, 330);
			selAlg.Items.AddRange (algs);
			selAlg.SelectedIndex = 0;

			pbox.Size = new Size (400, 300);
			pbox.Location = new Point (40, 10);
			pbox.Paint += HandlePaint;
			hsq = pbox.Height / rows;
			wsq = pbox.Width / rows;
			xlett = wsq / 5;
			ylett = hsq / 5;
			nfont = new Font (FontFamily.GenericMonospace, hsq/2);

			back.Size = new Size (60, 20);
			back.Location = new Point (10, 330); 
			back.Text = "Atras";
			back.Click += (object sender, EventArgs e) => {DrawPrev();};
			back.Enabled = false;

			next.Size = new Size (60, 20);
			next.Location = new Point (80, 330);
			next.Text = "Proximo";
			next.Click += (object sender, EventArgs e) => {DrawNext();};
			next.Enabled = false;

			info.Size = new Size (65, 30);
			info.Location = new Point (305, 310);
			info.Text = "Informacion";


			init.Text = "Inicializar";
			var bw = new BackgroundWorker();
			bw.DoWork += (object snd, DoWorkEventArgs ev) => {
				bool ok;
				var ini = Puzzle.Solver.Inicial(rows,rows);
				var reg = Puzzle.Solver.Regar(ini,out ok);
				if (selAlg.SelectedIndex == 0) {
					sol = Puzzle.Solver.SolDFS(reg);
				} else {
					sol = Puzzle.Solver.SolBFS(reg);
				}


			};
			bw.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) => {
				back.Enabled = true;
				next.Enabled = true;
				curr = 0;
				info.Text = "Solucionado";
				pbox.Refresh();
			};
			init.Size = new Size (65, 20);
			init.Location = new Point (305, 330);
			init.Click += (object sender, EventArgs e) => {
				back.Enabled = false;
				next.Enabled = false;
				if (!bw.IsBusy) {
					if (sol!=null) {
						for (int i = 0; i < sol.Length; i++) {
							sol[i].Dispose();
							sol[i] = null;
						}					
					}

					bw.RunWorkerAsync();
				}

				info.Text = "Espere...";
			};

			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

			Size = new Size (480, 400);
			StartPosition = FormStartPosition.CenterScreen;

			Controls.Add (pbox);
			Controls.Add (back);
			Controls.Add (next);
			Controls.Add (init);
			Controls.Add (info);
			Controls.Add (selAlg);
			Controls.Add (selSize);
			ResumeLayout (false);
			PerformLayout ();

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}



		void HandlePaint (object sender, PaintEventArgs e)
		{			
			if (sol != null && 0 <= curr && curr < sol.Length && sol[curr] != null) {
				

				for (int i = 0; i < sol[curr].GetLength(0); i++) {
					for (int j = 0; j < sol[curr].GetLength(1); j++) {
						if (sol [curr] [i, j] == rows * rows - 1) {
							e.Graphics.FillRectangle (Brushes.SlateGray, new Rectangle (j * wsq, i * hsq, wsq-1,hsq-1));
						} else {
							e.Graphics.FillRectangle (Brushes.SteelBlue, new Rectangle (j * wsq, i * hsq, wsq-1,hsq-1));
							e.Graphics.DrawString ((sol [curr] [i, j] + 1).ToString (), nfont,
							                       Brushes.White, new RectangleF (j * wsq+xlett, i * hsq+ylett, wsq-1,hsq-1));
						}
					}
				}
			}
		}

		void DrawPrev ()
		{
			if (curr > 0) {
				curr--;
				pbox.Refresh ();
			}
			info.Text = curr.ToString ();
		}

		void DrawNext ()
		{
			if (curr < sol.Length-1) {
				curr++;
				pbox.Refresh ();
			}
			info.Text = curr.ToString ();
		}
	}
}

