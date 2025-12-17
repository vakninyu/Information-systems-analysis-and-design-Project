using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormEditDeleteTrainingCourse : Form
    {
        public FormEditDeleteTrainingCourse()
        {
            InitializeComponent();
            LoadCourseIds();
            LoadStatuses();

            comboBox2.DropDownStyle = ComboBoxStyle.DropDown; // מאפשר הקלדה
            comboBox2.Leave += comboBox2_Leave;               // חיפוש לפי ת"ז
            comboBox2.KeyDown += comboBox2_KeyDown;           // גם ENTER יעבוד
        }

        private void LoadCourseIds()
        {
            comboBox2.Items.Clear();
            SqlCommand c = new SqlCommand("EXEC dbo.GetAllTrainingCourseIds");
            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);
            while (rdr.Read())
            {
                comboBox2.Items.Add(rdr["courseId"].ToString());
            }
            rdr.Close();
        }

        private void LoadStatuses()
        {
            comboBox1.Items.Clear();
            SqlCommand c = new SqlCommand("EXEC dbo.GetAllCourseStatuses");
            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);
            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr["courseStatus"].ToString());
            }
            rdr.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourseById(comboBox2.SelectedItem.ToString());
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                LoadCourseById(comboBox2.Text.Trim());
            }
        }

        private void comboBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboBox2_Leave(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void LoadCourseById(string courseId)
        {
            SqlCommand c = new SqlCommand("EXEC dbo.GetTrainingCourseById @courseId");
            c.Parameters.AddWithValue("@courseId", courseId);

            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);

            if (rdr.Read())
            {
                textBox2.Text = rdr["name"].ToString();
                richTextBox1.Text = rdr["description"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(rdr["startDate"]);
                dateTimePicker2.Value = Convert.ToDateTime(rdr["endDate"]);
                textBox5.Text = rdr["instructor"].ToString();
                textBox6.Text = rdr["location"].ToString();
                comboBox1.SelectedItem = rdr["status"].ToString();
            }
            else
            {
                MessageBox.Show("Course not found.");
            }

            rdr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand c = new SqlCommand("EXEC dbo.UpdateTrainingCourse @courseId, @name, @description, @startDate, @endDate, @instructor, @location, @status");
            c.Parameters.AddWithValue("@courseId", comboBox2.Text);
            c.Parameters.AddWithValue("@name", textBox2.Text);
            c.Parameters.AddWithValue("@description", richTextBox1.Text);
            c.Parameters.AddWithValue("@startDate", dateTimePicker1.Value);
            c.Parameters.AddWithValue("@endDate", dateTimePicker2.Value);
            c.Parameters.AddWithValue("@instructor", textBox5.Text);
            c.Parameters.AddWithValue("@location", textBox6.Text);
            c.Parameters.AddWithValue("@status", comboBox1.SelectedItem?.ToString());

            SQL_CON sc = new SQL_CON();
            try
            {
                sc.execute_non_query(c);
                MessageBox.Show("Training course updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Please enter a Course ID to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this course?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                SqlCommand c = new SqlCommand("EXEC dbo.DeleteTrainingCourse @courseId");
                c.Parameters.AddWithValue("@courseId", comboBox2.Text);

                SQL_CON sc = new SQL_CON();
                try
                {
                    sc.execute_non_query(c);
                    MessageBox.Show("Training course deleted.");
                    ClearForm();
                    LoadCourseIds(); // רענון
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            comboBox2.Text = "";
            textBox2.Clear();
            richTextBox1.Clear();
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            textBox5.Clear();
            textBox6.Clear();
            comboBox1.SelectedIndex = -1;

            comboBox2.Focus(); // מחזיר לפוקוס
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // אין צורך בקוד כאן כרגע
        }
    }
}
