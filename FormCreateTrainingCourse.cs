using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MatanProject2
{
    public partial class FormCreateTrainingCourse : Form
    {
        public FormCreateTrainingCourse()
        {
            InitializeComponent();
            LoadCourseStatuses();
        }

        private void LoadCourseStatuses()
        {
            comboBox1.Items.Clear();

            SqlCommand c = new SqlCommand();
            c.CommandText = "EXEC dbo.GetAllCourseStatuses"; // קריאה לפרוצדורה במקום SELECT
            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);
            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr["courseStatus"].ToString());
            }
            rdr.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // קריאת ערכים מהשדות
            string courseId = textBox1.Text;
            string name = textBox2.Text;
            string description = richTextBox1.Text;
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;
            string instructor = textBox5.Text;
            string location = textBox6.Text;
            string status = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(courseId) || string.IsNullOrWhiteSpace(name) || status == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            SqlCommand c = new SqlCommand();
            c.CommandText = "EXECUTE dbo.CreateTrainingCourse @courseId, @name, @description, @startDate, @endDate, @instructor, @location, @status";
            c.Parameters.AddWithValue("@courseId", courseId);
            c.Parameters.AddWithValue("@name", name);
            c.Parameters.AddWithValue("@description", description);
            c.Parameters.AddWithValue("@startDate", startDate);
            c.Parameters.AddWithValue("@endDate", endDate);
            c.Parameters.AddWithValue("@instructor", instructor);
            c.Parameters.AddWithValue("@location", location);
            c.Parameters.AddWithValue("@status", status);

            SQL_CON sc = new SQL_CON();
            try
            {
                sc.execute_non_query(c);
                MessageBox.Show("Training course created successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void label8_Click(object sender, EventArgs e)
        {
            // לא דרוש כרגע
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

