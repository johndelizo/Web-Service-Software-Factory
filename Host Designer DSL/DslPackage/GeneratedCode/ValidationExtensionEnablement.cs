﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DslValidation = global::Microsoft.VisualStudio.Modeling.Validation;
using ExtensionEnablement = global::Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using DesignerExtensionEnablement = global::Microsoft.Practices.ServiceFactory.HostDesigner.ExtensionEnablement;
using MEF = global::System.ComponentModel.Composition;
using DslShell = global::Microsoft.VisualStudio.Modeling.Shell;

namespace Microsoft.Practices.ServiceFactory.HostDesigner
{
	/// <summary>
	/// Partial implementation to initialize the ValidationExtensionRegistrar for this designer
	/// </summary>
	internal abstract partial class HostDesignerDocDataBase
	{
		/// <summary>
		/// Add ValidationExtensionRegistrar to the ValidationController and handle related MEF Initialization operations
		/// </summary>	
		partial void SetValidationExtensionRegistrar(DslValidation::ValidationController validationController)
		{
			if (validationController != null)
			{
				MEF::ICompositionService compositionService = this.CompositionService;
				if (compositionService != null)
				{

					validationController.ValidationExtensionRegistrar = new DesignerExtensionEnablement::HostDesignerValidationExtensionRegistrar();
	
					// ValidationExtensionRegistrar is not registered for MEF Recomposition notification. The default ValidationExtensionRegistrar does not handle the MEF Recomposition call.
					compositionService.SatisfyImportsOnce(MEF::AttributedModelServices.CreatePart(validationController.ValidationExtensionRegistrar));
				}
			}
		}
	}
}