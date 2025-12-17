using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormFindCaregiverForPatient : Form
    {
        private string selectedCaregiverId = "";
        SQL_CON sql = new SQL_CON();

        public FormFindCaregiverForPatient()
        {
            InitializeComponent();
            dataGridView1.CellClick += dataGridView1_CellClick;
            button1.Click += button1_Click;
            buttonLoadPatient.Click += buttonLoadPatient_Click;
        }

        private void buttonLoadPatient_Click(object sender, EventArgs e)
        {
            string patientId = textBoxPatientId.Text.Trim();

            if (string.IsNullOrEmpty(patientId))
            {
                MessageBox.Show("Please enter a patient ID.");
                return;
            }

            // בדיקה אם קיימת התאמה קיימת
            SqlCommand checkMatchCmd = new SqlCommand("dbo.CheckIfMatchExistsForPatient");
            checkMatchCmd.CommandType = CommandType.StoredProcedure;
            checkMatchCmd.Parameters.AddWithValue("@patientId", patientId);
            object result = sql.execute_scalar(checkMatchCmd);

            if (Convert.ToInt32(result) > 0)
            {
                MessageBox.Show("A match already exists for the selected patient.", "Match Exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // נצא כאן בלי לקרוא לשום דבר נוסף
            }

            // שליפת פרטי המטופל
            SqlCommand cmd = new SqlCommand("dbo.GetPatientDetailsById");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@patientId", patientId);

            using (SqlConnection tempConn = new SqlConnection(sql.GetConnectionString()))
            {
                cmd.Connection = tempConn;
                tempConn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        MessageBox.Show("Patient not found.");
                        ClearPatientDetails();
                        dataGridView1.DataSource = null;
                        return;
                    }

                    labelFullName.Text = reader["fullName"].ToString();
                    label7.Text = reader["language"].ToString();
                    label8.Text = reader["preferredGender"].ToString();
                    label10.Text = reader["treatmentType"].ToString();
                    label11.Text = reader["area"].ToString();
                }
            }

            LoadMatchingCaregivers(patientId);
        }
        private void ClearForm()
        {
            textBoxPatientId.Clear();
            ClearPatientDetails(); // כולל כל התוויות וה־grid
        }

        private void LoadMatchingCaregivers(string patientId)
        {
            SqlCommand cmd = new SqlCommand("dbo.GetMatchingCaregiversForPatient");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@patientId", patientId);

            DataTable dt = sql.execute_dataTable(cmd);

            if (dt == null)
            {
                MessageBox.Show("No data returned for matching caregivers.");
                return;
            }

            if (!dt.Columns.Contains("matchPercentFormatted"))
                dt.Columns.Add("matchPercentFormatted", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["matchPercentFormatted"] = row["matchPercent"] + "%";
            }

            dataGridView1.DataSource = dt;

            if (dataGridView1.Columns.Contains("matchPercent"))
                dataGridView1.Columns["matchPercent"].Visible = false;
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectedCaregiverId = row.Cells["caregiverId"].Value.ToString();
                label13.Text = row.Cells["firstName"].Value.ToString() + " " + row.Cells["lastName"].Value.ToString();
                label12.Text = labelFullName.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string patientId = textBoxPatientId.Text.Trim();

            if (selectedCaregiverId == "" || string.IsNullOrEmpty(patientId))
            {
                MessageBox.Show("Please select a caregiver and enter a patient ID.");
                return;
            }

            // יצירת ההתאמה
            SqlCommand cmd = new SqlCommand("INSERT INTO Match (caregiverId, patientId, startDate) VALUES (@caregiverId, @patientId, @startDate)");
            cmd.Parameters.AddWithValue("@caregiverId", selectedCaregiverId);
            cmd.Parameters.AddWithValue("@patientId", patientId);
            cmd.Parameters.AddWithValue("@startDate", DateTime.Now);

            sql.execute_non_query(cmd);

            // ✅ עדכון זמינות המטפל ל-notAvailable
            SqlCommand updateCmd = new SqlCommand("dbo.UpdateCaregiverAvailability");
            updateCmd.CommandType = CommandType.StoredProcedure;
            updateCmd.Parameters.AddWithValue("@caregiverId", selectedCaregiverId);
            updateCmd.Parameters.AddWithValue("@newStatus", "notAvailable");

            sql.execute_non_query(updateCmd);

            MessageBox.Show("Match created successfully.");
        }

        private void ClearPatientDetails()
        {
            labelFullName.Text = "";
            label7.Text = "";
            label8.Text = "";
            label10.Text = "";
            label11.Text = "";
            label12.Text = "";
            label13.Text = "";
            dataGridView1.DataSource = null;
            selectedCaregiverId = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void textBoxPatientId_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

    }
}
