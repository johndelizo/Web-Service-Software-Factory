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
using System.Collections;
using System.Globalization;

namespace Microsoft.Practices.Modeling.Common
{
	/// <summary>
	/// A static helper class that includes various parameter checking routines.
	/// </summary>
    public static class Guard
    {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the given argument is null.
        /// </summary>
        /// <exception cref="ArgumentNullException"> if tested value if null.</exception>
        /// <param name="argumentValue">Argument value to test.</param>
        /// <param name="argumentName">Name of the argument being tested.</param>
        public static void ArgumentNotNull(object argumentValue,
                                           string argumentName)
        {
            if (argumentValue == null) throw new ArgumentNullException(argumentName);
        }

        /// <summary>
        /// Throws an exception if the tested string argument is null or the empty string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if string value is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the string is empty</exception>
        /// <param name="argumentValue">Argument value to check.</param>
        /// <param name="argumentName">Name of argument being checked.</param>
        public static void ArgumentNotNullOrEmptyString(string argumentValue,
                                                  string argumentName)
        {
            if (argumentValue == null) throw new ArgumentNullException(argumentName);
            if (argumentValue.Length == 0) throw new ArgumentException(Properties.Resources.ArgumentMustNotBeEmpty, argumentName);
        }

        /// <summary>
        /// Throws an exception if the tested string argument is null, empty or have only spaces.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if string value is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the string is empty</exception>
        /// <param name="argumentValue">Argument value to check.</param>
        /// <param name="argumentName">Name of argument being checked.</param>
        public static void ArgumentNotNullOrWhiteSpaceString(string argumentValue,
                                                  string argumentName)
        {
            if (argumentValue == null) throw new ArgumentNullException(argumentName);
            if (string.IsNullOrWhiteSpace(argumentValue)) throw new ArgumentException(Properties.Resources.ArgumentMustNotBeEmptyOrWhiteSpace, argumentName);
        }

        /// <summary>
        /// Throws an exception if the tested collection argument is null or has not elements.
        /// </summary>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        public static void ArgumentNotNullOrEmpty(ICollection argumentValue,
                                          string argumentName)
        {
            if (argumentValue == null) throw new ArgumentNullException(argumentName);
            if (argumentValue.Count == 0) throw new ArgumentException(Properties.Resources.ArgumentMustNotBeEmpty, argumentName);
        }

        /// <summary>
        /// Verifies that an argument type is assignable from the provided type (meaning
        /// interfaces are implemented, or classes exist in the base class hierarchy).
        /// </summary>
        /// <param name="assignmentTargetType">The argument type that will be assigned to.</param>
        /// <param name="assignmentValueType">The type of the value being assigned.</param>
        /// <param name="argumentName">Argument name.</param>
        public static void TypeIsAssignable(Type assignmentTargetType, Type assignmentValueType, string argumentName)
        {
            if (assignmentTargetType == null) throw new ArgumentNullException("assignmentTargetType");
            if (assignmentValueType == null) throw new ArgumentNullException("assignmentValueType");

            if (!assignmentTargetType.IsAssignableFrom(assignmentValueType))
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.TypesAreNotAssignable,
                    assignmentTargetType,
                    assignmentValueType),
                    argumentName);
            }
        }
    }
}
