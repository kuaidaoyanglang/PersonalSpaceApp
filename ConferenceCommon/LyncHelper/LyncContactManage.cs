using ConferenceCommon.LogHelper;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Group;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ConferenceCommon.LyncHelper
{
    public class LyncContactManage : INotifyPropertyChanged
    {
        ContactManager ContactManager;
        public Group OtherContactsGroup;
        public ObservableCollection<ContactInfo> OtherContacts { get; set; }

        public LyncContactManage(ContactManager contactManager)
        {
            try
            {
                OtherContacts = new ObservableCollection<ContactInfo>();
                ContactManager = contactManager;
                OtherContactsGroup = GetOtherContactsGroup();
                if (OtherContactsGroup != null)
                {
                    OtherContactsGroup.ContactAdded += new EventHandler<GroupMemberChangedEventArgs>(OtherContactsGroup_ContactAdded);
                    OtherContactsGroup.ContactRemoved += new EventHandler<GroupMemberChangedEventArgs>(OtherContactsGroup_ContactRemoved);
                    UpdateOtherContactsGroupContactInfo();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        void OtherContactsGroup_ContactRemoved(object sender, GroupMemberChangedEventArgs e)
        {
            UpdateOtherContactsGroupContactInfo();
        }

        void OtherContactsGroup_ContactAdded(object sender, GroupMemberChangedEventArgs e)
        {
            UpdateOtherContactsGroupContactInfo();
        }

        private void UpdateOtherContactsGroupContactInfo()
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                  {
                      OtherContacts.Clear();
                      foreach (Contact contact in OtherContactsGroup)
                      {
                          OtherContacts.Add(new ContactInfo(contact));
                      }
                      OtherContacts = new ObservableCollection<ContactInfo>(OtherContacts.OrderBy(p => p.DisplayName));

                      if (PropertyChanged != null)
                      {
                          PropertyChanged(this, new PropertyChangedEventArgs("OtherContacts"));
                      }
                  }), null);

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        private Group GetOtherContactsGroup()
        {
            Group returnGroup = null;
            try
            {
                foreach (Group group in ContactManager.Groups)
                {
                    if (group.Type == GroupType.CustomGroup && group.Name == "其他联系人")
                    {
                        returnGroup = group;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return returnGroup;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void AddContact_Callback(IAsyncResult ar)
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    OtherContactsGroup.EndAddContact(ar);

                }), null);

                UpdateOtherContactsGroupContactInfo();

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }


        }

        private void RemoveContact_Callback(IAsyncResult ar)
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    OtherContactsGroup.EndRemoveContact(ar);
                }), null);
                UpdateOtherContactsGroupContactInfo();

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }
    }

    public class ContactInfo
    {
        private string displayName = null;
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(displayName))
                {
                    return SipUri;
                }
                else
                {
                    return displayName + " (" + SipUri + ")";
                }
            }
        }

        public string SipUri { get; set; }

        public Contact Contact { get; set; }

        public ContactInfo(Contact contact)
        {
            try
            {
                this.Contact = contact;
                this.SipUri = contact.Uri;
                displayName = (string)this.Contact.GetContactInformation(ContactInformationType.DisplayName);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }
    }
}
