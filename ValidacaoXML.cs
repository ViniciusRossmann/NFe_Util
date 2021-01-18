/*
    Classe com as funções de validação de documento XML através de um schema .xsd
    Autor: Vinícius Rossmann Nunes
    Ultima modificação: outubro/2020 - implementado
 */

using System.Xml;
using System.Xml.Schema;

namespace DFe_Util_HM
{
    public class ValidacaoXML
    {
        private bool falhou;     //Armazena se o validação falhou
        private string motivos;  //Armazena os erros encontrados
        private int numErros;    //Armazena o número de erros encontrados

        public bool Falhou
        {
            get { return falhou; }
        }
        public string Motivos
        {
            get { return motivos;  }
        }
        public int NumErros
        {
            get { return numErros; }
        }

        //Valida um documento xml através de um schema XSD
        public bool ValidarXml(XmlDocument doc, string schemaFilename)
        {
            XmlNodeReader nodeReader = new XmlNodeReader(doc);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            XmlSchemaSet schemas = new XmlSchemaSet();
            settings.Schemas = schemas;
            schemas.Add(null, schemaFilename);
            settings.ValidationEventHandler += ValidationEventHandler;
            XmlReader validator = XmlReader.Create(nodeReader, settings);
            falhou = false;
            numErros = 0;
            try { while (validator.Read()) { } }
            catch { falhou = true; }
            finally { validator.Close(); }
            return !falhou;
        }

        //Evento chamado quando encontra um erro no documento xml
        private void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            falhou = true;
            numErros++;
            motivos += args.Message;
        }
    }
}
