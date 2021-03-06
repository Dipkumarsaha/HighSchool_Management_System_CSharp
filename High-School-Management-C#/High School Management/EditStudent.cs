using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace High_School_Management
{
    public partial class EditStudent : Form
    {
        SqlConnection conn = new SqlConnection(@"Server=.\SQLEXPRESS;Database=school;Integrated Security=true");
        string imgurl;
        string stID;
        Home h;
        string RadioValue = "Male";
        public EditStudent(string stID,Home h)
        {
            InitializeComponent();
            this.stID = stID;
            this.h = h;
            LoadData();
        }
        public EditStudent(string stID)
        {
            InitializeComponent();
            this.stID = stID;
            LoadDataReadOnly();
        }
        void LoadDataReadOnly()
        {

            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select * from viewAllStudent where [ID] = '" + stID + "'", conn);
            SqlDataAdapter sda2 = new SqlDataAdapter("select photo from students where [st_id] = '" + stID + "'", conn);

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            sda.Fill(dt);
            sda2.Fill(dt2);


            DataSet ds = new DataSet();
            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT [class_name] FROM [class]", conn);
            sda1.Fill(ds);
            this.comboClass.DataSource = ds.Tables[0];
            this.comboClass.DisplayMember = "class_name";
            comboClass.BindingContext = this.BindingContext;


            try
            {
                textRoll.Text = dt.Rows[0][3].ToString();
                textName.Text = dt.Rows[0][1].ToString();
                //comboClass.SelectedText = dt.Rows[0][2].ToString();
                //comboClass.SelectedIndex = dt.Rows[0][2].ToString();
                comboClass.Text = dt.Rows[0][2].ToString();
                textFather.Text = dt.Rows[0][5].ToString();
                textMother.Text = dt.Rows[0][6].ToString();
                textContact.Text = dt.Rows[0][7].ToString();
                if (dt.Rows[0][4].ToString() == "Male")
                    radioMale.Checked = true;
                else
                    radioFemale.Checked = true;
                dateDob.Value = Convert.ToDateTime(dt.Rows[0][8].ToString());
                dateAdmit.Value = Convert.ToDateTime(dt.Rows[0][9].ToString());
                textAddress.Text = dt.Rows[0][10].ToString();
                profileImage.Image = Image.FromFile(@"..\..\StudentImages\" + dt2.Rows[0][0].ToString());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Error"); }

            conn.Close();
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;

            textRoll.Enabled = false;
            textName.Enabled = false;
            comboClass.Enabled = false;
            textFather.Enabled = false;
            textMother.Enabled = false;
            textContact.Enabled = false;
            radioMale.Enabled = false;
            radioFemale.Enabled = false;
            dateDob.Enabled = false;
            dateAdmit.Enabled = false;
            textAddress.Enabled = false;

        }
        void LoadData()
        {

            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select * from viewAllStudent where [ID] = '" + stID + "'", conn);
            SqlDataAdapter sda2 = new SqlDataAdapter("select photo from students where [st_id] = '" + stID + "'", conn);

            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();

            sda.Fill(dt);
            sda2.Fill(dt2);


            DataSet ds = new DataSet();
            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT [class_name] FROM [class]", conn);
            sda1.Fill(ds);
            this.comboClass.DataSource = ds.Tables[0];
            this.comboClass.DisplayMember = "class_name";
            comboClass.BindingContext = this.BindingContext;


            try
            {
                textRoll.Text = dt.Rows[0][3].ToString();
                textName.Text = dt.Rows[0][1].ToString();
                //comboClass.SelectedText = dt.Rows[0][2].ToString();
                //comboClass.SelectedIndex = dt.Rows[0][2].ToString();
                comboClass.Text = dt.Rows[0][2].ToString();
                textFather.Text = dt.Rows[0][5].ToString();
                textMother.Text = dt.Rows[0][6].ToString();
                textContact.Text = dt.Rows[0][7].ToString();
                if (dt.Rows[0][4].ToString() == "Male")
                    radioMale.Checked = true;
                else
                    radioFemale.Checked = true;
                dateDob.Value = Convert.ToDateTime(dt.Rows[0][8].ToString());
                dateAdmit.Value = Convert.ToDateTime(dt.Rows[0][9].ToString());
                textAddress.Text = dt.Rows[0][10].ToString();
                profileImage.Image = Image.FromFile(@"..\..\StudentImages\" + dt2.Rows[0][0].ToString());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Error"); }

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd1 = new OpenFileDialog();
            fd1.Filter = "image files|*.jpg;*.jpeg;*.png;*.gif;*.icon;";
            DialogResult dres1 = fd1.ShowDialog();
            if (dres1 == DialogResult.Abort)
                return;
            if (dres1 == DialogResult.Cancel)
                return;
            profileImage.Image = Image.FromFile(fd1.FileName);
            Image img = Image.FromFile(fd1.FileName);
            imgurl = "img_" + textName.Text.Replace(' ','_') + "_" + comboClass.Text + "_" + textRoll.Text + ".jpg";

            try
            {
                img.Save(@"..\..\StudentImages\" + imgurl);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Error"); }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("update [students] set Roll = " + textRoll.Text + ",Name = '" + textName.Text + "',fk_class_id = (select [class_id] from [class] where class_name = '" + comboClass.Text + "'),father = '" + textFather.Text + "',mother = '" + textMother.Text + "',contact =" + textContact.Text + ",gender = '" + RadioValue + "',dob='" + dateDob.Value.Date.ToString("yyyyMMdd") + "',admissionDate= '" + dateAdmit.Value.Date.ToString("yyyyMMdd") + "',address = '" + textAddress.Text + "',photo ='"+imgurl+"' where st_id = "+stID+"", conn);
            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    MessageBox.Show("Update Success!!!", "Succesfull");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Error"); }
            conn.Close();
            h.RefreshStudentTable();
        }

        private void radioMale_CheckedChanged(object sender, EventArgs e)
        {
            RadioValue = "Male";
        }

        private void radioFemale_CheckedChanged(object sender, EventArgs e)
        {
            RadioValue = "Female";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to delete this record ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete from [students] where st_id = " + stID + "", conn);
                try
                {
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Deleted Successfully!!!", "Succesfull");
                        this.Dispose();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message.ToString(), "Error"); }
                conn.Close();
                h.RefreshStudentTable();
            }
        }
    }
}
