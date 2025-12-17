using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormViewComplaints : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormViewComplaints()
        {
            InitializeComponent();

            // אירועים
            button1.Click += button1_Click; // Search
            button2.Click += button2_Click; // Refresh
            button3.Click += button3_Click; // New Complaint

            this.Load += FormViewComplaints_Load;
        }

        private void FormViewComplaints_Load(object sender, EventArgs e)
        {
            LoadComplaintTypes();
            LoadComplaintStatuses();
            LoadComplaints(); // טוען הכל
        }

        private void LoadComplaintTypes()
        {
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllComplaintType")
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            reader.Close();
        }

        private void LoadComplaintStatuses()
        {
            comboBox2.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllComplaintStatuses")
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox2.Items.Add(reader[0].ToString());
            }
            reader.Close();
        }

        private void LoadComplaints(string type = "", string status = "", string id = "")
        {
            SqlCommand cmd = new SqlCommand("dbo.GetFilteredComplaints")
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@type", string.IsNullOrEmpty(type) ? (object)DBNull.Value : type);
            cmd.Parameters.AddWithValue("@status", string.IsNullOrEmpty(status) ? (object)DBNull.Value : status);
            cmd.Parameters.AddWithValue("@complainantId", string.IsNullOrEmpty(id) ? (object)DBNull.Value : id);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sql.execute_adapter(cmd, da, dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string typeFilter = comboBox1.Text;
            string statusFilter = comboBox2.Text;
            string idFilter = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(idFilter))
            {
                string complainantType = CheckComplainantType(idFilter);
                if (complainantType == "NotFound")
                {
                    MessageBox.Show("תעודת זהות לא קיימת במערכת", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            LoadComplaints(typeFilter, statusFilter, idFilter);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            textBox1.Clear();
            LoadComplaints(); // טען הכל מחדש
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormCreateComplaint createForm = new FormCreateComplaint();
            createForm.ShowDialog();
        }

        private string CheckComplainantType(string id)
        {
            SqlCommand cmd = new SqlCommand("dbo.CheckComplainantType")
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id", id);

            SqlParameter output = new SqlParameter("@type", SqlDbType.NVarChar, 20)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(output);

            sql.execute_non_query(cmd);
            return output.Value.ToString();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            // לא נדרש כלום – נועד רק למנוע שגיאה
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
