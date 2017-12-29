using ConferenceCommon.TimerHelper;
using ConferenceModel;
using ConferenceModel.Common;
using ConferenceModel.ConferenceInfoWebService;
using ConferenceModel.DetectionHelper;
using ConferenceModel.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using infoService = ConferenceModel.ConferenceInfoWebService;

namespace ConferenceModel
{
    public class CommunicationManage
    {
        #region 字段

        /// <summary>
        /// 知识树服务对应套接字
        /// </summary>
        ClientSocket TreeClientSocket = new ClientSocket();

        /// <summary>
        /// 语音服务对应套接字
        /// </summary>
        ClientSocket AudioClientSocket = new ClientSocket();

        /// <summary>
        /// 消息服务对应套接字
        /// </summary>
        ClientSocket InfoClientSocket = new ClientSocket();

        /// <summary>
        /// 文件服务对应套接字【甩屏】
        /// </summary>
        ClientSocket FileClientSocket = new ClientSocket();

        /// <summary>
        /// Lync服务对应套接字
        /// </summary>
        ClientSocket LyncClientSocket = new ClientSocket();

        /// <summary>
        /// Office服务对应套接字
        /// </summary>
        ClientSocket SpaceClientSocket = new ClientSocket();

        /// <summary>
        /// 矩阵服务对应套接字
        /// </summary>
        ClientSocket MatrixClientSocket = new ClientSocket();

        /// <summary>
        /// 远程服务器端口（tree,conference）
        /// </summary>
        int intPortNow = 0;

        /// <summary>
        /// 之前是否断开过
        /// </summary>
        bool isDisconnectedBefore = false;

        #endregion

        #region 事件委托

        /// <summary>
        /// 获取服务器所传送的数据
        /// </summary>
        public Action<ConferenceWebCommon.EntityCommon.PackageBase> clientSocket_TCPDataArrivalCallBack;

        /// <summary>
        /// 修复主窗体并导航到首页回调事件
        /// </summary>
        public Action RepareMainWindowAndNavicateToIndeCallBack = null;

        /// <summary>
        /// 刷新回调事件
        /// </summary>
        public Action PageRefleshCallBack = null;

        #endregion

        #region 获取服务端口

