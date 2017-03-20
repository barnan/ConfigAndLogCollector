using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ListNetworkComputers
{
    /// <summary>
    /// A simply test form that creates a new NetworkBrowser
    /// object, and displays a list of the network computers
    /// found by the NetworkBrowser
    /// </summary>
    public partial class frmMain : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
        }


        private void frmMain_Load(object sender, EventArgs e)
        {

            //create a new NetworkBrowser object, and get the
            //list of network computers it found, and add each
            //entry to the combo box on this form
            try
            {
                NetworkBrowser nb = new NetworkBrowser();
                foreach (string pc in nb.getNetworkComputers())
                {
                    cmbNetworkComputers.Items.Add(pc);
                }
            }
            catch (Exception ex) {
                MessageBox.Show("An error occurred trying to access the network computers", "error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }
    }
}