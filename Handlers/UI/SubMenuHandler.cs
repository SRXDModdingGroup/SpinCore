using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinCore.Handlers.UI
{
    public class SubMenuHandler
    {
        public static void CreateAcceptDenySubMenu(string question, Action action) {
            ModalMessageDialog.Instance.AddMessage(question, null, new ModalMessageDialog.NullCallback(action), Strings.Accept, delegate ()
            {
            }, Strings.Cancel, null, null, -1f);
        }

        public static void CreateAcceptDenySubMenu(string question, Action actionAccept, Action actionDeny) {
            ModalMessageDialog.Instance.AddMessage(question, null, new ModalMessageDialog.NullCallback(actionAccept), Strings.Accept, new ModalMessageDialog.NullCallback(actionDeny), Strings.Cancel, null, null, -1f);
        }

        public static void CreateInfoSubMenu(string info, float timeLasting) {
            ModalMessageDialog.Instance.AddMessage(info, null, null, null, delegate ()
            {
            }, null, null, null, timeLasting);
        }

        public static void CreateInfoSubMenu(string info) {
            ModalMessageDialog.Instance.AddMessage(info, null, null, null, delegate ()
            {
            }, Strings.Okay, null, null, -1f);
        }        
        
        public static void CreateInfoSubMenu(string info, Action action) {
            ModalMessageDialog.Instance.AddMessage(info, null, new ModalMessageDialog.NullCallback(action), null, delegate ()
            {
            }, Strings.Okay, null, null, -1f);
        }

    }
}
