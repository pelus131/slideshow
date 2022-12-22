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
    }
}