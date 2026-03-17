using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ClipSaver
{
    public class ClipboardMonitor : Form
    {
        // Win32 API
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        private const int WM_CLIPBOARDUPDATE = 0x031D;

        public event Action<string>? ImageSaved;
        public event Action<string>? ErrorOccurred;

        private string _lastImageHash = string.Empty;
        private AppSettings _settings;

        public ClipboardMonitor(AppSettings settings)
        {
            _settings = settings;

            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Size = new System.Drawing.Size(1, 1);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AddClipboardFormatListener(this.Handle);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            RemoveClipboardFormatListener(this.Handle);
            base.OnFormClosed(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CLIPBOARDUPDATE)
            {
                OnClipboardChanged();
            }
            base.WndProc(ref m);
        }

        private void OnClipboardChanged()
        {
            try
            {
                if (!Clipboard.ContainsImage()) return;

                Image? img = Clipboard.GetImage();
                if (img == null) return;

                string hash = ComputeImageHash((Bitmap)img);
                if (hash == _lastImageHash) return;
                _lastImageHash = hash;

                string folder = _settings.SaveFolder;
                Directory.CreateDirectory(folder);

                string fileName = $"clipsaver_{DateTime.Now:yyyy-MM-dd_HHmmss}.png";
                string fullPath = Path.Combine(folder, fileName);

                img.Save(fullPath, ImageFormat.Png);
                img.Dispose();

                ImageSaved?.Invoke(fullPath);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(ex.Message);
            }
        }

        private string ComputeImageHash(Bitmap bmp)
        {
            try
            {
                using var ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                byte[] bytes = ms.ToArray();
                byte[] hash = MD5.HashData(bytes);
                return Convert.ToHexString(hash);
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
        }

        public void UpdateSettings(AppSettings settings)
        {
            _settings = settings;
        }
    }
}
