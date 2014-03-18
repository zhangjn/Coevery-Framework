using System;

namespace Coevery.DeveloperTools.CodeGeneration.Utils {
    /// <summary>Provides extension methods for <see cref="Type"/>.</summary>
    public static class TypeExtensions {
        /// <summary>Gets the C# friendly name of a type, if any.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The type name.</returns>
        /// <remarks>
        /// This method uses the <see cref="CSharpTypeNameProvider"/> for getting the name.
        /// </remarks>
        public static string GetFriendlyName(this Type source) {
            return GetFriendlyName(source, new CSharpTypeNameProvider());
        }

        /// <summary>Gets the C# friendly name of a type, if any.</summary>
        /// <param name="type">The type.</param>
        /// <param name="includeNamespace"><see langword="true"/> to include the namespace in the type.</param>
        /// <returns>The type name.</returns>
        /// <remarks>
        /// This method uses the <see cref="CSharpTypeNameProvider"/> for getting the name.
        /// </remarks>
        public static string GetFriendlyName(this Type source, bool includeNamespace) {
            return GetFriendlyName(source, new CSharpTypeNameProvider() {IncludeNamespace = includeNamespace});
        }

        /// <summary>Gets the C# friendly name of a type, if any.</summary>
        /// <param name="type">The type.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>The type name.</returns>        
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> is <see langword="null"/>.</exception>
        public static string GetFriendlyName(this Type source, TypeNameProvider provider) {
            if (provider == null)
                throw new ArgumentNullException("provider");

            return provider.GetTypeName(source);
        }
    }
}
