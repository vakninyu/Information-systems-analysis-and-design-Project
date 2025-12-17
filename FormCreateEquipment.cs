using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormCreateEquipment : Form
    {
        public FormCreateEquipment()
        {
            InitializeComponent();
            LoadStatusOptions();       // Load enum values into comboBox1
            button2.Click += button2_Click; // Assign event handler for Create button
        }

        private void LoadStatusOptions()
        {
            comboBox1.Items.Clear();

            SqlCommand cmd = new SqlCommand("dbo.GetAllEquipmentStatuses");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SQL_CON sc = new SQL_CON();
            SqlDataReader reader = sc.execute_query(cmd);

            while (reader.Read())
            {
                comboBox1.Items.Add(reader["equipmentStatus"].ToString());
            }

            reader.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string id = textboxId.Text.Trim();
                string name = textBox1.Text.Trim();
                string status = comboBox1.SelectedItem?.ToString();
                string details = richTextBox1.Text.Trim();

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(status))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                // צור את האובייקט וכתוב למסד דרך הפרוצדורה
                MedicalEquipment newEq = new MedicalEquipment(id, name, status, details, true);
                MessageBox.Show("Equipment added successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while adding equipment: " + ex.Message);
            }
        }

        private void textboxId_TextChanged(object sender, EventArgs e)
        {
            // leave empty or implement if needed
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // leave empty or implement if needed
        }
    }
}
