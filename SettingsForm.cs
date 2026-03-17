using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ClipSaver
{
    public class SettingsForm : Form
    {
        private TextBox _folderTextBox = null!;
        private Button _browseButton = null!;
        private Button _saveButton = null!;
        private Button _openFolderButton = null!;
        private Label _statusLabel = null!;

        private AppSettings _settings;
        public event Action<AppSettings>? SettingsSaved;

        public SettingsForm(AppSettings settings)
        {
            _settings = settings;
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "ClipSaver 설정";
            this.Size = new Size(420, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;

            var folderLabel = new Label
            {
                Text = "저장 폴더",
                Location = new Point(16, 20),
                Size = new Size(80, 20),
                ForeColor = Color.FromArgb(180, 180, 180)
            };

            _folderTextBox = new TextBox
            {
                Location = new Point(16, 42),
                Size = new Size(280, 24),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            _browseButton = new Button
            {
                Text = "찾아보기",
                Location = new Point(304, 41),
                Size = new Size(90, 26),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _browseButton.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            _browseButton.Click += BrowseButton_Click;

            _openFolderButton = new Button
            {
                Text = "폴더 열기",
                Location = new Point(16, 88),
                Size = new Size(90, 28),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _openFolderButton.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            _openFolderButton.Click += OpenFolderButton_Click;

            _saveButton = new Button
            {
                Text = "저장",
                Location = new Point(304, 88),
                Size = new Size(90, 28),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _saveButton.FlatAppearance.BorderColor = Color.FromArgb(0, 100, 190);
            _saveButton.Click += SaveButton_Click;

            _statusLabel = new Label
            {
                Text = "",
                Location = new Point(16, 125),
                Size = new Size(380, 20),
                ForeColor = Color.FromArgb(100, 200, 100),
                Font = new Font("Segoe UI", 8f)
            };

            this.Controls.AddRange(new Control[]
            {
                folderLabel, _folderTextBox, _browseButton,
                _openFolderButton, _saveButton, _statusLabel
            });
        }

        private void LoadSettings()
        {
            _folderTextBox.Text = _settings.SaveFolder;
        }

        private void BrowseButton_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "이미지를 저장할 폴더를 선택하세요",
                SelectedPath = _folderTextBox.Text
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _folderTextBox.Text = dialog.SelectedPath;
            }
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            string folder = _folderTextBox.Text.Trim();
            if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("폴더를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _settings.SaveFolder = folder;
            _settings.Save();
            SettingsSaved?.Invoke(_settings);

            _statusLabel.Text = "✓ 설정이 저장되었습니다.";
        }

        private void OpenFolderButton_Click(object? sender, EventArgs e)
        {
            string folder = _settings.SaveFolder;
            if (Directory.Exists(folder))
            {
                System.Diagnostics.Process.Start("explorer.exe", folder);
            }
            else
            {
                MessageBox.Show("폴더가 존재하지 않습니다.\n먼저 설정을 저장해주세요.", "알림",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
