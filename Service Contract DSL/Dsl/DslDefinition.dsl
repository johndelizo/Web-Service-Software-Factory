<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="108ced64-5475-4baa-bebc-6f5be3107d9e" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractDsl" Name="ServiceContractDsl" DisplayName="Service Contract Model" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" ProductName="Web Service Software Factory: Modeling Edition" CompanyName="Microsoft" PackageGuid="43805256-e844-436e-8835-035cc972a441" PackageNamespace="Microsoft.Practices.ServiceFactory.ServiceContracts" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="86959dd4-26ec-48e6-8907-b47c4c419546" Description="The root in which all other elements are embedded. Appears as a diagram." Name="ServiceContractModel" DisplayName="Service Contract Model" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Notes>The service contract model roor element.</Notes>
      <Properties>
        <DomainProperty Id="3a8e83a2-da82-4090-b06d-cfbfd72b490d" Description="This associates a platform technology with each of the shapes on the design surface. As a result, each shape may acquire additional properties specific for the chosen technology." Name="ImplementationTechnology" DisplayName="Implementation Technology" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.ExtensionProvider.Design.UITypeEditors.ExtensionProviderEditor),typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.ExtensionProvider.Design.Converters.ExtensionProviderConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/IExtensionProvider" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="926a3128-ed5b-4ea6-9c2e-03084646fb4e" Description="Allows you to select the project mapping you would like to generate artifacts to. The drop-down lists the named entries in the ProjectMapping.xml file. Creating an implementation project structure will add a new entry to the project mapping table. The Project Mapping Table entry associates shapes on the design surface with the projects that will contain the code for those shapes." Name="ProjectMappingTable" DisplayName="Project Mapping Table" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.ProjectMapping.Design.ProjectMappingTableConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="23783d61-83d3-4cf9-95c0-f6088397c64c" Description="This value represents the default XML namespace that will be applied to each shape added to the design surface." Name="Namespace" DisplayName="XML Namespace" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.Dsl.Integration.Design.XmlNamespaceStringConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4ae7d31e-dd5a-4303-9257-444bcfb5db8a" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModel.Serializer Type" Name="SerializerType" DisplayName="Serializer Type" Category="General">
          <Type>
            <DomainEnumerationMoniker Name="SerializerType" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Notes>Creates an embedding link when an element is dropped onto a model. </Notes>
          <Index>
            <DomainClassMoniker Name="ServiceContract" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ServiceContractModelHasServiceContracts.ServiceContracts</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Operation" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ServiceContractModelHasOperations.Operations</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Service" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ServiceContractModelHasServices.Services</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="MessageBase" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ServiceContractModelHasMessages.Messages</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="482361f7-975a-457d-b04a-d191e4e32f89" Description="Elements embedded in the model. Appear as boxes on the diagram." Name="ServiceContract" DisplayName="Service Contract" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.ServiceContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <Properties>
        <DomainProperty Id="3a5cbf68-c59b-4036-a6fd-feebca005a0e" Description="The name of the service contract." Name="Name" DisplayName="Name" DefaultValue="" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2ba6123b-4ea0-49d7-92ab-1c558fa13ccb" Description="The XML namespace of the service contract." Name="Namespace" DisplayName="XML Namespace" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.Dsl.Integration.Design.XmlNamespaceStringConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7f5d3080-23ff-4760-aed0-a1dae78f4d41" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContract.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1792c489-ed1b-4323-8cb3-938aaf115a87" Description="Provides additional documentation information to this element. Its content will be added as a &lt;remarks&gt; element to the generated code." Name="Remarks" DisplayName="Remarks" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="bbb75d22-1c71-4c93-88f9-dc598c649fd4" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.Operation" Name="Operation" DisplayName="Operation" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Properties>
        <DomainProperty Id="9dbb418a-4725-4aa8-893a-bd16f3b46ff4" Description="The name of the operation." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f5c8401f-f943-40e7-af1c-a1074cc2f5d7" Description="Represents the WS-Addressing action attribute, which is used for dispatching the request." Name="Action" DisplayName="Action" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="50ec053b-125e-4dcf-93d2-ed30c1492007" Description="Indicates whether or not an operation returns a reply or fault message." Name="IsOneWay" DisplayName="Is One Way" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f8ee2a06-bd20-48e3-bdb6-8f4a5373332c" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.Operation.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="844361d0-bd8b-4b00-b6e0-9d1079bd48c8" Description="Provides additional documentation information to this element. Its content will be added as a &lt;remarks&gt; element to the generated code." Name="Remarks" DisplayName="Remarks" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Fault" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>OperationHasFaults.Faults</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="b5147e13-b866-4cdf-bd90-dbeae9e91a11" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.Service" Name="Service" DisplayName="Service" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.ServiceRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <Properties>
        <DomainProperty Id="9a3df394-b295-4377-9202-eecf4aa6f002" Description="The name of the service." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1c220b59-b297-4e4e-a69f-7f4cfa5257d9" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.Service.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b91d74cf-95c4-46c3-95b8-0d7fd8954b52" Description="The XML namespace of the service." Name="Namespace" DisplayName="XML Namespace" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.Dsl.Integration.Design.XmlNamespaceStringConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="136f4872-44cd-415b-8574-984a4807e78e" Description="Provides additional documentation information to this element. Its content will be added as a &lt;remarks&gt; element to the generated code." Name="Remarks" DisplayName="Remarks" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="e5d605dc-eb6f-4b44-bbb4-f812a784f04e" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.Message" Name="Message" DisplayName="Message" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.MessageContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
        <DomainClassMoniker Name="MessageBase" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="DataContractMessagePart" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>MessageHasMessageParts.MessageParts</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="PrimitiveMessagePart" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>MessageHasMessageParts.MessageParts</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="116281c4-9d4f-441b-8ab7-4f52e7206230" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessagePart" Name="MessagePart" DisplayName="Message Part" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Properties>
        <DomainProperty Id="61d27623-7035-4bab-85bd-9e433cfbd98e" Description="The name of the data contract." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="641f688e-79ee-472a-9ea8-1ebaa6faed94" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessagePart.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9578acbc-e576-4765-8328-10e5aadc702b" Description="Provides additional documentation information to this element. Its content will be added as a &lt;remarks&gt; element to the generated code." Name="Remarks" DisplayName="Remarks" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="0c6f407e-fbf2-454a-af8f-8e03ad226563" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.Fault" Name="Fault" DisplayName="Fault" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Properties>
        <DomainProperty Id="709d5f7b-f1b2-4c1c-8b2c-8b7e84eb6422" Description="The name of the fault contract." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="13960a6e-a7c5-44e4-81d1-0749f27f7c5d" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.Fault.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5fec8f49-52e4-462c-a871-351ff39f6dc1" Description="Provides additional documentation information to this element. Its content will be added as a &lt;remarks&gt; element to the generated code." Name="Remarks" DisplayName="Remarks" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="5f9c7b4d-0ba2-45b5-9ad1-43e03b1cf255" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.DataContractMessagePart" Name="DataContractMessagePart" DisplayName="Data Contract Message Part" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <BaseClass>
        <DomainClassMoniker Name="MessagePart" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="2243a296-c566-42fb-8052-35f0a291249e" Description="A cross-model reference to the associated data contract type on the data contract model." Name="Type" DisplayName="Type" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.VisualStudio.Modeling.Integration.Picker.ModelElementReferenceEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.VisualStudio.Modeling.Integration.ModelBusReferenceTypeConverter)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="Microsoft.VisualStudio.Modeling.Integration.Picker.SupplyFileBasedBrowserConfiguration">
              <Parameters>
                <AttributeParameter Value="&quot;Please choose a DataContract file&quot;" />
                <AttributeParameter Value=" &quot;DataContract files|*.datacontract&quot;" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="Microsoft.VisualStudio.Modeling.Integration.Picker.ApplyElementTypeLimitations">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.DataContracts.DataContract)" />
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.DataContracts.DataContractEnum)" />
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.DataContracts.DataContractCollection)" />
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.DataContracts.PrimitiveDataTypeCollection)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.CLSCompliant">
              <Parameters>
                <AttributeParameter Value="false" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.VisualStudio.Modeling.Integration/ModelBusReference" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="1be9f8fe-13c0-4d40-866a-8d613f69c476" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.PrimitiveMessagePart" Name="PrimitiveMessagePart" DisplayName="Primitive Message Part" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <BaseClass>
        <DomainClassMoniker Name="MessagePart" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="11e59170-7ebe-4d6d-b128-fc8449d1faa5" Description="A primitive .NET type." Name="Type" DisplayName="Type" DefaultValue="System.String" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors.TypeBrowser.FilteredTypeBrowser)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverterAttribute">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.TypeConverter)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.Workflow.ComponentModel.Design.TypeFilterProvider">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors.TypeBrowser.PublicPrimitiveTypeFilter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d99be5ca-4b9c-4197-ab2b-c111363edfe9" Description="Indicates whether or not this part can have a null value." Name="IsNullable" DisplayName="Is Nullable" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0c208891-8244-42d1-8568-3430d5bd3c84" Description="Indicates whether or not this part is a generic list collection. " Name="IsCollection" DisplayName="Is Collection" DefaultValue="" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="c2e3c28a-4faf-4831-8542-836ff69f17b0" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase" Name="MessageBase" DisplayName="Message Base" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Properties>
        <DomainProperty Id="078daf85-4e09-48dc-bcc6-068337ad1343" Description="The name of this message." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="82d7a5a3-7d88-41b6-b99e-58fc1c0242fe" Description="The XML namespace of this message." Name="Namespace" DisplayName="XML Namespace" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.Dsl.Integration.Design.XmlNamespaceStringConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3fbd8fb5-9313-40bd-bb89-41ebb31ad160" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageBase.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ebdcb4a2-8a9e-40bf-b219-8d5c51e27b2c" Description="Provides additional documentation information to this element. Its content will be added as a &lt;remarks&gt; element to the generated code." Name="Remarks" DisplayName="Remarks" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="ced5d2b3-cd2c-458c-9850-788dbb4a5f1c" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.XsdMessage" Name="XsdMessage" DisplayName="XSD Message" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.MessageContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
        <DomainClassMoniker Name="MessageBase" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="fe68a7c5-8173-4290-9b19-7495439b9ee7" Description="A reference to a complexType or element in an XSD file that represents this message." Name="Element" DisplayName="Element" Category="General">
          <Attributes>
            <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
              <Parameters>
                <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.DataContractRole" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.ServiceContracts.XsdElementPickerEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverterAttribute">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.TypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="695596b7-cc70-4644-8d91-4a641446a765" Description="Indicates if the message contains a wrapper element." Name="IsWrapped" DisplayName="Is Wrapped" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="71530164-07b8-460b-be36-729c10871d18" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.DataContractFault" Name="DataContractFault" DisplayName="Data Contract Fault" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <BaseClass>
        <DomainClassMoniker Name="Fault" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="5f0afdf3-b096-46b0-b311-729372d83358" Description="A cross-model reference to the associated data contract type on the data contract model." Name="Type" DisplayName="Type" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.VisualStudio.Modeling.Integration.Picker.ModelElementReferenceEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.VisualStudio.Modeling.Integration.ModelBusReferenceTypeConverter)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="Microsoft.VisualStudio.Modeling.Integration.Picker.SupplyFileBasedBrowserConfiguration">
              <Parameters>
                <AttributeParameter Value="&quot;Please choose a DataContract file&quot;" />
                <AttributeParameter Value=" &quot;DataContract files|*.datacontract&quot;" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="Microsoft.VisualStudio.Modeling.Integration.Picker.ApplyElementTypeLimitations">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.DataContracts.FaultContract)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.CLSCompliant">
              <Parameters>
                <AttributeParameter Value="false" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.VisualStudio.Modeling.Integration/ModelBusReference" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="cbe50d8d-a5dc-4a54-ae20-e347ebedd2b5" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.XsdElementFault" Name="XsdElementFault" DisplayName="XSD Element Fault" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.FaultContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
        <DomainClassMoniker Name="Fault" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="a78a1b4c-64d9-4d1f-a9f2-2628ff59e9c1" Description="A reference to a complexType or element in an XSD file that represents this fault." Name="Element" DisplayName="Element" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.ServiceContracts.XsdElementPickerEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverterAttribute">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.TypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="fc321a85-682a-47c1-ab36-ebdd439b3c7a" Description="Embedding relationship between the Model and Elements" Name="ServiceContractModelHasServiceContracts" DisplayName="Service Contract Model Has Service Contracts" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="9e8820d7-e00c-484a-9b39-1ee0e307510e" Description="" Name="ServiceContractModel" DisplayName="Service Contract Model" PropertyName="ServiceContracts" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Service Contracts">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContractModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="0d3776fa-ed1d-48e4-b5ca-18a7f0961fea" Description="" Name="ServiceContract" DisplayName="Service Contract" PropertyName="ServiceContractModel" Multiplicity="One" PropagatesDelete="true" Category="General" PropertyDisplayName="Service Contract Model">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContract" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="9810bec1-75ca-4600-847f-7e32604c2531" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasOperations" Name="ServiceContractModelHasOperations" DisplayName="Service Contract Model Has Operations" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="a58d0693-8972-4fb4-8b81-9052a90abfd4" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasOperations.ServiceContractModel" Name="ServiceContractModel" DisplayName="Service Contract Model" PropertyName="Operations" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Operations">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContractModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="fb4a732f-79c8-4576-8fad-0e13b46cfa32" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasOperations.Operation" Name="Operation" DisplayName="Operation" PropertyName="ServiceContractModel" Multiplicity="One" PropagatesDelete="true" Category="General" PropertyDisplayName="Service Contract Model">
          <RolePlayer>
            <DomainClassMoniker Name="Operation" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="096ae312-0e53-4109-8819-6eefa07035a1" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractReferencesOperations" Name="ServiceContractReferencesOperations" DisplayName="Service Contract References Operations" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Source>
        <DomainRole Id="1395dbef-7477-449b-acdf-f6be770630ae" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractReferencesOperations.ServiceContract" Name="ServiceContract" DisplayName="Service Contract" PropertyName="Operations" Category="General" PropertyDisplayName="Operations">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContract" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="b2e9f4f7-1e9c-49c6-ad18-52d55acb6ee3" Description="The service contract this operation is associated with." Name="Operation" DisplayName="Operation" PropertyName="ServiceContract" Multiplicity="ZeroOne" Category="General" PropertyDisplayName="Service Contract">
          <RolePlayer>
            <DomainClassMoniker Name="Operation" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="a95f0175-0f05-4a6e-80ba-9949cf8267a7" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasServices" Name="ServiceContractModelHasServices" DisplayName="Service Contract Model Has Services" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="f5e8c9e2-66a4-44ae-abed-2119b3e49b63" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasServices.ServiceContractModel" Name="ServiceContractModel" DisplayName="Service Contract Model" PropertyName="Services" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Services">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContractModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="95c39005-60a2-479a-ac80-e3606d8bdb4c" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasServices.Service" Name="Service" DisplayName="Service" PropertyName="ServiceContractModel" Multiplicity="One" PropagatesDelete="true" Category="General" PropertyDisplayName="Service Contract Model">
          <RolePlayer>
            <DomainClassMoniker Name="Service" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="a2a99593-2a34-4d79-9c31-8759acdcf4e1" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts" Name="ServiceReferencesServiceContract" DisplayName="Service References Service Contract" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Source>
        <DomainRole Id="0377668c-e435-4e95-bde4-e853ed1f3aa9" Description="The contract that the service implements." Name="Service" DisplayName="Service" PropertyName="ServiceContract" Multiplicity="One" Category="General" PropertyDisplayName="Service Contract">
          <RolePlayer>
            <DomainClassMoniker Name="Service" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="931190c0-2e6e-4636-8b65-e31f40d03461" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceReferencesServiceContract.ServiceContract" Name="ServiceContract" DisplayName="Service Contract" PropertyName="Service" Multiplicity="ZeroOne" Category="General" PropertyDisplayName="Service">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContract" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="780f41e2-b2eb-43e4-9797-1d4d6ed0ca84" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.OperationHasFaults" Name="OperationHasFaults" DisplayName="Operation Has Faults" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="bfcabfa4-2cdb-4b90-bb4b-eb5753e46720" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.OperationHasFaults.Operation" Name="Operation" DisplayName="Operation" PropertyName="Faults" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Faults">
          <RolePlayer>
            <DomainClassMoniker Name="Operation" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="f167f11b-328d-43ae-841e-90b25ddff053" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.OperationHasFaults.Fault" Name="Fault" DisplayName="Fault" PropertyName="Operation" Multiplicity="One" PropagatesDelete="true" Category="General" PropertyDisplayName="Operation">
          <RolePlayer>
            <DomainClassMoniker Name="Fault" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="46173368-dadd-4e71-b51f-780ba06e1111" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasMessages" Name="ServiceContractModelHasMessages" DisplayName="Service Contract Model Has Messages" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="feb77b0a-97f7-4672-8d85-a8898cab21c2" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasMessages.ServiceContractModel" Name="ServiceContractModel" DisplayName="Service Contract Model" PropertyName="Messages" PropagatesCopy="PropagateCopyToLinkOnly" Category="General" PropertyDisplayName="Messages">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceContractModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="3e9d92a4-46e8-4568-82ec-906e6d460892" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractModelHasMessages.MessageBase" Name="MessageBase" DisplayName="Message Base" PropertyName="ServiceContractModel" Multiplicity="One" PropagatesDelete="true" Category="General" PropertyDisplayName="Service Contract Model">
          <RolePlayer>
            <DomainClassMoniker Name="MessageBase" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="8295600c-96eb-44ad-ab6f-8bc00704d9e5" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.RequestReferencedByOperation" Name="RequestReferencedByOperation" DisplayName="Request Referenced By Operation" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Source>
        <DomainRole Id="8c611f7e-0ee1-4660-82c3-a72978df8452" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.RequestReferencedByOperation.MessageBase" Name="MessageBase" DisplayName="Message Base" PropertyName="RequestFor" Category="General" PropertyDisplayName="Request For">
          <RolePlayer>
            <DomainClassMoniker Name="MessageBase" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="9a1f5b7f-6504-4517-931b-6c5de6f390b1" Description="The message that functions as a request to this operation. These values can be controlled by connecting the messages on the design surface." Name="Operation" DisplayName="Operation" PropertyName="Request" Multiplicity="ZeroOne" Category="General" PropertyDisplayName="Request">
          <RolePlayer>
            <DomainClassMoniker Name="Operation" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="02d27431-b5de-4fed-80f3-ccbd93466f72" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.OperationReferencesResponse" Name="OperationReferencesResponse" DisplayName="Operation References Response" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
      <Source>
        <DomainRole Id="99e92426-c19f-4041-9277-3f9485ac5349" Description="The message that functions as a response to this operation. These values can be controlled by connecting the messages on the design surface." Name="Operation" DisplayName="Operation" PropertyName="Response" Multiplicity="ZeroOne" Category="General" PropertyDisplayName="Response">
          <RolePlayer>
            <DomainClassMoniker Name="Operation" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="c368ce4e-31cd-4d7a-a6e8-c8abd5db1a1c" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.OperationReferencesResponse.MessageBase" Name="MessageBase" DisplayName="Message Base" PropertyName="ResponseFor" Category="General" PropertyDisplayName="Response For">
          <RolePlayer>
            <DomainClassMoniker Name="MessageBase" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="ebb784ad-4d26-4df2-81fc-5d4fa935b63b" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageHasMessageParts" Name="MessageHasMessageParts" DisplayName="Message Has Message Parts" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="7f4f7430-6088-4035-ae29-fbb5ac17cbd3" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageHasMessageParts.MessageBase" Name="MessageBase" DisplayName="Message Base" PropertyName="MessageParts" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="MessageParts">
          <RolePlayer>
            <DomainClassMoniker Name="MessageBase" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5659501f-e9bf-4314-b48d-108a17271b40" Description="A cross-model reference to the associated data contract type on the data contract model." Name="MessagePart" DisplayName="Message" PropertyName="Message" Multiplicity="One" PropagatesDelete="true" Category="General" PropertyDisplayName="Message">
          <RolePlayer>
            <DomainClassMoniker Name="MessagePart" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <DomainEnumeration Name="BindingStyle" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.BindingStyle">
      <Literals>
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.BindingStyle.RPC" Name="RPC" Value="" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.BindingStyle.Document" Name="Document" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="ServiceEncoding" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceEncoding">
      <Literals>
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceEncoding.Literal" Name="Literal" Value="" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceEncoding.Encoding" Name="Encoding" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="IExtensionProvider" Namespace="Microsoft.Practices.Modeling.ExtensionProvider.Extension" />
    <ExternalType Name="ObjectExtenderContainer" Namespace="Microsoft.Practices.Modeling.ExtensionProvider.Extension" />
    <DomainEnumeration Name="MessageType" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageType">
      <Literals>
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageType.RequestMessage" Name="RequestMessage" Value="1" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageType.ResponseMessage" Name="ResponseMessage" Value="2" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="SerializerType" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType">
      <Literals>
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.XmlSerializer" Name="XmlSerializer" Value="" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.SerializerType.DataContractSerializer" Name="DataContractSerializer" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="ModelBusReference" Namespace="Microsoft.VisualStudio.Modeling.Integration" />
  </Types>
  <Shapes>
    <GeometryShape Id="766a0826-a8e9-45d7-92f3-7988c474d679" Description="Shape used to represent ExampleElements on a Diagram." Name="ServiceContractShape" DisplayName="Service Contract Shape" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Service Contract Shape" FillColor="Gainsboro" OutlineColor="DimGray" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="Rectangle">
      <Notes>The shape has a text decorator used to display the Name property of the mapped ExampleElement.</Notes>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\ServiceContract.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Service Contract" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
    </GeometryShape>
    <CompartmentShape Id="7a10309d-4936-4693-a7e9-08f86f17722a" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageShape" Name="MessageShape" DisplayName="Message Shape" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Message Shape" FillColor="Cornsilk" OutlineColor="DarkGoldenrod" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator" DisplayName="Expand Collapse Decorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Message" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\Message.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="White" Name="Parts" Title="Parts" />
    </CompartmentShape>
    <CompartmentShape Id="c3db09af-fa07-4427-9f19-d469015cec2d" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.OperationShape" Name="OperationShape" DisplayName="Operation Shape" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Operation Shape" FillColor="Thistle" OutlineColor="Indigo" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator" DisplayName="Expand Collapse Decorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Operation" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\Operation.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="White" Name="Faults" Title="Faults" />
    </CompartmentShape>
    <CompartmentShape Id="c5398f53-f61d-44f1-86d8-53c2a3b1c81e" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceShape" Name="ServiceShape" DisplayName="Service Shape" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Service Shape" FillColor="LightSteelBlue" OutlineColor="SteelBlue" InitialHeight="0.4" OutlineThickness="0.02" Geometry="Rectangle" IsSingleCompartmentHeaderVisible="false" DefaultExpandCollapseState="Collapsed">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Service" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\Service.bmp" />
      </ShapeHasDecorators>
    </CompartmentShape>
    <CompartmentShape Id="1ab94f18-2894-465e-9ed0-ee06d8eb34e0" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.XsdMessageShape" Name="XsdMessageShape" DisplayName="XSD Message Shape" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Xsd Message Shape" FillColor="Cornsilk" OutlineColor="DarkGoldenrod" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="XSD Message" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\Message.bmp" />
      </ShapeHasDecorators>
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="c35bedc8-cb46-472a-a3a0-77658cb0bf10" Description="Connect operations to service contracts" Name="OperationConnector" DisplayName="Operation Connector" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Operation Connector" Color="Indigo" Thickness="0.0125" />
    <Connector Id="98afe0b3-f852-4dfe-aaa4-cb409a67ee14" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceConnector" Name="ServiceConnector" DisplayName="Service Connector" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Service Connector" Color="SteelBlue" Thickness="0.0125" />
    <Connector Id="a33bbaef-56e9-4f72-826c-f117f004b232" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.MessageOperationConnector" Name="MessageOperationConnector" DisplayName="Message Operation Connector" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" FixedTooltipText="Message Operation Connector" Color="DarkGoldenrod" TargetEndStyle="EmptyArrow" Thickness="0.0125" />
  </Connectors>
  <XmlSerializationBehavior Name="ServiceContractDslSerializationBehavior" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts">
    <ClassData>
      <XmlClassData TypeName="ServiceContractModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="serviceContractModelMoniker" ElementName="serviceContractModel" MonikerTypeName="ServiceContractModelMoniker">
        <DomainClassMoniker Name="ServiceContractModel" />
        <ElementData>
          <XmlRelationshipData RoleElementName="serviceContracts">
            <DomainRelationshipMoniker Name="ServiceContractModelHasServiceContracts" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="operations">
            <DomainRelationshipMoniker Name="ServiceContractModelHasOperations" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="services">
            <DomainRelationshipMoniker Name="ServiceContractModelHasServices" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="implementationTechnology">
            <DomainPropertyMoniker Name="ServiceContractModel/ImplementationTechnology" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="projectMappingTable">
            <DomainPropertyMoniker Name="ServiceContractModel/ProjectMappingTable" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="messages">
            <DomainRelationshipMoniker Name="ServiceContractModelHasMessages" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="ServiceContractModel/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="serializerType">
            <DomainPropertyMoniker Name="ServiceContractModel/SerializerType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ServiceContract" MonikerAttributeName="name" SerializeId="true" MonikerElementName="serviceContractMoniker" ElementName="serviceContract" MonikerTypeName="ServiceContractMoniker">
        <DomainClassMoniker Name="ServiceContract" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="ServiceContract/Name" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="operations">
            <DomainRelationshipMoniker Name="ServiceContractReferencesOperations" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="ServiceContract/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="ServiceContract/ObjectExtenderContainer" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="remarks">
            <DomainPropertyMoniker Name="ServiceContract/Remarks" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractModelHasServiceContracts" MonikerAttributeName="" MonikerElementName="serviceContractModelHasServiceContractsMoniker" ElementName="serviceContractModelHasServiceContracts" MonikerTypeName="ServiceContractModelHasServiceContractsMoniker">
        <DomainRelationshipMoniker Name="ServiceContractModelHasServiceContracts" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractShape" MonikerAttributeName="" MonikerElementName="serviceContractShapeMoniker" ElementName="serviceContractShape" MonikerTypeName="ServiceContractShapeMoniker">
        <GeometryShapeMoniker Name="ServiceContractShape" />
      </XmlClassData>
      <XmlClassData TypeName="OperationConnector" MonikerAttributeName="" MonikerElementName="operationConnectorMoniker" ElementName="operationConnector" MonikerTypeName="OperationConnectorMoniker">
        <ConnectorMoniker Name="OperationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractDiagram" MonikerAttributeName="" MonikerElementName="serviceContractDiagramMoniker" ElementName="serviceContractDiagram" MonikerTypeName="ServiceContractDiagramMoniker">
        <DiagramMoniker Name="ServiceContractDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="Operation" MonikerAttributeName="" SerializeId="true" MonikerElementName="operationMoniker" ElementName="operation" MonikerTypeName="OperationMoniker">
        <DomainClassMoniker Name="Operation" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Operation/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="action">
            <DomainPropertyMoniker Name="Operation/Action" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isOneWay">
            <DomainPropertyMoniker Name="Operation/IsOneWay" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="Operation/ObjectExtenderContainer" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="faults">
            <DomainRelationshipMoniker Name="OperationHasFaults" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="response">
            <DomainRelationshipMoniker Name="OperationReferencesResponse" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="remarks">
            <DomainPropertyMoniker Name="Operation/Remarks" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractModelHasOperations" MonikerAttributeName="" MonikerElementName="serviceContractModelHasOperationsMoniker" ElementName="serviceContractModelHasOperations" MonikerTypeName="ServiceContractModelHasOperationsMoniker">
        <DomainRelationshipMoniker Name="ServiceContractModelHasOperations" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractReferencesOperations" MonikerAttributeName="" MonikerElementName="serviceContractReferencesOperationsMoniker" ElementName="serviceContractReferencesOperations" MonikerTypeName="ServiceContractReferencesOperationsMoniker">
        <DomainRelationshipMoniker Name="ServiceContractReferencesOperations" />
      </XmlClassData>
      <XmlClassData TypeName="Service" MonikerAttributeName="" SerializeId="true" MonikerElementName="serviceMoniker" ElementName="service" MonikerTypeName="ServiceMoniker">
        <DomainClassMoniker Name="Service" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Service/Name" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="serviceContract">
            <DomainRelationshipMoniker Name="ServiceReferencesServiceContract" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="Service/ObjectExtenderContainer" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="Service/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="remarks">
            <DomainPropertyMoniker Name="Service/Remarks" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractModelHasServices" MonikerAttributeName="" MonikerElementName="serviceContractModelHasServicesMoniker" ElementName="serviceContractModelHasServices" MonikerTypeName="ServiceContractModelHasServicesMoniker">
        <DomainRelationshipMoniker Name="ServiceContractModelHasServices" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceReferencesServiceContract" MonikerAttributeName="" MonikerElementName="serviceReferencesServiceContractMoniker" ElementName="serviceReferencesServiceContract" MonikerTypeName="ServiceReferencesServiceContractMoniker">
        <DomainRelationshipMoniker Name="ServiceReferencesServiceContract" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceConnector" MonikerAttributeName="" MonikerElementName="serviceConnectorMoniker" ElementName="serviceConnector" MonikerTypeName="ServiceConnectorMoniker">
        <ConnectorMoniker Name="ServiceConnector" />
      </XmlClassData>
      <XmlClassData TypeName="Message" MonikerAttributeName="" SerializeId="true" MonikerElementName="messageMoniker" ElementName="message" MonikerTypeName="MessageMoniker">
        <DomainClassMoniker Name="Message" />
      </XmlClassData>
      <XmlClassData TypeName="MessagePart" MonikerAttributeName="" MonikerElementName="messagePartMoniker" ElementName="messagePart" MonikerTypeName="MessagePartMoniker">
        <DomainClassMoniker Name="MessagePart" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="MessagePart/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="MessagePart/ObjectExtenderContainer" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="remarks">
            <DomainPropertyMoniker Name="MessagePart/Remarks" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="MessageShape" MonikerAttributeName="" MonikerElementName="messageShapeMoniker" ElementName="messageShape" MonikerTypeName="MessageShapeMoniker">
        <CompartmentShapeMoniker Name="MessageShape" />
      </XmlClassData>
      <XmlClassData TypeName="MessageOperationConnector" MonikerAttributeName="" MonikerElementName="messageOperationConnectorMoniker" ElementName="messageOperationConnector" MonikerTypeName="MessageOperationConnectorMoniker">
        <ConnectorMoniker Name="MessageOperationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="OperationShape" MonikerAttributeName="" MonikerElementName="operationShapeMoniker" ElementName="operationShape" MonikerTypeName="OperationShapeMoniker">
        <CompartmentShapeMoniker Name="OperationShape" />
      </XmlClassData>
      <XmlClassData TypeName="Fault" MonikerAttributeName="" MonikerElementName="faultMoniker" ElementName="fault" MonikerTypeName="FaultMoniker">
        <DomainClassMoniker Name="Fault" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Fault/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="Fault/ObjectExtenderContainer" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="remarks">
            <DomainPropertyMoniker Name="Fault/Remarks" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="OperationHasFaults" MonikerAttributeName="" MonikerElementName="operationHasFaultsMoniker" ElementName="operationHasFaults" MonikerTypeName="OperationHasFaultsMoniker">
        <DomainRelationshipMoniker Name="OperationHasFaults" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceShape" MonikerAttributeName="" MonikerElementName="serviceShapeMoniker" ElementName="serviceShape" MonikerTypeName="ServiceShapeMoniker">
        <CompartmentShapeMoniker Name="ServiceShape" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractMessagePart" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataContractMessagePartMoniker" ElementName="dataContractMessagePart" MonikerTypeName="DataContractMessagePartMoniker">
        <DomainClassMoniker Name="DataContractMessagePart" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="DataContractMessagePart/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="PrimitiveMessagePart" MonikerAttributeName="" SerializeId="true" MonikerElementName="primitiveMessagePartMoniker" ElementName="primitiveMessagePart" MonikerTypeName="PrimitiveMessagePartMoniker">
        <DomainClassMoniker Name="PrimitiveMessagePart" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="PrimitiveMessagePart/Type" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isNullable">
            <DomainPropertyMoniker Name="PrimitiveMessagePart/IsNullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isCollection">
            <DomainPropertyMoniker Name="PrimitiveMessagePart/IsCollection" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="MessageBase" MonikerAttributeName="" MonikerElementName="messageBaseMoniker" ElementName="messageBase" MonikerTypeName="MessageBaseMoniker">
        <DomainClassMoniker Name="MessageBase" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="MessageBase/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="MessageBase/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="MessageBase/ObjectExtenderContainer" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="requestFor">
            <DomainRelationshipMoniker Name="RequestReferencedByOperation" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="messageParts">
            <DomainRelationshipMoniker Name="MessageHasMessageParts" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="remarks">
            <DomainPropertyMoniker Name="MessageBase/Remarks" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="XsdMessage" MonikerAttributeName="" SerializeId="true" MonikerElementName="xsdMessageMoniker" ElementName="xsdMessage" MonikerTypeName="XsdMessageMoniker">
        <DomainClassMoniker Name="XsdMessage" />
        <ElementData>
          <XmlPropertyData XmlName="element">
            <DomainPropertyMoniker Name="XsdMessage/Element" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isWrapped">
            <DomainPropertyMoniker Name="XsdMessage/IsWrapped" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ServiceContractModelHasMessages" MonikerAttributeName="" MonikerElementName="serviceContractModelHasMessagesMoniker" ElementName="serviceContractModelHasMessages" MonikerTypeName="ServiceContractModelHasMessagesMoniker">
        <DomainRelationshipMoniker Name="ServiceContractModelHasMessages" />
      </XmlClassData>
      <XmlClassData TypeName="RequestReferencedByOperation" MonikerAttributeName="" MonikerElementName="requestReferencedByOperationMoniker" ElementName="requestReferencedByOperation" MonikerTypeName="RequestReferencedByOperationMoniker">
        <DomainRelationshipMoniker Name="RequestReferencedByOperation" />
      </XmlClassData>
      <XmlClassData TypeName="OperationReferencesResponse" MonikerAttributeName="" MonikerElementName="operationReferencesResponseMoniker" ElementName="operationReferencesResponse" MonikerTypeName="OperationReferencesResponseMoniker">
        <DomainRelationshipMoniker Name="OperationReferencesResponse" />
      </XmlClassData>
      <XmlClassData TypeName="MessageHasMessageParts" MonikerAttributeName="" MonikerElementName="messageHasMessagePartsMoniker" ElementName="messageHasMessageParts" MonikerTypeName="MessageHasMessagePartsMoniker">
        <DomainRelationshipMoniker Name="MessageHasMessageParts" />
      </XmlClassData>
      <XmlClassData TypeName="XsdMessageShape" MonikerAttributeName="" MonikerElementName="xsdMessageShapeMoniker" ElementName="xsdMessageShape" MonikerTypeName="XsdMessageShapeMoniker">
        <CompartmentShapeMoniker Name="XsdMessageShape" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractFault" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataContractFaultMoniker" ElementName="dataContractFault" MonikerTypeName="DataContractFaultMoniker">
        <DomainClassMoniker Name="DataContractFault" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="DataContractFault/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="XsdElementFault" MonikerAttributeName="" MonikerElementName="xsdElementFaultMoniker" ElementName="xsdElementFault" MonikerTypeName="XsdElementFaultMoniker">
        <DomainClassMoniker Name="XsdElementFault" />
        <ElementData>
          <XmlPropertyData XmlName="element">
            <DomainPropertyMoniker Name="XsdElementFault/Element" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="ServiceContractExplorer">
    <CustomNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\Message.bmp">
        <Class>
          <DomainClassMoniker Name="Message" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\Service.bmp">
        <Class>
          <DomainClassMoniker Name="Service" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\Operation.bmp">
        <Class>
          <DomainClassMoniker Name="Operation" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\ServiceContract.bmp">
        <Class>
          <DomainClassMoniker Name="ServiceContract" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\Message.bmp">
        <Class>
          <DomainClassMoniker Name="XsdMessage" />
        </Class>
      </ExplorerNodeSettings>
    </CustomNodeSettings>
  </ExplorerBehavior>
  <ConnectionBuilders>
    <ConnectionBuilder Name="ConnectionBuilder" IsCustom="true">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="OperationReferencesResponse" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Operation" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="MessageBase" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="RequestReferencedByOperation" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="MessageBase" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Operation" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="ServiceReferencesServiceContract" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Service" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="ServiceContract" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="ServiceContractReferencesOperations" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="ServiceContract" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Operation" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="a41ec408-78bc-4bb2-80b6-9717d7d24b04" Description="Description for Microsoft.Practices.ServiceFactory.ServiceContracts.ServiceContractDiagram" Name="ServiceContractDiagram" DisplayName="Service Contract Diagram" Namespace="Microsoft.Practices.ServiceFactory.ServiceContracts" GeneratesDoubleDerived="true">
    <Class>
      <DomainClassMoniker Name="ServiceContractModel" />
    </Class>
    <ShapeMaps>
      <ShapeMap>
        <DomainClassMoniker Name="ServiceContract" />
        <ParentElementPath>
          <DomainPath>ServiceContractModelHasServiceContracts.ServiceContractModel/!ServiceContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ServiceContractShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ServiceContract/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="ServiceContractShape" />
      </ShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Message" />
        <ParentElementPath>
          <DomainPath>ServiceContractModelHasMessages.ServiceContractModel/!ServiceContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="MessageShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="MessageBase/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="MessageShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="MessageShape/Parts" />
          <ElementsDisplayed>
            <DomainPath>MessageHasMessageParts.MessageParts/!MessagePart</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="MessagePart/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Operation" />
        <ParentElementPath>
          <DomainPath>ServiceContractModelHasOperations.ServiceContractModel/!ServiceContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="OperationShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Operation/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="OperationShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="OperationShape/Faults" />
          <ElementsDisplayed>
            <DomainPath>OperationHasFaults.Faults/!Fault</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Fault/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Service" />
        <ParentElementPath>
          <DomainPath>ServiceContractModelHasServices.ServiceContractModel/!ServiceContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ServiceShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Service/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="ServiceShape" />
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="XsdMessage" />
        <ParentElementPath>
          <DomainPath>ServiceContractModelHasMessages.ServiceContractModel/!ServiceContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="XsdMessageShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="MessageBase/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="XsdMessageShape" />
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="OperationConnector" />
        <DomainRelationshipMoniker Name="ServiceContractReferencesOperations" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ServiceConnector" />
        <DomainRelationshipMoniker Name="ServiceReferencesServiceContract" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="MessageOperationConnector" />
        <DomainRelationshipMoniker Name="OperationReferencesResponse" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="MessageOperationConnector" />
        <DomainRelationshipMoniker Name="RequestReferencedByOperation" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="servicecontract" EditorGuid="74a1ee75-1c00-4e23-9e74-1e24af934cfd" usesStickyToolboxItems="true">
    <RootClass>
      <DomainClassMoniker Name="ServiceContractModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="true">
      <XmlSerializationBehaviorMoniker Name="ServiceContractDslSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="Service Contract Tools">
      <ElementTool Name="Service" ToolboxIcon="Resources\Service.bmp" Caption="Service" Tooltip="Represents the implementation of the contract. Unlike class definitions, this shape is not intended to implement multiple service interfaces (contracts). This shape can only connect to a single service contract." HelpKeyword="Service">
        <DomainClassMoniker Name="Service" />
      </ElementTool>
      <ElementTool Name="ServiceContract" ToolboxIcon="Resources\ServiceContract.bmp" Caption="Service Contract" Tooltip="Represents the service interface that associates multiple operations (methods). This shape connects to a single service shape and can connect to multiple operation shapes." HelpKeyword="ServiceContract">
        <DomainClassMoniker Name="ServiceContract" />
      </ElementTool>
      <ElementTool Name="Operation" ToolboxIcon="Resources\Operation.bmp" Caption="Operation" Tooltip="Represents a method that can be invoked on the service. This shape connects to a single service contract and one or two message contracts." HelpKeyword="Operation">
        <DomainClassMoniker Name="Operation" />
      </ElementTool>
      <ElementTool Name="Message" ToolboxIcon="Resources\Message.bmp" Caption="Message" Tooltip="Represents a request or response message that contains data contracts or primitive types (not types from an XSD). This shape connects to a single operation. To make it a request message, drag the connector shape from the message shape to the operation shape. To make it a response message, drag the connector shape from the operation shape to the message shape." HelpKeyword="Message">
        <DomainClassMoniker Name="Message" />
      </ElementTool>
      <ElementTool Name="XsdMessage" ToolboxIcon="Resources\Message.bmp" Caption="XSD Message" Tooltip="Represents a reference to an element or a complex type within an XSD file that represents a request or response message. To use this shape, you need to add a schema as an existing item to the Schemas folder of the solution. This shape connects to a single operation. To make it a request message, drag the connector shape from the message shape to the operation shape. To make it a response message, drag the connector shape from the operation shape to the message shape." HelpKeyword="XsdMessage">
        <DomainClassMoniker Name="XsdMessage" />
      </ElementTool>
      <ConnectionTool Name="ConnectTool" ToolboxIcon="Resources\ConnectorTool.bmp" Caption="Connector" Tooltip="Makes valid connections between shapes on the design surface. To use the connector, click the Connector shape in the Toolbox, click the first shape, and then click the second shape. For more information about connecting message shapes, hover over the Message or XSD Message shapes in the Toolbox." HelpKeyword="ConnectTool">
        <ConnectionBuilderMoniker Name="ServiceContractDsl/ConnectionBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="false" UsesSave="false" UsesCustom="true" UsesLoad="false" />
    <DiagramMoniker Name="ServiceContractDiagram" />
  </Designer>
  <Explorer ExplorerGuid="71faf429-f9c6-4209-8c26-244724418087" Title="Service Contract Explorer">
    <ExplorerBehaviorMoniker Name="ServiceContractDsl/ServiceContractExplorer" />
  </Explorer>
</Dsl>