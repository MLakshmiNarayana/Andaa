﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrjAndaa.MasterServices {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MasterServices.IMasterServices")]
    public interface IMasterServices {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/SetSessions", ReplyAction="http://tempuri.org/ISessionMgmt/SetSessionsResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/ISessionMgmt/SetSessionsInvalidSessionExceptionFault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        void SetSessions(System.Collections.Generic.Dictionary<string, Focus.Common.DataStructs.Session> m_Sessions);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/SetSessions", ReplyAction="http://tempuri.org/ISessionMgmt/SetSessionsResponse")]
        System.Threading.Tasks.Task SetSessionsAsync(System.Collections.Generic.Dictionary<string, Focus.Common.DataStructs.Session> m_Sessions);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/GetCompanyId", ReplyAction="http://tempuri.org/ISessionMgmt/GetCompanyIdResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/ISessionMgmt/GetCompanyIdInvalidSessionExceptionFault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        int GetCompanyId(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/GetCompanyId", ReplyAction="http://tempuri.org/ISessionMgmt/GetCompanyIdResponse")]
        System.Threading.Tasks.Task<int> GetCompanyIdAsync(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/GetLoginId", ReplyAction="http://tempuri.org/ISessionMgmt/GetLoginIdResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/ISessionMgmt/GetLoginIdInvalidSessionExceptionFault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        int GetLoginId(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/GetLoginId", ReplyAction="http://tempuri.org/ISessionMgmt/GetLoginIdResponse")]
        System.Threading.Tasks.Task<int> GetLoginIdAsync(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/GetLogId", ReplyAction="http://tempuri.org/ISessionMgmt/GetLogIdResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/ISessionMgmt/GetLogIdInvalidSessionExceptionFault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        int GetLogId(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/GetLogId", ReplyAction="http://tempuri.org/ISessionMgmt/GetLogIdResponse")]
        System.Threading.Tasks.Task<int> GetLogIdAsync(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/SetSessionLastAccessed", ReplyAction="http://tempuri.org/ISessionMgmt/SetSessionLastAccessedResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/ISessionMgmt/SetSessionLastAccessedInvalidSessionExceptionFaul" +
            "t", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        void SetSessionLastAccessed(string strSessionId, long iDateTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/SetSessionLastAccessed", ReplyAction="http://tempuri.org/ISessionMgmt/SetSessionLastAccessedResponse")]
        System.Threading.Tasks.Task SetSessionLastAccessedAsync(string strSessionId, long iDateTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/LogoutSession", ReplyAction="http://tempuri.org/ISessionMgmt/LogoutSessionResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/ISessionMgmt/LogoutSessionInvalidSessionExceptionFault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        void LogoutSession(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISessionMgmt/LogoutSession", ReplyAction="http://tempuri.org/ISessionMgmt/LogoutSessionResponse")]
        System.Threading.Tasks.Task LogoutSessionAsync(string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMasterServices/ServeRequest", ReplyAction="http://tempuri.org/IMasterServices/ServeRequestResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/IMasterServices/ServeRequestInvalidSessionExceptionFault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        Focus.Common.DataStructs.Param ServeRequest(Focus.Common.DataStructs.Param Params, Focus.Common.DataStructs.ServiceType type, string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMasterServices/ServeRequest", ReplyAction="http://tempuri.org/IMasterServices/ServeRequestResponse")]
        System.Threading.Tasks.Task<Focus.Common.DataStructs.Param> ServeRequestAsync(Focus.Common.DataStructs.Param Params, Focus.Common.DataStructs.ServiceType type, string strSessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMasterServices/CreateAndSendReminders", ReplyAction="http://tempuri.org/IMasterServices/CreateAndSendRemindersResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/IMasterServices/CreateAndSendRemindersInvalidSessionExceptionF" +
            "ault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        bool CreateAndSendReminders(
                    int iCompanyId, 
                    int iReminderTermsId, 
                    bool bIsBillWise, 
                    int iLocationId, 
                    int iCreatedBy, 
                    bool bIsMailNow, 
                    int AuthenticationType, 
                    bool EnableSecureConnection, 
                    string FromPassword, 
                    string FromUserName, 
                    bool FullEmailId, 
                    bool MailServerRequires, 
                    int Port, 
                    int SecureConnection, 
                    bool SecurePassword, 
                    int SettingsType, 
                    string SMTPAdress, 
                    int SSLPort);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMasterServices/CreateAndSendReminders", ReplyAction="http://tempuri.org/IMasterServices/CreateAndSendRemindersResponse")]
        System.Threading.Tasks.Task<bool> CreateAndSendRemindersAsync(
                    int iCompanyId, 
                    int iReminderTermsId, 
                    bool bIsBillWise, 
                    int iLocationId, 
                    int iCreatedBy, 
                    bool bIsMailNow, 
                    int AuthenticationType, 
                    bool EnableSecureConnection, 
                    string FromPassword, 
                    string FromUserName, 
                    bool FullEmailId, 
                    bool MailServerRequires, 
                    int Port, 
                    int SecureConnection, 
                    bool SecurePassword, 
                    int SettingsType, 
                    string SMTPAdress, 
                    int SSLPort);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMasterServices/AutoPostInterestCalculation", ReplyAction="http://tempuri.org/IMasterServices/AutoPostInterestCalculationResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Focus.Common.DataStructs.InvalidSessionException), Action="http://tempuri.org/IMasterServices/AutoPostInterestCalculationInvalidSessionExcep" +
            "tionFault", Name="InvalidSessionException", Namespace="http://schemas.datacontract.org/2004/07/Focus.Common.DataStructs")]
        bool AutoPostInterestCalculation(int iCompanyId, int iLoginId, int iLogId, int iFinanceChargeTermId, int iUserId, Focus.Common.DataStructs.CalendarType objCalendarType, bool bIsAutoPostInterest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMasterServices/AutoPostInterestCalculation", ReplyAction="http://tempuri.org/IMasterServices/AutoPostInterestCalculationResponse")]
        System.Threading.Tasks.Task<bool> AutoPostInterestCalculationAsync(int iCompanyId, int iLoginId, int iLogId, int iFinanceChargeTermId, int iUserId, Focus.Common.DataStructs.CalendarType objCalendarType, bool bIsAutoPostInterest);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMasterServicesChannel : PrjAndaa.MasterServices.IMasterServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MasterServicesClient : System.ServiceModel.ClientBase<PrjAndaa.MasterServices.IMasterServices>, PrjAndaa.MasterServices.IMasterServices {
        
        public MasterServicesClient() {
        }
        
        public MasterServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MasterServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MasterServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MasterServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void SetSessions(System.Collections.Generic.Dictionary<string, Focus.Common.DataStructs.Session> m_Sessions) {
            base.Channel.SetSessions(m_Sessions);
        }
        
        public System.Threading.Tasks.Task SetSessionsAsync(System.Collections.Generic.Dictionary<string, Focus.Common.DataStructs.Session> m_Sessions) {
            return base.Channel.SetSessionsAsync(m_Sessions);
        }
        
        public int GetCompanyId(string strSessionId) {
            return base.Channel.GetCompanyId(strSessionId);
        }
        
        public System.Threading.Tasks.Task<int> GetCompanyIdAsync(string strSessionId) {
            return base.Channel.GetCompanyIdAsync(strSessionId);
        }
        
        public int GetLoginId(string strSessionId) {
            return base.Channel.GetLoginId(strSessionId);
        }
        
        public System.Threading.Tasks.Task<int> GetLoginIdAsync(string strSessionId) {
            return base.Channel.GetLoginIdAsync(strSessionId);
        }
        
        public int GetLogId(string strSessionId) {
            return base.Channel.GetLogId(strSessionId);
        }
        
        public System.Threading.Tasks.Task<int> GetLogIdAsync(string strSessionId) {
            return base.Channel.GetLogIdAsync(strSessionId);
        }
        
        public void SetSessionLastAccessed(string strSessionId, long iDateTime) {
            base.Channel.SetSessionLastAccessed(strSessionId, iDateTime);
        }
        
        public System.Threading.Tasks.Task SetSessionLastAccessedAsync(string strSessionId, long iDateTime) {
            return base.Channel.SetSessionLastAccessedAsync(strSessionId, iDateTime);
        }
        
        public void LogoutSession(string strSessionId) {
            base.Channel.LogoutSession(strSessionId);
        }
        
        public System.Threading.Tasks.Task LogoutSessionAsync(string strSessionId) {
            return base.Channel.LogoutSessionAsync(strSessionId);
        }
        
        public Focus.Common.DataStructs.Param ServeRequest(Focus.Common.DataStructs.Param Params, Focus.Common.DataStructs.ServiceType type, string strSessionId) {
            return base.Channel.ServeRequest(Params, type, strSessionId);
        }
        
        public System.Threading.Tasks.Task<Focus.Common.DataStructs.Param> ServeRequestAsync(Focus.Common.DataStructs.Param Params, Focus.Common.DataStructs.ServiceType type, string strSessionId) {
            return base.Channel.ServeRequestAsync(Params, type, strSessionId);
        }
        
        public bool CreateAndSendReminders(
                    int iCompanyId, 
                    int iReminderTermsId, 
                    bool bIsBillWise, 
                    int iLocationId, 
                    int iCreatedBy, 
                    bool bIsMailNow, 
                    int AuthenticationType, 
                    bool EnableSecureConnection, 
                    string FromPassword, 
                    string FromUserName, 
                    bool FullEmailId, 
                    bool MailServerRequires, 
                    int Port, 
                    int SecureConnection, 
                    bool SecurePassword, 
                    int SettingsType, 
                    string SMTPAdress, 
                    int SSLPort) {
            return base.Channel.CreateAndSendReminders(iCompanyId, iReminderTermsId, bIsBillWise, iLocationId, iCreatedBy, bIsMailNow, AuthenticationType, EnableSecureConnection, FromPassword, FromUserName, FullEmailId, MailServerRequires, Port, SecureConnection, SecurePassword, SettingsType, SMTPAdress, SSLPort);
        }
        
        public System.Threading.Tasks.Task<bool> CreateAndSendRemindersAsync(
                    int iCompanyId, 
                    int iReminderTermsId, 
                    bool bIsBillWise, 
                    int iLocationId, 
                    int iCreatedBy, 
                    bool bIsMailNow, 
                    int AuthenticationType, 
                    bool EnableSecureConnection, 
                    string FromPassword, 
                    string FromUserName, 
                    bool FullEmailId, 
                    bool MailServerRequires, 
                    int Port, 
                    int SecureConnection, 
                    bool SecurePassword, 
                    int SettingsType, 
                    string SMTPAdress, 
                    int SSLPort) {
            return base.Channel.CreateAndSendRemindersAsync(iCompanyId, iReminderTermsId, bIsBillWise, iLocationId, iCreatedBy, bIsMailNow, AuthenticationType, EnableSecureConnection, FromPassword, FromUserName, FullEmailId, MailServerRequires, Port, SecureConnection, SecurePassword, SettingsType, SMTPAdress, SSLPort);
        }
        
        public bool AutoPostInterestCalculation(int iCompanyId, int iLoginId, int iLogId, int iFinanceChargeTermId, int iUserId, Focus.Common.DataStructs.CalendarType objCalendarType, bool bIsAutoPostInterest) {
            return base.Channel.AutoPostInterestCalculation(iCompanyId, iLoginId, iLogId, iFinanceChargeTermId, iUserId, objCalendarType, bIsAutoPostInterest);
        }
        
        public System.Threading.Tasks.Task<bool> AutoPostInterestCalculationAsync(int iCompanyId, int iLoginId, int iLogId, int iFinanceChargeTermId, int iUserId, Focus.Common.DataStructs.CalendarType objCalendarType, bool bIsAutoPostInterest) {
            return base.Channel.AutoPostInterestCalculationAsync(iCompanyId, iLoginId, iLogId, iFinanceChargeTermId, iUserId, objCalendarType, bIsAutoPostInterest);
        }
    }
}