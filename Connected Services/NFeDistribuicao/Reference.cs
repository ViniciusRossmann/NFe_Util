﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DFe_Util_HM.NFeDistribuicao {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe", ConfigurationName="NFeDistribuicao.NFeDistribuicaoDFeSoap")]
    public interface NFeDistribuicaoDFeSoap {
        
        // CODEGEN: Gerando contrato de mensagem porque o nome do elemento nfeDadosMsg no namespace http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe não está marcado como nulo
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe/nfeDistDFeInteresse", ReplyAction="*")]
        DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponse nfeDistDFeInteresse(DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe/nfeDistDFeInteresse", ReplyAction="*")]
        System.Threading.Tasks.Task<DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponse> nfeDistDFeInteresseAsync(DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeDistDFeInteresseRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="nfeDistDFeInteresse", Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe", Order=0)]
        public DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequestBody Body;
        
        public nfeDistDFeInteresseRequest() {
        }
        
        public nfeDistDFeInteresseRequest(DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe")]
    public partial class nfeDistDFeInteresseRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement nfeDadosMsg;
        
        public nfeDistDFeInteresseRequestBody() {
        }
        
        public nfeDistDFeInteresseRequestBody(System.Xml.Linq.XElement nfeDadosMsg) {
            this.nfeDadosMsg = nfeDadosMsg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class nfeDistDFeInteresseResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="nfeDistDFeInteresseResponse", Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe", Order=0)]
        public DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponseBody Body;
        
        public nfeDistDFeInteresseResponse() {
        }
        
        public nfeDistDFeInteresseResponse(DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.portalfiscal.inf.br/nfe/wsdl/NFeDistribuicaoDFe")]
    public partial class nfeDistDFeInteresseResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement nfeDistDFeInteresseResult;
        
        public nfeDistDFeInteresseResponseBody() {
        }
        
        public nfeDistDFeInteresseResponseBody(System.Xml.Linq.XElement nfeDistDFeInteresseResult) {
            this.nfeDistDFeInteresseResult = nfeDistDFeInteresseResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface NFeDistribuicaoDFeSoapChannel : DFe_Util_HM.NFeDistribuicao.NFeDistribuicaoDFeSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NFeDistribuicaoDFeSoapClient : System.ServiceModel.ClientBase<DFe_Util_HM.NFeDistribuicao.NFeDistribuicaoDFeSoap>, DFe_Util_HM.NFeDistribuicao.NFeDistribuicaoDFeSoap {
        
        public NFeDistribuicaoDFeSoapClient() {
        }
        
        public NFeDistribuicaoDFeSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NFeDistribuicaoDFeSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeDistribuicaoDFeSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NFeDistribuicaoDFeSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponse DFe_Util_HM.NFeDistribuicao.NFeDistribuicaoDFeSoap.nfeDistDFeInteresse(DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest request) {
            return base.Channel.nfeDistDFeInteresse(request);
        }
        
        public System.Xml.Linq.XElement nfeDistDFeInteresse(System.Xml.Linq.XElement nfeDadosMsg) {
            DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest inValue = new DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest();
            inValue.Body = new DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequestBody();
            inValue.Body.nfeDadosMsg = nfeDadosMsg;
            DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponse retVal = ((DFe_Util_HM.NFeDistribuicao.NFeDistribuicaoDFeSoap)(this)).nfeDistDFeInteresse(inValue);
            return retVal.Body.nfeDistDFeInteresseResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponse> DFe_Util_HM.NFeDistribuicao.NFeDistribuicaoDFeSoap.nfeDistDFeInteresseAsync(DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest request) {
            return base.Channel.nfeDistDFeInteresseAsync(request);
        }
        
        public System.Threading.Tasks.Task<DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseResponse> nfeDistDFeInteresseAsync(System.Xml.Linq.XElement nfeDadosMsg) {
            DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest inValue = new DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequest();
            inValue.Body = new DFe_Util_HM.NFeDistribuicao.nfeDistDFeInteresseRequestBody();
            inValue.Body.nfeDadosMsg = nfeDadosMsg;
            return ((DFe_Util_HM.NFeDistribuicao.NFeDistribuicaoDFeSoap)(this)).nfeDistDFeInteresseAsync(inValue);
        }
    }
}
