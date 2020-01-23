//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="urn:aggateway:names:ws:docexchange", ConfigurationName="EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExcha" +
        "ngePortType")]
    public interface DocExchangePortType
    {
        
        // CODEGEN: Generating message contract since the operation execute is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="execute", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute()]
        EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeResponse execute(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="execute", ReplyAction="*")]
        System.Threading.Tasks.Task<EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeResponse> executeAsync(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:aggateway:names:ws:docexchange")]
    public partial class inboundData
    {
        
        private string businessProcessField;
        
        private string processStepField;
        
        private string partnerIdField;
        
        private string partnerTypeField;
        
        private string conversationIdField;
        
        private string messageIdField;
        
        private System.Xml.XmlElement xmlPayloadField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=0)]
        public string businessProcess
        {
            get
            {
                return this.businessProcessField;
            }
            set
            {
                this.businessProcessField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=1)]
        public string processStep
        {
            get
            {
                return this.processStepField;
            }
            set
            {
                this.processStepField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=2)]
        public string partnerId
        {
            get
            {
                return this.partnerIdField;
            }
            set
            {
                this.partnerIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=3)]
        public string partnerType
        {
            get
            {
                return this.partnerTypeField;
            }
            set
            {
                this.partnerTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=4)]
        public string conversationId
        {
            get
            {
                return this.conversationIdField;
            }
            set
            {
                this.conversationIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=5)]
        public string messageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public System.Xml.XmlElement xmlPayload
        {
            get
            {
                return this.xmlPayloadField;
            }
            set
            {
                this.xmlPayloadField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("svcutil", "4.7.3081.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:aggateway:names:ws:docexchange")]
    public partial class outboundData
    {
        
        private string processStepField;
        
        private string messageIdField;
        
        private System.Xml.XmlElement[] xmlPayloadField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=0)]
        public string processStep
        {
            get
            {
                return this.processStepField;
            }
            set
            {
                this.processStepField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="token", Order=1)]
        public string messageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("xmlPayload", Order=2)]
        public System.Xml.XmlElement[] xmlPayload
        {
            get
            {
                return this.xmlPayloadField;
            }
            set
            {
                this.xmlPayloadField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class executeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="urn:aggateway:names:ws:docexchange", Order=0)]
        public EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.inboundData inboundData;
        
        public executeRequest()
        {
        }
        
        public executeRequest(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.inboundData inboundData)
        {
            this.inboundData = inboundData;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class executeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="outboundData", Order=0)]
        public EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.outboundData result;
        
        public executeResponse()
        {
        }
        
        public executeResponse(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.outboundData result)
        {
            this.result = result;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DocExchangePortTypeChannel : EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExchangePortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DocExchangePortTypeClient : System.ServiceModel.ClientBase<EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExchangePortType>, EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExchangePortType
    {
        
        public DocExchangePortTypeClient()
        {
        }
        
        public DocExchangePortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName)
        {
        }
        
        public DocExchangePortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public DocExchangePortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress)
        {
        }
        
        public DocExchangePortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeResponse EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExchangePortType.execute(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest request)
        {
            return base.Channel.execute(request);
        }
        
        public EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.outboundData execute(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.inboundData inboundData)
        {
            EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest inValue = new EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest();
            inValue.inboundData = inboundData;
            EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeResponse retVal = ((EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExchangePortType)(this)).execute(inValue);
            return retVal.result;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeResponse> EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExchangePortType.executeAsync(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest request)
        {
            return base.Channel.executeAsync(request);
        }
        
        public System.Threading.Tasks.Task<EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeResponse> executeAsync(EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.inboundData inboundData)
        {
            EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest inValue = new EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.executeRequest();
            inValue.inboundData = inboundData;
            return ((EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract.DocExchangePortType)(this)).executeAsync(inValue);
        }
    }
}