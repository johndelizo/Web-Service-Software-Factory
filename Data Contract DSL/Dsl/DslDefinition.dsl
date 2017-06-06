<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="f79dc996-e28c-4367-bde9-4e5d7b9a3870" Description="This is the Data Contract Model for the Web Service Software Factory." Name="DataContractDsl" DisplayName="Data Contract Model" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" ProductName="Web Service Software Factory: Modeling Edition" CompanyName="Microsoft" PackageGuid="1a168a56-ee16-4520-a071-3431a82d3e1f" PackageNamespace="Microsoft.Practices.ServiceFactory.DataContracts" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="5200c72f-45bf-45e7-adef-3fab8302f9ba" Description="The root in which all other elements are embedded. Appears as a diagram." Name="DataContractModel" DisplayName="Data Contract Model" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Properties>
        <DomainProperty Id="e9f1452b-1136-4702-9171-be75e6590701" Description="This associates a platform technology with each of the shapes on the design surface. As a result, each shape may acquire additional properties specific for the chosen technology." Name="ImplementationTechnology" DisplayName="Implementation Technology" Category="General">
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
        <DomainProperty Id="799673b2-9e6f-4951-aefd-4b62f0c9b562" Description="The named entries in the ProjectMapping.xml file. A new entry is created each time you create an implementation project structure. The Project Mapping Table entry associates shapes on the design surface with the projects that will contain the code for those shapes." Name="ProjectMappingTable" DisplayName="Project Mapping Table" Category="General">
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
        <DomainProperty Id="fc50fce3-bc71-4dcc-a73f-5d3108647c57" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractModel.Name" Name="Name" DisplayName="Name" Category="General" IsElementName="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6a95c677-4a65-4b89-b3b8-43973b86183e" Description="This value represents the default XML namespace that will be applied to each shape added to the design surface." Name="Namespace" DisplayName="XML Namespace" Category="General">
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
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Contract" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>DataContractModelHasContracts.Contracts</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="04635e26-5a8d-4061-85d4-c04467c9bc42" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContract" Name="DataContract" DisplayName="Data Contract" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.DataContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
        <DomainClassMoniker Name="DataContractBase" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="PrimitiveDataType" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>DataContractHasDataMembers.DataMembers</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ReferenceDataType" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>DataContractHasDataMembers.DataMembers</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="e1747dbd-3b81-420b-957d-1b77bad6a33e" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.ModelElementReference" Name="ModelElementReference" DisplayName="Model Element Reference" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="DataMember" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="ed001487-c6e3-4e6b-93ee-7337b9027d72" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.ModelElementReference.Model Element Guid" Name="ModelElementGuid" DisplayName="Model Element Guid" Category="General" GetterAccessModifier="Assembly" SetterAccessModifier="Assembly" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0af438e9-86ae-47e3-90a4-75f89bf63a8c" Description="A cross-model reference to the associated data contract type." Name="Type" DisplayName="Type" Category="General" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="b9b9dbf7-eec8-4181-8a3a-81ad1e4e63bf" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.PrimitiveDataType" Name="PrimitiveDataType" DisplayName="Primitive Data Type" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="DataMember" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="5404b15f-f07f-4ae7-b701-65aff946cd8b" Description="Indicates whether or not this part can have a null value." Name="IsNullable" DisplayName="Is Nullable" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e7c05794-884c-453e-95a2-6a6ae5bfd063" Description="A .NET primitive type." Name="Type" DisplayName="Type" DefaultValue="System.String" Category="General">
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
      </Properties>
    </DomainClass>
    <DomainClass Id="4cac4673-ab9f-43b8-b2e0-122262ec97c8" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.FaultContract" Name="FaultContract" DisplayName="Fault Contract" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.FaultContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
        <DomainClassMoniker Name="Contract" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="PrimitiveDataType" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>FaultContractHasDataMembers.DataMembers</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="c932bc1a-8a9b-4fd8-b95e-b307951d8cb4" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractBase" Name="DataContractBase" DisplayName="Data Contract Base" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="Contract" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="03aed388-b285-423b-a2c0-557118ae35cb" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractCollection" Name="DataContractCollection" DisplayName="Data Contract Collection" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="DataContractCollectionBase" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="1223d55d-832f-43ca-80c0-f3ca789ede57" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractEnum" Name="DataContractEnum" DisplayName="Data Contract Enumeration" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.DataContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
        <DomainClassMoniker Name="DataContractBase" />
      </BaseClass>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="EnumNamedValue" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>DataContractEnumHasEnumNamedValues.EnumNamedValues</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="57b615db-0b54-476b-a04a-d4dec6666e94" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.EnumNamedValue" Name="EnumNamedValue" DisplayName="Value" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Properties>
        <DomainProperty Id="9f96c3f4-3f45-4c67-9759-c67dba91ff01" Description="The value of the enum value." Name="Value" DisplayName="Value" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fe3492af-d412-4641-b2f0-27cdeac132d3" Description="The name of the enum value." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="b29bc5c1-1e94-443e-8962-782c95966279" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.ExtensibleModelElement" Name="ExtensibleModelElement" DisplayName="Extensible Model Element" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Properties>
        <DomainProperty Id="3e1aec86-c60b-4dc1-a30c-c9341b31e131" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.ExtensibleModelElement.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ed45a70b-11f9-4e36-a95c-70d378e7c42d" Description="The name of this model element." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d8b6637c-6207-4e13-aeb6-b084f1eecdb2" Description="Provides additional documentation information to this element. Its content will be added as a &lt;remarks&gt; element to the generated code." Name="Remarks" DisplayName="Remarks" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="98d1cea1-fbc0-4b05-93c6-76a3e88889fa" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataMember" Name="DataMember" DisplayName="Data Member" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="ExtensibleModelElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="661025df-2833-47f0-8c1b-694efc4c1218" Description="The default value of true ensures this member is serialized into messages. Otherwise, it will not be serialized." Name="IsDataMember" DisplayName="Is Data Member" DefaultValue="true" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e1f7793c-31b2-4b6b-a60a-b9cf3379a9ed" Description="If this member represents a collection, this value controls the type of collection. None indicates that this member is not a collection." Name="CollectionType" DisplayName="Collection Type" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.DataContracts.CollectionTypesEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverterAttribute">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.DataContracts.CollectionTypesConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Type" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="19fca223-ff6b-4096-9d6c-2ea2db247b78" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.PrimitiveDataTypeCollection" Name="PrimitiveDataTypeCollection" DisplayName="Primitive Data Type Collection" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="DataContractCollectionBase" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="06027595-4a6d-4a1a-9675-ebe46a1f25a8" Description="The .NET primitive that defines this collection’s type. " Name="ItemType" DisplayName="Item Type" Category="General">
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
      </Properties>
    </DomainClass>
    <DomainClass Id="87149ac1-6fed-4095-9ced-8c54a8d94c6d" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractCollectionBase" Name="DataContractCollectionBase" DisplayName="Data Contract Collection Base" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Attributes>
        <ClrAttribute Name="Microsoft.Practices.Modeling.CodeGeneration.Metadata.ProjectMappingRoleAttribute">
          <Parameters>
            <AttributeParameter Value="Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Enums.ServiceFactoryRoleType.DataContractRole" />
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
        <DomainClassMoniker Name="DataContractBase" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="8c9f551e-afb1-4c86-bf9f-be3341429813" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.Contract" Name="Contract" DisplayName="Contract" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="ExtensibleModelElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="e79fac2e-3798-467c-b858-2dbc0e6f68c7" Description="The XML namespace of this data/fault contract." Name="Namespace" DisplayName="XML Namespace" Category="General">
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
      </Properties>
    </DomainClass>
    <DomainClass Id="5fc92e8f-fd51-439c-ba82-561f865a1c18" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.ReferenceDataType" Name="ReferenceDataType" DisplayName="Reference Data Type" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <BaseClass>
        <DomainClassMoniker Name="DataMember" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="7f9c814c-dafe-4708-b522-94435a71b33f" Description="A cross-model reference to the associated data contract type on the data contract model." Name="Type" DisplayName="Type" Category="General">
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
  </Classes>
  <Relationships>
    <DomainRelationship Id="4be12864-5768-498d-8a27-5aa73581b446" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractEnumHasEnumNamedValues" Name="DataContractEnumHasEnumNamedValues" DisplayName="Data Contract Enum Has Enum Named Values" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="2bc152fd-42a6-4af5-9a68-bce8359243f6" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractEnumHasEnumNamedValues.DataContractEnum" Name="DataContractEnum" DisplayName="Data Contract Enum" PropertyName="EnumNamedValues" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Values">
          <RolePlayer>
            <DomainClassMoniker Name="DataContractEnum" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="72493a96-b3ae-4f4c-800d-f9c2965920af" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractEnumHasEnumNamedValues.EnumNamedValue" Name="EnumNamedValue" DisplayName="Enum Named Value" PropertyName="DataContractEnum" Multiplicity="ZeroOne" PropagatesDelete="true" Category="General" PropertyDisplayName="Data Contract Enum">
          <RolePlayer>
            <DomainClassMoniker Name="EnumNamedValue" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e24f9f24-0666-49c9-b9e0-6731e355e922" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractModelHasContracts" Name="DataContractModelHasContracts" DisplayName="Data Contract Model Has Contracts" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="7d834c44-11e3-4c14-a313-e1158beacdcf" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractModelHasContracts.DataContractModel" Name="DataContractModel" DisplayName="Data Contract Model" PropertyName="Contracts" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Contracts">
          <RolePlayer>
            <DomainClassMoniker Name="DataContractModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="b3b2c8c6-6520-493e-ad82-5cdb4c580ba7" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractModelHasContracts.Contract" Name="Contract" DisplayName="Contract" PropertyName="DataContractModel" Multiplicity="One" PropagatesDelete="true" Category="General" PropertyDisplayName="Data Contract Model">
          <RolePlayer>
            <DomainClassMoniker Name="Contract" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="865c7878-5838-48b8-ad89-afe533896900" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractHasDataMembers" Name="DataContractHasDataMembers" DisplayName="Data Contract Has Data Members" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="8c7b6548-289a-4d45-8c5c-3d86569e58f2" Description="A reference to a data contract shape that represents this collection’s type. When this value is defined, a connector will associate the two shapes on the design surface." Name="DataContract" DisplayName="Data Contract" PropertyName="DataMembers" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Members">
          <RolePlayer>
            <DomainClassMoniker Name="DataContract" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="13d406e6-6ea2-4236-8606-ffe8bf899b1b" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractHasDataMembers.DataMember" Name="DataMember" DisplayName="Data Member" PropertyName="DataContract" Multiplicity="ZeroOne" PropagatesDelete="true" Category="General" PropertyDisplayName="Data Contract">
          <RolePlayer>
            <DomainClassMoniker Name="DataMember" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="9945a887-c2ba-4161-bcc3-84584e4cd36c" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractBaseCanBeContainedOnContracts" Name="DataContractBaseCanBeContainedOnContracts" DisplayName="Data Contract Base Can Be Contained On Contracts" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" AllowsDuplicates="true">
      <Source>
        <DomainRole Id="d4fb32b7-e696-4892-8a27-e41cdd8e1525" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractBaseCanBeContainedOnContracts.DataContractBase" Name="DataContractBase" DisplayName="Data Contract Base" PropertyName="Contracts" Category="General" PropertyDisplayName="Contracts">
          <RolePlayer>
            <DomainClassMoniker Name="DataContractBase" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="d9f0ffca-dae8-47c0-bbc6-2b76382d4f92" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractBaseCanBeContainedOnContracts.Contract" Name="Contract" DisplayName="Contract" PropertyName="DataContractElements" Category="General" PropertyDisplayName="Data Contract Elements">
          <RolePlayer>
            <DomainClassMoniker Name="Contract" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="43328c47-57c3-4017-9c01-66b6a2b45c42" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractBaseIsBeingReferenceOnDataContractCollections" Name="DataContractBaseIsBeingReferenceOnDataContractCollections" DisplayName="Data Contract Base Is Being Reference On Data Contract Collections" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
      <Source>
        <DomainRole Id="6b9d4acc-bc0c-4d88-b8d0-7eee05d6f440" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractBaseIsBeingReferenceOnDataContractCollections.DataContractBase" Name="DataContractBase" DisplayName="Data Contract Base" PropertyName="DataContractCollections" Category="General" PropertyDisplayName="Data Contract Collections">
          <RolePlayer>
            <DomainClassMoniker Name="DataContractBase" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="ef08d2a1-b33c-4a10-9b6f-a6aa99ee5806" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractBaseIsBeingReferenceOnDataContractCollections.DataContractCollection" Name="DataContractCollection" DisplayName="Data Contract Collection" PropertyName="DataContract" Multiplicity="ZeroOne" Category="General" PropertyDisplayName="Data Contract">
          <RolePlayer>
            <DomainClassMoniker Name="DataContractCollection" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="40410903-3e65-4c16-9b27-16edad397c71" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.FaultContractHasDataMembers" Name="FaultContractHasDataMembers" DisplayName="Fault Contract Has Data Members" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" IsEmbedding="true">
      <Source>
        <DomainRole Id="ae9c5707-e70a-406d-82f4-bb441eb9ad33" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.FaultContractHasDataMembers.FaultContract" Name="FaultContract" DisplayName="Fault Contract" PropertyName="DataMembers" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Members">
          <RolePlayer>
            <DomainClassMoniker Name="FaultContract" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="37ffe634-fb9e-4697-9f80-ed5272876b49" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.FaultContractHasDataMembers.DataMember" Name="DataMember" DisplayName="Data Member" PropertyName="FaultContract" Multiplicity="ZeroOne" PropagatesDelete="true" Category="General" PropertyDisplayName="Fault Contract">
          <RolePlayer>
            <DomainClassMoniker Name="DataMember" />
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
    <ExternalType Name="IExtensionProvider" Namespace="Microsoft.Practices.Modeling.ExtensionProvider.Extension" />
    <ExternalType Name="ObjectExtenderContainer" Namespace="Microsoft.Practices.Modeling.ExtensionProvider.Extension" />
    <ExternalType Name="ElementPropertyType" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" />
    <DomainEnumeration Name="Multiplicity" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.Multiplicity">
      <Literals>
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.Multiplicity.Single" Name="Single" Value="0" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.Multiplicity.Multiple" Name="Multiple" Value="2" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="Color" Namespace="System.Drawing" />
    <ExternalType Name="Type" Namespace="System" />
    <ExternalType Name="ModelBusReference" Namespace="Microsoft.VisualStudio.Modeling.Integration" />
    <ExternalType Name="DashStyle" Namespace="System.Drawing.Drawing2D" />
  </Types>
  <Shapes>
    <CompartmentShape Id="e25789d5-3d71-49e9-a477-0644175a2f06" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractCompartmentShape" Name="DataContractCompartmentShape" DisplayName="DataContractCompartmentShape" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Drag onto the model surface to add a Data Contract shape." FillColor="Cornsilk" OutlineColor="DarkGoldenrod" InitialWidth="2" InitialHeight="0.4" OutlineThickness="0.0125" ExposesOutlineDashStyleAsProperty="true" ExposesOutlineThicknessAsProperty="true" Geometry="Rectangle">
      <Properties>
        <DomainProperty Id="4ade08f0-d559-45b3-9cb5-975b87dbf446" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractCompartmentShape.Outline Dash Style" Name="OutlineDashStyle" DisplayName="Outline Dash Style" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing.Drawing2D/DashStyle" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ea6fca1f-99c4-497d-9670-40e7bc6da90c" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractCompartmentShape.Outline Thickness" Name="OutlineThickness" DisplayName="Outline Thickness" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Single" />
          </Type>
        </DomainProperty>
      </Properties>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator" DisplayName="Expand Collapse Decorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Data Contract" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\DataContract.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="White" Name="DataMembersCompartment" Title="Members" EntryTextColor="DarkGoldenrod" />
    </CompartmentShape>
    <CompartmentShape Id="44c0d536-28ad-4490-a607-1246bccbb1e1" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.FaultContractCompartmentShape" Name="FaultContractCompartmentShape" DisplayName="Fault Contract Compartment Shape" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Drag onto the model surface to add a Fault Contract shape." FillColor="LightCoral" OutlineColor="IndianRed" InitialWidth="2" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator" DisplayName="Expand Collapse Decorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Fault Contract" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\FaultContract.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="White" Name="FaultMembersCompartment" Title="Members" EntryTextColor="IndianRed" />
    </CompartmentShape>
    <CompartmentShape Id="edd3576a-0334-4d57-ac2a-cc4a245ba6d5" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractEnumCompartmentShape" Name="DataContractEnumCompartmentShape" DisplayName="Data Contract Enum Compartment Shape" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Drag onto the model surface to add a Data Contract Enumeration shape." FillColor="Cornsilk" OutlineColor="DarkGoldenrod" InitialWidth="2" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapseDecorator" DisplayName="Expand Collapse Decorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Data Contract Enumeration" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\DataContract.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="White" Name="ValuesCompartment" Title="Values" EntryTextColor="DarkGoldenrod" />
    </CompartmentShape>
    <GeometryShape Id="4f90ce7d-13e5-4ec4-845b-2d43dd8f18b7" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractCollectionShape" Name="DataContractCollectionShape" DisplayName="Data Contract Collection Shape" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Data Contract Collection Shape" FillColor="Cornsilk" OutlineColor="DarkGoldenrod" InitialWidth="2" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Data Contract Collection" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\DataContract.bmp" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="44ec79e7-6921-4b82-811f-2515ff488210" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.PrimitiveDataTypeCollectionShape" Name="PrimitiveDataTypeCollectionShape" DisplayName="Primitive Data Type Collection Shape" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Primitive Data Type Collection Shape" FillColor="Cornsilk" OutlineColor="DarkGoldenrod" InitialWidth="2" InitialHeight="0.4" OutlineThickness="0.0125" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.14">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Primitive Data Type Collection" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\DataContract.bmp" />
      </ShapeHasDecorators>
    </GeometryShape>
  </Shapes>
  <Connectors>
    <Connector Id="c6f4dbc5-7e34-4ecb-98e6-2ac4840d7571" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractAggregationConnector" Name="DataContractAggregationConnector" DisplayName="Data Contract Aggregation Connector" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Data Contract Aggregation Connector" Color="DarkGoldenrod" TargetEndStyle="EmptyDiamond" Thickness="0.0125" />
    <Connector Id="472bfcf6-8392-4473-97a6-d278a5268368" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.ReferenceConnector" Name="ReferenceConnector" DisplayName="Reference Connector" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Reference Connector" Color="DarkGoldenrod" DashStyle="Dash" TargetEndStyle="EmptyArrow" Thickness="0.0125" />
    <Connector Id="d364317d-fcef-4b3a-90d1-b7e8d58cdfe1" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.AggregationConnector" Name="AggregationConnector" DisplayName="Aggregation Connector" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" FixedTooltipText="Aggregation Connector" Color="DarkGoldenrod" TargetEndStyle="EmptyDiamond" Thickness="0.0125" />
  </Connectors>
  <XmlSerializationBehavior Name="DataContractDslSerializationBehavior" Namespace="Microsoft.Practices.ServiceFactory.DataContracts">
    <ClassData>
      <XmlClassData TypeName="DataContractModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataContractModelMoniker" ElementName="dataContractModel" MonikerTypeName="DataContractModelMoniker">
        <DomainClassMoniker Name="DataContractModel" />
        <ElementData>
          <XmlPropertyData XmlName="implementationTechnology">
            <DomainPropertyMoniker Name="DataContractModel/ImplementationTechnology" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="projectMappingTable">
            <DomainPropertyMoniker Name="DataContractModel/ProjectMappingTable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="DataContractModel/Name" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="contracts">
            <DomainRelationshipMoniker Name="DataContractModelHasContracts" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="DataContractModel/Namespace" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractDiagram" MonikerAttributeName="" MonikerElementName="dataContractDiagramMoniker" ElementName="dataContractDiagram" MonikerTypeName="DataContractDiagramMoniker">
        <DiagramMoniker Name="DataContractDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="DataContract" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataContractMoniker" ElementName="dataContract" MonikerTypeName="DataContractMoniker">
        <DomainClassMoniker Name="DataContract" />
        <ElementData>
          <XmlRelationshipData RoleElementName="dataMembers">
            <DomainRelationshipMoniker Name="DataContractHasDataMembers" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractCompartmentShape" MonikerAttributeName="" MonikerElementName="dataContractCompartmentShapeMoniker" ElementName="dataContractCompartmentShape" MonikerTypeName="DataContractCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="DataContractCompartmentShape" />
        <ElementData>
          <XmlPropertyData XmlName="outlineDashStyle">
            <DomainPropertyMoniker Name="DataContractCompartmentShape/OutlineDashStyle" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineThickness">
            <DomainPropertyMoniker Name="DataContractCompartmentShape/OutlineThickness" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModelElementReference" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelElementReferenceMoniker" ElementName="modelElementReference" MonikerTypeName="ModelElementReferenceMoniker">
        <DomainClassMoniker Name="ModelElementReference" />
        <ElementData>
          <XmlPropertyData XmlName="modelElementGuid">
            <DomainPropertyMoniker Name="ModelElementReference/ModelElementGuid" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="ModelElementReference/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractAggregationConnector" MonikerAttributeName="" MonikerElementName="dataContractAggregationConnectorMoniker" ElementName="dataContractAggregationConnector" MonikerTypeName="DataContractAggregationConnectorMoniker">
        <ConnectorMoniker Name="DataContractAggregationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="PrimitiveDataType" MonikerAttributeName="" SerializeId="true" MonikerElementName="primitiveDataTypeMoniker" ElementName="primitiveDataType" MonikerTypeName="PrimitiveDataTypeMoniker">
        <DomainClassMoniker Name="PrimitiveDataType" />
        <ElementData>
          <XmlPropertyData XmlName="isNullable">
            <DomainPropertyMoniker Name="PrimitiveDataType/IsNullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="PrimitiveDataType/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="FaultContract" MonikerAttributeName="" SerializeId="true" MonikerElementName="faultContractMoniker" ElementName="faultContract" MonikerTypeName="FaultContractMoniker">
        <DomainClassMoniker Name="FaultContract" />
        <ElementData>
          <XmlRelationshipData RoleElementName="dataMembers">
            <DomainRelationshipMoniker Name="FaultContractHasDataMembers" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="FaultContractCompartmentShape" MonikerAttributeName="" MonikerElementName="faultContractCompartmentShapeMoniker" ElementName="faultContractCompartmentShape" MonikerTypeName="FaultContractCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="FaultContractCompartmentShape" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractBase" MonikerAttributeName="" MonikerElementName="dataContractBaseMoniker" ElementName="dataContractBase" MonikerTypeName="DataContractBaseMoniker">
        <DomainClassMoniker Name="DataContractBase" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="contracts">
            <DomainRelationshipMoniker Name="DataContractBaseCanBeContainedOnContracts" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="dataContractCollections">
            <DomainRelationshipMoniker Name="DataContractBaseIsBeingReferenceOnDataContractCollections" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractCollection" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataContractCollectionMoniker" ElementName="dataContractCollection" MonikerTypeName="DataContractCollectionMoniker">
        <DomainClassMoniker Name="DataContractCollection" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractEnum" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataContractEnumMoniker" ElementName="dataContractEnum" MonikerTypeName="DataContractEnumMoniker">
        <DomainClassMoniker Name="DataContractEnum" />
        <ElementData>
          <XmlRelationshipData RoleElementName="enumNamedValues">
            <DomainRelationshipMoniker Name="DataContractEnumHasEnumNamedValues" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractEnumCompartmentShape" MonikerAttributeName="" MonikerElementName="dataContractEnumCompartmentShapeMoniker" ElementName="dataContractEnumCompartmentShape" MonikerTypeName="DataContractEnumCompartmentShapeMoniker">
        <CompartmentShapeMoniker Name="DataContractEnumCompartmentShape" />
      </XmlClassData>
      <XmlClassData TypeName="ReferenceConnector" MonikerAttributeName="" MonikerElementName="referenceConnectorMoniker" ElementName="referenceConnector" MonikerTypeName="ReferenceConnectorMoniker">
        <ConnectorMoniker Name="ReferenceConnector" />
      </XmlClassData>
      <XmlClassData TypeName="EnumNamedValue" MonikerAttributeName="" SerializeId="true" MonikerElementName="enumNamedValueMoniker" ElementName="enumNamedValue" MonikerTypeName="EnumNamedValueMoniker">
        <DomainClassMoniker Name="EnumNamedValue" />
        <ElementData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="EnumNamedValue/Value" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="EnumNamedValue/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractEnumHasEnumNamedValues" MonikerAttributeName="" MonikerElementName="dataContractEnumHasEnumNamedValuesMoniker" ElementName="dataContractEnumHasEnumNamedValues" MonikerTypeName="DataContractEnumHasEnumNamedValuesMoniker">
        <DomainRelationshipMoniker Name="DataContractEnumHasEnumNamedValues" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractCollectionShape" MonikerAttributeName="" MonikerElementName="dataContractCollectionShapeMoniker" ElementName="dataContractCollectionShape" MonikerTypeName="DataContractCollectionShapeMoniker">
        <GeometryShapeMoniker Name="DataContractCollectionShape" />
      </XmlClassData>
      <XmlClassData TypeName="ExtensibleModelElement" MonikerAttributeName="" MonikerElementName="extensibleModelElementMoniker" ElementName="extensibleModelElement" MonikerTypeName="ExtensibleModelElementMoniker">
        <DomainClassMoniker Name="ExtensibleModelElement" />
        <ElementData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="ExtensibleModelElement/ObjectExtenderContainer" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="remarks">
            <DomainPropertyMoniker Name="ExtensibleModelElement/Remarks" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataMember" MonikerAttributeName="" MonikerElementName="dataMemberMoniker" ElementName="dataMember" MonikerTypeName="DataMemberMoniker">
        <DomainClassMoniker Name="DataMember" />
        <ElementData>
          <XmlPropertyData XmlName="isDataMember">
            <DomainPropertyMoniker Name="DataMember/IsDataMember" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="collectionType">
            <DomainPropertyMoniker Name="DataMember/CollectionType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="PrimitiveDataTypeCollection" MonikerAttributeName="" SerializeId="true" MonikerElementName="primitiveDataTypeCollectionMoniker" ElementName="primitiveDataTypeCollection" MonikerTypeName="PrimitiveDataTypeCollectionMoniker">
        <DomainClassMoniker Name="PrimitiveDataTypeCollection" />
        <ElementData>
          <XmlPropertyData XmlName="itemType">
            <DomainPropertyMoniker Name="PrimitiveDataTypeCollection/ItemType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractCollectionBase" MonikerAttributeName="" MonikerElementName="dataContractCollectionBaseMoniker" ElementName="dataContractCollectionBase" MonikerTypeName="DataContractCollectionBaseMoniker">
        <DomainClassMoniker Name="DataContractCollectionBase" />
      </XmlClassData>
      <XmlClassData TypeName="Contract" MonikerAttributeName="" MonikerElementName="contractMoniker" ElementName="contract" MonikerTypeName="ContractMoniker">
        <DomainClassMoniker Name="Contract" />
        <ElementData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="Contract/Namespace" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="DataContractModelHasContracts" MonikerAttributeName="" MonikerElementName="dataContractModelHasContractsMoniker" ElementName="dataContractModelHasContracts" MonikerTypeName="DataContractModelHasContractsMoniker">
        <DomainRelationshipMoniker Name="DataContractModelHasContracts" />
      </XmlClassData>
      <XmlClassData TypeName="PrimitiveDataTypeCollectionShape" MonikerAttributeName="" MonikerElementName="primitiveDataTypeCollectionShapeMoniker" ElementName="primitiveDataTypeCollectionShape" MonikerTypeName="PrimitiveDataTypeCollectionShapeMoniker">
        <GeometryShapeMoniker Name="PrimitiveDataTypeCollectionShape" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractHasDataMembers" MonikerAttributeName="" MonikerElementName="dataContractHasDataMembersMoniker" ElementName="dataContractHasDataMembers" MonikerTypeName="DataContractHasDataMembersMoniker">
        <DomainRelationshipMoniker Name="DataContractHasDataMembers" />
      </XmlClassData>
      <XmlClassData TypeName="AggregationConnector" MonikerAttributeName="" MonikerElementName="aggregationConnectorMoniker" ElementName="aggregationConnector" MonikerTypeName="AggregationConnectorMoniker">
        <ConnectorMoniker Name="AggregationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractBaseCanBeContainedOnContracts" MonikerAttributeName="" SerializeId="true" MonikerElementName="dataContractBaseCanBeContainedOnContractsMoniker" ElementName="dataContractBaseCanBeContainedOnContracts" MonikerTypeName="DataContractBaseCanBeContainedOnContractsMoniker">
        <DomainRelationshipMoniker Name="DataContractBaseCanBeContainedOnContracts" />
      </XmlClassData>
      <XmlClassData TypeName="DataContractBaseIsBeingReferenceOnDataContractCollections" MonikerAttributeName="" MonikerElementName="dataContractBaseIsBeingReferenceOnDataContractCollectionsMoniker" ElementName="dataContractBaseIsBeingReferenceOnDataContractCollections" MonikerTypeName="DataContractBaseIsBeingReferenceOnDataContractCollectionsMoniker">
        <DomainRelationshipMoniker Name="DataContractBaseIsBeingReferenceOnDataContractCollections" />
      </XmlClassData>
      <XmlClassData TypeName="FaultContractHasDataMembers" MonikerAttributeName="" MonikerElementName="faultContractHasDataMembersMoniker" ElementName="faultContractHasDataMembers" MonikerTypeName="FaultContractHasDataMembersMoniker">
        <DomainRelationshipMoniker Name="FaultContractHasDataMembers" />
      </XmlClassData>
      <XmlClassData TypeName="ReferenceDataType" MonikerAttributeName="" MonikerElementName="referenceDataTypeMoniker" ElementName="referenceDataType" MonikerTypeName="ReferenceDataTypeMoniker">
        <DomainClassMoniker Name="ReferenceDataType" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="ReferenceDataType/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="DataContractExplorer">
    <CustomNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\DataContract.bmp">
        <Class>
          <DomainClassMoniker Name="DataContract" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\FaultContract.bmp">
        <Class>
          <DomainClassMoniker Name="FaultContract" />
        </Class>
      </ExplorerNodeSettings>
    </CustomNodeSettings>
  </ExplorerBehavior>
  <ConnectionBuilders>
    <ConnectionBuilder Name="AggregationConnectionBuilder" IsCustom="true">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="DataContractBaseCanBeContainedOnContracts" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="DataContractBase" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Contract" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="ReferenceConnectionBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="DataContractBaseIsBeingReferenceOnDataContractCollections" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="DataContractBase" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="DataContractCollection" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="0bbd8a85-2e98-4635-8616-f27cb83caae3" Description="Description for Microsoft.Practices.ServiceFactory.DataContracts.DataContractDiagram" Name="DataContractDiagram" DisplayName="Data Contract Diagram" Namespace="Microsoft.Practices.ServiceFactory.DataContracts" GeneratesDoubleDerived="true">
    <Class>
      <DomainClassMoniker Name="DataContractModel" />
    </Class>
    <ShapeMaps>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="FaultContract" />
        <ParentElementPath>
          <DomainPath>DataContractModelHasContracts.DataContractModel/!DataContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="FaultContractCompartmentShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="FaultContractCompartmentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="FaultContractCompartmentShape/FaultMembersCompartment" />
          <ElementsDisplayed>
            <DomainPath>FaultContractHasDataMembers.DataMembers/!DataMember</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="DataContractEnum" />
        <ParentElementPath>
          <DomainPath>DataContractModelHasContracts.DataContractModel/!DataContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="DataContractEnumCompartmentShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="DataContractEnumCompartmentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="DataContractEnumCompartmentShape/ValuesCompartment" />
          <ElementsDisplayed>
            <DomainPath>DataContractEnumHasEnumNamedValues.EnumNamedValues/!EnumNamedValue</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EnumNamedValue/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="DataContractCollection" />
        <ParentElementPath>
          <DomainPath>DataContractModelHasContracts.DataContractModel/!DataContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="PrimitiveDataTypeCollectionShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="DataContractCollectionShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="DataContractCollectionShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="PrimitiveDataTypeCollection" />
        <ParentElementPath>
          <DomainPath>DataContractModelHasContracts.DataContractModel/!DataContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="PrimitiveDataTypeCollectionShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="PrimitiveDataTypeCollectionShape" />
      </ShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="DataContract" />
        <ParentElementPath>
          <DomainPath>DataContractModelHasContracts.DataContractModel/!DataContractModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="DataContractCompartmentShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="DataContractCompartmentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="DataContractCompartmentShape/DataMembersCompartment" />
          <ElementsDisplayed>
            <DomainPath>DataContractHasDataMembers.DataMembers/!DataMember</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ExtensibleModelElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="AggregationConnector" />
        <DomainRelationshipMoniker Name="DataContractBaseCanBeContainedOnContracts" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ReferenceConnector" />
        <DomainRelationshipMoniker Name="DataContractBaseIsBeingReferenceOnDataContractCollections" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="datacontract" EditorGuid="8a145541-905e-48df-8b48-640b4db62c9d" usesStickyToolboxItems="true">
    <RootClass>
      <DomainClassMoniker Name="DataContractModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="true">
      <XmlSerializationBehaviorMoniker Name="DataContractDslSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="Data Contract Tools">
      <ElementTool Name="DataContractTool" ToolboxIcon="Resources\DataContract.bmp" Caption="Data Contract" Tooltip="Represents a serializable type that can be reused across multiple services. Its members can be primitive types, enumerated types, other data contracts, and collections." HelpKeyword="DataContractTool">
        <DomainClassMoniker Name="DataContract" />
      </ElementTool>
      <ElementTool Name="DataContractEnumTool" ToolboxIcon="Resources\DataContract.bmp" Caption="Data Contract Enumeration" Tooltip="Represents a serializable enumeration that can be reused across multiple services. After adding the Data Contract Enumeration to the design surface, add new values by right-clicking the Values label on the Data Contract Enumeration shape." HelpKeyword="DataContractEnumTool">
        <DomainClassMoniker Name="DataContractEnum" />
      </ElementTool>
      <ElementTool Name="PrimitiveDataTypeCollectionTool" ToolboxIcon="Resources\DataContract.bmp" Caption="Primitive Data Type Collection" Tooltip="Represents a serializable collection of a primitive type that can be reused across multiple services. To specify what type this is a collection of, use the Item Type property in the Properties window. " HelpKeyword="PrimitiveDataTypeCollectionTool">
        <DomainClassMoniker Name="PrimitiveDataTypeCollection" />
      </ElementTool>
      <ElementTool Name="DataContractCollectionTool" ToolboxIcon="Resources\DataContract.bmp" Caption="Data Contract Collection" Tooltip="Represents a serializable collection that can be reused across multiple services. To specify what type this is a collection of, use the Data Contract property for the collection. To do this, drag a Data Contract shape onto the design surface, drag a Data Contract Collection shape onto the design surface, select the Data Contract Collection, and then click the data contract type in the Data Contract drop-down list in the Properties window." HelpKeyword="DataContractCollectionTool">
        <Notes>The DataContractCollectionTool element</Notes>
        <DomainClassMoniker Name="DataContractCollection" />
      </ElementTool>
      <ElementTool Name="FaultContractTool" ToolboxIcon="Resources\FaultContract.bmp" Caption="Fault Contract" Tooltip="Represents a serializable SOAP fault that can be reused across multiple services. Its members can be primitive types, enumerated types, other data contracts, and collections." HelpKeyword="FaultContractTool">
        <DomainClassMoniker Name="FaultContract" />
      </ElementTool>
      <ConnectionTool Name="AggregationConnectionTool" ToolboxIcon="Resources\Agregation.bmp" Caption="Aggregation" Tooltip="Creates aggregation relationships between Data Contracts." HelpKeyword="Aggregation">
        <Notes>The AggregationConnectionTool element</Notes>
        <ConnectionBuilderMoniker Name="DataContractDsl/AggregationConnectionBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="false" UsesSave="false" UsesCustom="true" UsesLoad="false" />
    <DiagramMoniker Name="DataContractDiagram" />
  </Designer>
  <Explorer ExplorerGuid="6c7b7cdc-4211-44b5-81ce-e25ed336fe80" Title="Data Contract Explorer">
    <ExplorerBehaviorMoniker Name="DataContractDsl/DataContractExplorer" />
  </Explorer>
</Dsl>