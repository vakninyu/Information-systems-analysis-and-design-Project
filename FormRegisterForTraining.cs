using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormRegisterForTraining : Form
    {
        public FormRegisterForTraining()
        {
            InitializeComponent();
            LoadCaregivers();
            LoadCourses();
        }

        private void LoadCaregivers()
        {
            comboBox1.Items.Clear(); // ת"ז מטפלים
            SqlCommand c = new SqlCommand("EXEC dbo.GetAllCaregiverIds");
            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);
            while (rdr.Read())
            {
                string id = rdr["caregiverId"].ToString();
                comboBox1.Items.Add(new ComboBoxItem(id, id)); // טקסט וערך זהים
            }
            rdr.Close();
        }

        private void LoadCourses()
        {
            comboBox2.Items.Clear(); // קורסים
            SqlCommand c = new SqlCommand("EXEC dbo.GetAllTrainingCourses");
            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);
            while (rdr.Read())
            {
                string id = rdr["courseId"].ToString();
                string name = rdr["name"].ToString();
                comboBox2.Items.Add(new ComboBoxItem(name, id));
            }
            rdr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedCaregiver = comboBox1.SelectedItem as ComboBoxItem;
            var selectedCourse = comboBox2.SelectedItem as ComboBoxItem;

            if (selectedCaregiver == null || selectedCourse == null)
            {
                MessageBox.Show("Please select a caregiver and a course.");
                return;
            }

            SqlCommand c = new SqlCommand("EXEC dbo.RegisterCaregiverToCourse @caregiverId, @courseId");
            c.Parameters.AddWithValue("@caregiverId", selectedCaregiver.Value);
            c.Parameters.AddWithValue("@courseId", selectedCourse.Value);

            SQL_CON sc = new SQL_CON();
            try
            {
                sc.execute_non_query(c);
                MessageBox.Show("Caregiver successfully registered to course!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }

    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public ComboBoxItem(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString() => Text;
    }
}
