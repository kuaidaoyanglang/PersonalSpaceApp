﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConferenceModel.SpaceService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SpaceService.ServiceSoap")]
    public interface ServiceSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 str 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Fontion", ReplyAction="*")]
        ConferenceModel.SpaceService.FontionResponse Fontion(ConferenceModel.SpaceService.FontionRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/Fontion", ReplyAction="*")]
        System.IAsyncResult BeginFontion(ConferenceModel.SpaceService.FontionRequest request, System.AsyncCallback callback, object asyncState);
        
        ConferenceModel.SpaceService.FontionResponse EndFontion(System.IAsyncResult result);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 str 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Fontion_Uplode", ReplyAction="*")]
        ConferenceModel.SpaceService.Fontion_UplodeResponse Fontion_Uplode(ConferenceModel.SpaceService.Fontion_UplodeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/Fontion_Uplode", ReplyAction="*")]
        System.IAsyncResult BeginFontion_Uplode(ConferenceModel.SpaceService.Fontion_UplodeRequest request, System.AsyncCallback callback, object asyncState);
        
        ConferenceModel.SpaceService.Fontion_UplodeResponse EndFontion_Uplode(System.IAsyncResult result);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class FontionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Fontion", Namespace="http://tempuri.org/", Order=0)]
        public ConferenceModel.SpaceService.FontionRequestBody Body;
        
        public FontionRequest() {
        }
        
        public FontionRequest(ConferenceModel.SpaceService.FontionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class FontionRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string str;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pwd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string domain;
        
        public FontionRequestBody() {
        }
        
        public FontionRequestBody(string str, string username, string pwd, string domain) {
            this.str = str;
            this.username = username;
            this.pwd = pwd;
            this.domain = domain;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class FontionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="FontionResponse", Namespace="http://tempuri.org/", Order=0)]
        public ConferenceModel.SpaceService.FontionResponseBody Body;
        
        public FontionResponse() {
        }
        
        public FontionResponse(ConferenceModel.SpaceService.FontionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class FontionResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string FontionResult;
        
        public FontionResponseBody() {
        }
        
        public FontionResponseBody(string FontionResult) {
            this.FontionResult = FontionResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Fontion_UplodeRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Fontion_Uplode", Namespace="http://tempuri.org/", Order=0)]
        public ConferenceModel.SpaceService.Fontion_UplodeRequestBody Body;
        
        public Fontion_UplodeRequest() {
        }
        
        public Fontion_UplodeRequest(ConferenceModel.SpaceService.Fontion_UplodeRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Fontion_UplodeRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string str;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pwd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string domain;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public byte[] neirong;
        
        public Fontion_UplodeRequestBody() {
        }
        
        public Fontion_UplodeRequestBody(string str, string username, string pwd, string domain, byte[] neirong) {
            this.str = str;
            this.username = username;
            this.pwd = pwd;
            this.domain = domain;
            this.neirong = neirong;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Fontion_UplodeResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Fontion_UplodeResponse", Namespace="http://tempuri.org/", Order=0)]
        public ConferenceModel.SpaceService.Fontion_UplodeResponseBody Body;
        
        public Fontion_UplodeResponse() {
        }
        
        public Fontion_UplodeResponse(ConferenceModel.SpaceService.Fontion_UplodeResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class Fontion_UplodeResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Fontion_UplodeResult;
        
        public Fontion_UplodeResponseBody() {
        }
        
        public Fontion_UplodeResponseBody(string Fontion_UplodeResult) {
            this.Fontion_UplodeResult = Fontion_UplodeResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServiceSoapChannel : ConferenceModel.SpaceService.ServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FontionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public FontionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Fontion_UplodeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public Fontion_UplodeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceSoapClient : System.ServiceModel.ClientBase<ConferenceModel.SpaceService.ServiceSoap>, ConferenceModel.SpaceService.ServiceSoap {
        
        private BeginOperationDelegate onBeginFontionDelegate;
        
        private EndOperationDelegate onEndFontionDelegate;
        
        private System.Threading.SendOrPostCallback onFontionCompletedDelegate;
        
        private BeginOperationDelegate onBeginFontion_UplodeDelegate;
        
        private EndOperationDelegate onEndFontion_UplodeDelegate;
        
        private System.Threading.SendOrPostCallback onFontion_UplodeCompletedDelegate;
        
        public ServiceSoapClient() {
        }
        
        public ServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<FontionCompletedEventArgs> FontionCompleted;
        
        public event System.EventHandler<Fontion_UplodeCompletedEventArgs> Fontion_UplodeCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ConferenceModel.SpaceService.FontionResponse ConferenceModel.SpaceService.ServiceSoap.Fontion(ConferenceModel.SpaceService.FontionRequest request) {
            return base.Channel.Fontion(request);
        }
        
        public string Fontion(string str, string username, string pwd, string domain) {
            ConferenceModel.SpaceService.FontionRequest inValue = new ConferenceModel.SpaceService.FontionRequest();
            inValue.Body = new ConferenceModel.SpaceService.FontionRequestBody();
            inValue.Body.str = str;
            inValue.Body.username = username;
            inValue.Body.pwd = pwd;
            inValue.Body.domain = domain;
            ConferenceModel.SpaceService.FontionResponse retVal = ((ConferenceModel.SpaceService.ServiceSoap)(this)).Fontion(inValue);
            return retVal.Body.FontionResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ConferenceModel.SpaceService.ServiceSoap.BeginFontion(ConferenceModel.SpaceService.FontionRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginFontion(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginFontion(string str, string username, string pwd, string domain, System.AsyncCallback callback, object asyncState) {
            ConferenceModel.SpaceService.FontionRequest inValue = new ConferenceModel.SpaceService.FontionRequest();
            inValue.Body = new ConferenceModel.SpaceService.FontionRequestBody();
            inValue.Body.str = str;
            inValue.Body.username = username;
            inValue.Body.pwd = pwd;
            inValue.Body.domain = domain;
            return ((ConferenceModel.SpaceService.ServiceSoap)(this)).BeginFontion(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ConferenceModel.SpaceService.FontionResponse ConferenceModel.SpaceService.ServiceSoap.EndFontion(System.IAsyncResult result) {
            return base.Channel.EndFontion(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public string EndFontion(System.IAsyncResult result) {
            ConferenceModel.SpaceService.FontionResponse retVal = ((ConferenceModel.SpaceService.ServiceSoap)(this)).EndFontion(result);
            return retVal.Body.FontionResult;
        }
        
        private System.IAsyncResult OnBeginFontion(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string str = ((string)(inValues[0]));
            string username = ((string)(inValues[1]));
            string pwd = ((string)(inValues[2]));
            string domain = ((string)(inValues[3]));
            return this.BeginFontion(str, username, pwd, domain, callback, asyncState);
        }
        
        private object[] OnEndFontion(System.IAsyncResult result) {
            string retVal = this.EndFontion(result);
            return new object[] {
                    retVal};
        }
        
        private void OnFontionCompleted(object state) {
            if ((this.FontionCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.FontionCompleted(this, new FontionCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void FontionAsync(string str, string username, string pwd, string domain) {
            this.FontionAsync(str, username, pwd, domain, null);
        }
        
        public void FontionAsync(string str, string username, string pwd, string domain, object userState) {
            if ((this.onBeginFontionDelegate == null)) {
                this.onBeginFontionDelegate = new BeginOperationDelegate(this.OnBeginFontion);
            }
            if ((this.onEndFontionDelegate == null)) {
                this.onEndFontionDelegate = new EndOperationDelegate(this.OnEndFontion);
            }
            if ((this.onFontionCompletedDelegate == null)) {
                this.onFontionCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnFontionCompleted);
            }
            base.InvokeAsync(this.onBeginFontionDelegate, new object[] {
                        str,
                        username,
                        pwd,
                        domain}, this.onEndFontionDelegate, this.onFontionCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ConferenceModel.SpaceService.Fontion_UplodeResponse ConferenceModel.SpaceService.ServiceSoap.Fontion_Uplode(ConferenceModel.SpaceService.Fontion_UplodeRequest request) {
            return base.Channel.Fontion_Uplode(request);
        }
        
        public string Fontion_Uplode(string str, string username, string pwd, string domain, byte[] neirong) {
            ConferenceModel.SpaceService.Fontion_UplodeRequest inValue = new ConferenceModel.SpaceService.Fontion_UplodeRequest();
            inValue.Body = new ConferenceModel.SpaceService.Fontion_UplodeRequestBody();
            inValue.Body.str = str;
            inValue.Body.username = username;
            inValue.Body.pwd = pwd;
            inValue.Body.domain = domain;
            inValue.Body.neirong = neirong;
            ConferenceModel.SpaceService.Fontion_UplodeResponse retVal = ((ConferenceModel.SpaceService.ServiceSoap)(this)).Fontion_Uplode(inValue);
            return retVal.Body.Fontion_UplodeResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ConferenceModel.SpaceService.ServiceSoap.BeginFontion_Uplode(ConferenceModel.SpaceService.Fontion_UplodeRequest request, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginFontion_Uplode(request, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginFontion_Uplode(string str, string username, string pwd, string domain, byte[] neirong, System.AsyncCallback callback, object asyncState) {
            ConferenceModel.SpaceService.Fontion_UplodeRequest inValue = new ConferenceModel.SpaceService.Fontion_UplodeRequest();
            inValue.Body = new ConferenceModel.SpaceService.Fontion_UplodeRequestBody();
            inValue.Body.str = str;
            inValue.Body.username = username;
            inValue.Body.pwd = pwd;
            inValue.Body.domain = domain;
            inValue.Body.neirong = neirong;
            return ((ConferenceModel.SpaceService.ServiceSoap)(this)).BeginFontion_Uplode(inValue, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        ConferenceModel.SpaceService.Fontion_UplodeResponse ConferenceModel.SpaceService.ServiceSoap.EndFontion_Uplode(System.IAsyncResult result) {
            return base.Channel.EndFontion_Uplode(result);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public string EndFontion_Uplode(System.IAsyncResult result) {
            ConferenceModel.SpaceService.Fontion_UplodeResponse retVal = ((ConferenceModel.SpaceService.ServiceSoap)(this)).EndFontion_Uplode(result);
            return retVal.Body.Fontion_UplodeResult;
        }
        
        private System.IAsyncResult OnBeginFontion_Uplode(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string str = ((string)(inValues[0]));
            string username = ((string)(inValues[1]));
            string pwd = ((string)(inValues[2]));
            string domain = ((string)(inValues[3]));
            byte[] neirong = ((byte[])(inValues[4]));
            return this.BeginFontion_Uplode(str, username, pwd, domain, neirong, callback, asyncState);
        }
        
        private object[] OnEndFontion_Uplode(System.IAsyncResult result) {
            string retVal = this.EndFontion_Uplode(result);
            return new object[] {
                    retVal};
        }
        
        private void OnFontion_UplodeCompleted(object state) {
            if ((this.Fontion_UplodeCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.Fontion_UplodeCompleted(this, new Fontion_UplodeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void Fontion_UplodeAsync(string str, string username, string pwd, string domain, byte[] neirong) {
            this.Fontion_UplodeAsync(str, username, pwd, domain, neirong, null);
        }
        
        public void Fontion_UplodeAsync(string str, string username, string pwd, string domain, byte[] neirong, object userState) {
            if ((this.onBeginFontion_UplodeDelegate == null)) {
                this.onBeginFontion_UplodeDelegate = new BeginOperationDelegate(this.OnBeginFontion_Uplode);
            }
            if ((this.onEndFontion_UplodeDelegate == null)) {
                this.onEndFontion_UplodeDelegate = new EndOperationDelegate(this.OnEndFontion_Uplode);
            }
            if ((this.onFontion_UplodeCompletedDelegate == null)) {
                this.onFontion_UplodeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnFontion_UplodeCompleted);
            }
            base.InvokeAsync(this.onBeginFontion_UplodeDelegate, new object[] {
                        str,
                        username,
                        pwd,
                        domain,
                        neirong}, this.onEndFontion_UplodeDelegate, this.onFontion_UplodeCompletedDelegate, userState);
        }
    }
}
