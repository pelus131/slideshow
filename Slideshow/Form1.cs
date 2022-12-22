using System.IO;
using System.Drawing.Printing;

namespace Slideshow
{
    public partial class Form1 : Form
    {
        List<string> Imagefiles = new List<string>();
        int imageCount = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void explore_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    findImagesInDirectory(fbd.SelectedPath);
                }
            }

        }
        private void findImagesInDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string s in files)
            {
                if (s.EndsWith(".jpg") || s.EndsWith(".jpeg")) 
                {
                    Imagefiles.Add(s);
                }
            }
            try
            {
                pictureBox1.ImageLocation = Imagefiles.First();
            }
            catch { MessageBox.Show("No files found!"); }

        }

        private void next_Click(object sender, EventArgs e)
        {

            if (imageCount + 1 == Imagefiles.Count)
            {
                MessageBox.Show("No Other Images!");
            }
            else
            {
                string nextImage = Imagefiles[imageCount + 1];
                pictureBox1.ImageLocation = nextImage;
                imageCount += 1;
                route.Text = nextImage;
            }
            
        }

        private void prev_Click(object sender, EventArgs e)
        {

            if (imageCount == 0)
            {
                MessageBox.Show("No Other Images!");
            }
            else
            {
                string prevImage = Imagefiles[imageCount - 1];
                pictureBox1.ImageLocation = prevImage;
                imageCount -= 1;
                route.Text = prevImage;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string var=textBox1.Text;
            // String[] value = Array.Find(Imagefiles, element => element.StartsWith(var, StringComparison.Ordinal));

            var match = Imagefiles.FirstOrDefault(value => value.Contains(var));
            route.Text = match;
            pictureBox1.ImageLocation = match;
        }

        private void print_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += Doc_PrintPage;
            pd.Document = doc;
            if (pd.ShowDialog() == DialogResult.OK)
            {

                doc.Print();
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Pinche codigo para reescalar o imprimir original,seguir investigando reescalas
            //Bitmap bm = new Bitmap(imageViewer.Width, imageViewer.Height);
            // imageViewer.DrawToBitmap(bm, new Rectangle(0, 0, imageViewer.Width, imageViewer.Height));
            //****(bm,0,0)
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0);
            // bm.Dispose();
        }
    }
}