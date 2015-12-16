using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb
{
    public partial class frmLogin : Form
    {
        XA_SESSIONLib.XASession xas;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            xas = new XA_SESSIONLib.XASession("XA_Session.XASession");
            ((XA_SESSIONLib._IXASessionEvents_Event)xas).Login += xingSession_Login;
        }

        public void xingSession_Login(string code, string msg)
        {
            if (code.Equals("0000"))
            {
                txtMsg.Text = msg;
                this.Close();
                //txtMsg.Text = msg;
                //frmInterList frm = new frmInterList();
                //frm.Owner = this;
                //frm.Show();
            }
            else
            {
                txtMsg.Text = msg;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            xas.DisconnectServer();

            bool chk = xas.ConnectServer(cmbServer.Text, 20001);

            if (chk)
            {
                bool chk1 = ((XA_SESSIONLib.IXASession)xas).Login(txtID.Text, txtPass.Text, txtKey.Text, 0, false);
            }
            else
            {
                MessageBox.Show("커넥션 실패");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
