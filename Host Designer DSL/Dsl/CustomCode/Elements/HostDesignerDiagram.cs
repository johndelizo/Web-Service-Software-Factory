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
using System.Drawing;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Practices.ServiceFactory.Common.Dsl;

namespace Microsoft.Practices.ServiceFactory.HostDesigner
{
    // GeneratesDoubleDerived flag must be set in DSL Definition to override this.
    public partial class HostDesignerDiagram
    { 
        // setting as true will create an AreaField named "Background"
        // that we can customize in InitializeShapeFields function.
        public override bool HasBackgroundGradient
        {
            get { return true; }
        }

        public override System.Drawing.Drawing2D.LinearGradientMode BackgroundGradientMode
        {
            get { return System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal; }
        }

        // This method runs per model
        protected override void InitializeResources(StyleSet classStyleSet)
        {
            base.InitializeResources(classStyleSet);

            DiagramUtility.SetBackgroundGradient(classStyleSet,
                Color.PaleGoldenrod, Color.PapayaWhip, Color.OldLace, this, Properties.Resources.SurfaceTitle);
         }
    }
}
