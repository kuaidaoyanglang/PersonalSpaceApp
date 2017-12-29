
using ConferenceModel.ConferenceTreeWebService;
using ConferenceModel.LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConferenceModel.Common
{
    public class ClientSocket
    {

        private const int BUFFER_SIZE = 1024 * 10;

        /// <summary>
        /// 连接到服务器的Socket（客户端Socket）
        /// </summary>
        public Socket _clientSocket;

        /// <summary>
        /// 启动接收服务器端发送过来消息的线程
        /// </summary>
        private Thread _getRecieveDataThread;

        #region 委托 && 事件

        //public delegate void TCPDataArrivalEventHandler(ConferenceWebCommon.Common.PackageBase args);
        ///// <summary>
        ///// 数据发送事件
        ///// </summary>
        //public event TCPDataArrivalEventHandler TCPDataArrival = null;

        public Action<ConferenceWebCommon.EntityCommon.PackageBase> TCPDataArrivalCallBack = null;

        #endregion

        /// <summary>
        /// 连接远程端点
        /// </summary>
        /// <param name="remoteIp">IP地址</param>
        /// <param name="port">端口</param>
        public void ConnectRemotePoint(string remoteIp, int port)
        {
            try
            {
                //生成地址
                IPAddress remoteAddress = IPAddress.Parse(remoteIp);

                this._clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //this._clientSocket.ReceiveTimeout = 3000;
                //this._clientSocket.SendTimeout = 3000;
                //建立远程主机的连接(服务器)
                this._clientSocket.Connect(new IPEndPoint(remoteAddress, port));
                //启动接收服务器端发送过来消息的线程
                this._getRecieveDataThread = new Thread(new ThreadStart(this.GetRemoteData));
                //设置为后台工作者
                this._getRecieveDataThread.IsBackground = true;
                //开始执行
                this._getRecieveDataThread.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 接收从远程主机发送的数据（服务器端）
        /// </summary>
        private void GetRemoteData()
        {
            var dd = this._clientSocket.Available;

            try
            {
                while (this._clientSocket != null)
                {
                    ConferenceWebCommon.EntityCommon.PackageBase code = null;
                    List<byte> lists = new List<byte>();
                callBack:
                    byte[] buffer = new byte[BUFFER_SIZE];

                    int count = this._clientSocket.Receive(buffer);//挂起操作
                    if (this._clientSocket == null) break;
                    if (count == 0)
                    {
                        //客户端与服务器端断开连接
                        break;
                    }
                    if (count == BUFFER_SIZE)
                    {
                        lists.AddRange(buffer);
                    }
                    else if (count < BUFFER_SIZE)
                    {
                        byte[] dataless = new byte[count];
                        Array.Copy(buffer, dataless, count);
                        lists.AddRange(dataless);
                    }
                    if (this._clientSocket.Available != 0)
                    {
                        goto callBack;
                    }

                    byte[] data = lists.ToArray();
                    lists.Clear();
                    lists = null;
                    MemoryStream stream = null;

                    using (stream = new MemoryStream(data))
                    {
                        stream.Position = 0;
                        BinaryFormatter formatter = new BinaryFormatter();
                        code = formatter.Deserialize(stream) as ConferenceWebCommon.EntityCommon.PackageBase;
                    }
                    Array.Clear(data, 0, data.Length);
                    data = null;
                    //this._clientSocket.Close();
                    if (this.TCPDataArrivalCallBack != null)
                    {
                        this.TCPDataArrivalCallBack(code);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 向远程主机发送消息
        /// </summary>
        /// <param name="packageBase">数据包</param>
        public void SendRemoteData(ConferenceWebCommon.EntityCommon.ConferenceClientAccept conferenceClientAccept)
        {
            try
            {
                if (this._clientSocket != null
                    && this._clientSocket.Connected)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Position = 0;
                        formatter.Serialize(ms, conferenceClientAccept);//发送到服务器端
                        byte[] data = ms.ToArray();
                        int count = this._clientSocket.Send(data, data.Length, SocketFlags.None);
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnect()
        {
            try
            {

                //if (this._getRecieveDataThread !=null)
                //{
                //    if(this._getRecieveDataThread.IsAlive)
                //    {
                //        this._getRecieveDataThread.Abort();
                //    }
                //}
                if (this._clientSocket != null)
                {
                    if (this._clientSocket.Connected)
                    {
                        this._clientSocket.Shutdown(SocketShutdown.Both);
                    }
                    this._clientSocket.Close();
                    this._clientSocket = null;
                }
                if (this._getRecieveDataThread != null)
                {
                    this._getRecieveDataThread.Abort();
                    this._getRecieveDataThread = null;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 检查是否已连接
        /// </summary>
        /// <returns></returns>
        public bool CheckIsConnected()
        {
            //是否可以
            bool isConnected = false;
            try
            {
                if (this._clientSocket.Connected)
                {
                    isConnected =true;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            return isConnected;
        }      
    }
}
