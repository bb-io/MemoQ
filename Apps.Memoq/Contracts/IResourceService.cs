﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Runtime.Serialization.ContractNamespaceAttribute("http://kilgray.com/memoqservices/2007", ClrNamespace="MQS.Resource")]

namespace MQS.Resource
{
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResourceType", Namespace="http://kilgray.com/memoqservices/2007")]
    public enum ResourceType : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AutoTrans = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NonTrans = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AutoCorrect = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        IgnoreLists = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SegRules = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TMSettings = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FilterConfigs = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        KeyboardShortcuts = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PathRules = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        QASettings = 9,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Stopwords = 10,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LQA = 11,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LiveDocsSettings = 12,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebSearchSettings = 13,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FontSubstitution = 14,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        KeyboardShortcuts2 = 15,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProjectTemplate = 16,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MTSettings = 17,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResourceInfo", Namespace="http://kilgray.com/memoqservices/2007")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.LightResourceInfo))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.LightResourceInfoWithLang))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.PathRuleResourceInfo))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.FilterConfigResourceInfo))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.ProjectTemplateResourceInfo))]
    public partial class ResourceInfo : object
    {
        
        private string DescriptionField;
        
        private System.Guid GuidField;
        
        private string NameField;
        
        private bool ReadonlyField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description
        {
            get
            {
                return this.DescriptionField;
            }
            set
            {
                this.DescriptionField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid Guid
        {
            get
            {
                return this.GuidField;
            }
            set
            {
                this.GuidField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Readonly
        {
            get
            {
                return this.ReadonlyField;
            }
            set
            {
                this.ReadonlyField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LightResourceInfo", Namespace="http://kilgray.com/memoqservices/2007")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.LightResourceInfoWithLang))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.PathRuleResourceInfo))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.FilterConfigResourceInfo))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(MQS.Resource.ProjectTemplateResourceInfo))]
    public partial class LightResourceInfo : MQS.Resource.ResourceInfo
    {
        
        private bool IsDefaultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsDefault
        {
            get
            {
                return this.IsDefaultField;
            }
            set
            {
                this.IsDefaultField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LightResourceInfoWithLang", Namespace="http://kilgray.com/memoqservices/2007")]
    public partial class LightResourceInfoWithLang : MQS.Resource.LightResourceInfo
    {
        
        private string LanguageCodeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LanguageCode
        {
            get
            {
                return this.LanguageCodeField;
            }
            set
            {
                this.LanguageCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PathRuleResourceInfo", Namespace="http://kilgray.com/memoqservices/2007")]
    public partial class PathRuleResourceInfo : MQS.Resource.LightResourceInfo
    {
        
        private MQS.Resource.PathRuleType RuleTypeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public MQS.Resource.PathRuleType RuleType
        {
            get
            {
                return this.RuleTypeField;
            }
            set
            {
                this.RuleTypeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FilterConfigResourceInfo", Namespace="http://kilgray.com/memoqservices/2007")]
    public partial class FilterConfigResourceInfo : MQS.Resource.LightResourceInfo
    {
        
        private string FilterNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FilterName
        {
            get
            {
                return this.FilterNameField;
            }
            set
            {
                this.FilterNameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProjectTemplateResourceInfo", Namespace="http://kilgray.com/memoqservices/2007")]
    public partial class ProjectTemplateResourceInfo : MQS.Resource.LightResourceInfo
    {
        
        private string SourceLangCodeField;
        
        private string[] TargetLangCodesField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SourceLangCode
        {
            get
            {
                return this.SourceLangCodeField;
            }
            set
            {
                this.SourceLangCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] TargetLangCodes
        {
            get
            {
                return this.TargetLangCodesField;
            }
            set
            {
                this.TargetLangCodesField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PathRuleType", Namespace="http://kilgray.com/memoqservices/2007")]
    public enum PathRuleType : int
    {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        File = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Folder = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UnexpectedFault", Namespace="http://kilgray.com/memoqservices/2007")]
    public partial class UnexpectedFault : object
    {
        
        private string StackTraceField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StackTrace
        {
            get
            {
                return this.StackTraceField;
            }
            set
            {
                this.StackTraceField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GenericFault", Namespace="http://kilgray.com/memoqservices/2007")]
    public partial class GenericFault : object
    {
        
        private string ErrorCodeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorCode
        {
            get
            {
                return this.ErrorCodeField;
            }
            set
            {
                this.ErrorCodeField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LightResourceListFilter", Namespace="http://kilgray.com/memoqservices/2007")]
    public partial class LightResourceListFilter : object
    {
        
        private string LanguageCodeField;
        
        private string NameOrDescriptionField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LanguageCode
        {
            get
            {
                return this.LanguageCodeField;
            }
            set
            {
                this.LanguageCodeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NameOrDescription
        {
            get
            {
                return this.NameOrDescriptionField;
            }
            set
            {
                this.NameOrDescriptionField = value;
            }
        }
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="http://kilgray.com/memoqservices/2007", ConfigurationName="IResourceService")]
public interface IResourceService
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/CreateAndPublish", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/CreateAndPublishResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/CreateAndPublishUnexpected" +
        "FaultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/CreateAndPublishGenericFau" +
        "ltFault", Name="GenericFault")]
    System.Threading.Tasks.Task<System.Guid> CreateAndPublishAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceInfo resourceInfo);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/GetResourceInfo", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/GetResourceInfoResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/GetResourceInfoUnexpectedF" +
        "aultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/GetResourceInfoGenericFaul" +
        "tFault", Name="GenericFault")]
    System.Threading.Tasks.Task<MQS.Resource.LightResourceInfo> GetResourceInfoAsync(MQS.Resource.ResourceType resourceType, System.Guid resourceGuid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/DeleteResource", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/DeleteResourceResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/DeleteResourceUnexpectedFa" +
        "ultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/DeleteResourceGenericFault" +
        "Fault", Name="GenericFault")]
    System.Threading.Tasks.Task DeleteResourceAsync(MQS.Resource.ResourceType resourceType, System.Guid resourceGuid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/CloneAndPublish", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/CloneAndPublishResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/CloneAndPublishUnexpectedF" +
        "aultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/CloneAndPublishGenericFaul" +
        "tFault", Name="GenericFault")]
    System.Threading.Tasks.Task<System.Guid> CloneAndPublishAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceInfo resourceInfo);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/UpdateResourceProperties", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/UpdateResourcePropertiesRe" +
        "sponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/UpdateResourcePropertiesUn" +
        "expectedFaultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/UpdateResourcePropertiesGe" +
        "nericFaultFault", Name="GenericFault")]
    System.Threading.Tasks.Task UpdateResourcePropertiesAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceInfo resourceInfo);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/ListResources", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/ListResourcesResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/ListResourcesUnexpectedFau" +
        "ltFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/ListResourcesGenericFaultF" +
        "ault", Name="GenericFault")]
    System.Threading.Tasks.Task<MQS.Resource.LightResourceInfo[]> ListResourcesAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceListFilter filter);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/ImportNewAndPublish", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/ImportNewAndPublishRespons" +
        "e")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/ImportNewAndPublishUnexpec" +
        "tedFaultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/ImportNewAndPublishGeneric" +
        "FaultFault", Name="GenericFault")]
    System.Threading.Tasks.Task<System.Guid> ImportNewAndPublishAsync(MQS.Resource.ResourceType resourceType, System.Guid fileGuid, MQS.Resource.LightResourceInfo resourceInfo);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/ExportResource", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/ExportResourceResponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/ExportResourceUnexpectedFa" +
        "ultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/ExportResourceGenericFault" +
        "Fault", Name="GenericFault")]
    System.Threading.Tasks.Task<System.Guid> ExportResourceAsync(MQS.Resource.ResourceType resourceType, System.Guid resourceGuid);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://kilgray.com/memoqservices/2007/IResourceService/GetGuidOfDefaultresource_", ReplyAction="http://kilgray.com/memoqservices/2007/IResourceService/GetGuidOfDefaultresource_R" +
        "esponse")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.UnexpectedFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/GetGuidOfDefaultresource_U" +
        "nexpectedFaultFault", Name="UnexpectedFault")]
    [System.ServiceModel.FaultContractAttribute(typeof(MQS.Resource.GenericFault), Action="http://kilgray.com/memoqservices/2007/IResourceService/GetGuidOfDefaultresource_G" +
        "enericFaultFault", Name="GenericFault")]
    System.Threading.Tasks.Task<System.Guid> GetGuidOfDefaultresource_Async(MQS.Resource.ResourceType resourceType, string objectId);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public interface IResourceServiceChannel : IResourceService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public partial class ResourceServiceClient : System.ServiceModel.ClientBase<IResourceService>, IResourceService
{
    
    public ResourceServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public System.Threading.Tasks.Task<System.Guid> CreateAndPublishAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceInfo resourceInfo)
    {
        return base.Channel.CreateAndPublishAsync(resourceType, resourceInfo);
    }
    
    public System.Threading.Tasks.Task<MQS.Resource.LightResourceInfo> GetResourceInfoAsync(MQS.Resource.ResourceType resourceType, System.Guid resourceGuid)
    {
        return base.Channel.GetResourceInfoAsync(resourceType, resourceGuid);
    }
    
    public System.Threading.Tasks.Task DeleteResourceAsync(MQS.Resource.ResourceType resourceType, System.Guid resourceGuid)
    {
        return base.Channel.DeleteResourceAsync(resourceType, resourceGuid);
    }
    
    public System.Threading.Tasks.Task<System.Guid> CloneAndPublishAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceInfo resourceInfo)
    {
        return base.Channel.CloneAndPublishAsync(resourceType, resourceInfo);
    }
    
    public System.Threading.Tasks.Task UpdateResourcePropertiesAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceInfo resourceInfo)
    {
        return base.Channel.UpdateResourcePropertiesAsync(resourceType, resourceInfo);
    }
    
    public System.Threading.Tasks.Task<MQS.Resource.LightResourceInfo[]> ListResourcesAsync(MQS.Resource.ResourceType resourceType, MQS.Resource.LightResourceListFilter filter)
    {
        return base.Channel.ListResourcesAsync(resourceType, filter);
    }
    
    public System.Threading.Tasks.Task<System.Guid> ImportNewAndPublishAsync(MQS.Resource.ResourceType resourceType, System.Guid fileGuid, MQS.Resource.LightResourceInfo resourceInfo)
    {
        return base.Channel.ImportNewAndPublishAsync(resourceType, fileGuid, resourceInfo);
    }
    
    public System.Threading.Tasks.Task<System.Guid> ExportResourceAsync(MQS.Resource.ResourceType resourceType, System.Guid resourceGuid)
    {
        return base.Channel.ExportResourceAsync(resourceType, resourceGuid);
    }
    
    public System.Threading.Tasks.Task<System.Guid> GetGuidOfDefaultresource_Async(MQS.Resource.ResourceType resourceType, string objectId)
    {
        return base.Channel.GetGuidOfDefaultresource_Async(resourceType, objectId);
    }
    
    public virtual System.Threading.Tasks.Task OpenAsync()
    {
        return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
    }
}
