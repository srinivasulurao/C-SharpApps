using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RightNow.AddIns.AddInViews;
using RightNow.AddIns.Common;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.ObjectModel;

namespace IncidentCopyAddin
{
    class CopyDetailsLogic
    {
        private bool designMode;
        private IGlobalContext globalContext;
        private IRecordContext recordContext;

        /// <summary>
        /// Contact info
        /// </summary>
        private IContact contactWorkspaceRecord { get; set; }

        /// <summary>
        /// incident info
        /// </summary>
        private IIncident incidentWorkspaceRecord { get; set; }

        /// <summary>
        /// Constructor. Loads up the workspace records and inits all values
        /// </summary>
        /// <param name="designMode">if true we're on a workspace designer</param>
        /// <param name="globalContext">info about the session</param>
        /// <param name="recordContext">info about the workspace/records</param>
        public CopyDetailsLogic(bool designMode, IGlobalContext globalContext, IRecordContext recordContext)
        {
            this.designMode = designMode;
            this.globalContext = globalContext;
            this.recordContext = recordContext;

            //for whatever reason workspace type is not known when we're on the 
            //workspace designer so this error message must be shown during execution
            if (!designMode)
            {
                if (recordContext.WorkspaceType != WorkspaceRecordType.Incident)
                {
                    throw new Exception("Incorrect workspace type. Please deploy this add-in to an incident workspace.");
                }
            }
        }

        /// <summary>
        /// Reload the workspace and load the incident and contact records
        /// </summary>
        public void LoadWorkspaceRecords()
        {
            try
            {
                recordContext.RefreshWorkspace();
                incidentWorkspaceRecord = (IIncident)recordContext.GetWorkspaceRecord(WorkspaceRecordType.Incident);
                contactWorkspaceRecord = (IContact)recordContext.GetWorkspaceRecord(WorkspaceRecordType.Contact);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load workspace records", ex);
            }
        }

        /// <summary>
        /// Creates a string of information stripped of whitespace about the incident and contact
        /// </summary>
        /// <returns>the created string</returns>
        public string CreateCopyString()
        { 
            /*
               Field	        DB Column	            Notes
               1 First Name	    contacts.first_name	
               2 Last Name	    contacts.last_name	
               3 VIN            incidents.c$vin         cf_id = 9
               4 Reference # 	incidents.ref_no	
               5 Date Created	incidents.created 	    MM/DD/YYYY HH:MM AM/PM
               6 Email Addr	    contacts.email		
               7 Customer Thread	threads.note 	    The newest customer created thread.
		    */

            try
            {
                List<string> output = new List<string>();
                output.Add("*Clarify");

                //-----------------CONTACT FIELDS--------------------
                if (contactWorkspaceRecord != null)
                {
                    /*************************************************************/
                    //      IF you add or remove things from this list make 
                    //      sure you update the for loop below with correct 
                    //      number of items
                    /*************************************************************/
                    output.Add(contactWorkspaceRecord.NameFirst ?? "");
                    output.Add(contactWorkspaceRecord.NameLast ?? "");
                }
                else
                {
                    //just add empty strings if there's nothing else to add
                    for (int i = 0; i < 2; i++)
                    {
                        output.Add("");
                    }
                }

                //-----------------INCIDENT FIELDS--------------------
                if (incidentWorkspaceRecord != null)
                {
                    /*************************************************************/
                    //      IF you add or remove things from this list make 
                    //      sure you update the for loop below with correct 
                    //      number of items
                    /*************************************************************/
                    output.Add(GetIncidentCustomField(ServerSettings.Instance.VIN_cfid));

                    output.Add(incidentWorkspaceRecord.RefNo ?? "");
					
                    //format: MM/DD/YYYY HH:MM AM/PM
                    if (incidentWorkspaceRecord.Created != null)
                    {
                        DateTime? createdDate = GetLatestCustomerThreadDate();
                        if (createdDate == null)
                        {
                            output.Add("");
                        }
                        else
                        {
                            DateTime validCreatedDate = (DateTime)createdDate;

                            //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"); //CS Ticket 180817-000105 requirement.
                            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(ServerSettings.Instance.SetupTimezone); 

                            int offset = tzi.BaseUtcOffset.Hours;

                            if (tzi.IsDaylightSavingTime(validCreatedDate))
                            {
                                offset += 1;
                            }

                            validCreatedDate = validCreatedDate.AddHours(offset); //convert to PST

                            string formattedDate = validCreatedDate.ToString("MM/dd/yyyy hh:mm tt");
                            output.Add(formattedDate);
                        }
                    }
                    else
                    {
                        output.Add("");
                    }
                }
                else
                {
                    //just add empty strings if there's nothing else to add
                    for (int i = 0; i < 4; i++)
                    {
                        output.Add("");
                    }
                }

                //-----------------CONTACT Email--------------------
                if (contactWorkspaceRecord != null)
                {
                    output.Add(contactWorkspaceRecord.EmailAddr ?? "");
                }
                else
                {
                    output.Add("");
				}

				output.Add(incidentWorkspaceRecord.Subject);

                //-----------------INCIDENT Thread--------------------
                if (incidentWorkspaceRecord != null)
                {
                    string thread = GetLatestCustomerThreadEntry();

                    //convert linux newline to windows newline
                    thread = thread.Replace("\n", "\r\n");

                    output.Add(thread);
                }
                else
                {
                    output.Add("");
                }

                //------------Clean up the data-----------------
                for (int i = 0; i < output.Count; i++)
                {
                    output[i] = output[i].Replace("|", "[pipe]");

                    //removed by customer after 2nd round of UAT
                    //output[i] = output[i].Replace("\r", " ");
                    //output[i] = output[i].Replace("\n", " ");

                    output[i] = output[i].Trim();
                }

                //create a pipe delimitated string from the array
                string results = String.Join("|", output.ToArray());
                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create copy string", ex);
            }
        }

