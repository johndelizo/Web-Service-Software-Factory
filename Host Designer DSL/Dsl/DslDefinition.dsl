<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="52466d50-8a95-4f6b-89d6-491d398e6d2a" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesigner" Name="HostDesigner" DisplayName="HostDesigner" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" ProductName="Service Factory: Host Designer Model" CompanyName="Microsoft" PackageGuid="e3821e6e-c0ad-4f8f-b300-de95e9776716" PackageNamespace="Microsoft.Practices.ServiceFactory.HostDesigner" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="3cd7ca2b-86c6-4bd5-84ba-c86fd2e467eb" Description="The root in which all other elements are embedded. Appears as a diagram." Name="HostDesignerModel" DisplayName="Host Model" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <Properties>
        <DomainProperty Id="418bae6b-54a1-421d-8c1b-aa4d467888f2" Description="This value is not used." Name="Namespace" DisplayName="XML Namespace" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="HostApplication" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>HostDesignerModelHasHostApplications.HostApplications</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ClientApplication" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>HostDesignerModelHasClientApplications.ClientApplications</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="54c43a4c-210e-4b4e-b661-4212ca224983" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostApplication" Name="HostApplication" DisplayName="Host Application" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <Properties>
        <DomainProperty Id="a7506df5-82da-4ea1-8b28-77adf988273f" Description="An identifier for the host application." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="93174466-0cba-4b5e-bcd0-1ddda6a99d9a" Description="The Visual Studio project that represents the host application." Name="ImplementationProject" DisplayName="Implementation Project" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.VisualStudio.Helper.Design.ProjectsOnlySolutionPickerEditor)" />
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
        <DomainProperty Id="358f830a-58a9-477b-ae2c-e4883e6ffc25" Description="Associates a platform technology (for example, ASMX or WCF) with the Visual Studio host project." Name="ImplementationTechnology" DisplayName="Implementation Technology" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.ExtensionProvider.Design.UITypeEditors.ExtensionProviderEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
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
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ServiceDescription" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>HostApplicationHasServiceDescriptions.ServiceDescriptions</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="0aa846e0-8237-42f8-9b7f-2c51438fe0b3" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ServiceDescription" Name="ServiceDescription" DisplayName="Service Description" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <BaseClass>
        <DomainClassMoniker Name="ExtensibleModelElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="6201b91c-00d3-468b-82c0-3f33d4b2b908" Description="The name of the service reference." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Endpoint" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ServiceDescriptionHasEndpoints.Endpoints</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="0acc8cb2-49e9-42e5-9925-e690475224cd" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ServiceReference" Name="ServiceReference" DisplayName="Service Reference" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <BaseClass>
        <DomainClassMoniker Name="ServiceDescription" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="c7bfcbe0-4e6f-4118-a948-36e3449791bf" Description="A cross-model reference to the associated service on the service contract model." Name="ServiceImplementationType" DisplayName="Service Implementation Type" Category="General">
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
                <AttributeParameter Value="&quot;Please choose a ServiceContract file&quot;" />
                <AttributeParameter Value=" &quot;Service Contract files|*.servicecontract&quot;" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="Microsoft.VisualStudio.Modeling.Integration.Picker.ApplyElementTypeLimitations">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.ServiceFactory.ServiceContracts.Service)" />
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
    <DomainClass Id="8be985a0-32af-4875-afca-1118e1bc483b" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.Endpoint" Name="Endpoint" DisplayName="Endpoint" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <BaseClass>
        <DomainClassMoniker Name="ExtensibleModelElement" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="56629039-721c-4137-a670-7864e6602817" Description="When used, this string value will be appended to the end of the address. This is useful when a service has more than one endpoint and each must have different bindings. " Name="Address" DisplayName="Address" Category="General">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="763763ca-3dfb-4ffd-a1a6-f4a8cc6a66ff" Description="The name of the endpoint." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="f1d19513-829d-44c4-b97a-66d7da96fa2a" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ClientApplication" Name="ClientApplication" DisplayName="Client Application" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <Properties>
        <DomainProperty Id="ac86f887-b41c-4747-bcea-fe36f16a4d52" Description="An identifier for the client application." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1788f448-5ac1-4568-a681-0338d568401d" Description="Associates a platform technology (for example, ASMX or WCF) with the Visual Studio client project." Name="ImplementationTechnology" DisplayName="Implementation Technology" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.Modeling.ExtensionProvider.Design.UITypeEditors.ExtensionProviderEditor)" />
                <AttributeParameter Value="typeof(System.Drawing.Design.UITypeEditor)" />
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
        <DomainProperty Id="47ae3232-25b1-4ad4-9bff-52e3e9842018" Description="The Visual Studio project that represents the host application." Name="ImplementationProject" DisplayName="Implementation Project" Category="General">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(Microsoft.Practices.VisualStudio.Helper.Design.ProjectsOnlySolutionPickerEditor)" />
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
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Proxy" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ClientApplicationHasProxies.Proxies</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="e2f24ffd-9f89-4e5c-aa64-2f16c4839482" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.Proxy" Name="Proxy" DisplayName="Proxy" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <Properties>
        <DomainProperty Id="c1a544f6-07cb-45d4-a929-a2901ebbc1b2" Description="The name of the proxy." Name="Name" DisplayName="Name" Category="General" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="1cfa548e-da98-49d9-a1d5-89002775439b" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ExtensibleModelElement" Name="ExtensibleModelElement" DisplayName="Extensible Model Element" InheritanceModifier="Abstract" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <Properties>
        <DomainProperty Id="77298aa0-e17b-4a89-a978-d36715812ac6" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ExtensibleModelElement.Object Extender Container" Name="ObjectExtenderContainer" DisplayName="Object Extender Container" Category="General" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/Microsoft.Practices.Modeling.ExtensionProvider.Extension/ObjectExtenderContainer" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="b451e65b-c67c-4770-b5dc-8463d8a81b2e" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesignerModelHasHostApplications" Name="HostDesignerModelHasHostApplications" DisplayName="Host Model Has Host Applications" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" IsEmbedding="true">
      <Source>
        <DomainRole Id="dd270672-a120-4e4c-b858-069ad96b0082" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesignerModelHasHostApplications.HostDesignerModel" Name="HostDesignerModel" DisplayName="Host Model" PropertyName="HostApplications" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Host Applications">
          <RolePlayer>
            <DomainClassMoniker Name="HostDesignerModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="05025203-aa7d-4129-978f-0c6edd399416" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesignerModelHasHostApplications.HostApplication" Name="HostApplication" DisplayName="Host Application" PropertyName="HostDesignerModel" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Host Model">
          <RolePlayer>
            <DomainClassMoniker Name="HostApplication" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="fcb7dde6-61aa-4b26-832a-bd9c8244de30" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostApplicationHasServiceDescriptions" Name="HostApplicationHasServiceDescriptions" DisplayName="Host Application Has Service Descriptions" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" IsEmbedding="true">
      <Source>
        <DomainRole Id="2516392f-bfe6-44ca-b0c8-b2f66135423b" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostApplicationHasServiceDescriptions.HostApplication" Name="HostApplication" DisplayName="Host Application" PropertyName="ServiceDescriptions" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Service Descriptions">
          <RolePlayer>
            <DomainClassMoniker Name="HostApplication" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="33a10620-d5ff-4e18-8b1a-6ab2a05f6586" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostApplicationHasServiceDescriptions.ServiceDescription" Name="ServiceDescription" DisplayName="Service Description" PropertyName="HostApplication" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Host Application">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceDescription" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="a2e72d28-0d27-422c-87d7-1df6735054e1" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ServiceDescriptionHasEndpoints" Name="ServiceDescriptionHasEndpoints" DisplayName="Service Description Has Endpoints" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" IsEmbedding="true">
      <Source>
        <DomainRole Id="8295c411-068d-422e-a54e-b49644482242" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ServiceDescriptionHasEndpoints.ServiceDescription" Name="ServiceDescription" DisplayName="Service Description" PropertyName="Endpoints" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Endpoints">
          <RolePlayer>
            <DomainClassMoniker Name="ServiceDescription" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="2d5fba69-a70c-4299-950c-0510754c8ce3" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ServiceDescriptionHasEndpoints.Endpoint" Name="Endpoint" DisplayName="Endpoint" PropertyName="ServiceDescription" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Service Description">
          <RolePlayer>
            <DomainClassMoniker Name="Endpoint" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="11b523b9-b40a-4263-84ce-b04cf2470990" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ClientApplicationHasProxies" Name="ClientApplicationHasProxies" DisplayName="Client Application Has Proxies" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" IsEmbedding="true">
      <Source>
        <DomainRole Id="001dcc80-3533-4768-a6fa-6630b4dd8c4c" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ClientApplicationHasProxies.ClientApplication" Name="ClientApplication" DisplayName="Client Application" PropertyName="Proxies" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Proxies">
          <RolePlayer>
            <DomainClassMoniker Name="ClientApplication" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="91dfc089-003c-4acb-8317-74438e0f8a96" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ClientApplicationHasProxies.Proxy" Name="Proxy" DisplayName="Proxy" PropertyName="ClientApplication" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Client Application">
          <RolePlayer>
            <DomainClassMoniker Name="Proxy" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="8fff9ca0-4f72-4067-ae83-45b028e27931" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ProxyReferencesEndpoint" Name="ProxyReferencesEndpoint" DisplayName="Proxy References Endpoint" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
      <Source>
        <DomainRole Id="52cb9333-1fa5-4d06-98d5-73297750d2ad" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ProxyReferencesEndpoint.Proxy" Name="Proxy" DisplayName="Proxy" PropertyName="Endpoint" Multiplicity="One" Category="General" PropertyDisplayName="Endpoint">
          <RolePlayer>
            <DomainClassMoniker Name="Proxy" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="7632f137-28eb-45c1-bed5-32d84d51cd1c" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ProxyReferencesEndpoint.Endpoint" Name="Endpoint" DisplayName="Endpoint" PropertyName="Proxies" Category="General" PropertyDisplayName="Proxies">
          <RolePlayer>
            <DomainClassMoniker Name="Endpoint" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="38537c39-659a-464c-8507-a756665dc570" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesignerModelHasClientApplications" Name="HostDesignerModelHasClientApplications" DisplayName="Host Model Has Client Applications" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" IsEmbedding="true">
      <Source>
        <DomainRole Id="a8fc125f-7d03-4348-a4e9-4069a969604e" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesignerModelHasClientApplications.HostDesignerModel" Name="HostDesignerModel" DisplayName="Host Model" PropertyName="ClientApplications" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Client Applications">
          <RolePlayer>
            <DomainClassMoniker Name="HostDesignerModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="dac0ced6-1241-40e0-98ea-2d3772179f83" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesignerModelHasClientApplications.ClientApplication" Name="ClientApplication" DisplayName="Client Application" PropertyName="HostDesignerModel" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="General" PropertyDisplayName="Host Model">
          <RolePlayer>
            <DomainClassMoniker Name="ClientApplication" />
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
    <DomainEnumeration Name="TransportType" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.TransportType">
      <Literals>
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.TransportType.Http" Name="Http" Value="" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.TransportType.Msmq" Name="Msmq" Value="" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.TransportType.Tcp" Name="Tcp" Value="" />
        <EnumerationLiteral Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.TransportType.MemoryPipe" Name="MemoryPipe" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="IExtensionProvider" Namespace="Microsoft.Practices.Modeling.ExtensionProvider.Extension" />
    <ExternalType Name="ObjectExtenderContainer" Namespace="Microsoft.Practices.Modeling.ExtensionProvider.Extension" />
    <ExternalType Name="ModelBusReference" Namespace="Microsoft.VisualStudio.Modeling.Integration" />
    <ExternalType Name="DashStyle" Namespace="System.Drawing.Drawing2D" />
  </Types>
  <Shapes>
    <GeometryShape Id="41b2aa05-ac38-4dc3-a54d-cf7f92f07512" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostApplicationShape" Name="HostApplicationShape" DisplayName="Host Application Shape" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Host Application Shape" FillColor="ForestGreen" InitialHeight="0.4" OutlineThickness="0.0125" FillGradientMode="ForwardDiagonal" Geometry="Rectangle">
      <BaseGeometryShape>
        <GeometryShapeMoniker Name="BaseGeometryShape" />
      </BaseGeometryShape>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Host Application" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\HostApplication.bmp" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="19474a32-4269-453a-8eca-a30fb0304f6c" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.BaseGeometryShape" Name="BaseGeometryShape" DisplayName="Base Geometry Shape" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Base Geometry Shape" InitialHeight="1" OutlineThickness="0.0125" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="NameDecorator" DisplayName="Name Decorator" DefaultText="NameDecorator" />
      </ShapeHasDecorators>
    </GeometryShape>
    <GeometryShape Id="c235dbe9-4ded-490e-80d2-fd0f3cf73226" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ServiceDescriptionShape" Name="ServiceDescriptionShape" DisplayName="Service Description Shape" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Service Description Shape" FillColor="PaleGreen" InitialHeight="0.4" OutlineThickness="0.0125" FillGradientMode="ForwardDiagonal" Geometry="Rectangle">
      <BaseGeometryShape>
        <GeometryShapeMoniker Name="BaseGeometryShape" />
      </BaseGeometryShape>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Service Reference" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\ServiceImplementation.bmp" />
      </ShapeHasDecorators>
    </GeometryShape>
    <Port Id="38484d5e-bc78-41ac-8ef9-17b2e2751fa7" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.EndpointPortShape" Name="EndpointPortShape" DisplayName="Endpoint Port Shape" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Endpoint Port Shape" FillColor="Transparent" OutlineColor="Transparent" InitialWidth="0.25" InitialHeight="0.25" OutlineThickness="0.0125" FillGradientMode="Vertical" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="Image" DisplayName="Image" DefaultIcon="Resources\ServiceEndpointPortIcon.bmp" />
      </ShapeHasDecorators>
    </Port>
    <Port Id="55bf95f2-2f35-4a79-ac85-cc07711c95b9" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ProxyPortShape" Name="ProxyPortShape" DisplayName="Proxy Port Shape" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Proxy Port Shape" FillColor="Transparent" OutlineColor="Transparent" InitialWidth="0.25" InitialHeight="0.25" OutlineThickness="0.0125" FillGradientMode="Vertical" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerMiddleLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="Image" DisplayName="Image" DefaultIcon="Resources\ProxyEndpointShape.bmp" />
      </ShapeHasDecorators>
    </Port>
    <GeometryShape Id="913d7e56-f195-4323-b75d-5327f7f5c921" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ClientApplicationShape" Name="ClientApplicationShape" DisplayName="Cilent Application Shape" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Cilent Application Shape" FillColor="LightYellow" InitialHeight="0.4" OutlineThickness="0.0125" FillGradientMode="BackwardDiagonal" Geometry="Rectangle">
      <BaseGeometryShape>
        <GeometryShapeMoniker Name="BaseGeometryShape" />
      </BaseGeometryShape>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="Type" DisplayName="Type" DefaultText="Client Application" FontStyle="Italic" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="TypeIcon" DisplayName="Type Icon" DefaultIcon="Resources\ClientApplication.bmp" />
      </ShapeHasDecorators>
    </GeometryShape>
  </Shapes>
  <Connectors>
    <Connector Id="5a0b927c-4ca8-4d03-b1cb-7bd68e4bc53d" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ServiceConnector" Name="ServiceConnector" DisplayName="Service Connector" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Service Connector" SourceEndStyle="EmptyDiamond" Thickness="0.0125" />
    <Connector Id="a1af4152-70a5-4742-8012-b0b26a102ce7" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.ProxyEndpointConnector" Name="ProxyEndpointConnector" DisplayName="Proxy Endpoint Connector" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" FixedTooltipText="Proxy Endpoint Connector" Thickness="0.0125" />
  </Connectors>
  <XmlSerializationBehavior Name="HostDesignerSerializationBehavior" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner">
    <ClassData>
      <XmlClassData TypeName="HostDesignerModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="hostDesignerModelMoniker" ElementName="hostDesignerModel" MonikerTypeName="HostDesignerModelMoniker">
        <DomainClassMoniker Name="HostDesignerModel" />
        <ElementData>
          <XmlRelationshipData RoleElementName="hostApplications">
            <DomainRelationshipMoniker Name="HostDesignerModelHasHostApplications" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="clientApplications">
            <DomainRelationshipMoniker Name="HostDesignerModelHasClientApplications" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="HostDesignerModel/Namespace" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="HostDesignerDiagram" MonikerAttributeName="" MonikerElementName="hostDesignerDiagramMoniker" ElementName="hostDesignerDiagram" MonikerTypeName="HostDesignerDiagramMoniker">
        <DiagramMoniker Name="HostDesignerDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="HostApplication" MonikerAttributeName="name" SerializeId="true" MonikerElementName="hostApplicationMoniker" ElementName="hostApplication" MonikerTypeName="HostApplicationMoniker">
        <DomainClassMoniker Name="HostApplication" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="HostApplication/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="implementationProject">
            <DomainPropertyMoniker Name="HostApplication/ImplementationProject" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="serviceDescriptions">
            <DomainRelationshipMoniker Name="HostApplicationHasServiceDescriptions" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="implementationTechnology">
            <DomainPropertyMoniker Name="HostApplication/ImplementationTechnology" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="HostDesignerModelHasHostApplications" MonikerAttributeName="" MonikerElementName="hostDesignerModelHasHostApplicationsMoniker" ElementName="hostDesignerModelHasHostApplications" MonikerTypeName="HostDesignerModelHasHostApplicationsMoniker">
        <DomainRelationshipMoniker Name="HostDesignerModelHasHostApplications" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceDescription" MonikerAttributeName="name" MonikerElementName="serviceDescriptionMoniker" ElementName="serviceDescription" MonikerTypeName="ServiceDescriptionMoniker">
        <DomainClassMoniker Name="ServiceDescription" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="ServiceDescription/Name" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="endpoints">
            <DomainRelationshipMoniker Name="ServiceDescriptionHasEndpoints" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ServiceReference" MonikerAttributeName="" SerializeId="true" MonikerElementName="serviceReferenceMoniker" ElementName="serviceReference" MonikerTypeName="ServiceReferenceMoniker">
        <DomainClassMoniker Name="ServiceReference" />
        <ElementData>
          <XmlPropertyData XmlName="serviceImplementationType">
            <DomainPropertyMoniker Name="ServiceReference/ServiceImplementationType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="HostApplicationHasServiceDescriptions" MonikerAttributeName="" MonikerElementName="hostApplicationHasServiceDescriptionsMoniker" ElementName="hostApplicationHasServiceDescriptions" MonikerTypeName="HostApplicationHasServiceDescriptionsMoniker">
        <DomainRelationshipMoniker Name="HostApplicationHasServiceDescriptions" />
      </XmlClassData>
      <XmlClassData TypeName="Endpoint" MonikerAttributeName="" SerializeId="true" MonikerElementName="endpointMoniker" ElementName="endpoint" MonikerTypeName="EndpointMoniker">
        <DomainClassMoniker Name="Endpoint" />
        <ElementData>
          <XmlPropertyData XmlName="address">
            <DomainPropertyMoniker Name="Endpoint/Address" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Endpoint/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ServiceDescriptionHasEndpoints" MonikerAttributeName="" MonikerElementName="serviceDescriptionHasEndpointsMoniker" ElementName="serviceDescriptionHasEndpoints" MonikerTypeName="ServiceDescriptionHasEndpointsMoniker">
        <DomainRelationshipMoniker Name="ServiceDescriptionHasEndpoints" />
      </XmlClassData>
      <XmlClassData TypeName="ClientApplication" MonikerAttributeName="name" SerializeId="true" MonikerElementName="clientApplicationMoniker" ElementName="clientApplication" MonikerTypeName="ClientApplicationMoniker">
        <DomainClassMoniker Name="ClientApplication" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="ClientApplication/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="implementationTechnology">
            <DomainPropertyMoniker Name="ClientApplication/ImplementationTechnology" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="implementationProject">
            <DomainPropertyMoniker Name="ClientApplication/ImplementationProject" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="proxies">
            <DomainRelationshipMoniker Name="ClientApplicationHasProxies" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Proxy" MonikerAttributeName="" SerializeId="true" MonikerElementName="proxyMoniker" ElementName="proxy" MonikerTypeName="ProxyMoniker">
        <DomainClassMoniker Name="Proxy" />
        <ElementData>
          <XmlRelationshipData RoleElementName="endpoint">
            <DomainRelationshipMoniker Name="ProxyReferencesEndpoint" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Proxy/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ClientApplicationHasProxies" MonikerAttributeName="" MonikerElementName="clientApplicationHasProxiesMoniker" ElementName="clientApplicationHasProxies" MonikerTypeName="ClientApplicationHasProxiesMoniker">
        <DomainRelationshipMoniker Name="ClientApplicationHasProxies" />
      </XmlClassData>
      <XmlClassData TypeName="ProxyReferencesEndpoint" MonikerAttributeName="" MonikerElementName="proxyReferencesEndpointMoniker" ElementName="proxyReferencesEndpoint" MonikerTypeName="ProxyReferencesEndpointMoniker">
        <DomainRelationshipMoniker Name="ProxyReferencesEndpoint" />
      </XmlClassData>
      <XmlClassData TypeName="HostApplicationShape" MonikerAttributeName="" MonikerElementName="hostApplicationShapeMoniker" ElementName="hostApplicationShape" MonikerTypeName="HostApplicationShapeMoniker">
        <GeometryShapeMoniker Name="HostApplicationShape" />
      </XmlClassData>
      <XmlClassData TypeName="BaseGeometryShape" MonikerAttributeName="" MonikerElementName="baseGeometryShapeMoniker" ElementName="baseGeometryShape" MonikerTypeName="BaseGeometryShapeMoniker">
        <GeometryShapeMoniker Name="BaseGeometryShape" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceDescriptionShape" MonikerAttributeName="" MonikerElementName="serviceDescriptionShapeMoniker" ElementName="serviceDescriptionShape" MonikerTypeName="ServiceDescriptionShapeMoniker">
        <GeometryShapeMoniker Name="ServiceDescriptionShape" />
      </XmlClassData>
      <XmlClassData TypeName="EndpointPortShape" MonikerAttributeName="" MonikerElementName="endpointPortShapeMoniker" ElementName="endpointPortShape" MonikerTypeName="EndpointPortShapeMoniker">
        <PortMoniker Name="EndpointPortShape" />
      </XmlClassData>
      <XmlClassData TypeName="HostDesignerModelHasClientApplications" MonikerAttributeName="" MonikerElementName="hostDesignerModelHasClientApplicationsMoniker" ElementName="hostDesignerModelHasClientApplications" MonikerTypeName="HostDesignerModelHasClientApplicationsMoniker">
        <DomainRelationshipMoniker Name="HostDesignerModelHasClientApplications" />
      </XmlClassData>
      <XmlClassData TypeName="ServiceConnector" MonikerAttributeName="" MonikerElementName="serviceConnectorMoniker" ElementName="serviceConnector" MonikerTypeName="ServiceConnectorMoniker">
        <ConnectorMoniker Name="ServiceConnector" />
      </XmlClassData>
      <XmlClassData TypeName="ProxyPortShape" MonikerAttributeName="" MonikerElementName="proxyPortShapeMoniker" ElementName="proxyPortShape" MonikerTypeName="ProxyPortShapeMoniker">
        <PortMoniker Name="ProxyPortShape" />
      </XmlClassData>
      <XmlClassData TypeName="ClientApplicationShape" MonikerAttributeName="" MonikerElementName="cilentApplicationShapeMoniker" ElementName="cilentApplicationShape" MonikerTypeName="ClientApplicationShapeMoniker">
        <GeometryShapeMoniker Name="ClientApplicationShape" />
      </XmlClassData>
      <XmlClassData TypeName="ProxyEndpointConnector" MonikerAttributeName="" MonikerElementName="proxyEndpointConnectorMoniker" ElementName="proxyEndpointConnector" MonikerTypeName="ProxyEndpointConnectorMoniker">
        <ConnectorMoniker Name="ProxyEndpointConnector" />
      </XmlClassData>
      <XmlClassData TypeName="ExtensibleModelElement" MonikerAttributeName="" MonikerElementName="extensibleModelElementMoniker" ElementName="extensibleModelElement" MonikerTypeName="ExtensibleModelElementMoniker">
        <DomainClassMoniker Name="ExtensibleModelElement" />
        <ElementData>
          <XmlPropertyData XmlName="objectExtenderContainer">
            <DomainPropertyMoniker Name="ExtensibleModelElement/ObjectExtenderContainer" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="HostDesignerExplorer" />
  <ConnectionBuilders>
    <ConnectionBuilder Name="ProxyReferencesEndpointBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="ProxyReferencesEndpoint" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Proxy" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Endpoint" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="e6b021f0-526f-4dfd-a19d-703fc1f22af6" Description="Description for Microsoft.Practices.ServiceFactory.HostDesigner.HostDesignerDiagram" Name="HostDesignerDiagram" DisplayName="Host Designer Diagram" Namespace="Microsoft.Practices.ServiceFactory.HostDesigner" GeneratesDoubleDerived="true">
    <Class>
      <DomainClassMoniker Name="HostDesignerModel" />
    </Class>
    <ShapeMaps>
      <ShapeMap>
        <DomainClassMoniker Name="HostApplication" />
        <ParentElementPath>
          <DomainPath>HostDesignerModelHasHostApplications.HostDesignerModel/!HostDesignerModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="BaseGeometryShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="HostApplication/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="HostApplicationShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="ServiceDescription" />
        <ParentElementPath>
          <DomainPath>HostApplicationHasServiceDescriptions.HostApplication/!HostApplication/HostDesignerModelHasHostApplications.HostDesignerModel/!HostDesignerModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="BaseGeometryShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ServiceDescription/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="ServiceDescriptionShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="Endpoint" />
        <ParentElementPath>
          <DomainPath>ServiceDescriptionHasEndpoints.ServiceDescription/!ServiceDescription</DomainPath>
        </ParentElementPath>
        <PortMoniker Name="EndpointPortShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="Proxy" />
        <ParentElementPath>
          <DomainPath>ClientApplicationHasProxies.ClientApplication/!ClientApplication</DomainPath>
        </ParentElementPath>
        <PortMoniker Name="ProxyPortShape" />
      </ShapeMap>
      <ShapeMap>
        <DomainClassMoniker Name="ClientApplication" />
        <ParentElementPath>
          <DomainPath>HostDesignerModelHasClientApplications.HostDesignerModel/!HostDesignerModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="BaseGeometryShape/NameDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ClientApplication/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <GeometryShapeMoniker Name="ClientApplicationShape" />
      </ShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="ServiceConnector" />
        <DomainRelationshipMoniker Name="HostApplicationHasServiceDescriptions" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="ProxyEndpointConnector" />
        <DomainRelationshipMoniker Name="ProxyReferencesEndpoint" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="host" EditorGuid="45afc6ba-9606-4371-8e07-fab9e769d645" usesStickyToolboxItems="true">
    <RootClass>
      <DomainClassMoniker Name="HostDesignerModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="true">
      <XmlSerializationBehaviorMoniker Name="HostDesignerSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="Host Tools">
      <ElementTool Name="HostApplication" ToolboxIcon="Resources\HostApplication.bmp" Caption="Host Application" Tooltip="Represents the application that will accept request messages and send response messages." HelpKeyword="HostApplication">
        <DomainClassMoniker Name="HostApplication" />
      </ElementTool>
      <ElementTool Name="Service" ToolboxIcon="Resources\ServiceImplementation.bmp" Caption="Service Reference" Tooltip="Associates the service implementation shape from the service contract designer with the application that will host the service. To add the service reference to the design surface, drag the Service Reference shape from the Toolbox and drop it on the Host Application shape. In the Properties window for the Service Reference shape, use the Service Implementation Type property to associate the service implementation on the Service Contract Model." HelpKeyword="Service">
        <DomainClassMoniker Name="ServiceReference" />
      </ElementTool>
      <ElementTool Name="Endpoint" ToolboxIcon="Resources\ServiceEndpointPortIcon.bmp" Caption="Endpoint" Tooltip="Describes in a standard-based way where messages should be sent, how they should be sent, and what the messages should look like." HelpKeyword="Endpoint">
        <DomainClassMoniker Name="Endpoint" />
      </ElementTool>
      <ElementTool Name="ClientApplication" ToolboxIcon="Resources\ClientApplication.bmp" Caption="Client Application" Tooltip="Represents the application that consumes the service." HelpKeyword="ClientApplication">
        <DomainClassMoniker Name="ClientApplication" />
      </ElementTool>
      <ElementTool Name="Proxy" ToolboxIcon="Resources\ProxyEndpointShape.bmp" Caption="Proxy" Tooltip="Represents the part of the client application that consumes the service. To add the proxy to the client application, drag the Proxy shape from the Toolbox and drop it onto the Client Application shape." HelpKeyword="Proxy">
        <DomainClassMoniker Name="Proxy" />
      </ElementTool>
      <ConnectionTool Name="ProxyEndpointConnectionTool" ToolboxIcon="Resources\ProxyEndpointConnection.bmp" Caption="Proxy Endpoint Connector" Tooltip="Associates the proxy on the Client Application with the Endpoint shape on the Service Reference." HelpKeyword="ProxyEndpointConnectionTool">
        <ConnectionBuilderMoniker Name="HostDesigner/ProxyReferencesEndpointBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="false" UsesSave="false" UsesCustom="true" UsesLoad="false" />
    <DiagramMoniker Name="HostDesignerDiagram" />
  </Designer>
  <Explorer ExplorerGuid="af1944ad-4b19-4ee6-9f7c-01f71e5b8c26" Title="Host Explorer">
    <ExplorerBehaviorMoniker Name="HostDesigner/HostDesignerExplorer" />
  </Explorer>
</Dsl>