using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormManageAttendance : Form
    {
        public FormManageAttendance()
        {
            InitializeComponent();
            LoadCourses();
        }

        private void LoadCourses()
        {
            comboBox1.Items.Clear();
            SqlCommand c = new SqlCommand("EXEC dbo.GetAllTrainingCourses");
            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);
            while (rdr.Read())
            {
                string id = rdr["courseId"].ToString();
                string name = rdr["name"].ToString();
                comboBox1.Items.Add(new ComboBoxItem(name, id));
            }
            rdr.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is ComboBoxItem course)
            {
                LoadAttendanceData(course.Value);
            }
        }

        private void LoadAttendanceData(string courseId)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("caregiverId", "Caregiver ID");

            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn
            {
                Name = "attendanceStatus",
                HeaderText = "Status",
                DataSource = new string[] { "present", "absent", "late" }
            };
            dataGridView1.Columns.Add(statusColumn);

            SqlCommand c = new SqlCommand("EXEC dbo.GetAttendanceByCourseId @courseId");
            c.Parameters.AddWithValue("@courseId", courseId);
            SQL_CON sc = new SQL_CON();
            SqlDataReader rdr = sc.execute_query(c);
            while (rdr.Read())
            {
                dataGridView1.Rows.Add(rdr["caregiverId"].ToString(), rdr["attendanceStatus"].ToString());
            }
            rdr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!(comboBox1.SelectedItem is ComboBoxItem course))
            {
                MessageBox.Show("Please select a course first.");
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string caregiverId = row.Cells["caregiverId"].Value?.ToString();
                string status = row.Cells["attendanceStatus"].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(caregiverId) && !string.IsNullOrWhiteSpace(status))
                {
                    SqlCommand c = new SqlCommand("EXEC dbo.UpdateAttendanceStatus @courseId, @caregiverId, @attendanceStatus");
                    c.Parameters.AddWithValue("@courseId", course.Value);
                    c.Parameters.AddWithValue("@caregiverId", caregiverId);
                    c.Parameters.AddWithValue("@attendanceStatus", status);

                    SQL_CON sc = new SQL_CON();
                    sc.execute_non_query(c);
                }
            }

            MessageBox.Show("Attendance updated successfully!");
        }

        // שים לב: יש להוסיף את ComboBoxItem רק פעם אחת בפרויקט
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
