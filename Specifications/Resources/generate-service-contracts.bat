svcutil Chem_eStandards_5.4_Finance.wsdl Chem_eStandards_5.4_Inventory.wsdl Chem_eStandards_5.4_Management.wsdl Chem_eStandards_5.4_Quality.wsdl Chem_eStandards_5.4_Transportation.wsdl OAGi_Chem_eStandards_5.4.0.xsd /noconfig /n:*,EFC.AgGateway.Integration.Specifications.AgGatewayServiceContract /o:..\AgGatewayServiceContract.cs

xsd schemas.xmlsoap.org.xsd /classes /language:CS /n:EFC.AgGateway.Integration.Specifications.XmlSoap /outputdir:..\

svcutil AgGateway.wsdl /noconfig /n:*,EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract /o:..\AgGatewayGenericServiceContract.cs

xsd OAGi_Chem_eStandards_5.4.0.xsd /classes /language:CS /n:EFC.AgGateway.Integration.Specifications.AgGatewayGenericServiceContract /outputdir:..\

scriptcs .\Scripts\RemoveEmptyNamespaces.csx -- ..\..\AgGatewayServiceContract.cs

scriptcs .\Scripts\FixXmlQualifiedName.csx -- ..\..\schemas_xmlsoap_org.cs

scriptcs .\Scripts\Fix2dArray.csx -- ..\..\OAGi_Chem_eStandards_5_4_0.cs

scriptcs .\Scripts\FixOutboundData.csx -- ..\..\AgGatewayGenericServiceContract.cs
