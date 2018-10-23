using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RightNow.AddIns.AddInViews;
using System.Text.RegularExpressions;

namespace IncidentCopyAddin
{
    public partial class CopyDetailsButton : UserControl
    {
        private CopyDetailsLogic logic;
        private IGlobalContext globalContext;
        private IRecordContext recordContext;

        /// <summary>
        /// Default constructor. Mainly for VS Designer
        /// </summary>
        public CopyDetailsButton()
        {
            InitializeComponent();
            button_copy.Text = ServerSettings.Instance.ButtonLabel;
        }

        /// <summary>
        /// Functional Designer. Use this one.
        /// </summary>
        /// <param name="designMode">if true we're on a workspace designer</param>
        /// <param name="globalContext">info about the session</param>
        /// <param name="recordContext">info about the workspace/record</param>
        public CopyDetailsButton(bool designMode, IGlobalContext globalContext, IRecordContext recordContext)
        {
            try
            {
                if (!designMode)
                    recordContext.Saved += new EventHandler(recordContext_Saved);

                //create a logic controller
                logic = new CopyDetailsLogic(designMode, globalContext, recordContext);
                this.globalContext = globalContext;
                this.recordContext = recordContext;

                //set up the UI
                InitializeComponent();

                //load the button text from the server settings
                button_copy.Text = ServerSettings.Instance.ButtonLabel;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                globalContext.LogMessage("Incident Copy Addin - Exception: " + ex.ToString());
                this.Enabled = false;
            }
        }

        void recordContext_Saved(object sender, EventArgs e)
        {
            logic = new CopyDetailsLogic(false, globalContext, recordContext);
        }

        /// <summary>
        /// On Click event handler. Copy details to clipboard and the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_copy_Click(object sender, EventArgs e)
        {
            try
            {
                //reload the workspace info to get any changes since opening
                logic.LoadWorkspaceRecords();

                //create the string
                string details = logic.CreateCopyString();

                //Remove all the html tags from the details text, as a part of CS ticket 180817-000105
                details = Regex.Replace(details,@"<[^>]+>|&nbsp;", String.Empty);       

                //set the clipboard
                Clipboard.SetText(details);

                //set the textbox text
                textBox_details.Text = "The following has been copied to your clipboard:\r\n" + details;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                globalContext.LogMessage("Incident Copy Addin - Exception: " + ex.ToString());
                this.Enabled = false;
            }
        }
    }
}
