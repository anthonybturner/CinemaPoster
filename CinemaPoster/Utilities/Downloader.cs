using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPoster.Utilities
{
    public class Downloader
    {

        public event EventHandler DownloadProgress;
        DownloaderEventArgs downloaderEventArgs;

        public void DownloadStarted(DownloaderEventArgs e)
        {
            EventHandler downloadProgress = DownloadProgress;
            if (downloadProgress != null)
                downloadProgress(this, e);
        }

        public async Task Download(string url, string saveAs)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));
            var parallelDownloadSuported = response.Headers.AcceptRanges.Contains("bytes");
            var contentLength = response.Content.Headers.ContentLength ?? 0;

            if (parallelDownloadSuported)
            {
                const double numberOfParts = 5.0;
                var tasks = new List<Task>();
                var partSize = (long)Math.Ceiling(contentLength / numberOfParts);

                File.Create(saveAs).Dispose();

                for (var i = 0; i < numberOfParts; i++)
                {
                    var start = i * partSize + Math.Min(1, i);
                    var end = Math.Min((i + 1) * partSize, contentLength);

                    tasks.Add(
                        Task.Run(() => DownloadPart(url, saveAs, start, end))
                        );
                }

                await Task.WhenAll(tasks);
            }
        }

        private async void DownloadPart(string url, string saveAs, long start, long end)
        {
            using (var httpClient = new HttpClient())
            using (var fileStream = new FileStream(saveAs, FileMode.Open, FileAccess.Write, FileShare.Write))
            {
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Add("Range", string.Format("bytes={0}-{1}", start, end));

                fileStream.Position = start;
                await httpClient.SendAsync(message).Result.Content.CopyToAsync(fileStream);
            }
        }
    }

    public class DownloaderEventArgs : EventArgs
    {
        public string Filename { get; private set; }
        public int Progress { get; private set; }

        public DownloaderEventArgs(int progress, string filename)
        {
            Progress = progress;
            Filename = filename;
        }
        public DownloaderEventArgs(int progress) : this(progress, String.Empty)
        {
            Progress = progress;
        }
    }
}

    