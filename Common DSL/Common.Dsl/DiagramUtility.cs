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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Drawing;

namespace Microsoft.Practices.ServiceFactory.Common.Dsl
{
    public static class DiagramUtility
    { 
        public static void SetBackgroundGradient(
            StyleSet classStyleSet,
            Color backgroundGradientColor, 
            Color backgroundSelectedGradientColor, 
            Color backgroundSelectedInactiveGradientColor,
            Diagram diagram,
            string surfaceTitle)
        {
            #region Set background gradient style and shape attributes

            // Fill brush settings for background (Start gradient color).
            BrushSettings backgroundBrush = new BrushSettings();
            backgroundBrush.Color = backgroundGradientColor;
            classStyleSet.OverrideBrush(DiagramBrushes.ShapeBackground, backgroundBrush);
            // Selected state
            backgroundBrush = new BrushSettings();
            backgroundBrush.Color = backgroundSelectedGradientColor;            
            classStyleSet.OverrideBrush(DiagramBrushes.ShapeBackgroundSelected, backgroundBrush);
            // SelectedInactive state
            backgroundBrush = new BrushSettings();
            backgroundBrush.Color = backgroundSelectedInactiveGradientColor;
            classStyleSet.OverrideBrush(DiagramBrushes.ShapeBackgroundSelectedInactive, backgroundBrush);
            
            // We should find a "Background" field created when we set the 
            // HasBackgroundGradient property to "true"
            AreaField background = diagram.FindShapeField("Background") as AreaField;
            if (background != null)
            {
                background.DefaultReflectParentSelectedState = true;
                //background.AnchoringBehavior.SetBottomAnchor(AnchoringBehavior.Edge.Bottom, diagram.MaximumSize.Height / 2);
            }

            #endregion

            #region Set Diagram font and text attributes

            // Custom font styles for diagram title
            FontSettings fontSettings;
            fontSettings = new FontSettings();
            fontSettings.Style = FontStyle.Bold;
            fontSettings.Size = 9 / 72.0F;
            classStyleSet.OverrideFont(DiagramFonts.ShapeTitle, fontSettings);

            // Create a text field for the Diagram Title
            TextField textField = new TextField("DiagramTitle");
            textField.DefaultText = surfaceTitle;
            textField.DefaultVisibility = true;
            textField.DefaultAutoSize = true;
            textField.DefaultFontId = DiagramFonts.ShapeTitle;
            textField.AnchoringBehavior.SetLeftAnchor(AnchoringBehavior.Edge.Left, 0.33);
            textField.AnchoringBehavior.SetTopAnchor(AnchoringBehavior.Edge.Top, 0.07);

            diagram.ShapeFields.Add(textField);

            #endregion
        }
    }
}
