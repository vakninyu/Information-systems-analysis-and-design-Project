using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormCreateComplaint : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormCreateComplaint()
        {
            InitializeComponent();
            LoadStatusEnum();
            LoadComplaintTypes();
            LoadEmployees();

            dateTimePicker1.Value = DateTime.Today;

            textBox1.Leave += textBox1_Leave;
            button1.Click += button1_Click;
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string id = textBox1.Text.Trim();
            string type = DetectComplainantType(id);

            if (type == "NotFound")
            {
                MessageBox.Show("המתלונן לא קיים במערכת. אנא נסה שנית.", "שגיאה", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.Text = $"Create or Edit Complaint – {type}";
            }
        }

        private bool IsLateComplaint(DateTime dateSubmitted)
        {
            int businessDays = 0;
            DateTime current = dateSubmitted;

            while (current < DateTime.Today)
            {
                if (current.DayOfWeek != DayOfWeek.Friday && current.DayOfWeek != DayOfWeek.Saturday)
                    businessDays++;

                current = current.AddDays(1);
            }

            return businessDays > 3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string complainantId = textBox1.Text.Trim();
            string type = comboBox1.Text;
            string status = comboBox2.Text;
            DateTime dateSubmitted = dateTimePicker1.Value;
            string description = richTextBox1.Text.Trim();
            string resolution = richTextBox2.Text.Trim();
            string assignedTo = comboBox3.SelectedValue?.ToString();

            if (DetectComplainantType(complainantId) == "NotFound")
            {
                MessageBox.Show("The complainant does not exist in the system.", "error");
                return;
            }

            SqlCommand cmd = new SqlCommand("dbo.CreateComplaint")
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@caregiverId", DetectComplainantType(complainantId) == "Caregiver" ? complainantId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@patientId", DetectComplainantType(complainantId) == "Patient" ? complainantId : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@dateSubmitted", dateSubmitted);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@solution", resolution);
            cmd.Parameters.AddWithValue("@assignedTo", string.IsNullOrEmpty(assignedTo) ? (object)DBNull.Value : assignedTo);

            sql.execute_non_query(cmd);

            if (IsLateComplaint(dateSubmitted))
            {
                MessageBox.Show("אזהרה: עברו יותר מ-3 ימי עסקים מאז פתיחת התלונה.", "איחור", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("The complaint was created successfully!");
            }

            // ✅ סגירת הטופס הנוכחי ופתיחת תפריט התלונות
            FormMenuComplaint menu = new FormMenuComplaint();
            menu.Show();
            this.Close(); // סוגר את FormCreateComplaint
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
