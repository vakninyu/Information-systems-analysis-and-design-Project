using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormLoanHistory : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormLoanHistory()
        {
            InitializeComponent();
            LoadInventory(checkBox1.Checked);
            LoadPatientComboBox();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        }

        private void LoadInventory(bool onlyAvailable)
        {
            string procName = onlyAvailable ? "dbo.GetAvailableEquipment" : "dbo.GetAllEquipment";
            SqlCommand cmd = new SqlCommand(procName);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();

            try
            {
                SqlDataReader rdr = sql.execute_query(cmd);
                if (rdr != null)
                {
                    dt.Load(rdr);
                    rdr.Close();
                    dataGridView1.DataSource = null;
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView1.ReadOnly = true;
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת המלאי:\n" + ex.Message);
            }
        }

        private void LoadPatientComboBox()
        {
            SqlCommand cmd = new SqlCommand("dbo.GetAllPatientIds");
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataReader rdr = sql.execute_query(cmd);
            if (rdr != null)
            {
                while (rdr.Read())
                {
                    comboBox1.Items.Add(rdr["patientId"].ToString());
                }
                rdr.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("dbo.GetLoanHistoryByPatientId");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@patientId", comboBox1.Text.Trim());

            DataTable dt = new DataTable();

            try
            {
                SqlDataReader rdr = sql.execute_query(cmd);
                if (rdr != null)
                {
                    dt.Load(rdr);
                    rdr.Close();
                    dataGridView2.DataSource = null;
                    dataGridView2.AutoGenerateColumns = true;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridView2.ReadOnly = true;
                    dataGridView2.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("שגיאה בטעינת היסטוריית ההשאלות:\n" + ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoadInventory(checkBox1.Checked);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // אירוע ריק כדי למנוע שגיאה ב-Designer
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // אירוע ריק כדי למנוע שגיאה ב-Designer
        }
    }
}