        /// <summary>
        /// 获取服务端口
        /// </summary>
        public void GetServicePort(Action callBack)
        {
            try
            {
                ModelManage.ConferenceInfo.GetMeetAllPort(CommunicationCodeEnterEntity.ConferenceID, new Action<bool, PortTypeEntity[]>((Successed, MeetPortList) =>
                {
                       new Thread(()=>
                    {
                        foreach (var item in MeetPortList)
                        {
                            ClientSocket clientSocket = this.GetClientSocket(item.conferenceClientAcceptType);
                            this.Communication_Server_Client(clientSocket, item.ServerPoint);
                        }
                        callBack();
                    }) { IsBackground = true }.Start();
                  
                }));

                #region old solution
                
                ////获取知识树服务端口
                //ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceTree, new Action<bool, ConferenceModel.ConferenceInfoWebService.PortTypeEntity>((Successed, MeetPort) =>
                //{
                //}));

                ////获取语音服务端口
                //ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceAudio, new Action<bool, ConferenceModel.ConferenceInfoWebService.PortTypeEntity>((Successed, MeetPort) =>
                //{
                //}));

                ////获取消息服务端口
                //ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceInfoSync, new Action<bool, ConferenceModel.ConferenceInfoWebService.PortTypeEntity>((Successed, MeetPort) =>
                //{
                //}));

                ////获取lync服务端口
                //ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.LyncConversationSync, new Action<bool, ConferenceModel.ConferenceInfoWebService.PortTypeEntity>((Successed, MeetPort) =>
                //{

                //}));

                ////获取智存空间服务端口
                //ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceSpaceSync, new Action<bool, ConferenceModel.ConferenceInfoWebService.PortTypeEntity>((Successed, MeetPort) =>
                //{

                //}));

                ////获取矩阵服务端口
                //ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceMatrixSync, new Action<bool, ConferenceModel.ConferenceInfoWebService.PortTypeEntity>((Successed, MeetPort) =>
                //{

                //}));

                ////获取文件服务端口【甩屏】
                //ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceFileSync, new Action<bool, ConferenceModel.ConferenceInfoWebService.PortTypeEntity>((Successed, portTypeEntity) =>
                //{
                //    //调用成功
                //    if (Successed)
                //    {
                //        TimerModelJob.StartRun(new Action(() =>
                //        {
                //            switch (portTypeEntity.conferenceClientAcceptType)
                //            {
                //                case ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceTree:
                //                    //通知服务端进行套接字的收集
                //                    this.Communication_Server_Client(this.TreeClientSocket, portTypeEntity.ServerPoint);
                //                    //进行通讯检测
                //                    //this.needCheckSocekt = true;

                //                    break;
                //                case ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceAudio:
                //                    //通知服务端进行套接字的收集
                //                    this.Communication_Server_Client(this.AudioClientSocket, portTypeEntity.ServerPoint);
                //                    break;
                //                case ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceFileSync:
                //                    //通知服务端进行套接字的收集
                //                    this.Communication_Server_Client(this.FileClientSocket, portTypeEntity.ServerPoint);
                //                    break;
                //                case ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceSpaceSync:
                //                    //通知服务端进行套接字的收集
                //                    this.Communication_Server_Client(this.SpaceClientSocket, portTypeEntity.ServerPoint);
                //                    break;
                //                case ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceInfoSync:
                //                    //通知服务端进行套接字的收集
                //                    this.Communication_Server_Client(this.InfoClientSocket, portTypeEntity.ServerPoint);
                //                    break;
                //                case ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.LyncConversationSync:
                //                    //通知服务端进行套接字的收集
                //                    this.Communication_Server_Client(this.LyncClientSocket, portTypeEntity.ServerPoint);
                //                    break;
                //                case ConferenceModel.ConferenceInfoWebService.ConferenceClientAcceptType.ConferenceMatrixSync:
                //                    //通知服务端进行套接字的收集
                //                    this.Communication_Server_Client(this.MatrixClientSocket, portTypeEntity.ServerPoint);

                //                    callBack();

                //                    break;

                //                default:
                //                    break;
                //            }

                //        }), 200);
                //    }
                //}));

                #endregion
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 端口绑定辅助

        public ClientSocket GetClientSocket(infoService.ConferenceClientAcceptType type)
        {
            ClientSocket clientSocket = null;
             try
            {
                switch (type)
                {
                    case ConferenceClientAcceptType.ConferenceTree:
                        clientSocket = this.TreeClientSocket;
                        break;
                    case ConferenceClientAcceptType.ConferenceAudio:
                        clientSocket = this.AudioClientSocket;
                        break;
                    case ConferenceClientAcceptType.ConferenceFileSync:
                        clientSocket = this.FileClientSocket;
                        break;
                    case ConferenceClientAcceptType.ConferenceSpaceSync:
                        clientSocket = this.SpaceClientSocket;
                        break;
                    case ConferenceClientAcceptType.ConferenceInfoSync:
                        clientSocket = this.InfoClientSocket;
                        break;
                    case ConferenceClientAcceptType.LyncConversationSync:
                        clientSocket = this.LyncClientSocket;
                        break;
                    case ConferenceClientAcceptType.ConferenceMatrixSync:
                        clientSocket = this.MatrixClientSocket;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
             return clientSocket;
        }

        #endregion

        #region 通讯机制

        /// <summary>
        /// 通知服务端进行套接字的收集
        /// </summary>
        protected void Communication_Server_Client(ClientSocket clientSocket, int port)
        {
            try
            {
                //服务器对该会议开放的端口
                this.intPortNow = port;
                //远程连接
                clientSocket.ConnectRemotePoint(CommunicationCodeEnterEntity.TreeServiceIP, port);
                //数据接收事件
                clientSocket.TCPDataArrivalCallBack = clientSocket_TCPDataArrival;

                //发送通知
                ConferenceWebCommon.EntityCommon.ConferenceClientAccept conferenceClientAccept = new ConferenceWebCommon.EntityCommon.ConferenceClientAccept();
                //会议名称
                conferenceClientAccept.ConferenceName = CommunicationCodeEnterEntity.ConferenceName;
                //会议ID
                conferenceClientAccept.ConferenceID = CommunicationCodeEnterEntity.ConferenceID;

                //当前参会人uri地址
                conferenceClientAccept.SelfUri = CommunicationCodeEnterEntity.SelfUri;

                //发送
                clientSocket.SendRemoteData(conferenceClientAccept);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 关闭本地套接字的连接
        /// </summary>
        public void Communication_Server_Client_Disopose()
        {
            try
            {
                if (this.TreeClientSocket != null)
                {
                    this.TreeClientSocket.TCPDataArrivalCallBack = null;
                    this.TreeClientSocket.CloseConnect();
                }

                if (this.AudioClientSocket != null)
                {
                    this.AudioClientSocket.TCPDataArrivalCallBack = null;
                    this.AudioClientSocket.CloseConnect();
                }

                if (this.FileClientSocket != null)
                {
                    this.FileClientSocket.TCPDataArrivalCallBack = null;
                    this.FileClientSocket.CloseConnect();
                }

                if (this.LyncClientSocket != null)
                {
                    this.LyncClientSocket.TCPDataArrivalCallBack = null;
                    this.LyncClientSocket.CloseConnect();
                }

                if (this.SpaceClientSocket != null)
                {
                    this.SpaceClientSocket.TCPDataArrivalCallBack = null;
                    this.SpaceClientSocket.CloseConnect();
                }

                if (this.MatrixClientSocket != null)
                {
                    this.MatrixClientSocket.TCPDataArrivalCallBack = null;
                    this.MatrixClientSocket.CloseConnect();
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

        #endregion

        #region 检测通讯连接

        DispatcherTimer timer = null;

        /// <summary>
        /// 检测通讯连接
        /// </summary>
        public void CheckAndRepairClientSocekt(Action<NetWorkErrTipType> CallBack)
        {
            try
            {
                if (timer != null) return;


                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(3);
                timer.Tick += (object sender, EventArgs e) =>
                {
                      new Thread((o) =>
                    {
                        try
                        {
                            if (DetectionModelManage.TestNetConnectity(CommunicationCodeEnterEntity.RouteIp))
                            {
                                if (DetectionModelManage.TestNetConnectity(CommunicationCodeEnterEntity.TreeServiceIP))
                                {
                                    if (DetectionModelManage.IsWebServiceAvaiable(CommunicationCodeEnterEntity.TreeServiceAddressFront + CommunicationCodeEnterEntity.ConferenceTreeServiceWebName))
                                    {
                                        CallBack(NetWorkErrTipType.Normal);
                                        //知识树通讯修复
                                        this.Repair_ClientSocket(this.TreeClientSocket, ConferenceClientAcceptType.ConferenceTree, true);
                                        //信息交流通讯修复
                                        this.Repair_ClientSocket(this.AudioClientSocket, ConferenceClientAcceptType.ConferenceAudio, true);
                                        //文件通讯修复
                                        this.Repair_ClientSocket(this.FileClientSocket, ConferenceClientAcceptType.ConferenceFileSync, false);
                                        //信息通讯修复
                                        this.Repair_ClientSocket(this.InfoClientSocket, ConferenceClientAcceptType.ConferenceInfoSync, false);
                                        //矩阵通讯修复
                                        this.Repair_ClientSocket(this.MatrixClientSocket, ConferenceClientAcceptType.ConferenceMatrixSync, false);
                                        //lync通讯修复
                                        this.Repair_ClientSocket(this.LyncClientSocket, ConferenceClientAcceptType.LyncConversationSync, false);
                                        //office通讯修复
                                        this.Repair_ClientSocket(this.SpaceClientSocket, ConferenceClientAcceptType.ConferenceSpaceSync, false);

                                        if (this.isDisconnectedBefore)
                                        {
                                            if (this.RepareMainWindowAndNavicateToIndeCallBack != null)
                                            {
                                                this.RepareMainWindowAndNavicateToIndeCallBack();
                                            }
                                        }
                                        this.isDisconnectedBefore = false;
                                    }
                                    else
                                    {
                                        CallBack(NetWorkErrTipType.ConnectedWebServiceFailed);
                                        this.isDisconnectedBefore = true;
                                    }
                                }
                                else
                                {
                                    CallBack(NetWorkErrTipType.ConnectedServiceFailed);
                                    this.isDisconnectedBefore = true;
                                }
                            }
                            else
                            {
                                CallBack(NetWorkErrTipType.ConnectedRouteFailed);
                                this.isDisconnectedBefore = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManage.WriteLog(this.GetType(), ex);
                        }
                        finally
                        {

                        }
                    }) { IsBackground = true }.Start();
                };
                timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        public void Repair_ClientSocket(ClientSocket clientSocket, ConferenceClientAcceptType conferenceClientAcceptType, bool NeedReflesh)
        {
            try
            {
                if (clientSocket != null && clientSocket._clientSocket != null)
                {
                    if (clientSocket._clientSocket.Poll(10, SelectMode.SelectRead))
                    {
                        //移除知识树通讯节点
                        ModelManage.ConferenceInfo.RemoveSelfClientSocket(CommunicationCodeEnterEntity.ConferenceID, conferenceClientAcceptType, CommunicationCodeEnterEntity.SelfUri, null);
                        //获取知识树服务端口
                        ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, conferenceClientAcceptType, new Action<bool, PortTypeEntity>((Successed, portTypeEntity) =>
                        {
                            //通知服务端进行套接字的收集
                            this.Communication_Server_Client(clientSocket, portTypeEntity.ServerPoint);
                            if (NeedReflesh)
                            {
                                if (this.PageRefleshCallBack != null)
                                {
                                    this.PageRefleshCallBack();
                                }
                            }
                        }));
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
        }

        public void RefleshBlockConnect(ConferenceClientAcceptType conferenceClientAcceptType)
        {
            try
            {
                ClientSocket clientSocket = default(ClientSocket);
                switch (conferenceClientAcceptType)
                {
                    case ConferenceClientAcceptType.ConferenceTree:
                        clientSocket = this.TreeClientSocket;
                        this.RefleshBlockConnectHelper(clientSocket, conferenceClientAcceptType);
                        break;
                    case ConferenceClientAcceptType.ConferenceAudio:
                        clientSocket = this.AudioClientSocket;
                        this.RefleshBlockConnectHelper(clientSocket, conferenceClientAcceptType);
                        break;
                    default:
                        break;
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

        public void RefleshBlockConnectHelper(ClientSocket clientSocket, ConferenceClientAcceptType conferenceClientAcceptType)
        {
            try
            {
                //移除知识树通讯节点
                ModelManage.ConferenceInfo.RemoveSelfClientSocket(CommunicationCodeEnterEntity.ConferenceID, conferenceClientAcceptType, CommunicationCodeEnterEntity.SelfUri, null);
                //获取知识树服务端口
                ModelManage.ConferenceInfo.GetMeetPort(CommunicationCodeEnterEntity.ConferenceID, conferenceClientAcceptType, new Action<bool, PortTypeEntity>((Successed, portTypeEntity) =>
                {
                    //通知服务端进行套接字的收集
                    this.Communication_Server_Client(clientSocket, portTypeEntity.ServerPoint);
                    //callBack();
                }));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }



        #endregion

        #region 数据回调中心

        protected void clientSocket_TCPDataArrival(ConferenceWebCommon.EntityCommon.PackageBase args)
        {
            try
            {
                if (Thread.CurrentThread.IsAlive)
                {
                    if (this.clientSocket_TCPDataArrivalCallBack != null)
                    {
                        this.clientSocket_TCPDataArrivalCallBack(args);
                    }
                }
            }
            catch (Exception ex)
            {
                //LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion
    }
}
