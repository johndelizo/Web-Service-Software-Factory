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
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Microsoft.FxCop.Sdk;

namespace Microsoft.Practices.FxCop.Rules.WcfSemantic
{
    public sealed class DuplicateEndpointAddress : ServiceModelConfigurationRule
    {
        public DuplicateEndpointAddress()
            : base("DuplicateEndpointAddress")
        {
        }

        public override ProblemCollection Check(ServiceModelConfigurationManager configurationManager)
        {
            foreach (ServiceElement service in configurationManager.ServiceModelSection.Services.Services)
            {
                List<string> keys = new List<string>();
                foreach (ServiceEndpointElement endpoint in service.Endpoints)
                {
                    if (keys.Contains(endpoint.Address.OriginalString))
                    {
                        Resolution resolution = base.GetResolution(endpoint.Address.OriginalString);
                        Problem problem = new Problem(resolution);
                        base.Problems.Add(problem);
                        continue;
                    }
                    keys.Add(endpoint.Address.OriginalString);
                }
            }
            return base.Problems;
        }
    }
}
