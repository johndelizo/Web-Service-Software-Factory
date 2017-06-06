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
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.ServiceFactory.DataContracts;
using Microsoft.Practices.ServiceFactory.Extenders.DataContract.Wcf;
using Microsoft.Practices.Modeling.CodeGeneration.Artifacts;
using Microsoft.Practices.Modeling.CodeGeneration.Strategies;

namespace DataContractDsl.Functional.Tests
{
	/// <summary>
	/// Summary description for DataContractHasChildrenContractsBuilderFixture
	/// </summary>
	[TestClass]
	public class DataContractHasChildrenContractsBuilderFixture : DataContractModelFixture
	{
		[TestMethod]
		public void CanAcceptNullSourceAndTarget()
		{
			bool result = AggregationConnectionBuilder.CanAcceptSourceAndTarget(null, null);

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void ShouldNotAcceptNullSource()
		{
			bool result = AggregationConnectionBuilder.CanAcceptSourceAndTarget(null, new DataContract(Store));
			
			Assert.IsFalse(result);
		}

		[TestMethod]
		public void CanAcceptNullTarget()
		{
			bool result = AggregationConnectionBuilder.CanAcceptSourceAndTarget(new DataContract(Store), null);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void ShouldAcceptValidSourceAndTarget()
		{
			bool result = AggregationConnectionBuilder.CanAcceptSourceAndTarget(new DataContract(Store), new DataContract(Store));

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void ShouldNotRejectSameSource()
		{
			DataContract source = new DataContract(Store);
			bool result = AggregationConnectionBuilder.CanAcceptSourceAndTarget(source, source);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void ShouldNotRejectSameLink()
		{
			DataContract source = new DataContract(Store);
			DataContract target = new DataContract(Store);
			DataContractBaseCanBeContainedOnContracts link = new DataContractBaseCanBeContainedOnContracts(source, target);
			ModelElementReference dataElement = new ModelElementReference(Store);
			dataElement.SetLinkedElement(link.Id);
			source.DataMembers.Add(dataElement);
			bool result = AggregationConnectionBuilder.CanAcceptSourceAndTarget(source, target);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void ShouldGetNewNameOnSelfReference()
		{
			DataContract source = new DataContract(Store);
			source.Name = "DC";
			AggregationConnectionBuilder.Connect(source, source);

			Assert.AreEqual(1, source.DataMembers.Count);
			ModelElementReference dataElement = (ModelElementReference)source.DataMembers[0];
			Assert.AreEqual("DC", source.Name);
			Assert.AreNotEqual(source.Name, dataElement.Name);
			Assert.AreEqual("DC1", dataElement.Name);
		}

		[TestMethod]
		public void ShouldGetNewNameOnMultipleTargets()
		{
			DataContract source = new DataContract(Store);
			source.Name = "Source";
			DataContract target = new DataContract(Store);
			target.Name = "Target";
			DataContractBaseCanBeContainedOnContracts link = new DataContractBaseCanBeContainedOnContracts(source, target);
			ModelElementReference dataElement = new ModelElementReference(Store);
			dataElement.Name = "DcdeName";
			dataElement.SetLinkedElement(link.Id);
			source.DataMembers.Add(dataElement);
			AggregationConnectionBuilder.Connect(source, target);

			Assert.AreEqual(1, target.DataMembers.Count);
			dataElement = (ModelElementReference)target.DataMembers[0];
			Assert.AreEqual("Source", dataElement.Name);

			AggregationConnectionBuilder.Connect(source, target);

			Assert.AreEqual(2, target.DataMembers.Count);
			dataElement = (ModelElementReference)target.DataMembers[1];
			Assert.AreEqual("Source1", dataElement.Name);

			AggregationConnectionBuilder.Connect(source, target);

			Assert.AreEqual(3, target.DataMembers.Count);
			dataElement = (ModelElementReference)target.DataMembers[2];
			Assert.AreEqual("Source2", dataElement.Name);
		}

		protected override string Template
		{
			get
			{
				DataContractLink link = new DataContractLink();
				return GetWrappedLink(link).GetTemplate(TextTemplateTargetLanguage.CSharp);
			}
		}

		protected override Type ContractType
		{
			get { return typeof(WCFDataContract); }
		}

		
	}
}
