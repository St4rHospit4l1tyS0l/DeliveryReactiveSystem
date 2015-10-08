﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Drs.Service.SyncService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResponseMessageFc", Namespace="http://schemas.datacontract.org/2004/07/FranchiseChannel.Service.Model")]
    [System.SerializableAttribute()]
    public partial class ResponseMessageFc : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool HasErrorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int TotalFilesField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool HasError {
            get {
                return this.HasErrorField;
            }
            set {
                if ((this.HasErrorField.Equals(value) != true)) {
                    this.HasErrorField = value;
                    this.RaisePropertyChanged("HasError");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Message {
            get {
                return this.MessageField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageField, value) != true)) {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalFiles {
            get {
                return this.TotalFilesField;
            }
            set {
                if ((this.TotalFilesField.Equals(value) != true)) {
                    this.TotalFilesField = value;
                    this.RaisePropertyChanged("TotalFiles");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SyncService.ISyncService")]
    public interface ISyncService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISyncService/QueryForFiles", ReplyAction="http://tempuri.org/ISyncService/QueryForFilesResponse")]
        Drs.Service.SyncService.ResponseMessageFc QueryForFiles(System.Guid uidVersion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISyncService/QueryForFiles", ReplyAction="http://tempuri.org/ISyncService/QueryForFilesResponse")]
        System.Threading.Tasks.Task<Drs.Service.SyncService.ResponseMessageFc> QueryForFilesAsync(System.Guid uidVersion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISyncServiceChannel : Drs.Service.SyncService.ISyncService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SyncServiceClient : System.ServiceModel.ClientBase<Drs.Service.SyncService.ISyncService>, Drs.Service.SyncService.ISyncService {
        
        public SyncServiceClient() {
        }
        
        public SyncServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SyncServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SyncServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SyncServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Drs.Service.SyncService.ResponseMessageFc QueryForFiles(System.Guid uidVersion) {
            return base.Channel.QueryForFiles(uidVersion);
        }
        
        public System.Threading.Tasks.Task<Drs.Service.SyncService.ResponseMessageFc> QueryForFilesAsync(System.Guid uidVersion) {
            return base.Channel.QueryForFilesAsync(uidVersion);
        }
    }
}