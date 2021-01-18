/*
    Funções da DLL NFe_Util
    Autor: Vinícius Rossmann Nunes
    Ultima modificação: outubro/2020 - implementado
*/

using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Reflection;
using System.Net;
using System.ServiceModel.Channels;
using System.Xml.Serialization;

namespace DFe_Util_HM
{
    [ComVisible(true)]
    [Guid("6560C426-3785-470E-8501-9E8E7D90926B")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IDFe_Util
    {
        [DispId(1)]
        bool GetNfeDistDfe(int ambiente, string pathSaida, string cnpj, int codUF, string nomeCertificado, ref string ult_nsu, ref string max_nsu, ref string msgRetorno, ref int codStatus, string schemaSalvo);
        [DispId(2)]
        bool inserirQrCode(string sxml, string idCsc, string csc, string versaoQRCode, string urlConsulta, string urlChave, ref string msgRetorno, ref string xmlRet);
        [DispId(3)]
        bool assinarNF(string sxml, string nomeCertificado, ref string msgRetorno, ref string xmlRet);
        [DispId(4)]
        int ValidaXML(string sXml, string nomeSchema, ref string msgRetorno);
        [DispId(5)]
        bool EnviaNFSync(string sxml, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string numProtocolo, ref string dhprotocolo, ref string xmlRet);
        [DispId(6)]
        bool InutilizaNumNF(string modelo, string serie, string numInicial, string numFinal, string justificativa, int ambiente, string nomeCertificado, string siglaWS, string cnpj, string ano, string codUF, string versao, ref string msgRetorno, ref int codStatus, ref string numProtocolo, ref string dataProtocolo, ref string xmlRet);
        [DispId(7)]
        bool CancelaNotaFiscal(string chaveNF, string protNF, string dhEvento, string justificativa, int ambiente, string nomeCertificado, string siglaWS, string versao, ref string msgRetorno, ref int codStatus, ref string xmlRet);
        [DispId(8)]
        bool StatusServicoNFe(string siglaWS, int ambiente, string nomeCertificado, string codUF, bool nfc, ref string msgRetorno, ref int codStatus, ref string xmlRet);
        [DispId(9)]
        bool EnviaNFAsync(string sxml, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string numRec, ref string dhprotocolo, ref string tempoMedio);
        [DispId(10)]
        bool RetAutorizacaoNF(string sxml, int ambiente, string nomeCertificado, string siglaWS, string numRecibo, ref int codStatus, ref string msgRetorno, ref string cMsg, ref string xMsg, ref string numProtocolo, ref string dataProtocolo, ref string xmlRet);
        [DispId(11)]
        bool ConsultaNF(string chaveNF, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string xmlRet);
        [DispId(12)]
        bool InfoCertificado(ref string nomeCertificado, ref string msgRetorno, ref string iniValidade, ref string fimValidade, ref string emissor, ref string titular, ref string cnpj);
        [DispId(13)]
        string GetNomeCertificado(string titulo, string subtitulo, ref string msgRetorno);
        [DispId(14)]
        bool EnviaCCe(string chaveNF, int numCorrecao, string dhEvento, string txtCorrecao, int ambiente, string nomeCertificado, string siglaWS, string versao, ref string msgRetorno, ref int codStatus, ref string xmlRet);
        [DispId(15)]
        string MontaLoteNF(string[] notas, string numLote, bool sincrono, bool gzip, ref string msgRetorno);
        [DispId(16)]
        bool EnviaLoteNF(string sLote, string modelo, bool gzip, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string numRec, ref string dhprotocolo, ref string tempoMedio, ref string xmlRet);
        [DispId(17)]
        bool BuscaLoteNF(int ambiente, bool nfc, string nomeCertificado, string siglaWS, string numRecibo, ref int codStatus, ref string msgRetorno, ref string cMsg, ref string xMsg, ref string xmlRet);
        [DispId(18)]
        bool EnviaManiDest(int ambiente, string cnpj, string nomeCertificado, string versao, string tipoEvento, string chaveNF, string justificativa, string dhEvento, ref string msgRetorno, ref int codStatus, ref string xmlRet);
        [DispId(19)]
        string DescompactaGzip(string strCompactada);
    }

    [ComVisible(true)]
    [Guid("ADDD32CC-B9B8-47AD-956F-14AA1B6F25A5")]
    [ClassInterface(ClassInterfaceType.None)]
    public class NFe_Util : IDFe_Util
    {
        //Função que obtem as notas fiscais emitidas para um determinado CNPJ
        public bool GetNfeDistDfe(int ambiente, string pathSaida, string cnpj, int codUF, string nomeCertificado, ref string ult_nsu, ref string max_nsu, ref string msgRetorno, ref int codStatus, string schemaSalvo)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //Gerando a requisição
                DistNFe.TAmb amb = ambiente == 1 ? DistNFe.TAmb.Item1 : DistNFe.TAmb.Item2;
                DistNFe.ItemChoiceType tipoPessoa;
                DistNFe.TCodUfIBGE ufIBGE;
                try
                {
                    ufIBGE = (DistNFe.TCodUfIBGE)Enum.Parse(typeof(DistNFe.TCodUfIBGE), "Item" + codUF);
                }
                catch
                {
                    msgRetorno = "Codigo de UF inválido. [" + codUF + "]";
                    return false;
                }
                if (cnpj.Length == 14) tipoPessoa = DistNFe.ItemChoiceType.CNPJ;
                else if (cnpj.Length == 11) tipoPessoa = DistNFe.ItemChoiceType.CPF;
                else
                {
                    msgRetorno = "CNPJ/CPF inválido. [" + cnpj + "]";
                    return false;
                }
                var distNsu = new DistNFe.distDFeIntDistNSU { ultNSU = ult_nsu };
                var distDfeInt = new DistNFe.distDFeInt { tpAmb = amb, cUFAutor = ufIBGE, ItemElementName = tipoPessoa, Item = cnpj, cUFAutorSpecified = true, Item1 = distNsu, versao = DistNFe.TVerDistDFe.Item101 };
                string str_req = serializar(distDfeInt, "http://www.portalfiscal.inf.br/nfe");
                XElement requisicao_xml = XElement.Parse(str_req);

                //Gerando o endpoint dependendo do ambiente (produção/homologação)
                Endpoints end = new Endpoints();
                string endereco = end.getUrlDistribuicao("NFe", ambiente);
                var endpoint = new EndpointAddress(endereco);

                //Gerando o binding com os atributos da conexão
                var binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeDistribuicao.NFeDistribuicaoDFeSoapClient(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var resposta_xml = cliente.nfeDistDFeInteresse(requisicao_xml);
                string xmlRet = resposta_xml.ToString();
                var resposta = (DistNFe.retDistDFeInt)deserializar(xmlRet, typeof(DistNFe.retDistDFeInt));

                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                ult_nsu = resposta.ultNSU;
                max_nsu = resposta.maxNSU;

                //Se encontrou documentos
                if (codStatus == 138)
                {
                    //percorre o resultado salvando os documentos
                    XmlDocument doc = new XmlDocument();
                    string esquema, pathArquivo;
                    var documentos = resposta.loteDistDFeInt.docZip;
                    foreach(DistNFe.retDistDFeIntLoteDistDFeIntDocZip documento in documentos)
                    {
                        try
                        {
                            doc.LoadXml(GzipDecode(documento.Value));
                            esquema = documento.schema.Split('_')[0];
                            if (schemaSalvo.Contains(esquema))
                            {
                                pathArquivo = pathSaida + "\\" + documento.NSU + "-" + esquema + ".xml";
                                FileStream destino_arquivo = File.Create(pathArquivo);
                                doc.PreserveWhitespace = true;
                                doc.Save(destino_arquivo);
                                destino_arquivo.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            msgRetorno = "Erro ao salvar documento n° " + documento.NSU + " : " + ex.Message;
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao buscar documentos. "+ procExcessao(ex);
                return false;
            }
        }

        //Função para inserir a tag xml com o qrcode em uma NFC-e
        public bool inserirQrCode(string sxml, string idCsc, string csc, string versaoQRCode, string urlConsulta, string urlChave, ref string msgRetorno, ref string xmlRet)
        {
            try
            {
                //Remove os zeros não significativos do idCsc
                idCsc = idCsc.TrimStart('0');

                //gera um objeto NFe a partir da string xml
                var NFe = (EnviNFe.TNFe)deserializar(sxml, typeof(EnviNFe.TNFe)); 

                //obtem os dados da nota necessarios para formar o qrcode
                string chavenota = NFe.infNFe.Id.Replace("NFe", "");
                string ambiente = NFe.infNFe.ide.tpAmb.ToString().Replace("Item", "");
                
                //gera o codigo hash
                string hash = chavenota + "|" + versaoQRCode + "|" + ambiente + "|" + idCsc + csc;
                hash = SHA1HashStringForUTF8String(hash);

                //gera o qrcode
                string qrcode = urlConsulta + "?p=";
                qrcode = qrcode + chavenota + "|" + versaoQRCode + "|" + ambiente + "|" + idCsc + "|" + hash;

                //insere as tags na NFe
                NFe.infNFeSupl = new EnviNFe.TNFeInfNFeSupl { qrCode = qrcode, urlChave = urlChave };

                //retorna os dados
                xmlRet = serializar(NFe, "http://www.portalfiscal.inf.br/nfe");
                msgRetorno = "QR code adicionado com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao inserir QR Code no xml. " + procExcessao(ex);
                return false;
            }
        }

        //Função para assinar uma nota fiscal
        public bool assinarNF(string sxml, string nomeCertificado, ref string msgRetorno, ref string xmlRet)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //obtendo a tag signature para o elemento infNFe
                string assinatura = getSignature(sxml, "infNFe", objCertificado);

                //gera um objeto NFe a partir da string xml
                var NFe = (EnviNFe.TNFe)deserializar(sxml, typeof(EnviNFe.TNFe));

                //insere a tag signature na NFe
                NFe.Signature = (EnviNFe.SignatureType)deserializar(assinatura, typeof(EnviNFe.SignatureType));

                //retorna os dados
                xmlRet = serializar(NFe, "http://www.portalfiscal.inf.br/nfe");
                msgRetorno = "Documento assinado com sucesso.";
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro na assinatura. "+ procExcessao(ex);
                return false;
            }
        }

        //Função para validar um documento xml através de um esquema
        public int ValidaXML(string sXml, string nomeSchema, ref string msgRetorno)
        {
            try
            {
                //gerando documento xml a partir da string
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXml);

                //criado o validador
                ValidacaoXML validadorXML = new ValidacaoXML();

                //gerando o caminho do schema
                string pathSchema = Assembly.GetExecutingAssembly().CodeBase.Replace("file:\\", "").ToUpper().Replace("DFE_UTIL_HM.DLL", "");
                pathSchema += "schemas\\" + nomeSchema + ".xsd";

                //fazendo a validação
                bool ok = validadorXML.ValidarXml(doc, pathSchema);

                //criado o retorno
                if (!ok) msgRetorno = validadorXML.Motivos;
                else msgRetorno = "Documento validado com sucesso.";
                return validadorXML.NumErros;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao executar validação. " + procExcessao(ex);
                return 1;
            }
        }

        //Função para transmitir uma nota fiscal para o SEFAZ de forma síncrona
        public bool EnviaNFSync(string sxml, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string numProtocolo, ref string dhprotocolo, ref string xmlRet)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //gera um objeto NFe a partir da string xml
                var NFe = (EnviNFe.TNFe)deserializar(sxml, typeof(EnviNFe.TNFe));

                //obtem as informações necessarias da nota a ser emitida
                string modNota = NFe.infNFe.ide.mod.ToString().Replace("Item", "");
                string serieNota = NFe.infNFe.ide.serie;
                string numNota = NFe.infNFe.ide.nNF;

                //gera o numero do lote a partir do modelo, serie e numero da nota
                string modSerie = modNota + "000".Remove(0, serieNota.Length) + serieNota;
                string numLote = modSerie + "00000000000000".Remove(0, (modSerie.Length + numNota.Length)) + numNota;

                //verifica se é nfc (modelo 65)
                bool nfc = modNota.Equals("65") ? true : false;

                //monta o lote para emissão
                EnviNFe.TNFe[] notas = { NFe };
                var enviNFe = new EnviNFe.TEnviNFe { versao = "4.00", idLote = numLote, NFe = notas, indSinc = EnviNFe.TEnviNFeIndSinc.Item1 };
                string str_req = serializar(enviNFe, "http://www.portalfiscal.inf.br/nfe");
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "Autorizacao", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada.";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeAutorizacao4.NFeAutorizacao4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeAutorizacaoLote(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (EnviNFe.TRetEnviNFe)deserializar(xmlRet, typeof(EnviNFe.TRetEnviNFe));

                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                dhprotocolo = resposta.dhRecbto;

                //se o lote não foi processado
                if (codStatus != 104)
                {
                    //em processamento
                    if (codStatus == 105)
                    {
                        var infRec = (EnviNFe.TRetEnviNFeInfRec) resposta.Item;
                        numProtocolo = infRec.nRec;
                        dhprotocolo = infRec.tMed; //tempo medio de processamento (segundos)
                        return true;
                    }
                    //não foi processado
                    else return false;
                }

                var protNFe = (EnviNFe.TProtNFe) resposta.Item;
                msgRetorno = protNFe.infProt.xMotivo;
                codStatus = int.Parse(protNFe.infProt.cStat);
                dhprotocolo = protNFe.infProt.dhRecbto;

                //se não foi autorizada
                if (codStatus != 100 && codStatus != 150 && codStatus != 110) return false;

                //pega o numero do protocolo
                numProtocolo = protNFe.infProt.nProt;

                //gera o nfeProc
                string str_prot = serializar(protNFe, "http://www.portalfiscal.inf.br/nfe");
                var nfep = (ProcNFe.TNFe)deserializar(sxml, typeof(ProcNFe.TNFe));
                var protp = (ProcNFe.TProtNFe)deserializar(str_prot, typeof(ProcNFe.TProtNFe));
                var nfeProc = new ProcNFe.TNfeProc { versao = "4.00", NFe = nfep, protNFe = protp };

                //retorna os dados
                xmlRet = serializar(nfeProc, "http://www.portalfiscal.inf.br/nfe");
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao transmitir nota. " + procExcessao(ex);
                return false;
            }
        }

