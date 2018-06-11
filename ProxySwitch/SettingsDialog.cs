﻿using ProxySwitch.Enums;
using ProxySwitch.EventArguments;
using ProxySwitch.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProxySwitch
{
    /// <summary>
    /// Dialog for changing the settings of the application.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class SettingsDialog : Form
    {
        #region Private fields

        private string customIconOffPath = string.Empty;
        private string customIconOnPath = string.Empty;

        private Icon customIconOff;
        private Icon customIconOn;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsDialog"/> class.
        /// </summary>
        public SettingsDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Handles the Load event of the SettingsDialog.
        /// Loads the settings.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SettingsDialog_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void RadioButton_theme_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_customTheme.Checked)
            {
                pictureBox_customIconOff.Enabled = true;
                pictureBox_customIconOn.Enabled = true;
                textBox_pathIconOff.Enabled = true;
                textBox_pathIconOn.Enabled = true;
                button_openIconOff.Enabled = true;
                button_openIconOn.Enabled = true;
            }
            else
            {
                pictureBox_customIconOff.Enabled = false;
                pictureBox_customIconOn.Enabled = false;
                textBox_pathIconOff.Enabled = false;
                textBox_pathIconOn.Enabled = false;
                button_openIconOff.Enabled = false;
                button_openIconOn.Enabled = false;
            }
        }

        private void Button_openIconOn_Click(object sender, EventArgs e)
        {
            OpenIcon(true);
        }

        private void Button_openIconOff_Click(object sender, EventArgs e)
        {
            OpenIcon(false);
        }

        private void CheckBox_reverseIcons_CheckedChanged(object sender, EventArgs e)
        {
            bool reversed = checkBox_reverseIcons.Checked;

            pictureBox_defaultIconOn.Image = reversed ? Resources.networking_32 : Resources.networking_green_32;
            pictureBox_defaultIconOff.Image = reversed ? Resources.networking_green_32 : Resources.networking_32;
            pictureBox_alarmIconOn.Image = reversed ? Resources.beacon_light_32 : Resources.beacon_light_bw_32;
            pictureBox_alarmIconOff.Image = reversed ? Resources.beacon_light_bw_32 : Resources.beacon_light_32;
            pictureBox_trafficLightIconOn.Image = reversed ? Resources.traffic_lights_red_32 : Resources.traffic_lights_green_32;
            pictureBox_trafficLightIconOff.Image = reversed ? Resources.traffic_lights_green_32 : Resources.traffic_lights_red_32;

            RefreshCustomIcons();
        }

        private void RadioButton_ProxySettings_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_overrideProxySettings.Checked)
            {
                textBox_proxyAddress.Enabled = true;
                textBox_proxyPort.Enabled = true;
                checkBox_bypassProxyLocal.Enabled = true;
            }
            else
            {
                textBox_proxyAddress.Enabled = false;
                textBox_proxyPort.Enabled = false;
                checkBox_bypassProxyLocal.Enabled = false;
            }
        }

        private void TextBox_proxyPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Is Digit?
            e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }

        private void TextBox_proxyPort_TextPasted(object sender, ClipboardEventArgs e)
        {
            if (!int.TryParse(e.ClipboardText, out int result))
            {
                // Pasted text is not a integer number
                e.Handled = true;
            }
        }

        private void Button_ok_Click(object sender, EventArgs e)
        {

        }

        private void Button_cancel_Click(object sender, EventArgs e)
        {

        }

        private void Button_apply_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            checkBox_autostart.Checked = Settings.Instance.StartWithWindows;
            checkBox_autoDisable.Checked = Settings.Instance.DisableProxyOnStart;

            switch (Settings.Instance.Theme)
            {
                case Themes.Default:
                    radioButton_defaultTheme.Checked = true;
                    radioButton_alarmTheme.Checked = false;
                    radioButton_trafficLightTheme.Checked = false;
                    radioButton_customTheme.Checked = false;
                    break;
                case Themes.Alarm:
                    radioButton_defaultTheme.Checked = false;
                    radioButton_alarmTheme.Checked = true;
                    radioButton_trafficLightTheme.Checked = false;
                    radioButton_customTheme.Checked = false;
                    break;
                case Themes.TrafficLight:
                    radioButton_defaultTheme.Checked = false;
                    radioButton_alarmTheme.Checked = false;
                    radioButton_trafficLightTheme.Checked = true;
                    radioButton_customTheme.Checked = false;
                    break;
                case Themes.Custom:
                    radioButton_defaultTheme.Checked = false;
                    radioButton_alarmTheme.Checked = false;
                    radioButton_trafficLightTheme.Checked = false;
                    radioButton_customTheme.Checked = true;
                    break;
            }

            customIconOffPath = Settings.Instance.CustomIconProxyOff;
            customIconOnPath = Settings.Instance.CustomIconProxyOn;

            RefreshCustomIcons();

            checkBox_reverseIcons.Checked = Settings.Instance.ReverseIcons;

            if (Settings.Instance.OverrideProxySettings)
            {
                radioButton_keepProxySettings.Checked = false;
                radioButton_overrideProxySettings.Checked = true;
            }
            else
            {
                radioButton_keepProxySettings.Checked = true;
                radioButton_overrideProxySettings.Checked = false;
            }

            textBox_proxyAddress.Text = Settings.Instance.ProxyServerAddress;
            textBox_proxyPort.Text = Settings.Instance.ProxyServerPort.HasValue ? Settings.Instance.ProxyServerPort.Value.ToString() : string.Empty;
            checkBox_bypassProxyLocal.Checked = Settings.Instance.BypassProxyServer;
        }

        private bool ApplySettings()
        {
            bool result = false;

            return result;
        }

        private void OpenIcon(bool iconOn)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                CheckFileExists = true,
                Filter = "",
                Multiselect = false
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (iconOn)
                    customIconOnPath = dlg.FileName;
                else
                    customIconOffPath = dlg.FileName;

                RefreshCustomIcons();
            }
        }

        private void RefreshCustomIcons()
        {
            if (!string.IsNullOrWhiteSpace(customIconOffPath))
                customIconOff = new Icon(customIconOffPath);

            if (!string.IsNullOrWhiteSpace(customIconOnPath))
                customIconOn = new Icon(customIconOnPath);

            textBox_pathIconOff.Text = customIconOffPath?.Split('\\').Last();
            textBox_pathIconOn.Text = customIconOnPath?.Split('\\').Last();

            pictureBox_customIconOff.Image = checkBox_reverseIcons.Checked ? customIconOn?.ToBitmap() : customIconOff?.ToBitmap();
            pictureBox_customIconOn.Image = checkBox_reverseIcons.Checked ? customIconOff?.ToBitmap() : customIconOn?.ToBitmap();
        }

        #endregion
    }
}