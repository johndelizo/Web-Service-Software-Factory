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
using EnvDTE;

namespace Microsoft.Practices.UnitTestLibrary
{
	public class MockCodeModel : CodeModel
	{
		public MockCodeModel(string language)
		{
			this.language = language;
		}

		private string language;

		#region CodeModel Members

		public CodeAttribute AddAttribute(string Name, object Location, string Value, object Position)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeClass AddClass(string Name, object Location, object Position, object Bases, object ImplementedInterfaces, vsCMAccess Access)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeDelegate AddDelegate(string Name, object Location, object Type, object Position, vsCMAccess Access)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeEnum AddEnum(string Name, object Location, object Position, object Bases, vsCMAccess Access)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeFunction AddFunction(string Name, object Location, vsCMFunction Kind, object Type, object Position, vsCMAccess Access)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeInterface AddInterface(string Name, object Location, object Position, object Bases, vsCMAccess Access)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeNamespace AddNamespace(string Name, object Location, object Position)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeStruct AddStruct(string Name, object Location, object Position, object Bases, object ImplementedInterfaces, vsCMAccess Access)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeVariable AddVariable(string Name, object Location, object Type, object Position, vsCMAccess Access)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeElements CodeElements
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public CodeType CodeTypeFromFullName(string Name)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public CodeTypeRef CreateCodeTypeRef(object Type)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public DTE DTE
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsCaseSensitive
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public bool IsValidID(string Name)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public string Language
		{
			get { return language; }
			internal set { language = value; }
		}

		public Project Parent
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void Remove(object Element)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}
