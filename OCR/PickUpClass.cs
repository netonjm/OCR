using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCR
{
    class PickUpClass
    {

        public bool IsDrawing { get; set; }
        public bool IsPressed { get; set; }

        public Color Color = Color.Red;

        public Point? InitialPoint { get; set; }
        public Point? MousePosition { get; set; }
        //public Rectangle Rectangle { get; set; }

        public  Pen GetPenColor()
        {
            return new Pen(Color);
        }

        public  void Reset()
        {
            InitialPoint = null;
            IsDrawing = IsPressed = false;
        }

        public  Rectangle? GetRectangle(Point mousePosition)
        {
            if (InitialPoint.HasValue)
            {
                return new Rectangle(
                    InitialPoint.Value.X,
                    InitialPoint.Value.Y,
                   mousePosition.X - InitialPoint.Value.X ,
                   mousePosition.Y - InitialPoint.Value.Y 
                    );
            }

            return null;

        }

        public PickUpClass()
        {
            
        }

    }
}
