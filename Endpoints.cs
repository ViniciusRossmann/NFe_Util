/*
    Classe com as funções para localizar os endereços dos serviços no arquivo xml
    Autor: Vinícius Rossmann Nunes
    Ultima modificação: outubro/2020 - implementado
*/

using System.Xml;
using System.Reflection;

namespace DFe_Util_HM
{
    class Endpoints
    {
        //Função que obtem o endpoint para as operações de distribuição (DFeDistribuicao)
        //Atenção: tipoDoc deve ser passado como está no arquivo de endpoints (NFe, CTe, etc.)
        public string getUrlDistribuicao(string tipoDoc, int ambiente)
        {
            try
            {
                //obtem o documento xml que contem as urls
                XmlDocument doc = getArquivoURL();

                //Se não conseguir obter o arquivo, retorna nulo
                if (doc == null) return null;

                //seleciona o nó do arquivo xml de acordo com o documento (NFe, CTe)
                XmlNode nodeDocumento = doc.GetElementsByTagName(tipoDoc).Item(0);
                if (nodeDocumento == null) return null;

                //Obtem a url desejada de acordo com o ambiente (homologacao/distribuicao)
                string sAmb;
                sAmb = ambiente == 1 ? "producao" : "homologacao";
                XmlNode nodeWS = nodeDocumento.SelectSingleNode("descendant::distribuicao").SelectSingleNode("descendant::" + sAmb);
                return nodeWS.InnerText;
            }
            catch
            {
                //caso ocorra erro durante a execução, retorna nulo
                return null;
            }
        }

        //Função que obtem o endpoint para operações de NF-e e NFC-e
        public string getUrlNFE(string siglaWS, int ambiente, string operacao, bool nfc)
        {
            try
            {
                //Coloca a sigla em caracteres maiúsculos (assim como está no arquivo xml)
                siglaWS = siglaWS.ToUpper();

                //obtem o documento xml que contem as urls
                XmlDocument doc = getArquivoURL();

                //Se não conseguir obter o arquivo, retorna nulo
                if (doc == null) return null;

                XmlNode nodeNFCe = null;
                XmlNode nodeAmbiente = null;

                //Se for NFC-e procura primeiro nos webservices específicos para NFC-e
                if (nfc) {
                    nodeNFCe = doc.GetElementsByTagName("NFCe").Item(0);
                    nodeAmbiente = nodeNFCe.SelectSingleNode("descendant::ambiente[sigla='" + siglaWS + "']");
                }

                //Caso não seja NFC ou não tenha encontrado, procura nos webservices de NF-e
                if (nodeAmbiente == null)
                {
                    XmlNode nodeNFe = doc.GetElementsByTagName("NFe").Item(0);
                    nodeAmbiente = nodeNFe.SelectSingleNode("descendant::ambiente[sigla='" + siglaWS + "']");
                }

                //Caso ainda não tenha encontrado, retorna nulo
                if (nodeAmbiente == null) return null;

                //Obtem a URL para o ambiente e operação específico
                string sAmb;
                sAmb = ambiente == 1 ? "producao" : "homologacao";
                var nodeWebService = nodeAmbiente.SelectSingleNode("descendant::"+sAmb+"").SelectSingleNode("descendant::"+operacao);
                return nodeWebService.InnerText;
            }
            catch
            {
                //caso ocorra erro durante a execução, retorna nulo
                return null;
            }
        }

        //Função que obtém o  documento XML que possui os endpoints (webservices4.xml)
        private XmlDocument getArquivoURL()
        {
            try
            {
                string path = Assembly.GetExecutingAssembly().CodeBase.Replace("file:\\", "").ToUpper().Replace("DFE_UTIL_HM.DLL", "");
                path += "URL\\webservices4.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                return doc;
            }
            catch
            {
                //caso ocorra erro durante a execução, retorna nulo
                return null;
            }
        }
    }
}
