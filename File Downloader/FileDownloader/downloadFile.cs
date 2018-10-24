using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace DownloadHelper
{
    public class downloadFile
    {
        #region Variables

        private readonly Uri uri;
        private HttpWebRequest req;
        private HttpWebResponse res;
        private Stream stream;
        private FileStream fStream;
        private DateTime dt;
        private byte[] buffer = new byte[1024];
        private int bufferReader;
        private long cLength;
        private double dSpeed;
        private bool isDownloading;
        private bool cancelDownload;
        private readonly bool overwrite;
        private readonly Timer dsTimer = new Timer(1000);
        private readonly Timer dlTimer = new Timer(1000);

        private readonly string filePath;

        #endregion

        #region Properties

        public long FileSize { get; private set; }

        public long DownloadedLength { get; private set; }

        public string DownloadingSpeed => $"{dSpeed:n1} Kb/s";

        public double DSpeedUI
        {
            get => dsTimer.Interval;
            set => dsTimer.Interval = value;
        }

        public double DLengthUI
        {
            get => dlTimer.Interval;
            set => dlTimer.Interval = value;
        }

        public int BufferSize
        {
            get => BufferSize;
            set
            {
                if (!isDownloading && value > 32 && value < int.MaxValue)
                {
                    buffer = new byte[value];
                    BufferSize = value;
                }
            }
        }

        public string DownloadState
        {
            get
            {
                if (isDownloading)
                {
                    return "Downloading";
                }

                if (DownloadedLength < FileSize)
                {
                    return "Paused";
                }

                return "Completed";
            }
        }

        #endregion

        #region EventHandler

        public EventHandler<long> eSize;
        public EventHandler<long> eDownloadedSize;
        public EventHandler<string> eSpeed;
        public EventHandler<string> eDownloadState;

        #endregion

        #region Methods

        private async void download()
        {
            dlTimer.Start();
            dsTimer.Start();

            cancelDownload = false;
            req = (HttpWebRequest) WebRequest.Create(uri);

            if (DownloadedLength > 0 && !overwrite)
            {
                req.AddRange(DownloadedLength);
            }

            isDownloading = true;
            using (res = (HttpWebResponse) await req.GetResponseAsync())
            {
                FileSize = res.ContentLength + DownloadedLength;
                eSize.Invoke(null, FileSize);
                eDownloadedSize.Invoke(null, DownloadedLength);
                dt = DateTime.Now;
                using (stream = res.GetResponseStream())
                {
                    await Task.Run(() =>
                    {
                        eDownloadState.Invoke(null, "Downloading");

                        while (!cancelDownload && (bufferReader = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fStream.Write(buffer, 0, bufferReader);
                            DownloadedLength += bufferReader;
                            cLength += bufferReader;
                        }
                    });
                }
            }

            dlTimer.Stop();
            dsTimer.Stop();
            isDownloading = false;
            eSpeed.Invoke(null, "0.0 Kb/s");
            eDownloadedSize.Invoke(null, DownloadedLength);
            eDownloadState.Invoke(null, DownloadState);
            fStream.Dispose();
        }


        private void DlTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            eDownloadedSize.Invoke(null, DownloadedLength);
        }

        private void DsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            dSpeed = cLength / 1024 / (DateTime.Now - dt).TotalSeconds;
            eSpeed.Invoke(null, DownloadingSpeed);
        }

        public void CancelDownload()
        {
            cancelDownload = true;
        }

        public void ResumeDownload()
        {
            if (DownloadState == "Paused")
            {
                if (File.Exists(filePath) && !overwrite)
                {
                    fStream = new FileStream(filePath, FileMode.Append, FileAccess.Write);
                    if (DownloadedLength == fStream.Length)
                    {
                        download();
                        return;
                    }
                }
            }

            throw new ArgumentException("Cannot Resume Download");
        }


        public downloadFile(string url, string filePath, bool overwrite = false)
        {
            var validUri = Uri.TryCreate(url, UriKind.Absolute, out uri);

            if (!validUri)
            {
                throw new ArgumentException("Invalid url");
            }

            this.filePath = filePath;
            if (File.Exists(filePath) && !overwrite)
            {
                ResumeDownload();
            }
            else
            {
                fStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

                DownloadedLength = fStream.Length;
                this.overwrite = overwrite;

                dlTimer.Elapsed += DlTimer_Elapsed;
                dsTimer.Elapsed += DsTimer_Elapsed;

                download();
            }
        }

        public void Dispose()
        {
            dlTimer.Stop();
            dsTimer.Stop();
            fStream?.Dispose();
            res?.Dispose();
            stream?.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}