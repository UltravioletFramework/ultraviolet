using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ultraviolet.Core;

namespace Ultraviolet.Presentation.Uvss
{
    /// <summary>
    /// Contains methods for serializing and deserializing syntax trees.
    /// </summary>
    public static class SyntaxSerializer
    {
        /// <summary>
        /// Represents a method which is used to construct deserialized instances
        /// of the <see cref="SyntaxNode"/> class.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The <see cref="SyntaxNode"/> instance which was created.</returns>
        private delegate SyntaxNode SyntaxNodeDeserializer(BinaryReader reader, Int32 version);

        /// <summary>
        /// Initializes the <see cref="SyntaxSerializer"/> type.
        /// </summary>
        static SyntaxSerializer()
        {
            var syntaxNodeTypes = typeof(SyntaxSerializer).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsSubclassOf(typeof(SyntaxNode))).ToList();

            foreach (var type in syntaxNodeTypes)
            {
                var attr = (SyntaxNodeTypeIDAttribute)type.GetCustomAttributes(
                    typeof(SyntaxNodeTypeIDAttribute), false).SingleOrDefault();

                if (attr == null)
                    throw new InvalidOperationException(UvssStrings.SyntaxNodeTypeNeedsTypeID.Format(type.Name));

                if (ctorByID.ContainsKey(attr.Value))
                    throw new InvalidOperationException(UvssStrings.SyntaxNodeTypeHasDuplicateTypeID.Format(type.Name));

                var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null,
                    new[] { typeof(BinaryReader), typeof(Int32) }, null);

                if (ctor == null)
                    throw new InvalidOperationException(UvssStrings.SyntaxNodeTypeNeedsDeserializationCtor.Format(type.Name));

                var expParameterReader = Expression.Parameter(typeof(BinaryReader), "reader");
                var expParameterVersion = Expression.Parameter(typeof(Int32), "version");
                var expNew = Expression.New(ctor, expParameterReader, expParameterVersion);
                var expLambda = Expression.Lambda(typeof(SyntaxNodeDeserializer), expNew, expParameterReader, expParameterVersion);
            
                idByType[type] = attr.Value;
                ctorByID[attr.Value] = (SyntaxNodeDeserializer)expLambda.Compile();
            }
        }

        /// <summary>
        /// Serializes a <see cref="SyntaxNode"/> instance to 
        /// the specified stream.
        /// </summary>
        /// <param name="writer">The binary writer with which to serialize the object.</param>
        /// <param name="node">The syntax node to serialize.</param>
        /// <param name="version">The file version of the data being written.</param>
        public static void ToStream(BinaryWriter writer, SyntaxNode node, Int32 version)
        {
            Contract.Require(writer, nameof(writer));
            Contract.Require(node, nameof(node));

            var typeID = default(Byte);
            if (!idByType.TryGetValue(node.GetType(), out typeID))
                throw new InvalidOperationException(UvssStrings.UnrecognizedSyntaxNodeType.Format(node.GetType().Name));

            writer.Write(typeID);

            node.Serialize(writer, version);
        }

        /// <summary>
        /// Deserializes a new <see cref="SyntaxNode"/> instance from
        /// the specified stream.
        /// </summary>
        /// <param name="reader">The binary reader with which to deserialize the object.</param>
        /// <param name="version">The file version of the data being read.</param>
        /// <returns>The <see cref="SyntaxNode"/> instance which was created.</returns>
        public static SyntaxNode FromStream(BinaryReader reader, Int32 version)
        {
            Contract.Require(reader, nameof(reader));

            var typeID = reader.ReadByte();
            var typeCtor = default(SyntaxNodeDeserializer);
            if (!ctorByID.TryGetValue(typeID, out typeCtor))
                throw new InvalidOperationException(UvssStrings.UnrecognizedSyntaxNodeTypeID.Format(typeID));

            return typeCtor(reader, version);
        }

        // Type IDs keyed by their types.
        private static readonly Dictionary<Type, Byte> idByType =
            new Dictionary<Type, Byte>();

        // Constructors keyed by their type IDs.
        private static readonly Dictionary<Byte, SyntaxNodeDeserializer> ctorByID =
            new Dictionary<Byte, SyntaxNodeDeserializer>();
    }
}
