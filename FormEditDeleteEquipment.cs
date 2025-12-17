using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormEditDeleteEquipment : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormEditDeleteEquipment()
        {
            InitializeComponent();
            LoadStatuses();
            button1.Click += button1_Click;
            button2.Click += button2_Click;
        }

        private void LoadStatuses()
        {
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("dbo.GetAllEquipmentStatuses");
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = sql.execute_query(cmd);

            while (reader.Read())
            {
                comboBox1.Items.Add(reader["equipmentStatus"].ToString());
            }

            reader.Close();
        }

 

        private void textboxId_TextChanged(object sender, EventArgs e)
        {
            if (textboxId.Text.Length > 0)
            {
                SqlCommand cmd = new SqlCommand("dbo.GetEquipmentById");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@equipmentId", textboxId.Text);

                SqlDataReader reader = sql.execute_query(cmd);

                if (reader.Read())
                {
                    textBox1.Text = reader["name"].ToString();
                    comboBox1.SelectedItem = reader["status"].ToString();
                    richTextBox1.Text = reader["details"].ToString();
                }

                reader.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("dbo.UpdateMedicalEquipment");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@equipmentId", textboxId.Text);
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@status", comboBox1.SelectedItem?.ToString());
            cmd.Parameters.AddWithValue("@details", richTextBox1.Text);            

            sql.execute_non_query(cmd);

            MessageBox.Show("Equipment updated successfully.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this equipment?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("dbo.DeleteMedicalEquipment");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@equipmentId", textboxId.Text);

                sql.execute_non_query(cmd);

                MessageBox.Show("Equipment deleted successfully.");
                ClearForm();
            }
        }

        private void ClearForm()
        {
            textboxId.Clear(); 
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            richTextBox1.Clear();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
