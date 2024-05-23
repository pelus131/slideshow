using System.IO;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;
using System;
using System.Security;
using System.Printing
namespace Slideshow
{
    public partial class Form1 : Form
    {
        List<string> Imagefiles = new List<string>();
        int imageCount = 0;
        int addcount = -1;
        PictureBox[] pictureboxes = new PictureBox[12];
        OpenFileDialog openFileDialog1 = new OpenFileDialog();



        public Form1()
        {
            InitializeComponent();
            pictureboxes[0] = pictureBox2; pictureboxes[1] = pictureBox3; pictureboxes[2] = pictureBox4;
            pictureboxes[3] = pictureBox5; pictureboxes[4] = pictureBox6; pictureboxes[5] = pictureBox7;
            pictureboxes[6] = pictureBox8; pictureboxes[7] = pictureBox9; pictureboxes[8] = pictureBox10;
            pictureboxes[9] = pictureBox11; pictureboxes[10] = pictureBox12; pictureboxes[11] = pictureBox13;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeOpenFileDialog();

        }

        private void explore_Click(object sender, EventArgs e)
        {

            if (Imagefiles.Count > 0)
            {
                Imagefiles = new List<string>();
                dataGridView1.DataSource = null;

            }
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
            table.Columns.Add("Image", typeof(Image));


            for (int i = 0; i < Imagefiles.Count; i++)
            {
                FileInfo file = new FileInfo(Imagefiles[i]);

                DataRow datarow = table.NewRow();
                Image img = Image.FromFile(file.FullName);
                datarow[0] = file.Name;
                datarow[1] = file.FullName;
                datarow[2] = img;
                table.Rows.Add(datarow);

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
            foreach (string file in files)
            {
                if (file.EndsWith(".jpg") || file.EndsWith(".jpeg"))
                {
                    Imagefiles.Add(file);
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
                string nextImageRoute = Imagefiles[imageCount + 1];
                pictureBox1.ImageLocation = nextImageRoute;
                imageCount += 1;
                route.Text = nextImageRoute;
                dataGridView1.ClearSelection();
                dataGridView1.Rows[imageCount].Selected = true;

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
                string prevImageRoute = Imagefiles[imageCount - 1];
                pictureBox1.ImageLocation = prevImageRoute;
                imageCount -= 1;
                route.Text = prevImageRoute;
                dataGridView1.ClearSelection();
                dataGridView1.Rows[imageCount].Selected = true;


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SearchInput = textBox1.Text;

            var matchpath = Imagefiles.Find(value => value.Contains(SearchInput));

            route.Text = matchpath;
            pictureBox1.ImageLocation = matchpath;


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

            route.Text = dataGridView1.SelectedCells[1].Value.ToString();
            pictureBox1.ImageLocation = route.Text;
            imageCount = Int32.Parse(dataGridView1.CurrentCell.RowIndex.ToString());
        }

        private void save_Click(object sender, EventArgs e)
        {
            //Change the destination directory later
            string destinationDirectory = "c:\\myDestinationFolder\\";

            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                // Read the files
                foreach (String file in openFileDialog1.FileNames)
                {
                    try
                    {
                        File.Copy(file, destinationDirectory + Path.GetFileName(file),true);
                    }
                    catch (SecurityException ex)
                    {
                        // The user lacks appropriate permissions to read files, discover paths, etc.
                        MessageBox.Show("Security error.\n\n" +
                            "Error message: " + ex.Message + "\n\n" +
                            "Details:\n\n" + ex.StackTrace
                        );
                    }
                    catch (Exception ex)
                    {
                        // Could not load the image in general
                        MessageBox.Show("Cannot display the image: " + file.Substring(file.LastIndexOf('\\'))
                            + ". You may not have permission to read the file, or " +
                            "it may be corrupt.\n\nReported error: " + ex.Message);
                    }
                }
            }
        }
        private void InitializeOpenFileDialog()
        {
            // Filter
            this.openFileDialog1.Filter =
                "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF,*.PNG|" +
                "All files (*.*)|*.*";

                   this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select the Image(s) to Save ";
            //Customizable path to start the browse
            this.openFileDialog1.InitialDirectory = "c:\\";
        }

        

    }
}
