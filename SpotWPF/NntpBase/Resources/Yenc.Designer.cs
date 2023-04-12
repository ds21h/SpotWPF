﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NntpBase.Resources {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Yenc {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Yenc() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Usenet.Resources.Yenc", typeof(Yenc).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Calculated checksum does not match footer checksum (crc32) property.
        /// </summary>
        internal static string ChecksumMismatch {
            get {
                return ResourceManager.GetString("ChecksumMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footer does not contain a checksum (crc32) property.
        /// </summary>
        internal static string MissingChecksum {
            get {
                return ResourceManager.GetString("MissingChecksum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing footer.
        /// </summary>
        internal static string MissingFooter {
            get {
                return ResourceManager.GetString("MissingFooter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing header.
        /// </summary>
        internal static string MissingHeader {
            get {
                return ResourceManager.GetString("MissingHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Footer does not contain a part checksum (pcrc32) property.
        /// </summary>
        internal static string MissingPartChecksum {
            get {
                return ResourceManager.GetString("MissingPartChecksum", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Missing part header.
        /// </summary>
        internal static string MissingPartHeader {
            get {
                return ResourceManager.GetString("MissingPartHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Calculated part checksum does not match footer part checksum (pcrc32) property.
        /// </summary>
        internal static string PartChecksumMismatch {
            get {
                return ResourceManager.GetString("PartChecksumMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Header part property does not match footer part property.
        /// </summary>
        internal static string PartMismatch {
            get {
                return ResourceManager.GetString("PartMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Part size does not match footer and/or header size property.
        /// </summary>
        internal static string PartSizeMismatch {
            get {
                return ResourceManager.GetString("PartSizeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Article size does not match footer size property.
        /// </summary>
        internal static string SizeMismatch {
            get {
                return ResourceManager.GetString("SizeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unexpected part header.
        /// </summary>
        internal static string UnexpectedPartHeader {
            get {
                return ResourceManager.GetString("UnexpectedPartHeader", resourceCulture);
            }
        }
    }
}
