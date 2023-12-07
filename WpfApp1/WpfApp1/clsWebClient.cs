using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;

namespace WpfApp1
{
    internal class clsWebClient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void clsWebClient_Delegate();

        // 속성 변경을 알리는 메서드
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private WebClient webClient = null;
        private Stopwatch swTime = new Stopwatch();
        private string m_strLocalPath = "";


        // 다운로드 상태를 나타내는 속성
        private string _clsWebClient_Status;
        public string clsWebClient_Status
        {
            get { return _clsWebClient_Status; }
            set
            {
                if (_clsWebClient_Status != value)
                {
                    _clsWebClient_Status = value;
                    OnPropertyChanged();
                }
            }
        }

        // 다운로드 속도를 나타내는 속성
        private string _clsWebClient_DownloadSpeed;
        public string clsWebClient_DownloadSpeed
        {
            get { return _clsWebClient_DownloadSpeed; }
            set
            {
                if (_clsWebClient_DownloadSpeed != value)
                {
                    _clsWebClient_DownloadSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        // 다운로드 진행률을 나타내는 속성
        private int _clsWebClient_ProgressPercentage;
        public int clsWebClient_ProgressPercentage
        {
            get { return _clsWebClient_ProgressPercentage; }
            set
            {
                if (_clsWebClient_ProgressPercentage != value)
                {
                    _clsWebClient_ProgressPercentage = value;
                    OnPropertyChanged();
                }
            }
        }


        public async void GetWebClient(string strUrl)
        {
            // 로컬에 저장될 파일 경로 설정
            m_strLocalPath = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, GetFileNameFromUrl(strUrl));

            // 초기화
            this.clsWebClient_Status = "";
            this.clsWebClient_DownloadSpeed = "";

            try
            {
                using (webClient = new WebClient())
                {
                    webClient.Headers.Set("User-Agent", "Test");
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
                    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;

                    // 스톱워치 시작
                    swTime.Start();

                    // 파일 다운로드 비동기 수행
                    await webClient.DownloadFileTaskAsync(new Uri(strUrl), m_strLocalPath);
                }
            }
            catch (Exception e)
            {
                this.clsWebClient_Status = e.Message;
            }
        }

        // 파일 다운로드 완료 시
        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // 다운로드 취소 시 상태 업데이트
                this.clsWebClient_Status = "Download canceled.";
            }
            else if (e.Error != null)
            {
                // 다운로드 중 오류 발생 시 상태 업데이트
                this.clsWebClient_Status = $"Error: {e.Error.Message}";
            }
            else
            {
                // 다운로드 완료 시 상태 업데이트
                string strMessage = $"{GetFileNameFromLocalPath(m_strLocalPath)} completed";
                this.clsWebClient_Status = strMessage;
            }

            // 속성 변경 알림
            OnPropertyChanged(nameof(clsWebClient_ProgressPercentage));

            // 스톱워치 초기화
            swTime.Reset();
        }

        // 파일 다운로드 중 진행률 변경 시 
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string strSpeedMB = $"{(e.BytesReceived / 1024d / 1024d / swTime.Elapsed.TotalSeconds):0.00} MB/s";
            string strSpeedKB = $"{(e.BytesReceived / 1024d / swTime.Elapsed.TotalSeconds):0.00} KB/s";

            string strSpeed = $"{strSpeedMB} / {strSpeedKB}";
            this.clsWebClient_DownloadSpeed = strSpeed;
            this.clsWebClient_ProgressPercentage = e.ProgressPercentage;

            // 속성 변경 알림
            OnPropertyChanged(nameof(clsWebClient_ProgressPercentage));
        }

        private string GetFileNameFromUrl(string strUrl)
        {
            return strUrl.Substring(strUrl.LastIndexOf("/") + 1);
        }

        private string GetFileNameFromLocalPath(string sFilePath)
        {
            return sFilePath.Substring(sFilePath.LastIndexOf("\\") + 1);
        }

        private string GetFilePathFromLocalPath(string sFilePath)
        {
            return sFilePath.Substring(0, sFilePath.LastIndexOf("\\"));
        }
    }
}
