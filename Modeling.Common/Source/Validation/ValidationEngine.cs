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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.Practices.Modeling.Common;
using Microsoft.Practices.Modeling.Common.Properties;
using Microsoft.Practices.Modeling.Common.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Practices.Modeling.Validation
{
	public static class ValidationEngine
	{
        private static ValidatorFactory validatorFactory;

        private const string RulesetPathKey = "rulesetPath";
        private const string DefaultRulesetFileName = "ruleset.config";
        private const string AbortCacheFlag = "abort";

		public const string CommonRuleSet = "Common";
		public const string MenuRuleSet = "Menu";

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void Validate(ValidationElementState state, ValidationContext context, ModelElement currentElement)
		{
			Guard.ArgumentNotNull(context, "context");
			Guard.ArgumentNotNull(currentElement, "currentElement");

            if (ShouldAbortValidation(context))
            {
                return;
            }
            
            try
            {
                InitializeFactory(context);

                if (validatorFactory != null)
                {
                    DoValidate(currentElement, context, CommonRuleSet);
                    DoValidate(currentElement, context, context.Categories.ToString());
                    ShowStatus(currentElement);
                    return;
                }
                // trace when no config service
                context.LogWarning(Resources.NoValidationServiceAvailable, Constants.ValidationCode, currentElement);
            }
            catch (Exception ex)
            {
                context.LogFatal(LogEntry.ErrorMessageToString(ex), Constants.ValidationCode, currentElement);
                Trace.TraceError(ex.ToString());
            }
		}

		public static string ModelElementToString(ModelElement element)
		{
			Guard.ArgumentNotNull(element, "element");

			string name;
			DomainClassInfo.TryGetName(element, out name);
			name = name ?? "no name";
			string type = element.GetType().Name;
			return String.Format(CultureInfo.CurrentUICulture, "{0} '{1}' {2}", type, name, element.Id);
		}

		public static string GetUniquePropertyValue(object element, string propertyName)
		{
			Guard.ArgumentNotNull(element, "element");
			Guard.ArgumentNotNull(propertyName, "propertyName");

			PropertyInfo property = element.GetType().GetProperty(propertyName);
			if (property == null)
			{
				return null;
			}
			return property.GetValue(element, null) as string;
		}

		public static string GetConfigurationRulePath()
		{
			return GetConfigurationRulePath(null);
		}

		public static string GetConfigurationRulePath(ValidationContext context)
		{
			return ProbeFromValidationContext(context) ?? RuntimeHelper.GetExecutionPath(DefaultRulesetFileName);
        }

        public static void Reset()
        {
            if (validatorFactory != null)
            {
                validatorFactory.ResetCache();
                validatorFactory = null;
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);    
            }
        }

        #region Private implementation

        private static void ShowStatus(ModelElement element)
        {
            VSStatusBar status = new VSStatusBar(element.Store);
            status.ShowMessage(string.Format(CultureInfo.CurrentCulture, Resources.ValidationStatusProgressMessage, ModelElementToString(element)));
        }

        private static void DoValidate(ModelElement element, ValidationContext context, string ruleSet)
        {
            Validator validator = validatorFactory.CreateValidator(element.GetType(), ruleSet);           
            ValidationResults results = validator.Validate(element);
            if (!results.IsValid)
            {
                WriteResultsToLog(context, results);
            }
        }

        private static void WriteResultsToLog(ValidationContext context, ValidationResults results)
        {
            Debug.Assert(context != null);
            Debug.Assert(results != null);

            foreach (ValidationResult result in results)
            {				
                if (result.Tag == Constants.LogWarningTag)
                {
					if (context.CurrentViolations.Count > 0)
					{
						// if we already have errors, then use context for warnings
						// otherwise use only Logger so we don't stop current process with only warnings.
						context.LogWarning(result.Message, Constants.ValidationCode, GetElements(result.Target));
					}
					else
					{
						Logger.Write(result.Message, TraceEventType.Warning);
					}
                }
                else
                {
					context.LogError(result.Message, Constants.ValidationCode, GetElements(result.Target));
                }
            }
        }

		private static ModelElement[] GetElements(object target)
		{
			ModelElement targetElement = target as ModelElement;
			if (targetElement != null)
			{
				return new ModelElement[] { targetElement };
			}
			else if (target != null)
			{
                if (target is IEnumerable)
                {
                    List<ModelElement> mels = new List<ModelElement>();
                    foreach (object item in (IEnumerable)target)
                    {
                        if (item is ModelElement)
                        {
                            mels.Add(item as ModelElement);
                        }
                    }
                    return mels.ToArray();
                }
				foreach (PropertyInfo property in target.GetType().GetProperties())
				{
					if (property.Name.Equals("ModelElement", StringComparison.Ordinal))
					{
						return new ModelElement[] { (ModelElement)property.GetValue(target, null) };
					}
				}
			}
			return new ModelElement[] { };
		}

        private static void MarkForAbort(ValidationContext context)
        {
            context.SetCacheValue<InvalidOperationException>(AbortCacheFlag, new InvalidOperationException());
        }

        private static bool ShouldAbortValidation(ValidationContext context)
        {
            InvalidOperationException value;
            return context.TryGetCacheValue<InvalidOperationException>(AbortCacheFlag, out value);
        }

        private static string ProbeFromValidationContext(ValidationContext context)
        {
            ValidationContextItem contextItem;
            if (context != null &&
				context.TryGetCacheValue<ValidationContextItem>(ValidationContextItem.RulesetPathKey, out contextItem))
            {
                return contextItem.Item.ToString();
            }
            return null;
        }

        private static void InitializeFactory(ValidationContext context)
        {
            if (validatorFactory == null)
            {
                string rulePath = GetConfigurationRulePath(context);
				if (!string.IsNullOrEmpty(rulePath))
                {
                    validatorFactory = ConfigurationValidatorFactory.FromConfigurationSource(new FileConfigurationSource(rulePath));
				}
            }
        }

        #endregion
    }
}