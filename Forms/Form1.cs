using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotProcessApplication
{
    public partial class InitialForm : Form
    {
        public InitialForm()
        {
            InitializeComponent();
        }

        private void InitialForm_Load(object sender, EventArgs e)
        {
            this.Text = "ロボットプロセスアプリケーション";
        }
    }
}
