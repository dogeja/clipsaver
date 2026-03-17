using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ClipSaver
{
    public class TrayApplicationContext : ApplicationContext
    {
        private NotifyIcon _trayIcon = null!;
        private ClipboardMonitor _monitor = null!;
        private AppSettings _settings;
        private int _savedCount = 0;

        public TrayApplicationContext()
        {
            _settings = AppSettings.Load();
            InitializeTray();
            InitializeMonitor();
        }

        private void InitializeTray()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.BackColor = Color.FromArgb(30, 30, 30);
            contextMenu.ForeColor = Color.White;
            contextMenu.Renderer = new DarkMenuRenderer();

            var statusItem = new ToolStripMenuItem("● 감지 중...")
            {
                Enabled = false,
                ForeColor = Color.FromArgb(100, 200, 100)
            };

            var settingsItem = new ToolStripMenuItem("설정");
            settingsItem.Click += (s, e) => OpenSettings();

            var openFolderItem = new ToolStripMenuItem("저장 폴더 열기");
            openFolderItem.Click += (s, e) => OpenSaveFolder();

            var separator = new ToolStripSeparator();

            var exitItem = new ToolStripMenuItem("종료");
            exitItem.Click += (s, e) => ExitApp();

            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                statusItem, new ToolStripSeparator(),
                settingsItem, openFolderItem,
                separator, exitItem
            });

            _trayIcon = new NotifyIcon
            {
                Icon = CreateTrayIcon(),
                Text = "ClipSaver - 클립보드 이미지 자동 저장",
                Visible = true,
                ContextMenuStrip = contextMenu
            };

            _trayIcon.DoubleClick += (s, e) => OpenSettings();
        }

        private void InitializeMonitor()
        {
            _monitor = new ClipboardMonitor(_settings);

            _monitor.ImageSaved += (filePath) =>
            {
                _savedCount++;
                string fileName = Path.GetFileName(filePath);
                _trayIcon.ShowBalloonTip(
                    2000,
                    "ClipSaver",
                    $"저장됨: {fileName}",
                    ToolTipIcon.Info
                );
                _trayIcon.Text = $"ClipSaver - {_savedCount}개 저장됨";
            };

            _monitor.ErrorOccurred += (error) =>
            {
                _trayIcon.ShowBalloonTip(
                    2000,
                    "ClipSaver - 오류",
                    error,
                    ToolTipIcon.Error
                );
            };

            _monitor.Show();
            _monitor.Hide();
        }

        private void OpenSettings()
        {
            var form = new SettingsForm(_settings);
            form.SettingsSaved += (newSettings) =>
            {
                _settings = newSettings;
                _monitor.UpdateSettings(_settings);
            };
            form.ShowDialog();
        }

        private void OpenSaveFolder()
        {
            string folder = _settings.SaveFolder;
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            System.Diagnostics.Process.Start("explorer.exe", folder);
        }

        private void ExitApp()
        {
            _trayIcon.Visible = false;
            _monitor.Close();
            Application.Exit();
        }

        private Icon CreateTrayIcon()
        {
            var bmp = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.FillRectangle(new SolidBrush(Color.FromArgb(0, 120, 215)), 1, 4, 14, 10);
                g.FillEllipse(Brushes.White, 4, 6, 8, 6);
                g.FillEllipse(new SolidBrush(Color.FromArgb(0, 120, 215)), 6, 7, 4, 4);
            }
            return Icon.FromHandle(bmp.GetHicon());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _trayIcon?.Dispose();
                _monitor?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class DarkMenuRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(60, 60, 60)),
                    e.Item.ContentRectangle
                );
            }
            else
            {
                e.Graphics.FillRectangle(
                    new SolidBrush(Color.FromArgb(30, 30, 30)),
                    e.Item.ContentRectangle
                );
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(Color.FromArgb(30, 30, 30)),
                e.AffectedBounds
            );
        }
    }
}
