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
    public partial class FormMenuComplaint : Form
    {
        public FormMenuComplaint()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCreateComplaint complaimtForm = new FormCreateComplaint();
            complaimtForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormEditComplaint complaimtForm = new FormEditComplaint();
            complaimtForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormViewComplaints complaimtForm = new FormViewComplaints();
            complaimtForm.ShowDialog();
        }
    }
}
