using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormEditDeleteCaregiver : Form
    {
        List<string> selectedCities = new List<string>();
        SQL_CON sql = new SQL_CON();

        public FormEditDeleteCaregiver()
        {
            InitializeComponent();
            pictureBoxMap.MouseClick += pictureBoxMap_MouseClick;
        }

        private void FormEditDeleteCaregiver_Load(object sender, EventArgs e)
        {
            LoadEmploymentStatuses();
            LoadTreatmentTypes();
            LoadAvailabilityStatuses();
            LoadGenders();
        }

        private void LoadEmploymentStatuses()
        {
            comboBoxEmolyment2.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllEmploymentStatuses")
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBoxEmolyment2.Items.Add(reader["employmentStatus"].ToString());
            }
            reader.Close();
        }

        private void LoadTreatmentTypes()
        {
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("SELECT treatmentType FROM TreatmentTypeEnum");
            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["treatmentType"].ToString());
            }
            reader.Close();
        }

        private void LoadAvailabilityStatuses()
        {
            comboBox2.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllAvailabilityStatuses")
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox2.Items.Add(reader["availabilityStatus"].ToString());
            }
            reader.Close();
        }

        private void LoadGenders()
        {
            comboBox3.Items.Clear();
            SqlCommand cmd = new SqlCommand("SELECT gender FROM GenderEnum");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sql.execute_adapter(cmd, da, dt);

            foreach (DataRow row in dt.Rows)
            {
                comboBox3.Items.Add(row["gender"].ToString());
            }
        }

        private void textboxId_TextChanged(object sender, EventArgs e)
        {
            if (textBoxId2.Text.Length > 0)
            {
                LoadEmploymentStatuses();
                LoadTreatmentTypes();
                LoadAvailabilityStatuses();
                LoadGenders();

                SqlCommand cmd = new SqlCommand("dbo.GetCaregiverById")
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@caregiverId", textBoxId2.Text);

                SqlDataReader reader = sql.execute_query(cmd);
                if (reader.Read())
                {
                    textBoxFirstName2.Text = reader["firstName"].ToString();
                    textBoxLastName2.Text = reader["lastName"].ToString();
                    textBoxAddress2.Text = reader["address"].ToString();
                    textBoxLanguages2.Text = reader["languages"].ToString();
                    numericExpe2.Value = Convert.ToDecimal(reader["experience"]);
                    textBoxPhone2.Text = reader["phone"].ToString();
                    dateTimeStartDate2.Value = Convert.ToDateTime(reader["startDate"]);

                    string workAreaString = reader["workArea"].ToString();
                    selectedCities = workAreaString.Split(',').Select(s => s.Trim()).ToList();
                    MessageBox.Show("Selected cities: " + workAreaString);

                    comboBoxEmolyment2.SelectedItem = reader["employmentStatus"].ToString();
                    comboBox1.SelectedItem = reader["treatmentType"].ToString();
                    comboBox2.SelectedItem = reader["isAvailable"].ToString();
                    comboBox3.SelectedItem = reader["gender"].ToString();
                }

                reader.Close();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("dbo.UpdateCaregiver")
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@caregiverId", textBoxId2.Text);
            cmd.Parameters.AddWithValue("@firstName", textBoxFirstName2.Text);
            cmd.Parameters.AddWithValue("@lastName", textBoxLastName2.Text);
            cmd.Parameters.AddWithValue("@address", textBoxAddress2.Text);
            cmd.Parameters.AddWithValue("@gender", comboBox3.SelectedItem?.ToString());
            cmd.Parameters.AddWithValue("@languages", textBoxLanguages2.Text);
            cmd.Parameters.AddWithValue("@experience", (int)numericExpe2.Value);
            cmd.Parameters.AddWithValue("@phone", textBoxPhone2.Text);
            cmd.Parameters.AddWithValue("@startDate", dateTimeStartDate2.Value);
            string workArea = string.Join(", ", selectedCities);
            cmd.Parameters.AddWithValue("@workArea", workArea);
            cmd.Parameters.AddWithValue("@employmentStatus", comboBoxEmolyment2.SelectedItem?.ToString());
            cmd.Parameters.AddWithValue("@treatmentType", comboBox1.SelectedItem?.ToString());
            cmd.Parameters.AddWithValue("@isAvailable", comboBox2.SelectedItem?.ToString());

            sql.execute_non_query(cmd);
            MessageBox.Show("Caregiver updated successfully.");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this caregiver?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("dbo.DeleteCaregiver")
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@caregiverId", textBoxId2.Text);
                sql.execute_non_query(cmd);
                MessageBox.Show("Caregiver deleted successfully.");
                ClearForm();
            }
        }

        private void ClearForm()
        {
            textBoxId2.Clear();
            textBoxFirstName2.Clear();
            textBoxLastName2.Clear();
            textBoxAddress2.Clear();
            textBoxLanguages2.Clear();
            numericExpe2.Value = 0;
            textBoxPhone2.Clear();
            comboBoxEmolyment2.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            dateTimeStartDate2.Value = DateTime.Today;
            selectedCities.Clear();
        }


        private void pictureBoxMap_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            string city = GetCityFromCoordinates(x, y);
            if (!string.IsNullOrEmpty(city) && !selectedCities.Contains(city))
            {
                selectedCities.Add(city);
                MessageBox.Show($"Selected city: {city}");
            }
        }

        private string GetCityFromCoordinates(int x, int y)
        {
            if (x >= 80 && x <= 120 && y >= 150 && y <= 200) return "Tel Aviv";
            if (x >= 50 && x <= 90 && y >= 60 && y <= 100) return "Haifa";
            if (x >= 250 && x <= 290 && y >= 240 && y <= 280) return "Jerusalem";
            if (x >= 200 && x <= 250 && y >= 350 && y <= 400) return "Beer Sheva";
            if (x >= 100 && x <= 140 && y >= 230 && y <= 270) return "Ashdod";
            if (x >= 90 && x <= 130 && y >= 90 && y <= 130) return "Afula";
            if (x >= 80 && x <= 120 && y >= 70 && y <= 110) return "Nazareth";
            if (x >= 270 && x <= 310 && y >= 430 && y <= 470) return "Eilat";
            if (x >= 180 && x <= 220 && y >= 230 && y <= 270) return "Modiin";
            if (x >= 120 && x <= 160 && y >= 200 && y <= 240) return "Rehovot";
            if (x >= 80 && x <= 120 && y >= 130 && y <= 170) return "Herzliya";
            if (x >= 150 && x <= 190 && y >= 270 && y <= 310) return "Rishon Lezion";
            if (x >= 230 && x <= 270 && y >= 310 && y <= 350) return "Ashkelon";
            if (x >= 190 && x <= 230 && y >= 290 && y <= 330) return "Holon";
            if (x >= 160 && x <= 200 && y >= 310 && y <= 350) return "Bat Yam";
            if (x >= 60 && x <= 100 && y >= 110 && y <= 150) return "Zichron Yaakov";
            if (x >= 110 && x <= 150 && y >= 60 && y <= 100) return "Tiberias";
            if (x >= 140 && x <= 180 && y >= 170 && y <= 210) return "Petah Tikva";

            return null;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void pictureBoxMap_Click(object sender, EventArgs e) { }
    }
}
