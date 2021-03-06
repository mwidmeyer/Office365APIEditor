﻿// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information. 

using Microsoft.Identity.Client;
using System;
using System.Windows.Forms;

namespace Office365APIEditor
{
    public partial class AcquireViewerTokenForm : Form
    {
        PublicClientApplication _pca;
        AuthenticationResult _ar;

        public AcquireViewerTokenForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(out PublicClientApplication pca, out AuthenticationResult ar)
        {
            DialogResult result = ShowDialog();

            pca = _pca;
            ar = _ar;

            return result;
        }

        private async void button_AcquireAccessToken_Click(object sender, EventArgs e)
        {
            if (textBox_ClientID.Text == "")
            {
                MessageBox.Show("Enter the Client ID.", "Office365APIEditor");
                return;
            }

            Cursor = Cursors.WaitCursor;

            string[] scopes = new string[] { "offline_access https://outlook.office.com/mail.read https://outlook.office.com/contacts.read https://outlook.office.com/calendars.read" };

            _pca = new PublicClientApplication(textBox_ClientID.Text);
            

            try
            {
                _ar = await _pca.AcquireTokenAsync(scopes, "", UiOptions.ForceLogin, "");

                Properties.Settings.Default.Save();
                DialogResult = DialogResult.OK;
                Cursor = Cursors.Default;
                Close();
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Office365APIEditor");
            }
        }

        private void AcquireViewerTokenForm_Load(object sender, EventArgs e)
        {
            linkLabel_Portal.Text = "Enter the Client ID of your application which registered in Application Registration Portal as a mobile application.";
            int startIndex = linkLabel_Portal.Text.IndexOf("Application Registration Portal", 0, linkLabel_Portal.Text.Length);
            linkLabel_Portal.Links.Add(startIndex, ("Application Registration Portal").Length, "https://apps.dev.microsoft.com/");
        }

        private void linkLabel_Portal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
