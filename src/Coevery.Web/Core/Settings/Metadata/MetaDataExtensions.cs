using Coevery.ContentManagement.MetaData.Builders;

namespace Coevery.Core.Settings.Metadata {
    public static class MetaDataExtensions {
        //todo: revisit "creatable" and "attachable", other words by be more fitting
        public static ContentTypeDefinitionBuilder Creatable(this ContentTypeDefinitionBuilder builder, bool creatable = true) {
            return builder.WithSetting("ContentTypeSettings.Creatable", creatable.ToString());
        }

        public static ContentTypeDefinitionBuilder CollectionDisplayNameAs(this ContentTypeDefinitionBuilder builder, string collectionDisplayName)
        {
            return builder.WithSetting("CollectionDisplayName", collectionDisplayName);
        }
    }
}