using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormCreateCaregiver : Form
    {
        List<string> selectedCities = new List<string>();
        SQL_CON sql = new SQL_CON();

        public FormCreateCaregiver()
        {
            InitializeComponent();
            loadEmploymentStatuses();
            loadTreatmentTypes();
            loadAvailabilityStatuses();
            LoadGenders();
            pictureBoxMap.MouseClick += pictureBoxMap_MouseClick;           

        }
      


        private void loadEmploymentStatuses()
        {
            comboBox2.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllEmploymentStatuses");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox2.Items.Add(reader.GetValue(0).ToString());
            }
            reader.Close();
        }

        private void loadTreatmentTypes()
        {
            comboBox3.Items.Clear();
            SqlCommand cmd = new SqlCommand("SELECT treatmentType FROM TreatmentTypeEnum");
            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox3.Items.Add(reader.GetString(0));
            }
            reader.Close();
        }

        private void loadAvailabilityStatuses()
        {
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllAvailabilityStatuses");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = sql.execute_query(cmd);
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["availabilityStatus"].ToString());
            }
            reader.Close();
        }

        private void LoadGenders()
        {
            comboBox4.Items.Clear();
            SqlCommand cmd = new SqlCommand("SELECT gender FROM dbo.GenderEnum");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sql.execute_adapter(cmd, da, dt);

            foreach (DataRow row in dt.Rows)
            {
                comboBox4.Items.Add(row["gender"].ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textboxId.Text;
                string firstName = textBoxFirstName.Text;
                string lastName = textBoxLast.Text;
                string address = textBox12.Text;
                string gender = comboBox4.Text;
                string languages = textBox14.Text;
                int experience = (int)numericUpDown2.Value;
                string phone = textBox15.Text;
                DateTime startDate = dateTimePicker2.Value;
                string workArea = string.Join(", ", selectedCities);
                string employmentStatus = comboBox2.Text;
                string treatmentType = comboBox3.Text;
                string availability = comboBox1.Text;

                SqlCommand cmd = new SqlCommand("dbo.CreateCaregiver");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@caregiverId", id);
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@languages", languages);
                cmd.Parameters.AddWithValue("@experience", experience);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@workArea", workArea);
                cmd.Parameters.AddWithValue("@employmentStatus", employmentStatus);
                cmd.Parameters.AddWithValue("@treatmentType", treatmentType);
                cmd.Parameters.AddWithValue("@isAvailable", availability);

                sql.execute_non_query(cmd);

                MessageBox.Show("Caregiver added successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding caregiver: " + ex.Message);
            }
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



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textboxId_TextChanged(object sender, EventArgs e) { }
        private void textBoxFirstName_TextChanged(object sender, EventArgs e) { }
        private void textBoxLast_TextChanged(object sender, EventArgs e) { }
        private void textBox12_TextChanged(object sender, EventArgs e) { }
        private void textBox13_TextChanged(object sender, EventArgs e) { }
        private void textBox14_TextChanged(object sender, EventArgs e) { }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e) { }
        private void textBox15_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void FormCreateCaregiver_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void textBox14_TextChanged_1(object sender, EventArgs e) { }
        private void pictureBoxMap_Click(object sender, EventArgs e) { }
        private void textboxId_TextChanged_1(object sender, EventArgs e) { }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}
