﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Practices.Modeling.CodeGeneration.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.Modeling.CodeGeneration.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generated artifact {0} should also be removed..
        /// </summary>
        internal static string ArtifactDeleted {
            get {
                return ResourceManager.GetString("ArtifactDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generated artifact {0} should also be renamed to {1}..
        /// </summary>
        internal static string ArtifactRenamed {
            get {
                return ResourceManager.GetString("ArtifactRenamed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timestamp: {timestamp}{newline}Message: {message}{newline}EventId: {eventid}{newline}Severity: {severity}{newline}Title:{title}{newline}.
        /// </summary>
        internal static string DefaultTextFormat {
            get {
                return ResourceManager.GetString("DefaultTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected error has occurred while generating code..
        /// </summary>
        internal static string Generation_Error {
            get {
                return ResourceManager.GetString("Generation_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Code Generation.
        /// </summary>
        internal static string Generation_Error_Title {
            get {
                return ResourceManager.GetString("Generation_Error_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An unexpected exception has occurred while generating code. Check the Error List Window..
        /// </summary>
        internal static string Generation_Exception {
            get {
                return ResourceManager.GetString("Generation_Exception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is more than one project with the role &quot;{0}&quot;..
        /// </summary>
        internal static string MoreThanOneProjectWithSameRole {
            get {
                return ResourceManager.GetString("MoreThanOneProjectWithSameRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No CodeGenerationStrategyAttribute found on IArtifactLink object..
        /// </summary>
        internal static string NoCodeGenerationStrategyAttribute {
            get {
                return ResourceManager.GetString("NoCodeGenerationStrategyAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DIS is null..
        /// </summary>
        internal static string NullDIS {
            get {
                return ResourceManager.GetString("NullDIS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DTE is null..
        /// </summary>
        internal static string NullDTE {
            get {
                return ResourceManager.GetString("NullDTE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ServiceProvider is null..
        /// </summary>
        internal static string NullServiceProvider {
            get {
                return ResourceManager.GetString("NullServiceProvider", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no role associated with this artifact link..
        /// </summary>
        internal static string UnmappedRole {
            get {
                return ResourceManager.GetString("UnmappedRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is no project with the role &quot;{0}&quot;..
        /// </summary>
        internal static string ZeroProjectWithRole {
            get {
                return ResourceManager.GetString("ZeroProjectWithRole", resourceCulture);
            }
        }
    }
}
