﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DFe_Util_HM.NFeConsultaProtocolo4 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4", ConfigurationName="NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12")]
    public interface NFeConsultaProtocolo4Soap12 {
        
        // CODEGEN: Gerando contrato de mensagem porque a operação nfeConsultaNF não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4/nfeConsultaNF", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFResponse nfeConsultaNF(DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4/nfeConsultaNF", ReplyAction="*")]
        System.Threading.Tasks.Task<DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFResponse> nfeConsultaNFAsync(DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeConsultaNFRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4", Order=0)]
        public System.Xml.XmlNode nfeDadosMsg;
        
        public nfeConsultaNFRequest() {
        }
        
        public nfeConsultaNFRequest(System.Xml.XmlNode nfeDadosMsg) {
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeConsultaNFResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4", Order=0)]
        public System.Xml.XmlNode nfeResultMsg;
        
        public nfeConsultaNFResponse() {
        }
        
        public nfeConsultaNFResponse(System.Xml.XmlNode nfeResultMsg) {
            this.nfeResultMsg = nfeResultMsg;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NFeConsultaProtocolo4Soap12Channel : DFe_Util_HM.NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NFeConsultaProtocolo4Soap12Client : System.ServiceModel.ClientBase<DFe_Util_HM.NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12>, DFe_Util_HM.NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12 {
        
        public NFeConsultaProtocolo4Soap12Client() {
        }
        
        public NFeConsultaProtocolo4Soap12Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NFeConsultaProtocolo4Soap12Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeConsultaProtocolo4Soap12Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeConsultaProtocolo4Soap12Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFResponse DFe_Util_HM.NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12.nfeConsultaNF(DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest request) {
            return base.Channel.nfeConsultaNF(request);
        }
        
        public System.Xml.XmlNode nfeConsultaNF(System.Xml.XmlNode nfeDadosMsg) {
            DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest inValue = new DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFResponse retVal = ((DFe_Util_HM.NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12)(this)).nfeConsultaNF(inValue);
            return retVal.nfeResultMsg;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFResponse> DFe_Util_HM.NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12.nfeConsultaNFAsync(DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest request) {
            return base.Channel.nfeConsultaNFAsync(request);
        }
        
        public System.Threading.Tasks.Task<DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFResponse> nfeConsultaNFAsync(System.Xml.XmlNode nfeDadosMsg) {
            DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest inValue = new DFe_Util_HM.NFeConsultaProtocolo4.nfeConsultaNFRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            return ((DFe_Util_HM.NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12)(this)).nfeConsultaNFAsync(inValue);
        }
    }
}