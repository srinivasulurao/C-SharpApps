using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RightNow.AddIns.AddInViews;
using System.Windows.Forms;

namespace IncidentCopyAddin
{
    public class AddinControlLauncher : IWorkspaceComponent2
    {
        private bool designMode;
        private IGlobalContext globalContext;
        private IRecordContext recordContext;
        private Control userControl;

        /// <summary>
        /// Creates a new add-in control
        /// </summary>
        /// <param name="designMode">if true we're in a workspace designer</param>
        /// <param name="globalContext">information about the session</param>
        /// <param name="recordContext">information about the workspace</param>
        public AddinControlLauncher(bool designMode, IGlobalContext globalContext, IRecordContext recordContext)
        {
            userControl = new CopyDetailsButton(designMode, globalContext, recordContext);
         
            this.designMode = designMode;
            this.globalContext = globalContext;
            this.recordContext = recordContext;
        }

        #region IWorkspaceComponent2 Members

        public bool ReadOnly { get; set; }

        /// <summary>
        /// unused
        /// </summary>
        /// <param name="actionName"></param>
        public void RuleActionInvoked(string actionName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// unused
        /// </summary>
        /// <param name="conditionName"></param>
        /// <returns></returns>
        public string RuleConditionInvoked(string conditionName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAddInControl Members
        /// <summary>
        /// returns the UI control
        /// </summary>
        /// <returns>UI for the button</returns>
        public Control GetControl()
        {
            return userControl;
        }

        #endregion
    }
}
