using System;

namespace Ultraviolet.Presentation.Compiler
{
    /// <summary>
    /// Represents the base interface for expression compiler state objects.
    /// </summary>
    internal interface IExpressionCompilerState
    {
        /// <summary>
        /// Gets the working directory for the specified compilation.
        /// </summary>
        /// <returns>The working directory for the specified compilation.</returns>
        String GetWorkingDirectory();

        /// <summary>
        /// Gets the known type with the specified name.
        /// </summary>
        /// <param name="name">The name of the known type to retrieve.</param>
        /// <param name="type">The type associated with the specified name.</param>
        /// <returns><see langword="true"/> if the specified known type was retrieved; otherwise, <see langword="false"/>.</returns>
        Boolean GetKnownType(String name, out Type type);

        /// <summary>
        /// Gets the name of the specified element's default property.
        /// </summary>
        /// <param name="type">The type of the element to evaluate.</param>
        /// <param name="property">The name of the element's default property.</param>
        /// <returns><see langword="true"/> if the specified element's default property was retrieved; otherwise, <see langword="false"/>.</returns>
        Boolean GetElementDefaultProperty(Type type, out String property);

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        ICrossThreadUltravioletContext Ultraviolet { get; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the compiler should generate its output assembly in memory
        /// rather than writing a file to disk.
        /// </summary>
        Boolean GenerateInMemory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the compiler should work in the user's temporary directory
        /// instead of the application directory.
        /// </summary>
        Boolean WorkInTemporaryDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value specifying whether to write errors to a file in the working directory.
        /// </summary>
        Boolean WriteErrorsToFile { get; set; }

        /// <summary>
        /// Gets the component template manager that contains the application's registered components.
        /// </summary>
        ComponentTemplateManager ComponentTemplateManager { get; }
    }
}
