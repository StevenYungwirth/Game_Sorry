//-----------------------------------------------------------------------
// <copyright file="Space.cs" company="CompanyName">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

using System.Windows.Controls;

namespace Game_Sorry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The class used to represent a space.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Encapsulation not yet taught.")]
    public class Space
    {
        /// <summary>
        /// The space's border.
        /// </summary>
        public Border SpaceBorder;

        /// <summary>
        /// The slide that is on the space.
        /// </summary>
        public Slide Slide;
    }
}
