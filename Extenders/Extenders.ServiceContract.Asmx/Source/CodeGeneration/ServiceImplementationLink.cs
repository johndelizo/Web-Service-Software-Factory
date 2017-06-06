//===============================================================================
// Microsoft patterns & practices
// Web Service Software Factory 2010
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
using Microsoft.Practices.Modeling.CodeGeneration;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts.Design;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;
using System;
using System.ComponentModel;

namespace Microsoft.Practices.ServiceFactory.Extenders.ServiceContract.Asmx.CodeGeneration
{
	[CLSCompliant(false)]
	[TextTemplate("Lib\\TextTemplates\\ASMX\\CS\\ServiceImplementation.tt", TextTemplateTargetLanguage.CSharp )]
	[AssemblyReference("System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
	[AssemblyReference("System.Web.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[TypeConverter(typeof(ArtifactLinkConverter<ASMXServiceImplementationLink>))]
	[CodeGenerationStrategy(typeof(TextTemplateCodeGenerationStrategy))]
	public sealed class ASMXServiceImplementationLink : ArtifactLink
	{
	}
}