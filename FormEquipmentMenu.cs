using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormEquipmentMenu : Form
    {
        public FormEquipmentMenu()
        {
            InitializeComponent();

            // חיבור האירוע Load ברגע שהטופס נוצר
            this.Load += FormEquipmentMenu_Load;
        }

        private void FormEquipmentMenu_Load(object sender, EventArgs e)
        {
            try
            {
                SQL_CON sql = new SQL_CON();
                SqlCommand cmd = new SqlCommand("GetOverdueEquipment");
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = sql.execute_query(cmd);

                if (reader != null && reader.HasRows)
                {
                    DialogResult result = MessageBox.Show(
                        "Some equipment has not been returned on time.\nWould you like to view the list?",
                        "Overdue Equipment Alert",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.OK)
                    {
                        // סוגרים את הטופס הנוכחי ופותחים את רשימת הציוד המאחר
                        this.Hide(); // או this.Close() אם מתאים
                        FormOverdueEquipment overdueForm = new FormOverdueEquipment();
                        overdueForm.ShowDialog();
                        this.Show(); // מחזירים את הטופס הנוכחי אם רוצים
                    }
                    else
                    {
                        // המשתמש לחץ על Cancel – לא עושים כלום וממשיכים במסך הנוכחי
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while checking overdue equipment:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Add Equipment
            FormCreateEquipment createForm = new FormCreateEquipment();
            createForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Edit/Delete Equipment
            FormEditDeleteEquipment editForm = new FormEditDeleteEquipment();
            editForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // View All Equipment
            FormViewAllEquipment viewForm = new FormViewAllEquipment();
            viewForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Loan Equipment
            FormLoanEquipment loanForm = new FormLoanEquipment();
            loanForm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Loan History
            FormLoanHistory historyForm = new FormLoanHistory();
            historyForm.ShowDialog();
        }
    }
}
