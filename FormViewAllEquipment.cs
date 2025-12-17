using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormViewAllEquipment : Form
    {
        SQL_CON sql = new SQL_CON();

        public FormViewAllEquipment()
        {
            InitializeComponent();

            // חיבור כפתורים לאירועים
            button1.Click += button1_Click; // Refresh
            button2.Click += button2_Click; //Search

            textBox1.Enter += textBox1_Enter;
            textBox1.Leave += textBox1_Leave;

            textBox1.Text = "Search by name";
            textBox1.ForeColor = Color.Gray;

            textBox1.TextChanged += textBox1_TextChanged;

        }

        private void FormViewAllEquipment_Load(object sender, EventArgs e)
        {
            LoadEquipmentData();
        }

        private void LoadEquipmentData()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("dbo.GetAllMedicalEquipment");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = sql.execute_query(cmd);
                DataTable dt = new DataTable();
                dt.Load(reader);

                dataGridView1.DataSource = dt;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadEquipmentData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(name) && name != "Search")
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("dbo.SearchEquipmentByName");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", name);

                    SQL_CON sql = new SQL_CON();
                    SqlDataReader reader = sql.execute_query(cmd);

                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dataGridView1.DataSource = dt;
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching equipment: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter a name to search.");
            }
        }
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Search by name")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Search by name";
                textBox1.ForeColor = Color.Gray;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();

            if (name == "Search by name" || string.IsNullOrEmpty(name))
            {
                LoadAllEquipment(); // מחזיר את כל הציוד
                return;
            }

            try
            {
                SqlCommand cmd = new SqlCommand("dbo.SearchEquipmentByName"); 
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", name);

                SQL_CON sql = new SQL_CON();
                SqlDataReader reader = sql.execute_query(cmd);

                DataTable dt = new DataTable();
                dt.Load(reader);

                dataGridView1.DataSource = dt;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching equipment: " + ex.Message);
            }
        }
        private void LoadAllEquipment()
        {
            try
            {
                SQL_CON sql = new SQL_CON();
                SqlCommand cmd = new SqlCommand("SELECT * FROM MedicalEquipment");
                SqlDataReader reader = sql.execute_query(cmd);

                DataTable dt = new DataTable();
                dt.Load(reader);

                dataGridView1.DataSource = dt;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading equipment: " + ex.Message);
            }
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // אפשר להשאיר ריק או להשתמש בהמשך לפתיחת טופס עריכה
        }

      

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
