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
using System.Collections.Specialized;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.ServiceFactory.ServiceContracts;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.Practices.ServiceFactory.Validation
{
	/// <summary>
	/// Validate that all the message parts in a service model have unique name when using XMLSerializer and the messages are not wrapped
	/// </summary>
	[ConfigurationElementType(typeof(CustomValidatorData))]
    public class UniqueMessagePartsCollectionValidator : Validator<IEnumerable<MessageBase>>
	{
        private const string WCFExtension = "WCF";

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters")]
        public UniqueMessagePartsCollectionValidator(NameValueCollection attributes)
			: base(null, null)
		{
		}

        protected override void DoValidate(IEnumerable<MessageBase> objectToValidate, object currentTarget, string key, Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResults validationResults)
        {
            ServiceContractModel scm = currentTarget as ServiceContractModel;
            if (scm == null)
            {
                return;
            }

            if (scm.ImplementationTechnology != null && 
                !scm.ImplementationTechnology.Name.Equals(WCFExtension, StringComparison.OrdinalIgnoreCase) ||
                scm.SerializerType != SerializerType.XmlSerializer)
            {
                return;
            }

			List<PrimitiveMessagePart> parts = new List<PrimitiveMessagePart>();

			foreach (MessageBase message in objectToValidate)
            {
                if (message is XsdMessage)
                {
                    continue;
                }

                if (!GetIsWrapped(message))
                {
					foreach (MessagePart part in message.MessageParts)
                    {
						PrimitiveMessagePart primitivePart = part as PrimitiveMessagePart;
						if (primitivePart != null)
						{
							if (parts.Count == 0)
							{
								parts.Add(primitivePart);
							}
							else
							{
								if (!IsValidPart(primitivePart, parts))
								{
									validationResults.AddResult(
									new ValidationResult(this.MessageTemplate, objectToValidate, key, String.Empty, this)
									);
									return;
								}
							}
						}
                    }
                }
            }
        }

		private bool IsValidPart(PrimitiveMessagePart item, List<PrimitiveMessagePart> parts)
        {
            foreach (PrimitiveMessagePart part in parts)
            {
                if (part.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase) &&
                    !part.Type.Equals(item.Type, StringComparison.OrdinalIgnoreCase) ||
                    part.IsCollection != item.IsCollection)
                {
                    return false;
                }
            }
            return true;
        }

        private bool GetIsWrapped(MessageBase message)
        {
            bool result = default(bool);
            object extender = message.ObjectExtender;
            if (extender == null)
            {
                return result;
            }

            PropertyInfo propInfo = extender.GetType().GetProperty("IsWrapped");
            if (propInfo != null)
            {
                bool iswrapped = (bool)propInfo.GetValue(extender, null);
                result = iswrapped;
            }

            return result;
        }

        protected override string DefaultMessageTemplate
        {
            get { return Resources.UniqueMessagePartsCollectionValidatorMessage; }
        }
	}
}
