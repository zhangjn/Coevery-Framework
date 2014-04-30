using System;
using Coevery.ContentManagement.MetaData.Builders;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Utility.Extensions;

namespace Coevery.ContentManagement.MetaData {
    public interface IContentDefinitionManager : IDependency {
        ContentTypeDefinition GetTypeDefinition(string name);
        ContentPartDefinition GetPartDefinition(string name);

        void DeleteTypeDefinition(string name);
        void DeletePartDefinition(string name);

        void StoreTypeDefinition(ContentTypeDefinition contentTypeDefinition, VersionOptions options);
        void StorePartDefinition(ContentPartDefinition contentPartDefinition, VersionOptions options);
    }

    public static class ContentDefinitionManagerExtensions{
        public static void AlterTypeDefinition(this IContentDefinitionManager manager, string name, Action<ContentTypeDefinitionBuilder> alteration) {
            AlterTypeDefinition(manager, name, VersionOptions.Published, alteration);
        }

        public static void AlterTypeDefinition(this IContentDefinitionManager manager, string name, VersionOptions options, Action<ContentTypeDefinitionBuilder> alteration) {
            var typeDefinition = manager.GetTypeDefinition(name) ?? new ContentTypeDefinition(name, name.CamelFriendly());
            var builder = new ContentTypeDefinitionBuilder(typeDefinition);
            alteration(builder);
            manager.StoreTypeDefinition(builder.Build(), options);
        }

        public static void AlterPartDefinition(this IContentDefinitionManager manager, string name, Action<ContentPartDefinitionBuilder> alteration) {
            AlterPartDefinition(manager, name, VersionOptions.Published, alteration);
        }

        public static void AlterPartDefinition(this IContentDefinitionManager manager, string name, VersionOptions options, Action<ContentPartDefinitionBuilder> alteration) {
            var partDefinition = manager.GetPartDefinition(name) ?? new ContentPartDefinition(name);
            var builder = new ContentPartDefinitionBuilder(partDefinition);
            alteration(builder);
            manager.StorePartDefinition(builder.Build(), options);
        }
    }
}

