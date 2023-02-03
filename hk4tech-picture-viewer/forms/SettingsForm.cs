﻿using Microsoft.Win32;
using QuickLibrary;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace hk4tech_picture_viewer
{
	partial class SettingsForm : QlibFixedForm
	{
		private struct Language
		{
			public string Code;
			public string[] Authors;
		}

		private Language[] languages = {
			new Language
			{
				Code = "en",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "ar",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "cn",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "hr",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "fr",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "de",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "hu",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "ru",
				Authors = new string[] { "AhmedNasserHK" }
			},
			new Language
			{
				Code = "es",
				Authors = new string[] { "AhmedNasserHK" }
			}
		};

		private bool settingsStarted = false;
		private MainForm owner;

		public SettingsForm()
		{
			InitializeComponent();

			int theme = hk4tech_picture_viewer.Properties.Settings.Default.Theme;
			if (theme == 0) systemThemeRadio.Checked = true;
			else if (theme == 1) lightThemeRadio.Checked = true;
			else darkThemeRadio.Checked = true;

			fullscrCursorCheckBox.Checked = hk4tech_picture_viewer.Properties.Settings.Default.ShowCursorInFullscreen;
			escToExitCheckBox.Checked = hk4tech_picture_viewer.Properties.Settings.Default.EscToExit;

			int mouseWheelAction = hk4tech_picture_viewer.Properties.Settings.Default.MouseWheelScrollAction;
			if (mouseWheelAction == 0) mouseWheelActionRadio1.Checked = true;
			else if (mouseWheelAction == 1) mouseWheelActionRadio2.Checked = true;
			else if (mouseWheelAction == 2) mouseWheelActionRadio3.Checked = true;

			updatesCheckBox.Checked = hk4tech_picture_viewer.Properties.Settings.Default.CheckForUpdates;
			startupPasteCheckBox.Checked = hk4tech_picture_viewer.Properties.Settings.Default.StartupPaste;
			startupBoundsCheckBox.Checked = hk4tech_picture_viewer.Properties.Settings.Default.StartupRestoreBounds;

			favExtTextBox.Text = hk4tech_picture_viewer.Properties.Settings.Default.FavoriteExternalApp;

			openWithCheckBox.Checked = GetOpenWithState();
			browseWithCheckBox.Checked = GetBrowseFolderWithState();

			slideshowTimeNumeric.Value = hk4tech_picture_viewer.Properties.Settings.Default.SlideshowTime;
			slideshowCounterCheckBox.Checked = hk4tech_picture_viewer.Properties.Settings.Default.SlideshowCounter;

			for (int i = 0; i < languages.Length; i++)
			{
				if (hk4tech_picture_viewer.Properties.Settings.Default.Language == languages[i].Code)
				{
					langComboBox.SelectedIndex = i;
					break;
				}
			}

			themeRestart.LinkColor = ThemeMan.AccentColor;
			localizationRestart.LinkColor = ThemeMan.AccentColor;

			if (ThemeMan.isWindows10()) makeDefaultBtn.Enabled = true;
		}

		private void InitLanguage()
		{
			Text = LangMan.Get("settings");
			langPage.Text = LangMan.Get("localization");
			startupPage.Text = LangMan.Get("startup");
			restartLabel1.Text = "* " + LangMan.Get("restart-required");
			restartLabel2.Text = "* " + LangMan.Get("restart-required");
			systemThemeRadio.Text = LangMan.Get("use-system-setting");
			lightThemeRadio.Text = LangMan.Get("light");
			darkThemeRadio.Text = LangMan.Get("dark");
			themePage.Text = LangMan.Get("theme");
			startupLabel.Text = LangMan.Get("app-startup-actions") + ":";
			startupPasteCheckBox.Text = LangMan.Get("paste-from-clipboard");
			startupBoundsCheckBox.Text = LangMan.Get("restore-last-window-bounds");
			updatesCheckBox.Text = LangMan.Get("check-for-app-updates");
			favExtLabel.Text = LangMan.Get("fav-external-app") + ":";
			browseBtn.Text = " " + LangMan.Get("browse");
			slideshowPage.Text = LangMan.Get("slideshow");
			slideshowTimeLabel.Text = LangMan.Get("switching-time") + ":";
			mousePage.Text = LangMan.Get("input");
			slideshowSecondsLabel.Text = LangMan.Get("seconds");
			slideshowCounterCheckBox.Text = LangMan.Get("show-slideshow-counter");
			fullscrCursorCheckBox.Text = LangMan.Get("fullscreen-cursor");
			langLabel.Text = LangMan.Get("ui-lang") + ":";
			translatedByLabel.Text = LangMan.Get("translated-by") + ": ";
			escToExitCheckBox.Text = string.Format(LangMan.Get("esc-to-exit"), "Esc");
			mouseWheelActionLabel.Text = LangMan.Get("mouse-wheel-action") + ":";
			mouseWheelActionRadio1.Text = LangMan.Get("scroll-up-down");
			mouseWheelActionRadio2.Text = LangMan.Get("zoom-in-out");
			mouseWheelActionRadio3.Text = LangMan.Get("next-prev-image");
			themeRestart.Text = LangMan.Get("restart");
			localizationRestart.Text = LangMan.Get("restart");
			contextMenuLabel.Text = LangMan.Get("context-menu") + ":";
			makeDefaultBtn.Text = LangMan.Get("set-as-default-image-viewer");
			openWithCheckBox.Text = LangMan.Get("open-with-qpv");
			browseWithCheckBox.Text = LangMan.Get("browse-folder-with-qpv");
			helpTranslateBtn.Text = LangMan.Get("help-us-translate-app");
			infoTooltip.SetToolTip(closeBtn, NativeMan.GetMessageBoxText(NativeMan.DialogBoxCommandID.IDCLOSE) + " | Alt+F4");
		}

		private void SetDarkMode(bool dark)
		{
			if (dark)
			{
				browseBtn.Image = hk4tech_picture_viewer.Properties.Resources.white_open;
				browseBtn.BackColor = ThemeMan.DarkSecondColor;
				browseBtn.ForeColor = Color.White;

				makeDefaultBtn.BackColor = ThemeMan.DarkSecondColor;
				makeDefaultBtn.ForeColor = Color.White;

				helpTranslateBtn.BackColor = ThemeMan.DarkSecondColor;
				helpTranslateBtn.ForeColor = Color.White;
			}

			DarkMode = dark;
			settingsTabs.DarkMode = dark;
			updatesCheckBox.DarkMode = dark;
			fullscrCursorCheckBox.DarkMode = dark;
			darkThemeRadio.DarkMode = dark;
			lightThemeRadio.DarkMode = dark;
			systemThemeRadio.DarkMode = dark;
			closeBtn.DarkMode = dark;
			slideshowTimeNumeric.DarkMode = dark;
			slideshowCounterCheckBox.DarkMode = dark;
			langComboBox.DarkMode = dark;
			favExtTextBox.DarkMode = dark;
			escToExitCheckBox.DarkMode = dark;
			mouseWheelActionRadio1.DarkMode = dark;
			mouseWheelActionRadio2.DarkMode = dark;
			mouseWheelActionRadio3.DarkMode = dark;
			startupPasteCheckBox.DarkMode = dark;
			startupBoundsCheckBox.DarkMode = dark;
			openWithCheckBox.DarkMode = dark;
			browseWithCheckBox.DarkMode = dark;
		}

		private void updatesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			//if (settingsStarted)
		//	{
		//		hk4tech_picture_viewer.Properties.Settings.Default.CheckForUpdates = updatesCheckBox.Checked;
		//		hk4tech_picture_viewer.Properties.Settings.Default.Save();
		//	}
		}

		private void systemThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted && systemThemeRadio.Checked)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.Theme = 0;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void lightThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted && lightThemeRadio.Checked)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.Theme = 1;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void darkThemeRadio_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted && darkThemeRadio.Checked)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.Theme = 2;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void fullscrCursorCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.ShowCursorInFullscreen = fullscrCursorCheckBox.Checked;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void closeBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void favExtTextBox_TextChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.FavoriteExternalApp = favExtTextBox.Text;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void browseBtn_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				favExtTextBox.Text = openFileDialog1.FileName;
			}
			openFileDialog1.Dispose();
		}

		private void slideshowCounterCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.SlideshowCounter = slideshowCounterCheckBox.Checked;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void slideshowTimeNumeric_ValueChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.SlideshowTime = (int)slideshowTimeNumeric.Value;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void langComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.Language = languages[langComboBox.SelectedIndex].Code;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}

			if (owner != null)
			{
				translateAuthorsPanel.Controls.Clear();
				translateAuthorsPanel.Left = translatedByLabel.Location.X + translatedByLabel.Width;

				int curX = 0;
				for (int i = 0; i < languages[langComboBox.SelectedIndex].Authors.Length; i++)
				{
					LinkLabel ll = new LinkLabel();
					ll.LinkBehavior = LinkBehavior.HoverUnderline;
					ll.AutoSize = true;
					ll.Text = languages[langComboBox.SelectedIndex].Authors[i];
					ll.LinkColor = ThemeMan.AccentColor;
					ll.ActiveLinkColor = ThemeMan.BorderColor;
					ll.Margin = new Padding(0, 0, 0, 0);
					ll.Click += Ll_Click;
					translateAuthorsPanel.Controls.Add(ll);
					ll.Left = curX;
					curX += ll.Width;
				}
			}
		}

		private void Ll_Click(object sender, EventArgs e)
		{
			Process.Start("https://github.com/" + (sender as LinkLabel).Text);
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			owner = Owner as MainForm;
			SetDarkMode(DarkMode);
			InitLanguage();
			langComboBox_SelectedIndexChanged(null, null);
			settingsStarted = true;
		}

		private void escToExitCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.EscToExit = escToExitCheckBox.Checked;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void mouseWheelActionRadio1_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.MouseWheelScrollAction = 0;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void mouseWheelActionRadio2_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.MouseWheelScrollAction = 1;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void mouseWheelActionRadio3_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.MouseWheelScrollAction = 2;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void restartLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			owner.RestartApp();
		}

		private void startupPasteCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.StartupPaste = startupPasteCheckBox.Checked;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void startupBoundsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				hk4tech_picture_viewer.Properties.Settings.Default.StartupRestoreBounds = startupBoundsCheckBox.Checked;
				hk4tech_picture_viewer.Properties.Settings.Default.Save();
			}
		}

		private void openWithCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				try
				{
					if (openWithCheckBox.Checked)
					{
						Registry.SetValue("HKEY_CLASSES_ROOT\\*\\shell\\QuickPictureViewer", "", "Open with HK4TECH Picture Viewer");
						Registry.SetValue("HKEY_CLASSES_ROOT\\*\\shell\\QuickPictureViewer", "Icon", string.Format("\"{0}picture.ico\"", AppDomain.CurrentDomain.BaseDirectory));
						Registry.SetValue("HKEY_CLASSES_ROOT\\*\\shell\\QuickPictureViewer\\command", "", string.Format("\"{0}hk4tech-picture-viewer.exe\" \"%V\"", AppDomain.CurrentDomain.BaseDirectory));
					}
					else
					{
						RegistryKey RegKey = Registry.ClassesRoot.OpenSubKey("*\\shell\\QuickPictureViewer", true);
						RegKey.DeleteSubKeyTree("");
					}
				}
				catch
				{
					settingsStarted = false;
					DialogMan.ShowInfo(
						this,
						LangMan.Get("context-menu-notice"),
						LangMan.Get("error"),
						DarkMode,
						TopMost
					);
					openWithCheckBox.Checked = !openWithCheckBox.Checked;
					settingsStarted = true;
				}
			}
		}

		private void browseWithCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (settingsStarted)
			{
				try
				{
					if (browseWithCheckBox.Checked)
					{
						Registry.SetValue("HKEY_CLASSES_ROOT\\Directory\\Background\\shell\\QuickPictureViewer", "", "Browse folder with HK4TECH Picture Viewer");
						Registry.SetValue("HKEY_CLASSES_ROOT\\Directory\\Background\\shell\\QuickPictureViewer", "Icon", string.Format("\"{0}picture.ico\"", AppDomain.CurrentDomain.BaseDirectory));
						Registry.SetValue("HKEY_CLASSES_ROOT\\Directory\\Background\\shell\\QuickPictureViewer\\command", "", string.Format("\"{0}hk4tech-picture-viewer.exe\" \"%V\"", AppDomain.CurrentDomain.BaseDirectory));

						Registry.SetValue("HKEY_CLASSES_ROOT\\Directory\\shell\\QuickPictureViewer", "", "Browse folder with HK4TECH Picture Viewer");
						Registry.SetValue("HKEY_CLASSES_ROOT\\Directory\\shell\\QuickPictureViewer", "Icon", string.Format("\"{0}picture.ico\"", AppDomain.CurrentDomain.BaseDirectory));
						Registry.SetValue("HKEY_CLASSES_ROOT\\Directory\\shell\\QuickPictureViewer\\command", "", string.Format("\"{0}hk4tech-picture-viewer.exe\" \"%V\"", AppDomain.CurrentDomain.BaseDirectory));
					}
					else
					{
						RegistryKey RegKey = Registry.ClassesRoot.OpenSubKey("Directory\\Background\\shell\\QuickPictureViewer", true);
						RegKey.DeleteSubKeyTree("");
						RegistryKey RegKey2 = Registry.ClassesRoot.OpenSubKey("Directory\\shell\\QuickPictureViewer", true);
						RegKey2.DeleteSubKeyTree("");
					}
				}
				catch
				{
					settingsStarted = false;
					DialogMan.ShowInfo(
						this,
						LangMan.Get("context-menu-notice"),
						LangMan.Get("error"),
						DarkMode,
						TopMost
					);
					browseWithCheckBox.Checked = !browseWithCheckBox.Checked;
					settingsStarted = true;
				}
			}
		}

		private bool GetOpenWithState()
		{
			try
			{
				string openWithValue = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\*\\shell\\QuickPictureViewer", string.Empty, string.Empty);
				return openWithValue.Length > 0;
			}
			catch
			{
				return false;
			}
		}

		private bool GetBrowseFolderWithState()
		{
			try
			{
				const string browseWithKey1 = "HKEY_CLASSES_ROOT\\Directory\\Background\\shell\\QuickPictureViewer";
				const string browseWithKey2 = "HKEY_CLASSES_ROOT\\Directory\\shell\\QuickPictureViewer";
				string browseWithValue1 = (string)Registry.GetValue(browseWithKey1, string.Empty, string.Empty);
				string browseWithValue2 = (string)Registry.GetValue(browseWithKey2, string.Empty, string.Empty);
				if (browseWithValue1.Length > 0 && browseWithValue2.Length > 0) return true;
				return false;
			}
			catch
			{
				return false;
			}
		}

		private void makeDefaultBtn_Click(object sender, EventArgs e)
		{
			Process.Start("ms-settings:defaultapps");
		}

		private void helpTranslateBtn_Click(object sender, EventArgs e)
		{
			Process.Start("https://hk4tech.com");
		}
	}
}
