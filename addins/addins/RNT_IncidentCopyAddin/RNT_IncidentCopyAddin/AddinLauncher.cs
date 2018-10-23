using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RightNow.AddIns.AddInViews;
using System.Drawing;
using System.AddIn;
using IncidentCopyAddin.Properties;

namespace IncidentCopyAddin
{
    [AddIn("Workspace Component Controller AddIn", Version = "1.0.0.0")]
    public class AddinLauncher : IWorkspaceComponentFactory2
    {
        #region server side settings
        /// <summary>
        /// The label for the button to display
        /// </summary>
        [ServerConfigProperty(DefaultValue = "Copy to Clipboard")]
        public string ButtonLabel
        {
            get
            {
                return ServerSettings.Instance.ButtonLabel;
            }
            set
            {
                ServerSettings.Instance.ButtonLabel = value;
            }
        }

        /// <summary>
        /// Custom Field ID for Incidents.c$vin
        /// </summary>
        [ServerConfigProperty(DefaultValue = "9")]
        public int VIN_cfid
        {
            get
            {
                return ServerSettings.Instance.VIN_cfid;
            }
            set
            {
                ServerSettings.Instance.VIN_cfid = value;
            }
        }

        [ServerConfigProperty(DefaultValue = "Central Standard Time")]
        public string SetupTimezone
        {
            get
            {
                return ServerSettings.Instance.SetupTimezone;
            }
            set
            {
                ServerSettings.Instance.SetupTimezone = value; 
            }
        }
          
        #endregion

        #region IWorkspaceComponentFactory2 Members
        private IGlobalContext globalContext;
        private AddinControlLauncher workspaceControl;

        public AddinLauncher()
        {
            
        }

        /// <summary>
        /// Create the workspace control
        /// </summary>
        /// <param name="inDesignMode">if true we're in a workspace designer</param>
        /// <param name="recordContext">information about the workspace</param>
        /// <returns>workspace component</returns>
        IWorkspaceComponent2 IWorkspaceComponentFactory2.CreateControl(bool inDesignMode, IRecordContext recordContext)
        {
            workspaceControl = new AddinControlLauncher(inDesignMode, globalContext, recordContext);
            
            return workspaceControl;
        }

        #endregion

        #region IFactoryBase Members

        /// <summary>
        /// icon to display for the add-in
        /// </summary>
        public Image Image16
        {
            get { return Resources.Copy16; }
        }

        /// <summary>
        /// Text to display for the add-in
        /// </summary>
        public string Text
        {
            get { return "Copy Incident Details Button"; }
        }

        /// <summary>
        /// tooltip to display for the add-in
        /// </summary>
        public string Tooltip
        {
            get { return "Displays a button for copying the current incident details to the clipboard"; }
        }

        #endregion

        #region IAddInBase Members

        /// <summary>
        /// set the session info
        /// </summary>
        /// <param name="context">information about the session</param>
        /// <returns>always true</returns>
        public bool Initialize(IGlobalContext context)
        {
            globalContext = context;
            return true;
        }

        #endregion
    }
}
