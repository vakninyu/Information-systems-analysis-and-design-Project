using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatanProject2
{
    public partial class FormCaregiverMenu : Form
    {
        public FormCaregiverMenu()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        private void label1_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
         
            FormCreateCaregiver createForm = new FormCreateCaregiver();
            createForm.ShowDialog();
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormEditDeleteCaregiver editForm = new FormEditDeleteCaregiver();
            editForm.ShowDialog();
        
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FormViewAllCaregivers viewForm = new FormViewAllCaregivers();
            viewForm.ShowDialog();
        }
    }
}
