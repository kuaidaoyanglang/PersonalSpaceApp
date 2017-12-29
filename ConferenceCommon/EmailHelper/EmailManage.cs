using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace ConferenceCommon.EmailHelper
{
    /// <summary>
    /// EmailManage 的摘要说明
    /// </summary>
    public class EmailManage
    {
        private static MailMessage mailMessage;
        private static SmtpClient smtpClient;
        private static string password;//发件人密码     

        public static void EmailInit(string From, string senderDisplayName, string Password)
        {
            try
            {
                if (mailMessage == null)
                {
                    var econding = System.Text.Encoding.UTF8;
                    mailMessage = new MailMessage();
                    mailMessage.From = new System.Net.Mail.MailAddress(From, senderDisplayName, econding);
                    mailMessage.IsBodyHtml = true;
                    mailMessage.BodyEncoding = econding;
                    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
                    password = Password;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(EmailManage), ex);
            }
        }


        /// <summary>  
        /// 添加附件  
        /// </summary>  
        public static void Attachments(string Path)
        {
            try
            {
                string[] path = Path.Split(',');
                Attachment data;
                ContentDisposition disposition;
                for (int i = 0; i < path.Length; i++)
                {
                    data = new Attachment(path[i], MediaTypeNames.Application.Octet);//实例化附件 

                    disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(path[i]);//获取附件的创建日期 

                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(path[i]);//获取附件的修改日期 

                    disposition.ReadDate = System.IO.File.GetLastAccessTime(path[i]);//获取附 

                    mailMessage.Attachments.Add(data);//添加到附件中件的读取日期  
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(EmailManage), ex);
            }
        }
        /// <summary>  
        /// 异步发送邮件  
        /// </summary>  
        /// <param name="CompletedMethod"></param>  
        public static void SendAsync(SendCompletedEventHandler CompletedMethod)
        {
            try
            {
                if (mailMessage != null)
                {
                    smtpClient = new SmtpClient();
                    smtpClient.Credentials = new System.Net.NetworkCredential
        (mailMessage.From.Address, password);//设置发件人身份的票据  
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtpClient.Host = "smtp." + mailMessage.From.Host;
                    smtpClient.SendCompleted += new SendCompletedEventHandler
        (CompletedMethod);//注册异步发送邮件完成时的事件  
                    smtpClient.SendAsync(mailMessage, mailMessage.Body);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(EmailManage), ex);
            }
        }

        public static void AddUser(string ToArray)
        {
            try
            {
                if (mailMessage != null)
                {
                    mailMessage.To.Clear();

                    var list = ToArray.Split(new char[] { ';' });
                    foreach (var item in list)
                    {
                        mailMessage.To.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(EmailManage), ex);
            }
        }
        /// <summary>  
        /// 发送邮件  
        /// </summary>  
        public static void Send(string Title, string Body)
        {
            try
            {
                if (mailMessage != null)
                {
                    if (smtpClient == null)
                    {
                        smtpClient = new SmtpClient();
                        smtpClient.Credentials = new System.Net.NetworkCredential
            (mailMessage.From.Address, password);//设置发件人身份的票据  
                        smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        smtpClient.Host = "smtp." + mailMessage.From.Host;
                    }

                    mailMessage.Subject = Title;
                    mailMessage.Body = Body;
                    smtpClient.Send(mailMessage);

                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(EmailManage), ex);
            }
            finally
            {
                if (smtpClient != null)
                {
                    mailMessage.Dispose();
                    smtpClient.Dispose();

                    mailMessage = null;
                    smtpClient = null;
                }
            }
        }

    }
}
