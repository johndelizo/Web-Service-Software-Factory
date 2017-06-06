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
using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.UnitTestLibrary;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using Microsoft.Practices.Modeling.Common;

namespace DataContractDsl.Tests
{
	public abstract class DataContractModelFixture: ModelFixture
	{
		protected const string ElementName = "MyElementName";
		protected const string ElementNamespace = "http://mynamespace";
		protected const string PrimitiveDataElementName1 = "Element1";
		protected const string PrimitiveDataElementType1 = "System.Int32";
		protected const string PrimitiveDataElementName2 = "Element2";
		protected const string PrimitiveDataElementType2 = "System.String";

		private Store store = null;
		private DataContractDslDomainModel dm = null;

		protected override Store Store
		{
			get
			{
				if ( store==null )
				{
					store = new Store(ServiceProvider, typeof(CoreDesignSurfaceDomainModel), typeof(DataContractDslDomainModel));
				}
				return store;
			}
		}

		protected override DomainModel DomainModel
		{
			get
			{
				if ( dm==null )
				{
					dm = Store.GetDomainModel<DataContractDslDomainModel>();
				}
				return dm;
			}
		}

		protected Type CompileAndGetType(string content)
		{
			EnsureNamespace(ref content);
			string typeName = DefaultNamespace + "." + ElementName;
            CompilerResults results = DynamicCompilation.CompileAssemblyFromSource(content);
			
            Type generatedType = results.CompiledAssembly.GetType(typeName, false);
			
            Assert.IsNotNull(generatedType, "Invalid type: " + typeName);
			return generatedType;
		}
	}
}