        //Função para inutilizar uma faixa de numeros de nota fiscal
        public bool InutilizaNumNF(string modelo, string serie, string numInicial, string numFinal, string justificativa, int ambiente, string nomeCertificado, string siglaWS, string cnpj, string ano, string codUF, string versao, ref string msgRetorno, ref int codStatus, ref string numProtocolo, ref string dataProtocolo, ref string xmlRet)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //verifica se é NFC-e (modelo 65)
                bool nfc = modelo.Equals("65") ? true : false;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "Inutilizacao", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada.";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //Gerando xml da requisição
                InutNFe.TAmb amb = ambiente == 1 ? InutNFe.TAmb.Item1 : InutNFe.TAmb.Item2;
                InutNFe.TCodUfIBGE ufIBGE;
                InutNFe.TMod mod;
                try
                {
                    ufIBGE = (InutNFe.TCodUfIBGE)Enum.Parse(typeof(InutNFe.TCodUfIBGE), "Item" + codUF);
                }
                catch
                {
                    msgRetorno = "Codigo de UF inválido. ["+codUF+"]";
                    return false;
                }
                try
                {
                    mod = (InutNFe.TMod)Enum.Parse(typeof(InutNFe.TMod), "Item"+modelo);
                }
                catch
                {
                    msgRetorno = "Modelo inválido. [" + modelo + "]";
                    return false;
                }
                ano = ano.Substring(ano.Length-2);
                string idIni = "000000000".Remove(0, numInicial.Length) + numInicial;
                string idFim = "000000000".Remove(0, numFinal.Length) + numFinal;
                string modSerie = modelo + "000".Remove(0, serie.Length) + serie;
                string id = "ID"+codUF+ano+cnpj+modSerie+idIni+idFim;
                var infInut = new InutNFe.TInutNFeInfInut { Id=id, tpAmb = amb, ano = ano, CNPJ=cnpj, cUF=ufIBGE, mod=mod, serie=serie, nNFIni=numInicial, nNFFin=numFinal, xJust=justificativa, xServ= InutNFe.TInutNFeInfInutXServ.INUTILIZAR };
                var inutNFe = new InutNFe.TInutNFe { infInut = infInut, versao = "4.00",  };
                
