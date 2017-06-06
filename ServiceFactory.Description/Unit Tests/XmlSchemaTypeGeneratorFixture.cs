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
using Microsoft.Practices.ServiceFactory.Description;
using System.IO;
using System.Reflection;
using System.Xml.Schema;
using System.Xml;
using Microsoft.Practices.UnitTestLibrary.Utilities;
using System.CodeDom;
using System.ServiceModel.Description;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Microsoft.Practices.ServiceFactory.Description.Tests
{
	/// <summary>
	/// Summary description for XmlSchemaTypeGeneratorFixture
	/// </summary>
	[TestClass]
	public class XmlSchemaTypeGeneratorFixture
	{
		private XmlSchemaTypeGenerator generator;

		[TestInitialize]
		public void TestInitialize()
		{
			generator = new XmlSchemaTypeGenerator();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ThrowOnEmptySchemaFile()
		{
			generator.GenerateTypes(string.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(UriFormatException))]
		public void ThrowOnInvalidLocation()
		{
			generator.GenerateTypes("foo");
		}

		[TestMethod]
		[ExpectedException(typeof(FileNotFoundException))]
		public void ThrowOnFileNotFound()
		{
			generator.GenerateTypes(@"C:\foo.xsd");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ThrowOnInvalidSchemaFile()
		{
			generator.GenerateTypes(Assembly.GetExecutingAssembly().Location);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		[DeploymentItem(@"SampleData\DescriptionModel\InvalidSchema.xsd")]
		public void ThrowOnInvalidSchema()
		{
			generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\InvalidSchema.xsd"));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		[DeploymentItem(@"SampleData\DescriptionModel\InvalidIncludeSchema.xsd")]
		public void ThrowOnInvalidInclude()
		{
			generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\InvalidIncludeSchema.xsd"));
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\InvalidIncludeSchema.xsd")]
		public void ShouldGenerateWithDefaultSerializer()
		{
            XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(false);
            CodeTypeDeclarationCollection types = generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\Company.xsd"));
            Assert.AreEqual<int>(4, types.Count);
        }

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\DataSetSchema.xsd")]
		public void ShouldGenerateWithDataSetSchemaAndImportXmlType()
		{
            XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(true);
			CodeTypeDeclarationCollection types = generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\DataSetSchema.xsd"));
			Assert.AreEqual<int>(2, types.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\EmptySchema.xsd")]
		public void ShouldNotGenerateWithEmptySchema()
		{
			CodeTypeDeclarationCollection types = generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\EmptySchema.xsd"));

			Assert.AreEqual<int>(0, types.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\SimpleSchema.xsd")]
		public void ShouldGenerateWithSimpleSchema()
		{
			CodeTypeDeclarationCollection types = generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\SimpleSchema.xsd"));

			Assert.AreEqual<int>(1, types.Count);
			Assert.IsTrue(types[0].IsClass);
			Assert.AreEqual<string>("element1", types[0].Name);
			Assert.AreEqual<int>(6, types[0].Members.Count); 
			Assert.AreEqual<string>("Data1", types[0].Members[3].Name);
			Assert.AreEqual<string>("Data2", types[0].Members[5].Name);
			Assert.AreEqual("http://tempuri.org/SimpleSchema.xsd",
                GetNamespaceCustomAttribute(types[0]));
		}


       

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\SimpleSchema.xsd")]
		public void ShouldGenerateUriLocation()
		{
			CodeTypeDeclarationCollection types = generator.GenerateTypes(new Uri(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\SimpleSchema.xsd")).AbsoluteUri);

			Assert.AreEqual<int>(1, types.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\IncludedSchema1.xsd")]
		[DeploymentItem(@"SampleData\DescriptionModel\IncludedSchema2.xsd")]
		public void ShouldGenerateWithIncludeSchema()
		{
			CodeTypeDeclarationCollection types = 
				generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\IncludedSchema1.xsd"));

			Assert.AreEqual<int>(2, types.Count);
			Assert.IsTrue(types[0].IsClass);
			Assert.IsTrue(types[1].IsClass);
			Assert.AreEqual<string>("element1", types[1].Name);
			Assert.AreEqual<int>(4, types[0].Members.Count);
			Assert.AreEqual<string>("Data", types[1].Members[3].Name);
			Assert.AreEqual<string>("element2", types[0].Name);
			Assert.AreEqual<int>(4, types[1].Members.Count);
			Assert.AreEqual<string>("SomeData", types[0].Members[3].Name);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\ComplexTypeSchema.xsd")]
		public void ShouldGenerateWithXmlSchemaImporter()
		{
            XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(true);
			CodeTypeDeclarationCollection types = generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\ComplexTypeSchema.xsd"));

			Assert.AreEqual<int>(2, types.Count);
			Assert.IsTrue(types[0].IsClass);
			Assert.IsTrue(types[1].IsClass);
			Assert.AreEqual<string>("complexType", types[1].Name);
			Assert.AreEqual<int>(2, types[0].Members.Count);
			Assert.AreEqual<string>("element1", types[0].Name);
			Assert.AreEqual<int>(8, types[1].Members.Count);
			Assert.AreEqual<string>("ComplexData", types[0].Members[1].Name);
			Assert.AreEqual("http://tempuri.org/ComplexTypeSchema.xsd",
				((CodePrimitiveExpression)types[0].CustomAttributes[5].Arguments[0].Value).Value);
		}

		[TestMethod]
		public void ShouldNotGenerateWithEmptyImporter()
		{
			CodeTypeDeclarationCollection types = generator.GenerateTypes(new WsdlImporter(new MetadataSet()));

			Assert.AreEqual<int>(0, types.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.wsdl0")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd0")]
		[DeploymentItem(@"SampleData\DescriptionModel\MockService.xsd1")]
		public void ShouldGenerateWithImporter()
		{
			WsdlImporter importer = DescriptionModelHelper.CreateImporter(@"SampleData\DescriptionModel\MockService.wsdl");
			CodeTypeDeclarationCollection types = generator.GenerateTypes(importer);

			Assert.AreEqual<int>(1, types.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\Enums.xsd")]
		public void ShouldGenerateEnumType()
		{
			CodeTypeDeclarationCollection types = 
				generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\Enums.xsd"));

			Assert.AreEqual<int>(1, types.Count);
			Assert.IsTrue(types[0].IsEnum);
			Assert.AreEqual<int>(4, types[0].Members.Count);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\Collections.xsd")]
		public void ShouldGenerateCollectionType()
		{
			CodeTypeDeclarationCollection types = 
				generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\Collections.xsd"));

			Assert.AreEqual<int>(1, types.Count);
			Assert.IsTrue(types[0].IsClass);
			Assert.AreEqual<string>(typeof(List<>).FullName, types[0].BaseTypes[0].BaseType);
			Assert.AreEqual<int>(1, types[0].BaseTypes[0].TypeArguments.Count);
			Assert.AreEqual<string>("System.String", types[0].BaseTypes[0].TypeArguments[0].BaseType);
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\Restriction.xsd")]
		public void ShouldGenerateIsNullableCollectionType()
		{
            XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(true);
			CodeCompileUnit unit = generator.GenerateCodeCompileUnit(
				ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\Restriction.xsd"));

			Assert.AreEqual<int>(3, unit.Namespaces[0].Types.Count);
			foreach (CodeAttributeDeclaration attribute in unit.Namespaces[0].Types[0].CustomAttributes)
			{
				if (attribute.AttributeType.BaseType == typeof(XmlRootAttribute).FullName)
				{
					foreach (CodeAttributeArgument argument in attribute.Arguments)
					{
						if (argument.Name == "IsNullable")
						{
							Assert.IsFalse((bool)((CodePrimitiveExpression)argument.Value).Value);
							return;
						}
					}
				}
			}
			Assert.Fail("No XmlRootAttribute or IsNullable argument found");
		}

		[TestMethod]
		[DeploymentItem(@"SampleData\DescriptionModel\Company.xsd")]
		[DeploymentItem(@"SampleData\DescriptionModel\Person.xsd")]
		[DeploymentItem(@"SampleData\DescriptionModel\Product.xsd")]
		[DeploymentItem(@"SampleData\DescriptionModel\Vertical.xsd")]
		public void ShouldGeneratedWithIncludesAndXmlSerializer()
		{
            XmlSchemaTypeGenerator generator = new XmlSchemaTypeGenerator(true);
			CodeTypeDeclarationCollection types = generator.GenerateTypes(ConfigurationLoader.GetConfigurationFilePath(@"SampleData\DescriptionModel\Company.xsd"));
			Assert.IsNotNull(types, "No exception thrown");
        }

        #region private

        private static string GetNamespaceCustomAttribute(CodeTypeDeclaration typeDeclaration)
        {
            string namespaceValue = null;

            foreach (CodeAttributeDeclaration attrib in typeDeclaration.CustomAttributes)
            {
                foreach (CodeAttributeArgument arg in attrib.Arguments)
                {
                    if (arg.Name == "Namespace")
                    {
                        CodePrimitiveExpression expression = arg.Value as CodePrimitiveExpression;
                        if (expression != null) namespaceValue = expression.Value.ToString();
                    }
                }
            }

            return namespaceValue;
        }
        
		#endregion
    }
}
