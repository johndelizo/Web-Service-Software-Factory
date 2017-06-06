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

namespace Microsoft.Practices.ServiceFactory.Extenders.DataContract.Asmx
{
    [TextTemplate("Lib\\TextTemplates\\ASMX\\CS\\DataContract.tt", TextTemplateTargetLanguage.CSharp)]
    [AssemblyReference("System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
    [TypeConverter(typeof(ArtifactLinkConverter<AsmxFaultContractLink>))]
    [CodeGenerationStrategy(typeof(TextTemplateCodeGenerationStrategy))]
    [CLSCompliant(false)]
    public sealed class AsmxFaultContractLink : ArtifactLink
    {
    }
}