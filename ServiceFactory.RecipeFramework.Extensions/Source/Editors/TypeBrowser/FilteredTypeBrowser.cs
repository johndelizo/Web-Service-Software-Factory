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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Design;
using EnvDTE;
using Microsoft.Practices.Common;
using Microsoft.Practices.VisualStudio.Helper;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Vs = VSLangProj;
using Web = VsWebSite;
using Microsoft.Practices.Modeling.Common;

namespace Microsoft.Practices.ServiceFactory.RecipeFramework.Extensions.Editors.TypeBrowser
{
	/// <summary>
	/// Editor that allows selection of a type from the current solution or a reference in the project.
	/// </summary>
	/// <remarks>
	/// The editor can optionally receive a configuration attribute named <c>Filter</c>, which must 
	/// contain a type that implements <see cref="ITypeFilterProvider"/>, used to filter the browse dialog. 
	/// <para>If no filter is specified through configuration, the type of the argument will be 
	/// inspected for the <see cref="TypeFilterProviderAttribute"/>.
	/// </para>
	/// <para>
	/// If no filter can be determined, a default filter that only allows browsing public types will be used.
	/// </para>
	/// </remarks>
	// FXCOP: False positive.
	[SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "filterTypeName")]
	public class FilteredTypeBrowser : UITypeEditor, IAttributesConfigurable
	{
		/// <summary>
		/// Configuration attribute with the name "Filter", to set an <see cref="ITypeFilterProvider"/> type 
		/// to use to filter the browse dialog.
		/// </summary>
		public const string FilterAttribute = "Filter";

		Type filterType;

		TypeBrowserEditor editor = new TypeBrowserEditor();
		ContextProxy flyweight;

		/// <summary>
		/// Edits the specified object's value using the editor style indicated by the <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> method.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
		/// <param name="provider">An <see cref="T:System.IServiceProvider"></see> that this editor can use to obtain services.</param>
		/// <param name="value">The object to edit.</param>
		/// <returns>
		/// The new value of the object. If the value of the object has not changed, this should return the same object it was passed.
		/// </returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			// Use flyweight pattern to improve performance. It's guaranteed that no more than one instance of 
			// this editor can ever be used at the same time. (it's modal)
			DetermineFilterType(context);

			if (flyweight == null)
			{
				flyweight = new ContextProxy(context, filterType);
			}
			else
			{
				flyweight.SetContext(context);
			}

			value = editor.EditValue(flyweight, flyweight, value);
			return value;
		}

		/// <summary>
		/// Gets the edit style.
		/// </summary>
		/// <param name="context">The type descriptor context.</param>
		/// <returns></returns>
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return editor.GetEditStyle(context);
		}

		#region Private Implementation

		private void DetermineFilterType(ITypeDescriptorContext context)
		{
			if (filterType == null)
			{
				// Look for the attribute in the argument type itself.
				TypeFilterProviderAttribute filterProvider = context.PropertyDescriptor.Attributes[typeof(TypeFilterProviderAttribute)] as TypeFilterProviderAttribute;
				if (filterProvider != null)
				{
					filterType = GetValidFilterType(filterProvider.TypeFilterProviderTypeName);
				}
			}
			if (filterType == null)
			{
				filterType = typeof(PublicTypeFilter);
			}
		}

		private Type GetValidFilterType(string filterTypeName)
		{
			Type ft = Type.GetType(filterTypeName, false, true);
			if (ft == null ||
				!typeof(ITypeFilterProvider).IsAssignableFrom(ft))
			{
				throw new ArgumentException(String.Format(
					CultureInfo.CurrentCulture,
					Properties.Resources.InvaInvalidTypeFilter,
					ft, typeof(ITypeFilterProvider)));
			}
			return ft;
		}

		#endregion

		#region ContextProxy class

		/// <summary>
		/// This proxy provides the additional ITypeProvider service required by the workflow type browser.
		/// </summary>
		private class ContextProxy : ITypeDescriptorContext
		{
			ITypeFilterProvider filterProvider;
			ITypeProvider typeProvider;
			ITypeDescriptorContext context;

			public ContextProxy(ITypeDescriptorContext context, Type filterType)
			{
				this.context = context;
				this.filterProvider = (ITypeFilterProvider)Activator.CreateInstance(filterType);
			}

			/// <summary>
			/// Allows resetting the context for the flyweight pattern.
			/// </summary>
			internal void SetContext(ITypeDescriptorContext context)
			{
				this.context = context;
			}

			#region ITypeDescriptorContext Members

			public IContainer Container
			{
				get { return context.Container; }
			}

			public object Instance
			{
				get { return context.Instance; }
			}

			public void OnComponentChanged()
			{
				context.OnComponentChanged();
			}

			public bool OnComponentChanging()
			{
				return context.OnComponentChanging();
			}

			/// <summary>
			/// Provides custom descriptor if a filter was specified, so that 
			/// the browser can use the filter.
			/// </summary>
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return new TypeFilterPropertyDescriptor(context.PropertyDescriptor, filterProvider);
				}
			}
			#endregion

			#region IServiceProvider Members

			/// <summary>
			/// Provides the <see cref="ITypeProvider"/> service.
			/// </summary>
			/// <param name="serviceType"></param>
			/// <returns></returns>
			public object GetService(Type serviceType)
			{
				if (serviceType == typeof(ITypeProvider))
				{
					if (typeProvider == null) typeProvider = new CustomTypeProvider(this);

					return typeProvider;
				}
				else if (serviceType == typeof(IDesignerHost))
				{
					return new DummyDesignerHost();
				}
				else if (serviceType == typeof(WorkflowDesignerLoader))
				{
					return new DummyWorkflowDesignerLoader();
				}
				else if (serviceType == typeof(IDictionaryService))
				{
					return new DummyDictionaryService();
				}

				return context.GetService(serviceType);
			}

			#endregion
		}

		#endregion

		#region CustomTypeProvider class

		/// <summary>
		/// Custom <see cref="ITypeProvider"/> that returns the types in the current project and 
		/// its references, using the <see cref="ITypeDiscoveryService"/> service acquired via the 
		/// <see cref="DynamicTypeService"/>.
		/// </summary>
		private class CustomTypeProvider : ITypeProvider
		{
			Dictionary<string, Type> availableTypes;

			/// <summary>
			/// Initializes a new instance of the <see cref="T:CustomTypeProvider"/> class.
			/// </summary>
			/// <param name="provider">The provider.</param>
			public CustomTypeProvider(IServiceProvider provider)
			{
				DynamicTypeService typeService = (DynamicTypeService)provider.GetService(typeof(DynamicTypeService));
				System.Diagnostics.Debug.Assert(typeService != null, "No dynamic type service registered.");

				IVsHierarchy hier = DteHelper2.GetCurrentSelection(provider);
				System.Diagnostics.Debug.Assert(hier != null, "No active hierarchy is selected.");

				ITypeDiscoveryService discovery = typeService.GetTypeDiscoveryService(hier);
				Project dteProject = VsHelper.ToDteProject(hier);

				availableTypes = new Dictionary<string, Type>();
				LoadTypes(availableTypes, discovery.GetTypes(typeof(object), false), dteProject);

                // Try loading Project references
                LoadProjectReferenceTypesFromCurrentProject(availableTypes, provider, dteProject);

				// If we don't get any type loaded, try with the rest of the projects in the current sln
				if (availableTypes.Count == 0)
				{
                    LoadTypesFromSolution(availableTypes, provider, dteProject);
				}

                EnsureKnownTypes(availableTypes);

				if (availableTypes.Count > 0 && TypesChanged != null)
				{
					TypesChanged(this, new EventArgs());
				}
			}

            // Add any missing KnownType (According to DataContractSerializer).
            private void EnsureKnownTypes(Dictionary<string, Type> availableTypes)
            {
                Type target = typeof(object);
                if (!availableTypes.ContainsValue(target))
                {
                    LoadTypes(availableTypes, target.Assembly.GetExportedTypes());
                }

                target = typeof(System.Guid);
                if (!availableTypes.ContainsValue(target))
                {
                    LoadTypes(availableTypes, target.Assembly.GetExportedTypes());
                }

                target = typeof(System.Xml.XmlQualifiedName);
                if (!availableTypes.ContainsValue(target))
                {
                    LoadTypes(availableTypes, target.Assembly.GetExportedTypes());
                }
            }

            private void LoadTypes(Dictionary<string, Type> availableTypes, ICollection types)
            {
                foreach (Type type in types)
                {
                    availableTypes.Add(type.FullName, type);
                }
            }

			private void LoadTypes(Dictionary<string, Type> availableTypes, ICollection types, Project dteProject)
			{
				bool isWebProject = DteHelper2.IsWebProject(dteProject);
				string projectAssemblyName = GetAssemblyName(dteProject);

				foreach (Type type in types)
				{
					// Filtering of non-public types must be done with a type filter provider. 
					// By default, if none is specified, the PublicTypeFilter will do just that.
					if (!availableTypes.ContainsKey(type.FullName))
					{
						if (!isWebProject &&
							!type.Assembly.FullName.StartsWith(projectAssemblyName, StringComparison.OrdinalIgnoreCase))
						{
							availableTypes.Add(type.FullName, type);
						}
						else
						{
							availableTypes.Add(type.FullName, new ProjectType(type));
						}
					}
				}
			}

            private void LoadTypesFromSolution(Dictionary<string, Type> availableTypes,
                IServiceProvider provider,
                Project dteProject)
            {
                IVsSolution solution = provider.GetService(typeof(SVsSolution)) as IVsSolution;
                if (solution != null)
                {
                    List<EnvDTE.Project> projectList = new List<Project>();

                    HierarchyNode solutionHierarchy = new HierarchyNode(solution);
                    solutionHierarchy.RecursiveForEach(delegate(HierarchyNode child)
                    {
                        // recurse if this node is a Solution Folder
                        if (child.TypeGuid != Microsoft.Practices.VisualStudio.Helper.Constants.SolutionFolderGuid)
                        {
                            // If this is a project add it to the list
                            EnvDTE.Project childProject = child.ExtObject as EnvDTE.Project;
                            if (childProject != null)
                                projectList.Add(childProject);
                        }
                    }
                    );
                    LoadTypesFromProjects(projectList, availableTypes, provider, dteProject);
                }
            }

            private void LoadProjectReferenceTypesFromCurrentProject(Dictionary<string, Type> availableTypes,
                IServiceProvider provider,
                Project currentProject)
            {
                List<Project> referencedProjects = new List<Project>();
                if (!DteHelper2.IsWebProject(currentProject))
                {
                    Vs.VSProject vsProject = currentProject.Object as Vs.VSProject;
                    foreach (Vs.Reference reference in vsProject.References)
                    {
                        if (reference.SourceProject != null)
                        {
                            referencedProjects.Add(reference.SourceProject);
                        }
                    }
                }
                else
                {
                    Web.VSWebSite webProject = currentProject.Object as Web.VSWebSite;
                    foreach (Web.AssemblyReference reference in webProject.References)
                    {
                        Project project = GetSourceProject(reference);
                        if (project != null)
                        {
                            referencedProjects.Add(project); ;
                        }
                    }
                }

                LoadTypesFromProjects(referencedProjects, availableTypes, provider, currentProject);
            }

			private void LoadTypesFromProjects(List<EnvDTE.Project> projects,
				Dictionary<string, Type> availableTypes, 
				IServiceProvider provider,
				Project currentProject)
			{
				DynamicTypeService typeService = (DynamicTypeService)provider.GetService(typeof(DynamicTypeService));
				foreach (Project project in projects)
				{
					if (project.UniqueName != currentProject.UniqueName)
					{
						IVsHierarchy hier = DteHelper2.GetVsHierarchy(provider, project);
						System.Diagnostics.Debug.Assert(hier != null, "No active hierarchy is selected.");
						ITypeDiscoveryService discovery = typeService.GetTypeDiscoveryService(hier);
						LoadTypes(availableTypes, discovery.GetTypes(typeof(object), false), project);
					}
				}
			}

			private string GetAssemblyName(Project project)
			{
				foreach (Property property in project.Properties)
				{
					if (property.Name == "AssemblyName")
					{
						return property.Value as string;
					}
				}
				return null;
			}

            private Project GetSourceProject(Web.AssemblyReference reference)
            {
                Project sourceProject = null;

                if (reference.ReferenceKind != Web.AssemblyReferenceType.AssemblyReferenceConfig)
                {
                    sourceProject = DteHelperEx.FindProject(reference.DTE, new Predicate<Project>(delegate(Project match)
                    {
                        return (match.Kind == VSLangProj.PrjKind.prjKindCSharpProject ||
                            match.Kind == VSLangProj.PrjKind.prjKindVBProject) &&
                            match.Name.Equals(reference.Name, StringComparison.OrdinalIgnoreCase);
                    }));
                }
                return sourceProject;
            }

			#region ITypeProvider Members

			/// <summary/>
			public event EventHandler TypeLoadErrorsChanged;

			/// <summary/>
			public event EventHandler TypesChanged;

			/// <summary>
			/// Gets a collection of all assemblies referenced by the <see cref="T:System.Type"></see>.
			/// </summary>
			/// <value></value>
			/// <returns>A collection of all assemblies referenced by the <see cref="T:System.Type"></see>.</returns>
			public ICollection<Assembly> ReferencedAssemblies
			{
				get { throw new NotImplementedException(); }
			}

			/// <summary>
			/// Gets the <see cref="T:System.Type"></see> of the named entity.
			/// </summary>
			/// <param name="name">A string that contains the name of the entity.</param>
			/// <param name="throwOnError">A value that indicates whether to throw an exception if name is not resolvable.</param>
			/// <returns>
			/// The <see cref="T:System.Type"></see> of the named entity.
			/// </returns>
			public Type GetType(string name, bool throwOnError)
			{
				if (String.IsNullOrEmpty(name))
				{
					return null;
				}

				if (availableTypes.ContainsKey(name))
				{
					Type type = availableTypes[name];
					if (type is TypeDelegator)
					{
						return ((TypeDelegator)type).UnderlyingSystemType;
					}
					else
					{
						return type;
					}
				}
				else
				{
					if (throwOnError)
					{
						if (TypeLoadErrorsChanged != null)
						{
							TypeLoadErrorsChanged(this, new EventArgs());
						}
						throw new TypeLoadException();
					}
					else
					{
						return null;
					}
				}
			}

			public Type GetType(string name)
			{
				return GetType(name, false);
			}

			public Type[] GetTypes()
			{
				Type[] result = new Type[availableTypes.Count];
				availableTypes.Values.CopyTo(result, 0);

				return result;
			}

			public System.Reflection.Assembly LocalAssembly
			{
				get { return this.GetType().Assembly; }
			}

			public System.Collections.Generic.IDictionary<object, Exception> TypeLoadErrors
			{
				get { return new Dictionary<object, Exception>(); }
			}

			#endregion

			#region ProjectType

			private class ProjectType : TypeDelegator
			{
				public ProjectType(Type delegatingType) : base(delegatingType) { }

				public override Assembly Assembly
				{
					get { return null; }
				}
			}

			#endregion
		}

		#endregion

		#region IAttributesConfigurable Members

		/// <summary>
		/// Configures the component using the dictionary of attributes specified
		/// in the configuration file.
		/// </summary>
		/// <param name="attributes">The attributes in the configuration element.</param>
		/// 		
		public void Configure(System.Collections.Specialized.StringDictionary attributes)
		{
			Guard.ArgumentNotNull(attributes, "attributes");
		}

		#endregion

		#region Dummy services for type browser initialization

		private class DummyDictionaryService : IDictionaryService
		{
			private Dictionary<object, object> dict = new Dictionary<object, object>();

			#region IDictionaryService Members

			public object GetKey(object value)
			{
				throw new NotImplementedException();
			}

			public object GetValue(object key)
			{
				if (dict.ContainsKey(key))
				{
					return dict[key];
				}
				return null;
			}

			public void SetValue(object key, object value)
			{
				if (dict.ContainsKey(key))
				{
					dict[key] = value;
				}
				else
				{
					dict.Add(key, value);
				}
			}

			#endregion
		}

		private class DummyDesignerHost : IDesignerHost
		{
			#region IDesignerHost Members

			public void Activate()
			{
				throw new NotImplementedException();
			}

#pragma warning disable 67
			public event EventHandler Activated;
#pragma warning restore 67

			public IContainer Container
			{
				get { return new Container(); }
			}

			public IComponent CreateComponent(Type componentClass, string name)
			{
				throw new NotImplementedException();
			}

			public IComponent CreateComponent(Type componentClass)
			{
				throw new NotImplementedException();
			}

			public DesignerTransaction CreateTransaction(string description)
			{
				throw new NotImplementedException();
			}

			public DesignerTransaction CreateTransaction()
			{
				throw new NotImplementedException();
			}
#pragma warning disable 67
			public event EventHandler Deactivated;
#pragma warning restore 67

			public void DestroyComponent(IComponent component)
			{
				throw new NotImplementedException();
			}

			public IDesigner GetDesigner(IComponent component)
			{
				throw new NotImplementedException();
			}

			public Type GetType(string typeName)
			{
				throw new NotImplementedException();
			}

			public bool InTransaction
			{
				get { throw new NotImplementedException(); }
			}

#pragma warning disable 67
			public event EventHandler LoadComplete;
#pragma warning restore 67

			public bool Loading
			{
				get { throw new NotImplementedException(); }
			}

			public IComponent RootComponent
			{
				get { return new Component(); }
			}

			public string RootComponentClassName
			{
				get { throw new NotImplementedException(); }
			}

#pragma warning disable 67
			public event DesignerTransactionCloseEventHandler TransactionClosed;

			public event DesignerTransactionCloseEventHandler TransactionClosing;
#pragma warning restore 67

			public string TransactionDescription
			{
				get { throw new NotImplementedException(); }
			}

#pragma warning disable 67
			public event EventHandler TransactionOpened;

			public event EventHandler TransactionOpening;
#pragma warning restore 67

			#endregion

			#region IServiceContainer Members

			public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
			{
				throw new NotImplementedException();
			}

			public void AddService(Type serviceType, ServiceCreatorCallback callback)
			{
				throw new NotImplementedException();
			}

			public void AddService(Type serviceType, object serviceInstance, bool promote)
			{
				throw new NotImplementedException();
			}

			public void AddService(Type serviceType, object serviceInstance)
			{
				throw new NotImplementedException();
			}

			public void RemoveService(Type serviceType, bool promote)
			{
				throw new NotImplementedException();
			}

			public void RemoveService(Type serviceType)
			{
				throw new NotImplementedException();
			}

			#endregion

			#region IServiceProvider Members

			public object GetService(Type serviceType)
			{
				return null;
			}

			#endregion
		}

		private class DummyWorkflowDesignerLoader : WorkflowDesignerLoader
		{
			public override string FileName
			{
				get { throw new NotImplementedException(); }
			}

			public override System.IO.TextReader GetFileReader(string filePath)
			{
				throw new NotImplementedException();
			}

			public override System.IO.TextWriter GetFileWriter(string filePath)
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}