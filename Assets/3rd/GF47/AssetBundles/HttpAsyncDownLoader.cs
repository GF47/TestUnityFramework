using System;
using System.IO;
using System.Net;
using UnityEngine;

namespace Assets
{
    public class HttpAsyncDownLoader : IDisposable
    {
        public enum State
        {
            DownLoading,
            Completed,
            Failed,
        }

        private const int TIME_OUT_THRESHOLD = 20000;
        private const int READ_WRITE_TIME_OUT_THRESHOLD = 10000;
        private const int BUFFER_SIZE = 1024;

        public static int TimeOutThreshold = TIME_OUT_THRESHOLD;
        public static int ReadWriteTimeOutThreshold = READ_WRITE_TIME_OUT_THRESHOLD;

        public string url;
        public string nativePath;
        public int percent;

        public Action<Exception> errorCallback;
        public Action<bool> downLoadFinishedCallback;

        private HttpWebRequest _request;
        private WebResponse _response;

        private long _startPos;
        private FileStream _fileStream;

        public HttpAsyncDownLoader(string url, string nativePath, Action<Exception> errorCallback = null, Action<bool> downLoadFinishedCallback = null)
        {
            this.url = url;
            this.nativePath = nativePath;
            this.errorCallback = errorCallback;
            this.downLoadFinishedCallback = downLoadFinishedCallback;
            
            //System.Net.ServicePointManager.DefaultConnectionLimit
        }

        public void Start()
        {
            if (File.Exists(nativePath))
            {
                _fileStream = File.OpenWrite(nativePath);
                _startPos = _fileStream.Length;
                _fileStream.Seek(_startPos, SeekOrigin.Current);
            }
            else
            {
                string dir = Path.GetDirectoryName(nativePath);
                if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }
                _fileStream = new FileStream(nativePath, FileMode.Create);
                _startPos = 0;
            }

            if (_request != null) { _request.Abort(); }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            _request = request;
            _request.Timeout = TimeOutThreshold;
            _request.ReadWriteTimeout = ReadWriteTimeOutThreshold;
            if (_startPos > 0)
            {
                _request.AddRange((int)_startPos);
            }

            _request.BeginGetResponse(DownLoadFinishedCallback, this);
        }

        public void Stop()
        {
            if (_fileStream != null)
            {
                _fileStream.Flush();
                _fileStream.Close();
            }
        }

        public void Dispose()
        {
            if (_fileStream != null) { _fileStream.Close(); _fileStream = null; }
            if (_response != null) { _response.Close(); _response = null; }
            if (_request != null) { _request.Abort(); _request = null; }
        }

        private static void DownLoadFinishedCallback(IAsyncResult ar)
        {
            HttpAsyncDownLoader state = (HttpAsyncDownLoader)ar.AsyncState;

            try
            {
                WebResponse response = state._request.EndGetResponse(ar);
                state._response = response;

                Stream stream = response.GetResponseStream();

                long contentLength = response.ContentLength;
                long currentSize = state._startPos;

                byte[] bytes = new byte[BUFFER_SIZE];
                int readSize = stream.Read(bytes, 0, bytes.Length);
                currentSize += readSize;
                while (readSize > 0)
                {
                    state._fileStream.Write(bytes, 0, readSize);

                    readSize = stream.Read(bytes, 0, bytes.Length);
                    currentSize += readSize;

                    int percent = (int)((double)currentSize * 100 / contentLength);
                    if (state.percent < percent) { state.percent = percent; }
                }

                state._fileStream.Flush();
                stream.Close();

                state.Dispose();
                if (state.downLoadFinishedCallback != null) { state.downLoadFinishedCallback(true); }

            }
            catch (Exception e)
            {
                state.Dispose();
                FileInfo info = new FileInfo(state.nativePath);
                if (info.Exists && info.Length == 0)
                {
                    info.Delete();
                }
                if (state.downLoadFinishedCallback != null) { state.downLoadFinishedCallback(false); }
                if (state.errorCallback != null) { state.errorCallback(e); }
            }
        }
    }
}
