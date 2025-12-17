using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace MatanProject2
{
    public partial class FormTimeReport : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormTimeReport()
        {
            InitializeComponent();
            LoadMonths();
            LoadYears();
            button1.Click += ButtonSearch_Click;
            button3.Click += ButtonSave_Click;
            button4.Click += ButtonClear_Click;
            button2.Click += ButtonExportToExcel_Click;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

        }

        private void LoadMonths()
        {
            comboBox1.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                comboBox1.Items.Add(i.ToString("00"));
            }
        }

        private void LoadYears()
        {
            comboBox2.Items.Clear();
            for (int year = 2025; year >= 2000; year--)
            {
                comboBox2.Items.Add(year.ToString());
            }
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            string selectedMonth = comboBox1.SelectedItem?.ToString();
            string selectedYear = comboBox2.SelectedItem?.ToString();
            string employeeId = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(selectedMonth) && string.IsNullOrEmpty(selectedYear) && string.IsNullOrEmpty(employeeId))
            {
                MessageBox.Show("Please enter at least one filter: Month, Year, or Employee ID.");
                return;
            }

            if (!string.IsNullOrEmpty(employeeId))
            {
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Employee WHERE employeeId = @id");
                checkCmd.Parameters.AddWithValue("@id", employeeId);
                int count = (int)sql.execute_scalar(checkCmd);
                if (count == 0)
                {
                    MessageBox.Show("Employee ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            LoadTimeReports(selectedMonth, selectedYear, employeeId);
        }

        private void LoadTimeReports(string month, string year, string employeeId)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            string query = @"
                SELECT TR.employeeId, E.name, TR.reportMonth, TR.reportYear, TR.hoursWorked, TR.exceptionStatus, TR.comment
                FROM TimeReport TR
                JOIN Employee E ON TR.employeeId = E.employeeId
                WHERE 1=1";

            SqlCommand cmd = new SqlCommand();

            if (!string.IsNullOrEmpty(employeeId))
            {
                query += " AND TR.employeeId = @employeeId";
                cmd.Parameters.AddWithValue("@employeeId", employeeId);
            }

            if (!string.IsNullOrEmpty(month))
            {
                query += " AND TR.reportMonth = @month";
                cmd.Parameters.AddWithValue("@month", int.Parse(month));
            }

            if (!string.IsNullOrEmpty(year))
            {
                query += " AND TR.reportYear = @year";
                cmd.Parameters.AddWithValue("@year", int.Parse(year));
            }

            cmd.CommandText = query;
            SqlDataReader reader = sql.execute_query(cmd);

            dataGridView1.Columns.Add("employeeId", "Employee ID");
            dataGridView1.Columns.Add("name", "Full Name");
            dataGridView1.Columns.Add("month", "Month");
            dataGridView1.Columns.Add("year", "Year");
            dataGridView1.Columns.Add("hoursWorked", "Hours Worked");

            DataGridViewComboBoxColumn exceptionColumn = new DataGridViewComboBoxColumn();
            exceptionColumn.HeaderText = "Exception Status";
            exceptionColumn.Name = "exceptionStatus";
            exceptionColumn.Items.AddRange("Normal", "Excess Hours", "Missing Hours", "Unapproved Overtime");
            dataGridView1.Columns.Add(exceptionColumn);

            DataGridViewTextBoxColumn commentColumn = new DataGridViewTextBoxColumn();
            commentColumn.HeaderText = "Comment";
            commentColumn.Name = "comment";
            dataGridView1.Columns.Add(commentColumn);

            while (reader.Read())
            {
                dataGridView1.Rows.Add(
                    reader["employeeId"].ToString(),
                    reader["name"].ToString(),
                    reader["reportMonth"].ToString(),
                    reader["reportYear"].ToString(),
                    reader["hoursWorked"].ToString(),
                    reader["exceptionStatus"].ToString(),
                    reader["comment"].ToString()
                );
            }

            reader.Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                string employeeId = row.Cells["employeeId"].Value?.ToString();
                int month = int.Parse(row.Cells["month"].Value.ToString());
                int year = int.Parse(row.Cells["year"].Value.ToString());
                string exception = row.Cells["exceptionStatus"].Value?.ToString();
                string comment = row.Cells["comment"].Value?.ToString();

                SqlCommand cmd = new SqlCommand("EXEC dbo.UpdateTimeReportExceptionAndComment @employeeId, @month, @year, @exceptionStatus, @comment");
                cmd.Parameters.AddWithValue("@employeeId", employeeId);
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@exceptionStatus", exception);
                cmd.Parameters.AddWithValue("@comment", comment);

                sql.execute_non_query(cmd);
            }

            MessageBox.Show("Changes saved successfully.");
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
        }

        private void ButtonExportToExcel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv";
            sfd.FileName = "TimeReportsExport.csv";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        sw.Write(dataGridView1.Columns[i].HeaderText);
                        if (i < dataGridView1.Columns.Count - 1) sw.Write(",");
                    }
                    sw.WriteLine();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            sw.Write(row.Cells[i].Value?.ToString());
                            if (i < dataGridView1.Columns.Count - 1) sw.Write(",");
                        }
                        sw.WriteLine();
                    }
                }

                MessageBox.Show("Export successful.");
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // מוודא שקיימת התאמה מלאה לשם שה-Designer מחפש
        }
    }
}
