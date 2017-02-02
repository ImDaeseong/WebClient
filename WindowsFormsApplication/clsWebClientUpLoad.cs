using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    //.Net 3.5
    public class clsWebClientUpLoad
    {
        public event clsWebClient_Delegate WebClientStatus;
        public delegate void clsWebClient_Delegate();


        private static WebClient webClient = null;

        private Stopwatch swTime = new Stopwatch();

        public string clsWebClient_Status { get; set; }
        public string clsWebClient_UPloadSpeed { get; set; }
        public int clsWebClient_ProgressPercentage { get; set; }


        private string m_strLocalPath = "";
        private string m_strContent = "";
                
        public void UploadFileAsync(string strUrl, string strLocalPath)
        {
            m_strLocalPath = strLocalPath;

            this.clsWebClient_Status = "";
            this.clsWebClient_UPloadSpeed = "";

            try
            {
                webClient = new WebClient();
                webClient.Headers.Set("User-Agent", "Test");
                webClient.UploadFileCompleted += new UploadFileCompletedEventHandler(webClient_UploadFileCompleted);
                webClient.UploadProgressChanged += new UploadProgressChangedEventHandler(webClient_UploadProgressChanged);

                swTime.Start();

                webClient.UploadFileAsync(new Uri(strUrl), strLocalPath);
            }
            catch (Exception e)
            {
                this.clsWebClient_Status = e.Message;
            }
        }

        void webClient_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.clsWebClient_Status = e.Cancelled.ToString();
            }
            else if (e.Error != null)
            {
                this.clsWebClient_Status = e.Error.ToString();
            }
            else
            {
                string strMessage = string.Format("{0} Completed", m_strLocalPath);
                this.clsWebClient_Status = strMessage;
            }

            WebClientStatus();

            swTime.Reset();

            webClient.CancelAsync();
            webClient.Dispose();
        }

        void webClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            string strSpeed = string.Format("{0} kb/s", (e.BytesReceived / 1024d / swTime.Elapsed.TotalSeconds).ToString("0.00"));
            this.clsWebClient_UPloadSpeed = strSpeed;
            this.clsWebClient_ProgressPercentage = e.ProgressPercentage;

            WebClientStatus();
        } 
      
        public void UploadStringAsync(string strUrl, string strContent)
        {
            m_strContent = strContent;

            this.clsWebClient_Status = "";
            this.clsWebClient_UPloadSpeed = "";

            try
            {
                webClient = new WebClient();
                webClient.Headers.Set("User-Agent", "Test");

                webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
                webClient.UploadProgressChanged += new UploadProgressChangedEventHandler(webClient_UploadProgressChanged);

                swTime.Start();

                webClient.UploadStringAsync(new Uri(strUrl), strContent);
            }
            catch (Exception e)
            {
                this.clsWebClient_Status = e.Message;
            }
        }

        void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.clsWebClient_Status = e.Cancelled.ToString();
            }
            else if (e.Error != null)
            {
                this.clsWebClient_Status = e.Error.ToString();
            }
            else
            {
                string strMessage = string.Format("{0} Completed", m_strContent);
                this.clsWebClient_Status = strMessage;
            }

            WebClientStatus();

            swTime.Reset();

            webClient.CancelAsync();
            webClient.Dispose();
        }

    }
}
