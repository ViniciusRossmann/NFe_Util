﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DFe_Util_HM.NFeInutilizacao4 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4", ConfigurationName="NFeInutilizacao4.NFeInutilizacao4Soap12")]
    public interface NFeInutilizacao4Soap12 {
        
        // CODEGEN: Gerando contrato de mensagem porque a operação nfeInutilizacaoNF não é RPC nem documento codificado.
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4/nfeInutilizacaoNF", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFResponse nfeInutilizacaoNF(DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4/nfeInutilizacaoNF", ReplyAction="*")]
        System.Threading.Tasks.Task<DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFResponse> nfeInutilizacaoNFAsync(DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeInutilizacaoNFRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4", Order=0)]
        public System.Xml.XmlNode nfeDadosMsg;
        
        public nfeInutilizacaoNFRequest() {
        }
        
        public nfeInutilizacaoNFRequest(System.Xml.XmlNode nfeDadosMsg) {
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeInutilizacaoNFResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4", Order=0)]
        public System.Xml.XmlNode nfeResultMsg;
        
        public nfeInutilizacaoNFResponse() {
        }
        
        public nfeInutilizacaoNFResponse(System.Xml.XmlNode nfeResultMsg) {
            this.nfeResultMsg = nfeResultMsg;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NFeInutilizacao4Soap12Channel : DFe_Util_HM.NFeInutilizacao4.NFeInutilizacao4Soap12, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NFeInutilizacao4Soap12Client : System.ServiceModel.ClientBase<DFe_Util_HM.NFeInutilizacao4.NFeInutilizacao4Soap12>, DFe_Util_HM.NFeInutilizacao4.NFeInutilizacao4Soap12 {
        
        public NFeInutilizacao4Soap12Client() {
        }
        
        public NFeInutilizacao4Soap12Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NFeInutilizacao4Soap12Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeInutilizacao4Soap12Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeInutilizacao4Soap12Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFResponse DFe_Util_HM.NFeInutilizacao4.NFeInutilizacao4Soap12.nfeInutilizacaoNF(DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest request) {
            return base.Channel.nfeInutilizacaoNF(request);
        }
        
        public System.Xml.XmlNode nfeInutilizacaoNF(System.Xml.XmlNode nfeDadosMsg) {
            DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest inValue = new DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFResponse retVal = ((DFe_Util_HM.NFeInutilizacao4.NFeInutilizacao4Soap12)(this)).nfeInutilizacaoNF(inValue);
            return retVal.nfeResultMsg;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFResponse> DFe_Util_HM.NFeInutilizacao4.NFeInutilizacao4Soap12.nfeInutilizacaoNFAsync(DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest request) {
            return base.Channel.nfeInutilizacaoNFAsync(request);
        }
        
        public System.Threading.Tasks.Task<DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFResponse> nfeInutilizacaoNFAsync(System.Xml.XmlNode nfeDadosMsg) {
            DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest inValue = new DFe_Util_HM.NFeInutilizacao4.nfeInutilizacaoNFRequest();
            inValue.nfeDadosMsg = nfeDadosMsg;
            return ((DFe_Util_HM.NFeInutilizacao4.NFeInutilizacao4Soap12)(this)).nfeInutilizacaoNFAsync(inValue);
        }
    }
}