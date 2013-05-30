using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCR
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public  void SetForm(int formWidth, int formHeight)
        {
            this.Width = formWidth;
            this.Height = formHeight;
        }

        public void  SetImage(Image img)
        {
            pictureBox1.Image = img;
        }

    }
}
