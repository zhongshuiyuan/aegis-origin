﻿/// <copyright file="ConcurrentContainer.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2019 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace ELTE.AEGIS.Management.InversionOfControl
{
    /// <summary>
    /// Represents a thread-safe container of services.
    /// </summary>
    /// <remarks>
    /// This implementation of the <see cref="IContainer" /> interface can be accessed by multiple threads concurrently.
    /// </remarks>
    public class ConcurrentContainer : Container
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentContainer" /> class.
        /// </summary>
        public ConcurrentContainer() : 
            base(new ConcurrentDictionary<String, TypeRegistration>(), 
                 new ConcurrentDictionary<String, InstanceRegistration>())
        {
        }

        #endregion
    }
}
