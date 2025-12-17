using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormOverdueEquipment : Form
    {
        public FormOverdueEquipment()
        {
            InitializeComponent();
            LoadOverdueEquipment();
        }

        private void LoadOverdueEquipment()
        {
            try
            {
                SQL_CON sql = new SQL_CON();
                SqlCommand cmd = new SqlCommand("dbo.GetOverdueEquipment");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = sql.execute_query(cmd);

                DataTable dt = new DataTable();
                dt.Load(reader); // ממיר את ה־SqlDataReader לטבלה

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading overdue equipment:\n" + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
