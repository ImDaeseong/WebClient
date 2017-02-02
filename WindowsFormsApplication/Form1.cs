using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace WindowsFormsApplication
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
            webDown.WebClientStatus += new clsWebClient.clsWebClient_Delegate(webDown_WebClientStatus);   
        }

        void webDown_WebClientStatus()
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



        //upload
        private bool UploadFile(string strUrl, string sFilePath, ref byte[] byResult)
        {
            bool bSuccess = true;
            using (var webClient = new WebClient())
            {
                try
                {
                    byResult = webClient.UploadFile(strUrl, sFilePath);
                }
                catch
                {
                    bSuccess = false;
                }

                webClient.Dispose();
                return bSuccess;
            }
        }
        private string UploadString(string strUrl, string strContent)
        {
            string strResult;
            using (var webClient = new WebClient())
            {
                try
                {
                    strResult = webClient.UploadString(strUrl, strContent);
                }
                catch
                {
                    strResult = "";
                }

                webClient.Dispose();
                return strResult;
            }
        }
        private void UploadFile(string sFilePath)
        {
            string strResult = UploadString("http://page", ""); 

            byte[] reponse = null;
            if (UploadFile("http://page", sFilePath, ref reponse))
            {
                string strMsg = Encoding.UTF8.GetString(reponse);
                Console.WriteLine(strMsg);
            }
            else
            {
                Console.WriteLine("fail");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                UploadFile(fileDialog.FileName);
            }
        }

    }
}
