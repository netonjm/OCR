using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using XBSlink.Translator;

namespace OCR
{
    public partial class Form1 : Form
    {

        private const int WindowTopBar = 31;
        private const int WindowLeftBar = 6;
        //private int WindowToolStripButtonStatusTopBar = 25;

        private readonly PickUpClass _pickUp;

        readonly Tesseract _ocRz = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_ONLY);
        public Image ActualImage { get { return pictureBox1.Image; } }

#if DEBUG
        private readonly Form2 _imageVisor;
#endif
        public Form1()
        {
            InitializeComponent();

            _pickUp = new PickUpClass();

#if DEBUG
            _imageVisor = new Form2();
            _imageVisor.Show();
#endif
      
        }


        #region Common Methods

        void ClearPictureBox()
        {
            Graphics graphics1 = pictureBox1.CreateGraphics();
            graphics1.Clear(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0))))));
            graphics1.Dispose();
        }

        Rectangle? DrawReturnRectangleOnPictureBox()
        {
            if (_pickUp.MousePosition != null)
            {
                var actualRectange = _pickUp.GetRectangle(_pickUp.MousePosition.Value);

                Graphics graphics1 = pictureBox1.CreateGraphics();
                if (actualRectange != null)
                    graphics1.DrawRectangle(_pickUp.GetPenColor(), new Rectangle(actualRectange.Value.X - 1, actualRectange.Value.Y - 1, actualRectange.Value.Width + 1, actualRectange.Value.Height + 1));

                return actualRectange;
            }
            return null;
        }

        Bitmap CreateImageFromRectangleArea(Rectangle? actualRectange)
        {
            if (actualRectange != null && (actualRectange.Value.Width > 10 && actualRectange.Value.Height > 10))
            {

                 if (_pickUp.InitialPoint != null)
                 {
                     Bitmap _temporalPicture = new Bitmap(actualRectange.Value.Width, actualRectange.Value.Height);
                     Graphics _temporalGraphics = Graphics.FromImage(_temporalPicture);
                     _temporalGraphics.CopyFromScreen(GetPoint(_pickUp.InitialPoint.Value), new Point(0, 0), _temporalPicture.Size);
                     _temporalGraphics.Dispose();
                     return _temporalPicture;
                 }
            }
            return null;
        }

        void CreateImageFromRectangleAreaAndTranslate(Rectangle? actualRectange)
        {
            if (actualRectange != null && (actualRectange.Value.Width > 10 && actualRectange.Value.Height > 10))
            {

                var temporalPicture = CreateImageFromRectangleArea(actualRectange);
                if (temporalPicture != null)
                {
#if DEBUG
                    _imageVisor.SetImage(temporalPicture);
#endif
                    RecognizeText(temporalPicture);
                }

            }
        }

        void DrawCompletePictureBox()
        {

            ClearPictureBox();

            if (_pickUp.MousePosition != null)
            {
                //Draw rectangle on PictureBox and return
                var actualRectange = DrawReturnRectangleOnPictureBox();

                CreateImageFromRectangleAreaAndTranslate(actualRectange);
            }

        }

        void RecognizeText(Bitmap image)
        {
            using (Image<Gray, byte> gray = new Image<Gray, Byte>(image))
            {
                _ocRz.Recognize(gray);
                textBox1.Text = _ocRz.GetText().Replace(Environment.NewLine, " ");
            }


        }

        void TranslateText()
        {
            textBox2.Text = TranslatorEngine.TranslateText(textBox1.Text, "English", "Spanish");
        }

        Point GetPoint(Point test)
        {

            return new Point(this.Left + WindowLeftBar + test.X, this.Top + WindowTopBar + toolStrip1.Height + test.Y);
        }

        #endregion

        #region FormMethods

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            _pickUp.InitialPoint = e.Location;
            _pickUp.IsDrawing = true;
            ClearPictureBox();

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _pickUp.IsDrawing = false;
            TranslateText();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            _pickUp.MousePosition = e.Location;

            if (_pickUp.IsDrawing && _pickUp.InitialPoint.HasValue)
            {
                DrawCompletePictureBox();

            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _pickUp.Reset();
                ClearPictureBox();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            toolStripButton2.Checked = !toolStripButton2.Checked;
            this.TopMost = toolStripButton2.Checked;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            TranslateText();
        }


        #endregion

      

    }
}
