﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApplication2015
{
    public class clsWebClient
    {
        public event clsWebClient_Delegate WebClientStatus;
        public delegate void clsWebClient_Delegate();


        private static WebClient webClient = null;

        private Stopwatch swTime = new Stopwatch();

        public string clsWebClient_Status { get; set; }
        public string clsWebClient_DownloadSpeed { get; set; }
        public int clsWebClient_ProgressPercentage { get; set; }


        private string m_strLocalPath = "";


        public async void GetWebClient(string strUrl)
        {
            m_strLocalPath = string.Format("{0}\\{1}", Application.StartupPath, GetFileNameFromUrl(strUrl));

            this.clsWebClient_Status = "";
            this.clsWebClient_DownloadSpeed = "";

            try
            {
                webClient = new WebClient();
                webClient.Headers.Set("User-Agent", "Test");
                webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                
                swTime.Start();

                await webClient.DownloadFileTaskAsync(strUrl, m_strLocalPath);
            }
            catch (Exception e)
            {
                this.clsWebClient_Status = e.Message;
            }
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
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
                string strMessage = string.Format("{0} Completed", GetFileNameFromLocalPath(m_strLocalPath));
                this.clsWebClient_Status = strMessage;
            }

            WebClientStatus();

            swTime.Reset();

            webClient.CancelAsync();
            webClient.Dispose();
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //string strPercent = e.ProgressPercentage.ToString() + "%";
            //string strDownloaded = string.Format("{0} MB's / {1} MB's", (e.BytesReceived / 1024d / 1024d).ToString("0.00"),  (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            //Console.WriteLine(e.ProgressPercentage.ToString());

            string strSpeed = string.Format("{0} kb/s", (e.BytesReceived / 1024d / swTime.Elapsed.TotalSeconds).ToString("0.00"));
            this.clsWebClient_DownloadSpeed = strSpeed;
            this.clsWebClient_ProgressPercentage = e.ProgressPercentage;

            WebClientStatus();
        }               

        private string GetFileNameFromUrl(string strUrl)
        {
            string strfileName = strUrl.Substring(strUrl.LastIndexOf("/") + 1);
            return strfileName;
        }

        private string GetFileNameFromLocalPath(string sFilePath)
        {
            string strfileName = sFilePath.Substring(sFilePath.LastIndexOf("\\") + 1);
            return strfileName;
        }

        private string GetFilePathFromLocalPath(string sFilePath)
        {
            string strfileName = sFilePath.Substring(0, sFilePath.LastIndexOf("\\"));
            return strfileName;
        }
          
    }
    
}
