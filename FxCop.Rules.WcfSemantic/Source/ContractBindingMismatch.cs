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
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.ServiceModel;
//using Microsoft.FxCop.Sdk;
//using System.ServiceModel.Configuration;
//using System.Diagnostics;
//using System.ServiceModel.Description;
//using System.ServiceModel.MsmqIntegration;

//namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
//{
//    /// <summary>
//    /// Class that implements the ContractBindingMismatch rule.
//    /// </summary>
//    /// <remarks>
//    /// This rule will check if there is a binding mismatch between
//    /// the defined service contract and the specified binding in config file.
//    /// An example may be having a duplex contract and a binding that does not
//    /// support it like <see cref="WSHttpBinding"/>. 
//    /// In this case, a <see cref="WSDualHttpBinding"/> should be used.
//    /// Note: This rule will not support 'offending line code navigation'. 
//    /// It's a 'configuration aware' rule.
//    /// </remarks>
//    public sealed class ContractBindingMismatch : ContractBindingRule
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="T:ContractBindingMismatch"/> class.
//        /// </summary>
//        public ContractBindingMismatch()
//            : base("ContractBindingMismatch")
//        {
//        }

//        /// <summary>
//        /// Evaluates the rule.
//        /// </summary>
//        /// <param name="type">The type.</param>
//        /// <param name="serviceContract">The service contract.</param>
//        /// <param name="binding">The binding.</param>
//        public override void EvaluateRule(TypeNode type, AttributeNode attribute, string binding)
//        {
//            if (ContractRequiresDuplex(SemanticRulesUtilities.GetAttributeValue<ClassNode>(attribute, "CallbackContract"), binding))
//            {
//                AddProblem(type.FullName, binding, Properties.Resources.ContractBindingRequiresDuplex);
//            }

//            foreach (Member member in type.Members)
//            {
//                EvaluateRuleForMember(member, binding);
//            }
//        }

//        private void EvaluateRuleForMember(Member member, string binding)
//        {
//            AttributeNode attribute = SemanticRulesUtilities.GetAttribute(member, typeof(OperationContractAttribute));

//            if (attribute != null &&
//                ConfigurationManager != null)
//            {
//                if (ContractRequiresTwoWay(SemanticRulesUtilities.GetAttributeValue<Boolean>(attribute, "IsOneWay"), binding))
//                {
//                    AddProblem(member.DeclaringType.FullName, binding, 
//                        Properties.Resources.ContractBindingRequiresTwoWay);
//                }
//            }
//        }

//        private void AddProblem(string contract, string binding, string message)
//        {
//            Resolution resolution = base.GetResolution(contract, message, binding);
//            Problem problem = new Problem(resolution);
//            base.Problems.Add(problem);
//        }

//        private bool ContractRequiresDuplex(ClassNode classNode, string binding)
//        {
//            return classNode != null && 
//                   !string.IsNullOrEmpty(classNode.FullName) &&
//                   !IsWSDualHttpBinding(binding);
//        }

//        private bool ContractRequiresTwoWay(Boolean isOneWay, string binding)
//        {
//            return !isOneWay &&
//                   (IsMsmqIntegrationBinding(binding) || IsNetMsmqBinding(binding));
//        }
//     }
//}
