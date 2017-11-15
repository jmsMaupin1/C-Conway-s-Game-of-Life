using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conways_Game_Of_Life
{
    public partial class Form1 : Form
    {
        Pen linePen;
        SolidBrush cellBrush;
        Graphics gfx;

        // Number of horizontal cells
        int gridLength = 10;

        CGOLBoard board;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            linePen = new Pen(System.Drawing.Color.Black);
            cellBrush = new SolidBrush(Color.Black);
            gfx = this.CreateGraphics();

            board = new CGOLBoard(10, 10, new bool[] {
                true, false, true, false, false, false, false, false, false, false,
                false, true, true, false, false, false, false, false, false, false,
                false, true, false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false, false, false,
            });
        }

        public void drawCell(int i)
        {
            int cellX = (i % gridLength);
            int cellY = (i / gridLength);
            gfx.FillRectangle(cellBrush, 50 + cellX * 20, 50 + cellY * 20, 20, 20);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            gfx.Clear(this.BackColor);
            int cellCount = board.GetCellCount();

            for(int i = 0; i < cellCount; ++i)
            {
                if (board.GetDispCellState(i))
                    drawCell(i);
            }
            base.OnPaint(e);
        }

        private void btnNextGeneration_Click(object sender, EventArgs e)
        {
            board.UpdateDisplay();
        }
    }
}
