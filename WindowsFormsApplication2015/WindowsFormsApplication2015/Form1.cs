using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication2015
{
    public partial class Form1 : Form
    {
        private clsWebClient webDown;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webDown = new clsWebClient();
            webDown.WebClientStatus += WebDown_WebClientStatus;
        }

        private void WebDown_WebClientStatus()
        {
            progressBar1.Value = webDown.clsWebClient_ProgressPercentage;
            label1.Text = webDown.clsWebClient_DownloadSpeed;
            label2.Text = webDown.clsWebClient_Status;
        }
               
        private void button1_Click(object sender, EventArgs e)
        {
            string strUrl = "http://images.freeimages.com/images/previews/0e6/war-soldier-military-arms-hand-legsnt-1633631.jpg";
            webDown.GetWebClient(strUrl);
        }        

    }
}
