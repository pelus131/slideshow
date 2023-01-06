using System.IO;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;

namespace Slideshow
{
    public partial class Form1 : Form
    {
        List<string> Imagefiles = new List<string>();
        int imageCount = 0;
        int addcount = -1;
        PictureBox[] pictureboxes = new PictureBox[12];




        public Form1()
        {
            InitializeComponent();
            pictureboxes[0] = pictureBox2; pictureboxes[1] = pictureBox3; pictureboxes[2] = pictureBox4;
            pictureboxes[3] = pictureBox5; pictureboxes[4] = pictureBox6; pictureboxes[5] = pictureBox7;
            pictureboxes[6] = pictureBox8; pictureboxes[7] = pictureBox9; pictureboxes[8] = pictureBox10;
            pictureboxes[9] = pictureBox11; pictureboxes[10] = pictureBox12; pictureboxes[11] = pictureBox13;
        }

        private void explore_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            Imagefiles = new List<string>();

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    findImagesInDirectory(fbd.SelectedPath);
                }
            }

            DataTable table = new DataTable();
            table.Columns.Add("File Name");
            table.Columns.Add("File path");
            table.Columns.Add("Image",typeof(Image));
            

            for (int i = 0; i < Imagefiles.Count; i++)
            {
                FileInfo file = new FileInfo(Imagefiles[i]);
                
                DataRow dr = table.NewRow();
                Image img = Image.FromFile(file.FullName);
                dr[0] = file.Name;
                dr[1] = file.FullName;
                dr[2] = img;
                table.Rows.Add(dr);
              
            }

            dataGridView1.DataSource = table;
         
            DataGridViewColumn column = dataGridView1.Columns[1];
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ((DataGridViewImageColumn)dataGridView1.Columns[2]).ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.Columns[1].Visible = false;
            foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            string var = textBox1.Text;

            var match = Imagefiles.Find(value => value.Contains(var));

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

        private void add_Click(object sender, EventArgs e)
        {
           
            if (addcount < 11)
            {
                if (route.Text == "")
                { MessageBox.Show("No Image slected!"); }
                else
                {
                    addcount++;

                    pictureboxes[addcount].ImageLocation = route.Text;
                }


            }
            else { MessageBox.Show("All picture boxex  are full!"); }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (addcount >= 0)
            {
                pictureboxes[addcount].Image = null;
                addcount--;
            }
            else { MessageBox.Show("All picture boxes are empty!"); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            route.Text=dataGridView1.SelectedCells[1].Value.ToString();
            pictureBox1.ImageLocation = route.Text;
        }

    }
    }
