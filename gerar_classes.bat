@echo off
cd schemas
mkdir classes

xsd /c /edb /o:classes consSitNFe_v4.00.xsd retConsSitNFe_v4.00.xsd xmldsig-core-schema_v1.01.xsd /n:ConsSitNFe
xsd /c /edb /o:classes consReciNFe_v4.00.xsd retConsReciNFe_v4.00.xsd xmldsig-core-schema_v1.01.xsd /n:ConsReciNFe
xsd /c /edb /o:classes inutNFe_v4.00.xsd retInutNFe_v4.00.xsd xmldsig-core-schema_v1.01.xsd /n:InutNFe
xsd /c /edb /o:classes .\Evento_Canc_PL_v1.01\envEventoCancNFe_v1.00.xsd .\Evento_Canc_PL_v1.01\procEventoCancNFe_v1.00.xsd .\Evento_Canc_PL_v1.01\retEnvEventoCancNFe_v1.00.xsd xmldsig-core-schema_v1.01.xsd /n:EnvEvCanc
xsd /c /edb /o:classes .\PL_NFeDistDFe_102\distDFeInt_v1.01.xsd .\PL_NFeDistDFe_102\retDistDFeInt_v1.01.xsd xmldsig-core-schema_v1.01.xsd /n:DistNFe
xsd /c /edb /o:classes consStatServ_v4.00.xsd retConsStatServ_v4.00.xsd /n:StatServ
xsd /c /edb /o:classes procInutNFe_v4.00.xsd procNFe_v4.00.xsd xmldsig-core-schema_v1.01.xsd /n:ProcNFe
xsd /c /edb /o:classes enviNFe_v4.00.xsd retEnviNFe_v4.00.xsd xmldsig-core-schema_v1.01.xsd /n:EnviNFe
xsd /c /edb /o:classes .\PL_CTeDistDFe_100\distDFeInt_v1.00.xsd .\PL_CTeDistDFe_100\retDistDFeInt_v1.00.xsd xmldsig-core-schema_v1.01.xsd /n:DistCTe
xsd /c /edb /o:classes .\Evento_CCe_PL_v1.01\envCCe_v1.00.xsd .\Evento_CCe_PL_v1.01\retEnvCCe_v1.00.xsd xmldsig-core-schema_v1.01.xsd /n:EnvEvCCe

xsd /c /edb /o:classes .\Evento_ManifestaDest_PL_v1.01\envConfRecebto_v1.00.xsd .\Evento_ManifestaDest_PL_v1.01\retEnvConfRecebto_v1.00.xsd xmldsig-core-schema_v1.01.xsd /n:ManifDest

cd classes
REN consSitNFe_v4_00_retConsSitNFe_v4_00_xmldsig-core-schema_v1_01.cs ConsSitNFe_v400.cs
REN consReciNFe_v4_00_retConsReciNFe_v4_00_xmldsig-core-schema_v1_01.cs ConsReciNFe_v400.cs
REN inutNFe_v4_00_retInutNFe_v4_00_xmldsig-core-schema_v1_01.cs InutNFe_v400.cs
REN retEnvEventoCancNFe_v1_00_xmldsig-core-schema_v1_01.cs CancNFe_v100.cs
REN retDistDFeInt_v1_01_xmldsig-core-schema_v1_01.cs DistNFe_v101.cs
REN consStatServ_v4_00_retConsStatServ_v4_00.cs ConsStatServNFe_v400.cs
REN procInutNFe_v4_00_procNFe_v4_00_xmldsig-core-schema_v1_01.cs ProcNFe_ProcInutNFe_v400.cs
REN enviNFe_v4_00_retEnviNFe_v4_00_xmldsig-core-schema_v1_01.cs EnviNFe_v400.cs
REN retDistDFeInt_v1_00_xmldsig-core-schema_v1_01.cs DistCTe_v100.cs
REN retEnvCCe_v1_00_xmldsig-core-schema_v1_01.cs CCeNFe_v100.cs
REN retEnvConfRecebto_v1_00_xmldsig-core-schema_v1_01.cs ConfRec_v100.cs

type nul > readme.txt
echo Inserir a linha de comando:>> readme.txt
echo [System.Xml.Serialization.XmlRootAttribute("NFe", Namespace="http://www.portalfiscal.inf.br/nfe", IsNullable=false)]>> readme.txt
echo Na linha 1328 do arquivo ProcNFe_ProcInutNFe_v400.cs>> readme.txt
echo Na linha 112 do arquivo EnviNFe_v400.cs>> readme.txt

cd ..
echo Siga o procedimento indicado em readme.txt!!!
echo Siga o procedimento indicado em readme.txt!!!
echo Siga o procedimento indicado em readme.txt!!!

