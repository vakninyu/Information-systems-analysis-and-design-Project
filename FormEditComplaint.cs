using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormEditComplaint : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormEditComplaint()
        {
            InitializeComponent();
            this.ActiveControl = textBox2;
            textBox2.KeyDown += textBox2_KeyDown;
            button1.Click += button1_Click;
            button2.Click += button2_Click;

            LoadStatusEnum();
            LoadComplaintTypes();
            LoadEmployees();
        }

        private void LoadStatusEnum()
        {
            comboBox2.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllComplaintStatuses")
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox2.Items.Add(reader["complaintStatus"].ToString());
            }
            reader.Close();
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
                comboBox1.Items.Add(reader["complaintType"].ToString());
            }
            reader.Close();
        }

        private void LoadEmployees()
        {
            SqlCommand cmd = new SqlCommand("dbo.GetAllEmployees")
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sql.execute_adapter(cmd, da, dt);

            comboBox3.DataSource = dt;
            comboBox3.DisplayMember = "fullNameWithRole";
            comboBox3.ValueMember = "employeeId";
            comboBox3.SelectedIndex = -1;
        }

        private void LoadComplaintDetails()
        {
            string complaintId = textBox2.Text.Trim();

            SqlCommand cmd = new SqlCommand("SELECT caregiverId, patientId, dateSubmitted, type, description, status, solution, assignedTo FROM Complaint WHERE complaintId = @complaintId")
            {
                CommandType = CommandType.Text
            };
            cmd.Parameters.AddWithValue("@complaintId", complaintId);

            SqlDataReader reader = sql.execute_query(cmd);

            if (reader.Read())
            {
                string caregiverId = reader["caregiverId"] == DBNull.Value ? null : reader["caregiverId"].ToString();
                string patientId = reader["patientId"] == DBNull.Value ? null : reader["patientId"].ToString();

                textBox1.Text = caregiverId ?? patientId;
                dateTimePicker1.Value = Convert.ToDateTime(reader["dateSubmitted"]);
                comboBox1.Text = reader["type"].ToString();
                comboBox2.Text = reader["status"].ToString();
                richTextBox1.Text = reader["description"].ToString();
                richTextBox2.Text = reader["solution"].ToString();

                string assignedTo = reader["assignedTo"] == DBNull.Value ? null : reader["assignedTo"].ToString();
                comboBox3.SelectedValue = assignedTo;
            }
            else
            {
                MessageBox.Show("Complaint ID not found.", "Error");
            }

            reader.Close();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadComplaintDetails();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string complaintId = textBox2.Text.Trim();
            string complainantId = textBox1.Text.Trim();
            string type = comboBox1.Text;
            string status = comboBox2.Text;
            DateTime dateSubmitted = dateTimePicker1.Value;
            string description = richTextBox1.Text.Trim();
            string resolution = richTextBox2.Text.Trim();
            string assignedTo = comboBox3.SelectedValue?.ToString();

            SqlCommand cmd = new SqlCommand("dbo.UpdateComplaint")
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@complaintId", complaintId);
            cmd.Parameters.AddWithValue("@caregiverId", DetectComplainantType(complainantId) == "Caregiver" ? complainantId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@patientId", DetectComplainantType(complainantId) == "Patient" ? complainantId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@solution", resolution);
            cmd.Parameters.AddWithValue("@assignedTo", string.IsNullOrEmpty(assignedTo) ? (object)DBNull.Value : assignedTo);

            sql.execute_non_query(cmd);
            MessageBox.Show("Complaint updated successfully.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox1.Text = string.Empty;
            comboBox2.Text = string.Empty;
            comboBox3.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Today;
            richTextBox1.Clear();
            richTextBox2.Clear();
            textBox2.Focus();
        }

        private string DetectComplainantType(string id)
        {
            SqlCommand cmd = new SqlCommand("dbo.CheckComplainantType")
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id", id);

            SqlParameter typeParam = new SqlParameter("@type", SqlDbType.NVarChar, 20)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(typeParam);

            sql.execute_non_query(cmd);

            return typeParam.Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