        /// <summary>
        /// Gets the value for a custom field
        /// </summary>
        /// <param name="cf_id">custom field ID to look for</param>
        /// <returns>string value of the custom field</returns>
        private string GetIncidentCustomField(int cf_id)
        {
            if (incidentWorkspaceRecord != null &&
                incidentWorkspaceRecord.CustomField != null &&
                incidentWorkspaceRecord.CustomField.Count > 0)
            {
                var customFieldLookup = from cf in incidentWorkspaceRecord.CustomField
                                        where cf.CfId == cf_id
                                        select cf;

                if (customFieldLookup.Count() > 0)
                {
                    ICfVal customField = customFieldLookup.FirstOrDefault();

                    if (customField.ValDate != null)
                        return customField.ValDate.ToString();
                    else if (customField.ValDttm != null)
                        return customField.ValDttm.ToString();
                    else if (customField.ValInt != null)
                        return customField.ValInt.ToString();
                    else if (customField.ValStr != null)
                        return customField.ValStr;
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets the newest customer-related thread and returns it. This includes customer entries and custom proxy entries
        /// </summary>
        /// <returns>the newest thread from the incident</returns>
        private string GetLatestCustomerThreadEntry()
        {
            try
            {
                //pull the newest customer created thread
                if (incidentWorkspaceRecord != null &&
                    incidentWorkspaceRecord.Threads != null &&
                    incidentWorkspaceRecord.Threads.Count > 0)
                {
                    var threadLookup = from t in incidentWorkspaceRecord.Threads
                                       where t.EntryType == 2 || //agent response
                                             t.EntryType == 3 || //customer entry 
                                             t.EntryType == 4    //customer proxy
                                       orderby t.Entered descending
                                       select t.Note;

                    if (threadLookup.Count() > 0)
                    {
                        return threadLookup.FirstOrDefault();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                globalContext.LogMessage("IncidentCopy - Failed to get thread: " + ex);
                return "";
            }
        }

        private DateTime? GetLatestCustomerThreadDate()
        {
            try
            {
                //pull the newest customer created thread
                if (incidentWorkspaceRecord != null &&
                    incidentWorkspaceRecord.Threads != null &&
                    incidentWorkspaceRecord.Threads.Count > 0)
                {
                    var threadLookup = from t in incidentWorkspaceRecord.Threads
                                       where t.EntryType == 2 || //agent response
                                             t.EntryType == 3 || //customer entry 
                                             t.EntryType == 4    //customer proxy
                                       orderby t.Entered descending
                                       select t.Entered;

                    if (threadLookup.Count() > 0)
                    {
                        return threadLookup.FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                globalContext.LogMessage("IncidentCopy - Failed to get thread: " + ex);
                return null;
            }
        }

    }
}