                //assina a requisição
                string assinatura = getSignature(serializar(inutNFe, "http://www.portalfiscal.inf.br/nfe"), "infInut", objCertificado);
                inutNFe.Signature = (InutNFe.SignatureType)deserializar(assinatura, typeof(InutNFe.SignatureType));
                
                //validando a requisição
                string erros = "";
                string str_req = serializar(inutNFe, "http://www.portalfiscal.inf.br/nfe");
                int numErros = ValidaXML(str_req, "inutNFe_v4.00", ref erros);
                if (numErros > 0)
                {
                    msgRetorno = "Erro no xml da requisição. "+erros;
                    return false;
                }

                //colocando a requisição no formato XmlNode
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeInutilizacao4.NFeInutilizacao4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeInutilizacaoNF(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (InutNFe.TRetInutNFe)deserializar(xmlRet, typeof(InutNFe.TRetInutNFe));
                
                msgRetorno = resposta.infInut.xMotivo;
                codStatus = int.Parse(resposta.infInut.cStat);

                //se não foi homologado
                if (codStatus != 102) return false;

                //retorna os valores
                numProtocolo = resposta.infInut.nProt;
                dataProtocolo = resposta.infInut.dhRecbto;
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao inutilizar. " + procExcessao(ex);
                return false;
            }
        }

        //função para cancelar uma nota fiscal
        public bool CancelaNotaFiscal(string chaveNF, string protNF, string dhEvento, string justificativa, int ambiente, string nomeCertificado, string siglaWS, string versao, ref string msgRetorno, ref int codStatus, ref string xmlRet)
        {
            try
            {
                if (chaveNF.Length != 44)
                {
                    msgRetorno = "A chave de acesso deve ter 44 posições.";
                    return false;
                }

                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //verifica se é NFC-e (Modelo 65)
                string modNota = chaveNF.Substring(20, 2);
                bool nfc = modNota.Equals("65") ? true : false;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "Evento", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada. [" + siglaWS + "]";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //criando a requisição
                string idlote = chaveNF.Substring(25, 12) + "01";
                string cnpj = chaveNF.Substring(6, 14);
                string numOrgao = chaveNF.Substring(0, 2);
                if ((siglaWS.ToUpper().Equals("SVAN")) || (siglaWS.ToUpper().Equals("SVC-AN")))
                {
                    numOrgao = "91";
                }

                EnvEvCanc.TCOrgaoIBGE orgaoIBGE;
                try
                {
                    orgaoIBGE = (EnvEvCanc.TCOrgaoIBGE)Enum.Parse(typeof(EnvEvCanc.TCOrgaoIBGE), "Item" + numOrgao);
                }
                catch
                {
                    msgRetorno = "Codigo do órgão de recepção inválido. [" + numOrgao + "]";
                    return false;
                }
                EnvEvCanc.TAmb amb = ambiente == 1 ? EnvEvCanc.TAmb.Item1 : EnvEvCanc.TAmb.Item2;
                string idEvento = "ID" + "110111" + chaveNF + "01";

                var detEvento = new EnvEvCanc.TEventoInfEventoDetEvento { descEvento = EnvEvCanc.TEventoInfEventoDetEventoDescEvento.Cancelamento, nProt = protNF, xJust = justificativa, versao = EnvEvCanc.TEventoInfEventoDetEventoVersao.Item100 };
                var infEvento = new EnvEvCanc.TEventoInfEvento { chNFe = chaveNF, cOrgao = orgaoIBGE, dhEvento = dhEvento, tpAmb = amb, tpEvento = EnvEvCanc.TEventoInfEventoTpEvento.Item110111, nSeqEvento = "1", verEvento = EnvEvCanc.TEventoInfEventoVerEvento.Item100, Id = idEvento, detEvento = detEvento, ItemElementName = EnvEvCanc.ItemChoiceType.CNPJ, Item = cnpj };
                var evento = new EnvEvCanc.TEvento { versao = versao, infEvento = infEvento };
                string assinatura = getSignature(serializar(evento, "http://www.portalfiscal.inf.br/nfe"), "infEvento", objCertificado);
                evento.Signature = (EnvEvCanc.SignatureType)deserializar(assinatura, typeof(EnvEvCanc.SignatureType));
                EnvEvCanc.TEvento[] eventos = { evento };
                var envEvento = new EnvEvCanc.TEnvEvento { versao = versao, evento = eventos, idLote = idlote };

                //convertendo a requisiçao para XmlNode
                string str_req = serializar(envEvento, "http://www.portalfiscal.inf.br/nfe");
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeEvento4.NFeRecepcaoEvento4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeRecepcaoEvento(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (EnvEvCanc.TRetEnvEvento)deserializar(xmlRet, typeof(EnvEvCanc.TRetEnvEvento));
                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                if (codStatus != 128)
                {
                    return false;
                }

                msgRetorno = resposta.retEvento[0].infEvento.xMotivo;
                codStatus = int.Parse(resposta.retEvento[0].infEvento.cStat);

                //se não foi aceito o cancelamento
                if (codStatus != 135 && codStatus != 155) return false;

                //monta o procEvento
                var  retEvento = resposta.retEvento[0];
                var procCanc = new EnvEvCanc.TProcEvento { evento = evento, versao = versao, retEvento = retEvento };
                xmlRet = serializar(procCanc, "http://www.portalfiscal.inf.br/nfe");

                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao cancelar nota. " + procExcessao(ex);
                return false;
            }
        }

        //Função para obter o status dos serviços do SEFAZ
        public bool StatusServicoNFe(string siglaWS, int ambiente, string nomeCertificado, string codUF, bool nfc, ref string msgRetorno, ref int codStatus, ref string xmlRet)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //gerando a consulta xml
                StatServ.TAmb amb = ambiente == 1 ? StatServ.TAmb.Item1 : StatServ.TAmb.Item2;
                StatServ.TCodUfIBGE ufIBGE;
                try { ufIBGE = (StatServ.TCodUfIBGE)Enum.Parse(typeof(StatServ.TCodUfIBGE), "Item" + codUF); }
                catch
                {
                    msgRetorno = "Codigo de UF inválido. [" + codUF + "]";
                    return false;
                }
                var consulta = new StatServ.TConsStatServ { cUF = ufIBGE, tpAmb = amb, versao = "4.00", xServ = StatServ.TConsStatServXServ.STATUS };

                //convertendo a requisiçao para XmlNode
                string str_req = serializar(consulta, "http://www.portalfiscal.inf.br/nfe");
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "StatusServico", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada. [" + siglaWS + "]";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeStatusServico4.NFeStatusServico4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeStatusServicoNF(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (StatServ.TRetConsStatServ)deserializar(xmlRet, typeof(StatServ.TRetConsStatServ));
                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao verificar status do serviço. " + procExcessao(ex);
                return false;
            }
        }

        //Função para transimitir uma nota fiscal para o SEFAZ de forma assíncrona
        public bool EnviaNFAsync(string sxml, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string numRec, ref string dhprotocolo, ref string tempoMedio)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //gera um objeto NFe a partir da string xml
                var NFe = (EnviNFe.TNFe)deserializar(sxml, typeof(EnviNFe.TNFe));

                //obtem as informações necessarias da nota a ser emitida
                string modNota = NFe.infNFe.ide.mod.ToString().Replace("Item", "");
                string serieNota = NFe.infNFe.ide.serie;
                string numNota = NFe.infNFe.ide.nNF;

                //gera o numero do lote a partir do modelo, serie e numero da nota
                string modSerie = modNota + "000".Remove(0, serieNota.Length) + serieNota;
                string numLote = modSerie + "00000000000000".Remove(0, (modSerie.Length + numNota.Length)) + numNota;

                //verifica se é nfc (modelo 65)
                bool nfc = modNota.Equals("65") ? true : false;

                //monta o lote para emissão
                EnviNFe.TNFe[] notas = { NFe };
                var enviNFe = new EnviNFe.TEnviNFe { versao = "4.00", idLote = numLote, NFe = notas, indSinc = EnviNFe.TEnviNFeIndSinc.Item0 };
                string str_req = serializar(enviNFe, "http://www.portalfiscal.inf.br/nfe");
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "Autorizacao", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada.";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeAutorizacao4.NFeAutorizacao4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeAutorizacaoLote(requisicao_xml);
                string xmlRet = nodeRes.OuterXml;
                var resposta = (EnviNFe.TRetEnviNFe)deserializar(xmlRet, typeof(EnviNFe.TRetEnviNFe));

                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                dhprotocolo = resposta.dhRecbto;

                //em processamento
                if (codStatus == 103)
                {
                    var infRec = (EnviNFe.TRetEnviNFeInfRec)resposta.Item;
                    numRec = infRec.nRec;
                    tempoMedio = infRec.tMed;
                    return true;
                }
                //Não foi aceito
                else return false;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao transmitir nota. " + procExcessao(ex);
                return false;
            }
        }

        //Função para obter o retorno da emissão da NF. Utilizada em conjunto com EnviaNFAsync
        public bool RetAutorizacaoNF(string sxml, int ambiente, string nomeCertificado, string siglaWS, string numRecibo, ref int codStatus, ref string msgRetorno, ref string cMsg, ref string xMsg, ref string numProtocolo, ref string dataProtocolo, ref string xmlRet)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //gera um objeto NFe a partir da string xml
                var NFe = (ProcNFe.TNFe)deserializar(sxml, typeof(ProcNFe.TNFe));

                //verifica se é nfc (modelo 65)
                string modNota = NFe.infNFe.ide.mod.ToString().Replace("Item", "");
                bool nfc = modNota.Equals("65") ? true : false;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "RetAutorizacao", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada. [" + siglaWS + "]";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //gerando a requisição do serviço
                ConsReciNFe.TAmb amb = ambiente == 1 ? ConsReciNFe.TAmb.Item1 : ConsReciNFe.TAmb.Item2;
                var consReciNFe = new ConsReciNFe.TConsReciNFe { versao = "4.00", tpAmb = amb, nRec = numRecibo };
                string str_req = serializar(consReciNFe, "http://www.portalfiscal.inf.br/nfe");
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeRetAutorizacao4.NFeRetAutorizacao4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeRetAutorizacaoLote(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (ConsReciNFe.TRetConsReciNFe)deserializar(xmlRet, typeof(ConsReciNFe.TRetConsReciNFe));

                codStatus = int.Parse(resposta.cStat);
                msgRetorno = resposta.xMotivo;
                try
                {
                    xMsg = resposta.xMsg;
                    cMsg = resposta.cMsg;
                } catch { }

                //se não foi processado
                if (codStatus != 104) return false;

                var protNFe = resposta.protNFe[0]; //ATENÇÃO: para o correto funcionamento o lote deve ser de apenas 1 nota
                msgRetorno = protNFe.infProt.xMotivo;
                codStatus = int.Parse(protNFe.infProt.cStat);

                //se não foi autorizada
                if (codStatus != 100 && codStatus != 150 && codStatus != 110) return false;

                numProtocolo = protNFe.infProt.nProt;
                dataProtocolo = protNFe.infProt.dhRecbto;

                //gera o NFeProc
                string str_prot = serializar(protNFe, "http://www.portalfiscal.inf.br/nfe");
                var protp = (ProcNFe.TProtNFe)deserializar(str_prot, typeof(ProcNFe.TProtNFe));
                var nfeProc = new ProcNFe.TNfeProc { versao = "4.00", NFe = NFe, protNFe = protp };

                //retorna os dados
                xmlRet = serializar(nfeProc, "http://www.portalfiscal.inf.br/nfe");
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao buscar resultado do processamento. " + procExcessao(ex);
                return false;
            }
        }

        //Função para consultar o status de uma NF no SEFAZ
        public bool ConsultaNF(string chaveNF, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string xmlRet)
        {
            try
            {
                if (chaveNF.Length != 44)
                {
                    msgRetorno = "A chave de acesso deve ter 44 posições.";
                    return false;
                }

                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //verifica se é NFC-e (Modelo 65)
                string modNota = chaveNF.Substring(20, 2);
                msgRetorno = modNota;
                bool nfc = modNota.Equals("65") ? true : false;

                //gerando a consulta xml
                ConsSitNFe.TAmb amb = ambiente == 1 ? ConsSitNFe.TAmb.Item1 : ConsSitNFe.TAmb.Item2;
                var consulta = new ConsSitNFe.TConsSitNFe { chNFe = chaveNF, tpAmb = amb, xServ = ConsSitNFe.TConsSitNFeXServ.CONSULTAR, versao = ConsSitNFe.TVerConsSitNFe.Item400 };

                //convertendo a requisiçao para XmlNode
                string str_req = serializar(consulta, "http://www.portalfiscal.inf.br/nfe");
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "Consulta", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada. [" + siglaWS + "]";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeConsultaProtocolo4.NFeConsultaProtocolo4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeConsultaNF(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (ConsSitNFe.TRetConsSitNFe)deserializar(xmlRet, typeof(ConsSitNFe.TRetConsSitNFe));
                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao consultar nota fiscal. " + procExcessao(ex);
                return false;
            }
        }

        //Função para obter as informações de um certificado digital
        public bool InfoCertificado(ref string nomeCertificado, ref string msgRetorno, ref  string iniValidade, ref string fimValidade, ref string emissor, ref string titular, ref string cnpj)
        {
            try
            {
                //caso o nome do certificado não seja passado
                if (nomeCertificado.Equals(""))
                {
                    nomeCertificado = GetNomeCertificado("Selecione um certificado", "Escolha um certificado para ser usado no sistema", ref msgRetorno);
                    if (nomeCertificado == null) return false;
                }

                //obtendo o objeto certificado a partir de seu nome
                var cert = getCertificado(nomeCertificado, ref msgRetorno);
                if (cert == null) return false;

                //obtendo as informações do certificado
                iniValidade = cert.GetEffectiveDateString();
                fimValidade = cert.GetExpirationDateString();
                emissor = cert.Issuer.Split(',')[0].Replace("CN=", "");
                string detentor = cert.SubjectName.Name.Split(',')[0];
                titular = detentor.Split(':')[0].Replace("CN=", "");
                cnpj = detentor.Split(':')[1];
                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erros aos buscar informações do certificado digital. " + procExcessao(ex);
                return false;
            }
        }

        //Função para obter o nome de um certificado digital escolhido pelo usuário
        public string GetNomeCertificado(string titulo, string subtitulo, ref string msgRetorno)
        {
            try
            {
                //cria uma lista com os certificados no repositorio do current user
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

                //mostra a janela de seleção com os certificados
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, titulo, subtitulo, X509SelectionFlag.SingleSelection);

                //Caso não tenha sido escolhido nenhum
                if (scollection.Count == 0)
                {
                    msgRetorno = "Nenhum certificado foi selecionado.";
                    return null;
                }

                //caso tenha sido escolhido um certificado
                else
                {
                    X509Certificate2 cert = scollection[0];
                    return cert.SubjectName.Name;
                }
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erros aos buscar certificado digital. " + procExcessao(ex);
                return null;
            }
        }

        //Função para transmitir uma carta de correção para uma nota fiscal
        public bool EnviaCCe(string chaveNF, int numCorrecao, string dhEvento, string txtCorrecao, int ambiente, string nomeCertificado, string siglaWS, string versao, ref string msgRetorno, ref int codStatus, ref string xmlRet)
        {
            try
            {
                if (chaveNF.Length != 44)
                {
                    msgRetorno = "A chave de acesso deve ter 44 posições.";
                    return false;
                }

                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //verifica se é NFC-e (Modelo 65)
                string modNota = chaveNF.Substring(20, 2);
                if (modNota.Equals("65"))
                {
                    msgRetorno = "A carta de correção é exclusiva para NF-e (modelo 55).";
                    return false;
                }

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "Evento", false);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada. [" + siglaWS + "]";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //criando a requisição
                string cnpj = chaveNF.Substring(6, 14);
                string numOrgao = chaveNF.Substring(0, 2);
                if ((siglaWS.ToUpper().Equals("SVAN")) || (siglaWS.ToUpper().Equals("SVC-AN")))
                {
                    numOrgao = "90";
                }

                EnvEvCCe.TCOrgaoIBGE orgaoIBGE;
                try
                {
                    orgaoIBGE = (EnvEvCCe.TCOrgaoIBGE)Enum.Parse(typeof(EnvEvCCe.TCOrgaoIBGE), "Item" + numOrgao);
                }
                catch
                {
                    msgRetorno = "Codigo do órgão de recepção inválido. [" + numOrgao + "]";
                    return false;
                }
                EnvEvCCe.TAmb amb = ambiente == 1 ? EnvEvCCe.TAmb.Item1 : EnvEvCCe.TAmb.Item2;
                string sNumSeq = numCorrecao >= 10 ? numCorrecao.ToString() : "0" + numCorrecao.ToString();
                string idlote = chaveNF.Substring(25, 12) + sNumSeq;
                string idEvento = "ID" + "110110" + chaveNF + sNumSeq;
                var detEvento = new EnvEvCCe.TEventoInfEventoDetEvento { descEvento = EnvEvCCe.TEventoInfEventoDetEventoDescEvento.CartadeCorrecao, versao = EnvEvCCe.TEventoInfEventoDetEventoVersao.Item100, xCorrecao = txtCorrecao, xCondUso = EnvEvCCe.TEventoInfEventoDetEventoXCondUso.ACartadeCorrecaoedisciplinadapeloparagrafo1oAdoart7odoConvenioSNde15dedezembrode1970epodeserutilizadapararegularizacaodeerroocorridonaemissaodedocumentofiscaldesdequeoerronaoestejarelacionadocomIasvariaveisquedeterminamovalordoimpostotaiscomobasedecalculoaliquotadiferencadeprecoquantidadevalordaoperacaooudaprestacaoIIacorrecaodedadoscadastraisqueimpliquemudancadoremetenteoudodestinatarioIIIadatadeemissaooudesaida };
                var infEvento = new EnvEvCCe.TEventoInfEvento { chNFe = chaveNF, cOrgao = orgaoIBGE, dhEvento = dhEvento, tpAmb = amb, tpEvento = EnvEvCCe.TEventoInfEventoTpEvento.Item110110, nSeqEvento = numCorrecao.ToString(), verEvento = EnvEvCCe.TEventoInfEventoVerEvento.Item100, Id = idEvento, detEvento = detEvento, ItemElementName = EnvEvCCe.ItemChoiceType.CNPJ, Item = cnpj };
                var evento = new EnvEvCCe.TEvento { versao = versao, infEvento = infEvento };
                string assinatura = getSignature(serializar(evento, "http://www.portalfiscal.inf.br/nfe"), "infEvento", objCertificado);
                evento.Signature = (EnvEvCCe.SignatureType)deserializar(assinatura, typeof(EnvEvCCe.SignatureType));
                EnvEvCCe.TEvento[] eventos = { evento };
                var envEvento = new EnvEvCCe.TEnvEvento { versao = versao, evento = eventos, idLote = idlote };

                string str_req = serializar(envEvento, "http://www.portalfiscal.inf.br/nfe");

                //validando a requisição
                string erros = "";
                int numErros = ValidaXML(str_req, "Evento_CCe_PL_v1.01\\envCCe_v1.00", ref erros);
                if (numErros > 0)
                {
                    msgRetorno = "Erro no xml da requisição. " + erros;
                    return false;
                }

                //convertendo a requisiçao para XmlNode
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeEvento4.NFeRecepcaoEvento4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeRecepcaoEvento(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (EnvEvCCe.TRetEnvEvento)deserializar(xmlRet, typeof(EnvEvCCe.TRetEnvEvento));
                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                if (codStatus != 128)
                {
                    return false;
                }

                msgRetorno = resposta.retEvento[0].infEvento.xMotivo;
                codStatus = int.Parse(resposta.retEvento[0].infEvento.cStat);

                //se não foi aceito
                if (codStatus != 135) return false;

                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao enviar carta de correção. " + procExcessao(ex);
                return false;
            }
        }

        //Função para montar um lote de notas fiscais para envio
        public string MontaLoteNF(string[] notas, string numLote, bool sincrono, bool gzip, ref string msgRetorno)
        {
            try
            {
                //Percorre as notas gerando o objeto TNFe
                EnviNFe.TNFe[] NFEs = new EnviNFe.TNFe[notas.Length];
                EnviNFe.TNFe NFe;
                int i = 0;
                foreach (string xmlNota in notas)
                {
                    NFe = (EnviNFe.TNFe)deserializar(xmlNota, typeof(EnviNFe.TNFe));
                    NFEs[i] = NFe;
                    i++;
                }

                //Gera o lote
                EnviNFe.TEnviNFeIndSinc sinc = sincrono ? EnviNFe.TEnviNFeIndSinc.Item1 : EnviNFe.TEnviNFeIndSinc.Item0;
                var enviNFe = new EnviNFe.TEnviNFe { versao = "4.00", idLote = numLote, NFe = NFEs, indSinc = sinc };
                string str_req = serializar(enviNFe, "http://www.portalfiscal.inf.br/nfe");

                if (!gzip) return str_req;

                else
                {
                    return StrToGzip(str_req);
                }
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao montar lote. " + procExcessao(ex);
                return null;
            }
        }

        //Função que transmite um lote de notas fiscais
        public bool EnviaLoteNF(string sLote, string modelo, bool gzip, string siglaWS, int ambiente, string nomeCertificado, ref string msgRetorno, ref int codStatus, ref string numRec, ref string dhprotocolo, ref string tempoMedio, ref string xmlRet)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //verifica se é nfc (modelo 65)
                bool nfc = modelo.Equals("65") ? true : false;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "Autorizacao", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada. [" + siglaWS + "]";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeAutorizacao4.NFeAutorizacao4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Gerando o xmlNode da requisição (se não for gzip)
                XmlNode requisicao_xml;
                if (!gzip)
                {
                    XmlDocument docreq = new XmlDocument();
                    docreq.LoadXml(sLote);
                    requisicao_xml = docreq.DocumentElement;
                }
                else requisicao_xml = null;

                //Obtendo a resposta do serviço web
                XmlNode nodeRes;
                if (gzip)
                    nodeRes = cliente.nfeAutorizacaoLoteZip(sLote);
                else
                    nodeRes = cliente.nfeAutorizacaoLote(requisicao_xml);

                xmlRet = nodeRes.OuterXml;
                var resposta = (EnviNFe.TRetEnviNFe)deserializar(xmlRet, typeof(EnviNFe.TRetEnviNFe));

                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);
                dhprotocolo = resposta.dhRecbto;

                //em processamento
                if (codStatus == 103)
                {
                    var infRec = (EnviNFe.TRetEnviNFeInfRec)resposta.Item;
                    numRec = infRec.nRec;
                    tempoMedio = infRec.tMed;
                    return true;
                }
                //Situação diferente de "Lote recebido com sucesso"
                else return false;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao transmitir lote. " + procExcessao(ex);
                return false;
            }
        }

        //Função para obter o retorno da emissão de um lote. Utilizada em conjunto com EnviaLoteNF
        public bool BuscaLoteNF(int ambiente, bool nfc, string nomeCertificado, string siglaWS, string numRecibo, ref int codStatus, ref string msgRetorno, ref string cMsg, ref string xMsg, ref string xmlRet)
        {
            try
            {
                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE(siglaWS, ambiente, "RetAutorizacao", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o serviço para a sigla informada. [" + siglaWS + "]";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //gerando a requisição do serviço
                ConsReciNFe.TAmb amb = ambiente == 1 ? ConsReciNFe.TAmb.Item1 : ConsReciNFe.TAmb.Item2;
                var consReciNFe = new ConsReciNFe.TConsReciNFe { versao = "4.00", tpAmb = amb, nRec = numRecibo };
                string str_req = serializar(consReciNFe, "http://www.portalfiscal.inf.br/nfe");
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeRetAutorizacao4.NFeRetAutorizacao4Soap12Client(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeRetAutorizacaoLote(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (ConsReciNFe.TRetConsReciNFe)deserializar(xmlRet, typeof(ConsReciNFe.TRetConsReciNFe));

                codStatus = int.Parse(resposta.cStat);
                msgRetorno = resposta.xMotivo;
                try
                {
                    xMsg = resposta.xMsg;
                    cMsg = resposta.cMsg;
                }
                catch { }

                //se não foi processado
                if (codStatus != 104) return false;

                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao buscar resultado do processamento. " + procExcessao(ex);
                return false;
            }
        }

        //Função que envia a manifestação do destinatário
        public bool EnviaManiDest(int ambiente, string cnpj, string nomeCertificado, string versao, string tipoEvento, string chaveNF, string justificativa, string dhEvento, ref string msgRetorno, ref int codStatus, ref string xmlRet)
        { 
            try
            {
                if (chaveNF.Length != 44)
                {
                    msgRetorno = "A chave de acesso deve ter 44 posições.";
                    return false;
                }

                //obtendo o objeto certificado a partir de seu nome
                var objCertificado = getCertificado(nomeCertificado, ref msgRetorno);
                if (objCertificado == null) return false;

                //verifica se é NFC-e (Modelo 65)
                string modNota = chaveNF.Substring(20, 2);
                bool nfc = modNota.Equals("65") ? true : false;

                //criando o endpoint a partir da siglaWS
                Endpoints end = new Endpoints();
                string endereco = end.getUrlNFE("AN", ambiente, "Evento", nfc);
                if (endereco == null)
                {
                    msgRetorno = "Não foi possível encontrar o arquivo com as URLs dos serviços.";
                    return false;
                }
                var endpoint = new EndpointAddress(endereco);

                //criando a requisição
                ManifDest.TCOrgaoIBGE orgaoIBGE;
                orgaoIBGE = ManifDest.TCOrgaoIBGE.Item91;
                ManifDest.TEventoInfEventoDetEventoDescEvento descEvento;
                ManifDest.TEventoInfEventoTpEvento tpEvento;
                switch (tipoEvento)
                {
                    case "210200":
                        descEvento = ManifDest.TEventoInfEventoDetEventoDescEvento.ConfirmacaodaOperacao;
                        tpEvento = ManifDest.TEventoInfEventoTpEvento.Item210200;
                        break;
                    case "210210":
                        descEvento = ManifDest.TEventoInfEventoDetEventoDescEvento.CienciadaOperacao;
                        tpEvento = ManifDest.TEventoInfEventoTpEvento.Item210210;
                        break;
                    case "210220":
                        descEvento = ManifDest.TEventoInfEventoDetEventoDescEvento.DesconhecimentodaOperacao;
                        tpEvento = ManifDest.TEventoInfEventoTpEvento.Item210220;
                        break;
                    case "210240":
                        descEvento = ManifDest.TEventoInfEventoDetEventoDescEvento.OperacaonaoRealizada;
                        tpEvento = ManifDest.TEventoInfEventoTpEvento.Item210240;
                        break;
                    default:
                        msgRetorno = "Tipo de evento inválido ["+tipoEvento+"]";
                        return false;
                }

                ManifDest.TAmb amb = ambiente == 1 ? ManifDest.TAmb.Item1 : ManifDest.TAmb.Item2;
                string idlote = chaveNF.Substring(25, 12) + "01";
                string idEvento = "ID" + tipoEvento + chaveNF + "01";
                ManifDest.TEventoInfEventoDetEvento detEvento;
                if (justificativa.Equals("") || justificativa == null)
                    detEvento = new ManifDest.TEventoInfEventoDetEvento { descEvento = descEvento, versao = ManifDest.TEventoInfEventoDetEventoVersao.Item100 };
                else
                    detEvento = new ManifDest.TEventoInfEventoDetEvento { descEvento = descEvento, versao = ManifDest.TEventoInfEventoDetEventoVersao.Item100, xJust = justificativa };
                var infEvento = new ManifDest.TEventoInfEvento { chNFe = chaveNF, cOrgao = orgaoIBGE, dhEvento = dhEvento, tpAmb = amb, tpEvento = tpEvento, nSeqEvento = "1", verEvento = versao, Id = idEvento, detEvento = detEvento, ItemElementName = ManifDest.ItemChoiceType.CNPJ, Item = cnpj };
                var evento = new ManifDest.TEvento { versao = versao, infEvento = infEvento };
                string assinatura = getSignature(serializar(evento, "http://www.portalfiscal.inf.br/nfe"), "infEvento", objCertificado);
                evento.Signature = (ManifDest.SignatureType)deserializar(assinatura, typeof(ManifDest.SignatureType));
                ManifDest.TEvento[] eventos = { evento };
                var envEvento = new ManifDest.TEnvEvento { versao = versao, evento = eventos, idLote = idlote };

                string str_req = serializar(envEvento, "http://www.portalfiscal.inf.br/nfe");

                //validando a requisição
                string erros = "";
                int numErros = ValidaXML(str_req, "Evento_ManifestaDest_PL_v1.01\\envConfRecebto_v1.00", ref erros);
                if (numErros > 0)
                {
                    msgRetorno = "Erro no xml da requisição. " + erros;
                    return false;
                }

                //convertendo a requisiçao para XmlNode
                XmlDocument docreq = new XmlDocument();
                docreq.LoadXml(str_req);
                XmlNode requisicao_xml = docreq.DocumentElement;

                //Gerando o binding com os atributos da conexão
                CustomBinding binding = getBinding();

                //Criando o cliente SOAP
                var cliente = new NFeEventoNacional4.NFeRecepcaoEvento4SoapClient(binding, endpoint);
                cliente.ClientCredentials.ClientCertificate.Certificate = objCertificado;

                //Obtendo a resposta do serviço web
                var nodeRes = cliente.nfeRecepcaoEventoNF(requisicao_xml);
                xmlRet = nodeRes.OuterXml;
                var resposta = (ManifDest.TRetEnvEvento)deserializar(xmlRet, typeof(ManifDest.TRetEnvEvento));
                msgRetorno = resposta.xMotivo;
                codStatus = int.Parse(resposta.cStat);

                if (codStatus != 128)
                {
                    return false;
                }

                msgRetorno = resposta.retEvento[0].infEvento.xMotivo;
                codStatus = int.Parse(resposta.retEvento[0].infEvento.cStat);

                return true;
            }
            catch (Exception ex)
            {
                //caso ocorra erro na execução da função
                msgRetorno = "Erro ao executar manifesto. " + procExcessao(ex);
                return false;
            }
        }


        //Função que descompacta uma string gzip base64 e retorna o resultado
        public string DescompactaGzip(string strCompactada)
        {
            try
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(strCompactada);
                using (var inputStream = new MemoryStream(inputBytes))
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                using (var outputStream = new MemoryStream())
                {
                    gZipStream.CopyTo(outputStream);
                    var outputBytes = outputStream.ToArray();
                    string decompressed = Encoding.UTF8.GetString(outputBytes);
                    return decompressed;
                }
            }
            catch
            {
                return null;
            }
        }

        //Função que processa a excessão e retorna a causa provável
        private string procExcessao(Exception ex)
        {
            string sret = string.Empty;
            if (ex.Message.Contains("Não havia um ponto"))
            {
                try
                {
                    //remove a causa provavel, para inserir depois com mais especificidade
                    sret = ex.Message.Substring(0, ex.Message.IndexOf("Em geral"));
                }
                catch
                {
                    sret = ex.Message;
                }
                sret += "Geralmente isso é causado por indisponibilidade do serviço ou problema no acesso à internet.";
            }
            else if (ex.Message.Contains("um canal seguro para SSL/TLS"))
            {
                sret = ex.Message;
                sret += " Geralmente isso é causado por um problema na instalação da cadeia certificadora.";
            }
            else sret = ex.Message;

            return sret;
        }

        //Função que converte uma string em um objeto xml serializado pelo schema
        private object deserializar(string sxml, Type tipo)
        {
            try
            {
                var deserializer = new XmlSerializer(tipo);
                object objeto = null;
                using (TextReader reader = new StringReader(sxml))
                {
                    objeto = (object)deserializer.Deserialize(reader);
                }
                return objeto;
            }
            catch
            {
                return null;
            }
        }

        //Função que converte um objeto xml serializado em uma string
        private string serializar(object objeto, string urlnamespace)
        {
            try
            {
                string resposta = null;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", urlnamespace);
                var serializer = new XmlSerializer(objeto.GetType());
                using (MemoryStream memStm = new MemoryStream())
                {
                    serializer.Serialize(memStm, objeto, ns);
                    memStm.Position = 0;
                    resposta = new StreamReader(memStm).ReadToEnd();
                }
                return resposta;
            }
            catch
            {
                return null;
            }
        }

        //Função que gera a tag signature (em string)
        private string getSignature(string sxml, string tagAssin, X509Certificate2 objCertificado)
        {
            try
            {
                //gerando documento xml a partir da string
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sxml);

                //obtendo os nós quem contém a tag que se deseja assinar
                XmlNodeList ListTags = doc.GetElementsByTagName(tagAssin);
                if (ListTags.Count == 0) return null;
                if (ListTags.Count > 1)  return null;

                //assinando a tag
                XmlElement xmlSignature = doc.CreateElement("Signature", "http://www.w3.org/2000/09/xmldsig#");
                foreach (XmlElement tag in ListTags)
                {
                    string id = tag.Attributes.GetNamedItem("Id").Value;
                    SignedXml signedXml = new SignedXml(tag);
                    signedXml.SigningKey = objCertificado.PrivateKey;
                    Reference reference = new Reference("#" + id);
                    reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                    reference.AddTransform(new XmlDsigC14NTransform());
                    signedXml.AddReference(reference);
                    KeyInfo keyInfo = new KeyInfo();
                    keyInfo.AddClause(new KeyInfoX509Data(objCertificado));
                    signedXml.KeyInfo = keyInfo;
                    signedXml.ComputeSignature();

                    XmlElement xmlSignedInfo = signedXml.SignedInfo.GetXml();
                    XmlElement xmlKeyInfo = signedXml.KeyInfo.GetXml();
                    XmlElement xmlSignatureValue = doc.CreateElement("SignatureValue", xmlSignature.NamespaceURI);

                    string signBase64 = Convert.ToBase64String(signedXml.Signature.SignatureValue);
                    XmlText text = doc.CreateTextNode(signBase64);
                    xmlSignatureValue.AppendChild(text);

                    xmlSignature.AppendChild(doc.ImportNode(xmlSignedInfo, true));
                    xmlSignature.AppendChild(xmlSignatureValue);
                    xmlSignature.AppendChild(doc.ImportNode(xmlKeyInfo, true));
                }
                return xmlSignature.OuterXml;
            }
            catch
            {
                //caso ocorra erro na execução da função
                return null;
            }
        }

        //função que gera o binding de requisição dos webservices
        private static CustomBinding getBinding()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            HttpsTransportBindingElement objHttpsTransportBindingElement = new HttpsTransportBindingElement();
            objHttpsTransportBindingElement.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            objHttpsTransportBindingElement.ManualAddressing = false;
            objHttpsTransportBindingElement.MaxReceivedMessageSize = int.MaxValue;
            objHttpsTransportBindingElement.AllowCookies = false;
            objHttpsTransportBindingElement.BypassProxyOnLocal = false;
            objHttpsTransportBindingElement.DecompressionEnabled = true;
            objHttpsTransportBindingElement.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            objHttpsTransportBindingElement.KeepAliveEnabled = true;
            objHttpsTransportBindingElement.Realm = string.Empty;
            objHttpsTransportBindingElement.TransferMode = TransferMode.Buffered;
            objHttpsTransportBindingElement.UnsafeConnectionNtlmAuthentication = false;
            objHttpsTransportBindingElement.UseDefaultWebProxy = true;
            objHttpsTransportBindingElement.AuthenticationScheme = AuthenticationSchemes.Digest;
            objHttpsTransportBindingElement.RequireClientCertificate = true;

            TextMessageEncodingBindingElement objTextMessageEncodingBindingElement = new TextMessageEncodingBindingElement();
            objTextMessageEncodingBindingElement.MessageVersion = MessageVersion.Soap12;
            objTextMessageEncodingBindingElement.WriteEncoding = Encoding.UTF8;

            CustomBinding binding = new CustomBinding(objTextMessageEncodingBindingElement, objHttpsTransportBindingElement);
            return binding;
        }

        //função que gera código o hash hexadecimal para o qrcode
        private static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);
            return HexStringFromBytes(hashBytes);
        }

        //função que converte o código hash para hexadecimal
        private static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString().ToUpper();
        }

        //Função que retorna o objeto certificado a partir de seu nome e informa caso ocorra erro
        private static X509Certificate2 getCertificado(string nome_certificado, ref string erros)
        {
            try
            {
                //se o nome do certificado vier vazio
                if (nome_certificado.Length == 0)
                {
                    erros = "Nome do certificado não foi passado.";
                    return null;
                }

                //se for informado um certificado e uma senha
                string[] certs = nome_certificado.Split('|');
                if (certs.Length == 3)
                {
                    //caso seja passado um caminho de arquivo
                    if (certs[0].Equals("ARQUIVO"))
                    {
                        //cria o certificado a partir do caminho do .pfx e da senha
                        X509Certificate2 cert = new X509Certificate2(certs[1], certs[2]);

                        //caso o certificado seja inválido
                        if (!cert.Verify())
                        {
                            erros = "Certificado digital inválido. Certificado válido de " +
                                cert.GetEffectiveDateString() + " até " +
                                cert.GetExpirationDateString();
                            return null;
                        }

                        //caso o certificado seja válido
                        else return cert;
                    }
                    else
                    {
                        erros = "Tipo de certificado digital inválido. [" + certs[0] + "]";
                        return null;
                    }
                }

                //se for informado o nome para ser buscado no repositorio do usuário atual
                else
                {
                    X509Certificate2 cert = GetCertificateFromStore(nome_certificado);
                    if (cert == null)
                    {
                        erros = "Certificado [" + nome_certificado + "] não foi encontrado.";
                        return null;
                    }
                    else return cert;
                }
            }
            catch
            {
                //caso ocorra erro na execução da função
                erros = "Certificado digital inválido.";
                return null;
            }
        }

        //Função que localiza um certificado no repositorio do usuário 
        private static X509Certificate2 GetCertificateFromStore(string certName)
        {
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = store.Certificates;
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindBySubjectDistinguishedName, certName, false);
                if (signingCert.Count == 0)
                    return null;
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }

        //Função que descompacta gzip (descompacta os documentos retornados no Webservice de distribuição)
        private static string GzipDecode(byte[] inputBytes)
        {
            try
            {
                using (var inputStream = new MemoryStream(inputBytes))
                using (var gZipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                using (var outputStream = new MemoryStream())
                {
                    gZipStream.CopyTo(outputStream);
                    var outputBytes = outputStream.ToArray();
                    string decompressed = Encoding.UTF8.GetString(outputBytes);
                    return decompressed;
                }
            }
            catch
            {
                return null;
            }
        }

        //Função que compacta uma string para gzip
        private static string StrToGzip(string text)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                MemoryStream ms = new MemoryStream();
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }
                ms.Position = 0;
                MemoryStream outStream = new MemoryStream();
                byte[] compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);
                byte[] gzBuffer = new byte[compressed.Length + 4];
                System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
                return Convert.ToBase64String(gzBuffer);
            }
            catch
            {
                return null;
            }
        }
    }
}