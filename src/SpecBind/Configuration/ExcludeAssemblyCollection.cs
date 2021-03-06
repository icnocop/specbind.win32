﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecBind.Configuration
{
    public class ExcludeAssemblyCollection : ConfigurationElementCollection
    {
        private readonly List<ExcludeAssemblyElement> excludeAssemblies = new List<ExcludeAssemblyElement>();

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            var newAssembly = new ExcludeAssemblyElement();
            this.excludeAssemblies.Add(newAssembly);
            return newAssembly;
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return this.excludeAssemblies.Find(a => a.Equals(element));
        }
    }
}
