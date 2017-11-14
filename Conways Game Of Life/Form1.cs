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
        Graphics gfx;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            linePen = new Pen(System.Drawing.Color.Black);
            gfx = this.CreateGraphics();

            CGOLBoard board = new CGOLBoard(5,5);
        }

        protected void drawGrid()
        {
            int canvasWidth = ClientSize.Width;
            int canvasHeight = ClientSize.Height;

            int xPadding = 50;
            int yPadding = 50;

            float gridWidth = canvasWidth - (2 * xPadding);
            float gridHeight = canvasHeight - (2 * yPadding);

            for (int i = 0; i < 21; ++i)
            {

                gfx.DrawLine(
                    linePen,
                    xPadding + ((gridWidth / 20.0f) * i),
                    yPadding,
                    xPadding + ((gridWidth / 20.0f) * i),
                    yPadding + gridHeight
                    );

                gfx.DrawLine(
                    linePen,
                    xPadding,
                    yPadding + ((gridHeight / 20.0f) * i),
                    xPadding + gridWidth,
                    yPadding + ((gridHeight / 20.0f) * i)
                    );
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            gfx.Clear(this.BackColor);
            drawGrid();
           
            base.OnPaint(e);
        }
    }
}
