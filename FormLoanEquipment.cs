using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormLoanEquipment : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormLoanEquipment()
        {
            InitializeComponent();
            LoadComboBoxes();

            // הכנה לפורמט מותאם של התאריך השלישי
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = " ";

            // חיבורים לכל הכפתורים
            button1.Click += button1_Click; // Create
            button2.Click += button2_Click; // Update
            button3.Click += button3_Click; // Delete
            button4.Click += button4_Click; // Search
            button5.Click += button5_Click; // Clear

            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        }



        private void LoadComboBoxes()
        {
            try
            {
                // Load equipment IDs
                SqlCommand cmd1 = new SqlCommand("dbo.GetAllEquipmentIds");
                cmd1.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr1 = sql.execute_query(cmd1);
                while (rdr1.Read())
                    comboBox1.Items.Add(rdr1["equipmentId"].ToString());
                rdr1.Close();

                // Load patient IDs
                SqlCommand cmd2 = new SqlCommand("dbo.GetAllPatientIds");
                cmd2.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr2 = sql.execute_query(cmd2);
                while (rdr2.Read())
                    comboBox2.Items.Add(rdr2["patientId"].ToString());
                rdr2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data:\n" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Create
        {
            SqlCommand cmd = new SqlCommand("dbo.InsertEquipmentLoan")
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@loanId", textBox1.Text);
            cmd.Parameters.AddWithValue("@equipmentId", comboBox1.Text);
            cmd.Parameters.AddWithValue("@patientId", comboBox2.Text);
            cmd.Parameters.AddWithValue("@loanDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@returnDate", dateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@actualReturnDate", checkBox1.Checked ? (object)dateTimePicker3.Value : DBNull.Value);

            try
            {
                sql.execute_non_query(cmd);
                MessageBox.Show("Loan created successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating loan:\n" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Update
        {
            SqlCommand cmd = new SqlCommand("dbo.UpdateEquipmentLoan")
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@loanId", textBox1.Text);
            cmd.Parameters.AddWithValue("@equipmentId", comboBox1.Text);
            cmd.Parameters.AddWithValue("@patientId", comboBox2.Text);
            cmd.Parameters.AddWithValue("@loanDate", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@returnDate", dateTimePicker2.Value);
            cmd.Parameters.AddWithValue("@actualReturnDate", checkBox1.Checked ? (object)dateTimePicker3.Value : DBNull.Value);

            try
            {
                sql.execute_non_query(cmd);
                MessageBox.Show("Loan updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating loan:\n" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e) // Delete
        {
            SqlCommand cmd = new SqlCommand("dbo.DeleteEquipmentLoan")
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@loanId", textBox1.Text);

            try
            {
                sql.execute_non_query(cmd);
                MessageBox.Show("Loan deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting loan:\n" + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e) // Search
        {
            SqlCommand cmd = new SqlCommand("GetLoanById")
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@loanId", textBox1.Text);

            SqlDataReader rdr = sql.execute_query(cmd);
            if (rdr != null && rdr.Read())
            {
                comboBox1.Text = rdr["equipmentId"].ToString();
                comboBox2.Text = rdr["patientId"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(rdr["loanDate"]);
                dateTimePicker2.Value = Convert.ToDateTime(rdr["returnDate"]);

                if (rdr["actualReturnDate"] != DBNull.Value)
                {
                    checkBox1.Checked = true;
                    dateTimePicker3.CustomFormat = "dd/MM/yyyy";
                    dateTimePicker3.Value = Convert.ToDateTime(rdr["actualReturnDate"]);
                }
                else
                {
                    checkBox1.Checked = false;
                    dateTimePicker3.CustomFormat = " ";
                    dateTimePicker3.Value = DateTime.Now;
                }

                rdr.Close();
            }
            else
            {
                MessageBox.Show("No loan found with this ID.");
            }
        }

        private void button5_Click(object sender, EventArgs e) // Clear
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker3.Value = DateTime.Now;
            dateTimePicker3.CustomFormat = " ";
            checkBox1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dateTimePicker3.CustomFormat = "dd/MM/yyyy";
                dateTimePicker3.Value = DateTime.Now;
            }
            else
            {
                dateTimePicker3.CustomFormat = " ";
            }
        }

        // Optional empty handlers
        private void label2_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}
