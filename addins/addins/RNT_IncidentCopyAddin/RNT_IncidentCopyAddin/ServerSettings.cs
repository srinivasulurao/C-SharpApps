using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace IncidentCopyAddin
{
    /// <summary>
    /// Singleton for accessing server set settings
    /// </summary>
    public class ServerSettings
    {
        #region singleton
        static ServerSettings instance = null;
        static readonly object padlock = new object();

        ServerSettings()
        {
        }

        public static ServerSettings Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ServerSettings();
                    }
                    return instance;
                }
            }
        }
        #endregion

        /// <summary>
        /// Custom Field ID for incidents.c$vin
        /// </summary>
        public int VIN_cfid
        {
            get
            {
                return _vin_cfid;
            }
            set
            {
                _vin_cfid = value;
            }
        }
        private int _vin_cfid = 9;

        /// <summary>
        /// The label for the button to display
        /// </summary>
        public string ButtonLabel
        {
            get
            {
                return _buttonLabel;
            }
            set
            {
                _buttonLabel = value;
            }
        }
        private string _buttonLabel = "Copy to Clipboard";


        /// <Summary>
        /// Settings for Storing the timeZone.
        /// </Summary>

        public string SetupTimezone
        {
            get
            {
                return _setupTimezone;
            }
            set
            {
                _setupTimezone = value;
            }
        }

        private string _setupTimezone = "Central Standard Time"; //This is basically the default value for the set up. 

    } //Class ends here.

} //Namespace ends here
