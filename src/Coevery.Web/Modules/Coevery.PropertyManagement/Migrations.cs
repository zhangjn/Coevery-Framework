using System;
using System.Globalization;
using Coevery.ContentManagement;
using Coevery.ContentManagement.MetaData;
using Coevery.ContentManagement.MetaData.Builders;
using Coevery.ContentManagement.MetaData.Models;
using Coevery.Core.OptionSet.Models;
using Coevery.Core.Settings.Models;
using Coevery.Data.Migration;
using Coevery.Localization.Services;
using Coevery.Settings;

namespace Coevery.PropertyManagement
{
    public class DataMigration : DataMigrationImpl
    {
        private readonly ICultureManager _cultureManager;
        private readonly IContentManager _contentManager;
        private readonly ISiteService _siteService;

        public DataMigration(IContentManager contentManager,
            ICultureManager cultureManager,
            ISiteService siteService)
        {
            _contentManager = contentManager;
            _cultureManager = cultureManager;
            _siteService = siteService;
        }

        public int Create()
        {
            SchemaBuilder.AlterTable("User",
                table => { });
            Action<ContentPartDefinitionBuilder> userPartAlteration =
                part => { }
                ;

            ContentDefinitionManager.AlterTypeDefinition("User",
                type => type
                    .DisplayedAs("用户")
                    .WithPart(new ContentPartDefinition("UserPart"), configuration => { }, userPartAlteration)
                    .WithSetting("CollectionDisplayName", "Users")
                    .WithSetting("ContentTypeSettings.Creatable", "True")
                );

            //SchemaBuilder.CreateTable("Building",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<int>("Apartment")
            //        .Column<string>("Name", column => column.WithLength(255))
            //        .Column<string>("Description", column => column.WithLength(5000))
            //    );
            //Action<ContentPartDefinitionBuilder> buildingPartAlteration =
            //    part => part
            //        .WithField("Apartment", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "楼盘")
            //            .WithSetting("EntityName", "Building")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "False")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "Apartment")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "Building_Apartment_Name")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("Name", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "名称")
            //            .WithSetting("EntityName", "Building")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Description", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "备注")
            //            .WithSetting("EntityName", "Building")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("Building",
            //    type => type
            //        .DisplayedAs("楼宇")
            //        .WithPart(new ContentPartDefinition("BuildingPart"), configuration => { }, buildingPartAlteration)
            //        .WithSetting("CollectionDisplayName", "Buildings")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Apartment\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            //SchemaBuilder.CreateTable("Customer",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<string>("Number", column => column.WithLength(255))
            //        .Column<string>("Name", column => column.WithLength(255))
            //        .Column<string>("CustomerType")
            //        .Column<string>("Phone")
            //        .Column<string>("Description", column => column.WithLength(5000))
            //    );
            //Action<ContentPartDefinitionBuilder> customerPartAlteration =
            //    part => part
            //        .WithField("Number", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "客户编号")
            //            .WithSetting("EntityName", "Customer")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Name", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "客户姓名")
            //            .WithSetting("EntityName", "Customer")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("CustomerType", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "客户类型")
            //            .WithSetting("EntityName", "Customer")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "True")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId", CreateCustomerPartCustomerTypeOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Phone", column => column
            //            .OfType("PhoneField")
            //            .WithSetting("DisplayName", "联系电话")
            //            .WithSetting("EntityName", "Customer")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("PhoneFieldSettings.HelpText", "")
            //            .WithSetting("PhoneFieldSettings.Required", "False")
            //            .WithSetting("PhoneFieldSettings.ReadOnly", "False")
            //            .WithSetting("PhoneFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("PhoneFieldSettings.IsSystemField", "False")
            //            .WithSetting("PhoneFieldSettings.IsAudit", "False")
            //            .WithSetting("PhoneFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Description", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "描述")
            //            .WithSetting("EntityName", "Customer")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("Customer",
            //    type => type
            //        .DisplayedAs("客户")
            //        .WithPart(new ContentPartDefinition("CustomerPart"), configuration => { }, customerPartAlteration)
            //        .WithSetting("CollectionDisplayName", "Customers")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Number\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomerType\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Phone\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            SchemaBuilder.CreateTable("Apartment",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("Name", column => column.WithLength(255))
                    .Column<string>("Address", column => column.WithLength(255))
                    .Column<string>("Description", column => column.WithLength(5000))
                );
            //Action<ContentPartDefinitionBuilder> apartmentPartAlteration =
            //    part => part
            //        .WithField("Name", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "名称")
            //            .WithSetting("EntityName", "Apartment")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "True")
            //        )
            //        .WithField("Address", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "地址")
            //            .WithSetting("EntityName", "Apartment")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "False")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Description", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "备注")
            //            .WithSetting("EntityName", "Apartment")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("Apartment",
            //    type => type
            //        .DisplayedAs("楼盘")
            //        .WithPart(new ContentPartDefinition("ApartmentPart"), configuration => { }, apartmentPartAlteration)
            //        .WithSetting("CollectionDisplayName", "Apartments")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Address\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            //SchemaBuilder.CreateTable("MeterType",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<string>("Name", column => column.WithLength(255))
            //        .Column<string>("Description", column => column.WithLength(5000))
            //    );
            //Action<ContentPartDefinitionBuilder> meterTypePartAlteration =
            //    part => part
            //        .WithField("Name", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "名称")
            //            .WithSetting("EntityName", "MeterType")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Description", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "备注")
            //            .WithSetting("EntityName", "MeterType")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("MeterType",
            //    type => type
            //        .DisplayedAs("仪表类型")
            //        .WithPart(new ContentPartDefinition("MeterTypePart"), configuration => { }, meterTypePartAlteration)
            //        .WithSetting("CollectionDisplayName", "MeterTypes")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            //SchemaBuilder.CreateTable("HouseChargeItem",
            //    table => table
            //        .Column<int>("Id", column => column.PrimaryKey().Identity())
            //        .Column<int>("ChargeItemId")
            //        .Column<decimal>("Unit")
            //        .Column<double>("Amount")
            //        .Column<DateTime>("BeginDate")
            //        .Column<DateTime>("EndDate")
            //        .Column<string>("Description", column => column.WithLength(5000))
            //        .Column<int>("HouseId")
            //        .Column<int>("ChargeItemSettingId")
            //    );

            //SchemaBuilder.CreateTable("HouseMeter",
            //    table => table
            //        .Column<int>("Id", column => column.PrimaryKey().Identity())
            //        .Column<int>("MeterTypeId")
            //        .Column<string>("MeterNumber", column => column.WithLength(255))
            //        .Column<double>("Ratio")
            //        .Column<int>("HouseId")
            //    );

            //SchemaBuilder.CreateTable("House",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<int>("Building")
            //        .Column<int>("Apartment")
            //        .Column<int>("OwnerId")
            //        .Column<string>("HouseNumber", column => column.WithLength(255))
            //        .Column<int>("OfficerId")
            //        .Column<double>("BuildingArea")
            //        .Column<double>("InsideArea")
            //        .Column<double>("PoolArea")
            //        .Column<string>("HouseStatus")
            //    );
            //Action<ContentPartDefinitionBuilder> housePartAlteration =
            //    part => part
            //        .WithField("Building", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "楼宇")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "True")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "Building")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Building_Name")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("Apartment", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "楼盘")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "True")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "Apartment")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Apartment_Name")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("OwnerId", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "业主")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "True")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Customer_Name")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("HouseNumber", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "房号")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("OfficerId", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "专管员")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "False")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "House_User_Name")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("BuildingArea", column => column
            //            .OfType("NumberField")
            //            .WithSetting("DisplayName", "建筑面积")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "18")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "0")
            //        )
            //        .WithField("InsideArea", column => column
            //            .OfType("NumberField")
            //            .WithSetting("DisplayName", "套内面积")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "18")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "0")
            //        )
            //        .WithField("PoolArea", column => column
            //            .OfType("NumberField")
            //            .WithSetting("DisplayName", "公摊面积")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "18")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "0")
            //        )
            //        .WithField("HouseStatus", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "房屋状态")
            //            .WithSetting("EntityName", "House")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "True")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId", CreateHousePartHouseStatusOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "False")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("House",
            //    type => type
            //        .DisplayedAs("房间")
            //        .WithPart(new ContentPartDefinition("HousePart"), configuration => { }, housePartAlteration)
            //        .WithSetting("CollectionDisplayName", "Houses")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Building\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Apartment\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"OwnerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseNumber\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"OfficerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"BuildingArea\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"InsideArea\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"PoolArea\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseStatus\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            //SchemaBuilder.CreateTable("ChargeItemSetting",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<string>("Name", column => column.WithLength(255))
            //        .Column<string>("CalculationMethod")
            //        .Column<decimal>("UnitPrice")
            //        .Column<string>("MeteringMode")
            //        .Column<string>("ChargingPeriod")
            //        .Column<string>("CustomFormula", column => column.WithLength(255))
            //        .Column<string>("Remark", column => column.WithLength(255))
            //        .Column<decimal>("Money")
            //        .Column<int>("ChargeItemId")
            //    );
            //Action<ContentPartDefinitionBuilder> chargeItemSettingPartAlteration =
            //    part => part
            //        .WithField("Name", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "名称")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("CalculationMethod", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "金额计算方式")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "True")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemSettingPartCalculationMethodOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("UnitPrice", column => column
            //            .OfType("CurrencyField")
            //            .WithSetting("DisplayName", "单价")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("CurrencyFieldSettings.HelpText", "")
            //            .WithSetting("CurrencyFieldSettings.Required", "False")
            //            .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
            //            .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
            //            .WithSetting("CurrencyFieldSettings.IsAudit", "False")
            //            .WithSetting("CurrencyFieldSettings.Length", "18")
            //            .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
            //            .WithSetting("CurrencyFieldSettings.DefaultValue", "")
            //        )
            //        .WithField("MeteringMode", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "计量方式")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "False")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemSettingPartMeteringModeOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("ChargingPeriod", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "收费周期")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "False")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemSettingPartChargingPeriodOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "True")
            //        )
            //        .WithField("CustomFormula", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "自定义公式")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "255")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Remark", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "备注")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "255")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Money", column => column
            //            .OfType("CurrencyField")
            //            .WithSetting("DisplayName", "指定金额")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("CurrencyFieldSettings.HelpText", "")
            //            .WithSetting("CurrencyFieldSettings.Required", "False")
            //            .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
            //            .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
            //            .WithSetting("CurrencyFieldSettings.IsAudit", "False")
            //            .WithSetting("CurrencyFieldSettings.Length", "18")
            //            .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
            //            .WithSetting("CurrencyFieldSettings.DefaultValue", "")
            //        )
            //        .WithField("ChargeItemId", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "收费项目名称")
            //            .WithSetting("EntityName", "ChargeItemSetting")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "True")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "ChargeItem")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("ChargeItemSetting",
            //    type => type
            //        .DisplayedAs("收费标准")
            //        .WithPart(new ContentPartDefinition("ChargeItemSettingPart"), configuration => { },
            //            chargeItemSettingPartAlteration)
            //        .WithSetting("CollectionDisplayName", "ChargeItemSettings")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"ChargeItemId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CalculationMethod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"UnitPrice\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeteringMode\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ChargingPeriod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomFormula\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remark\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Money\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            //SchemaBuilder.CreateTable("ChargeItem",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<string>("ItemCategory")
            //        .Column<string>("Name", column => column.WithLength(255))
            //        .Column<string>("Remarks", column => column.WithLength(255))
            //        .Column<int>("MeterType")
            //        .Column<int>("DelayChargeDays")
            //        .Column<decimal>("DelayChargeRatio")
            //        .Column<string>("DelayChargeCalculationMethod")
            //        .Column<string>("StartCalculationDatetime")
            //        .Column<string>("ChargingPeriodPrecision")
            //        .Column<string>("DefaultChargingPeriod")
            //    );
            //Action<ContentPartDefinitionBuilder> chargeItemPartAlteration =
            //    part => part
            //        .WithField("ItemCategory", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "项目类别")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "True")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemPartItemCategoryOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Name", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "名称")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Remarks", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "描述")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "255")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("MeterType", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "仪表种类")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "False")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "MeterType")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "ChargeItem_MeterType")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("DelayChargeDays", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "欠费XX天滞纳金")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "2")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "0")
            //        )
            //        .WithField("DelayChargeRatio", column => column
            //            .OfType("NumberField")
            //            .WithSetting("DisplayName", "每天收取比例")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "18")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "")
            //        )
            //        .WithField("DelayChargeCalculationMethod", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "滞纳金计算方式")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "False")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemPartDelayChargeCalculationMethodOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "True")
            //        )
            //        .WithField("StartCalculationDatetime", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "开始计算时间")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "False")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemPartStartCalculationDatetimeOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "True")
            //        )
            //        .WithField("ChargingPeriodPrecision", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "不足一个收费周期")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "False")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemPartChargingPeriodPrecisionOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "True")
            //        )
            //        .WithField("DefaultChargingPeriod", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "默认收费周期")
            //            .WithSetting("EntityName", "ChargeItem")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "False")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateChargeItemPartDefaultChargingPeriodOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "True")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("ChargeItem",
            //    type => type
            //        .DisplayedAs("收费项目")
            //        .WithPart(new ContentPartDefinition("ChargeItemPart"), configuration => { },
            //            chargeItemPartAlteration)
            //        .WithSetting("CollectionDisplayName", "ChargeItems")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ItemCategory\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remarks\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeterType\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeDays\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeRatio\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeCalculationMethod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"StartCalculationDatetime\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ChargingPeriodPrecision\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DefaultChargingPeriod\",\"IsValid\":false}}],\"IsMerged\":false}]}]"));

            //SchemaBuilder.CreateTable("HouseMeterReading",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<int>("Year")
            //        .Column<int>("Month")
            //        .Column<int>("HouseMeterId")
            //        .Column<double>("MeterData")
            //        .Column<double>("Amount")
            //        .Column<string>("Status")
            //        .Column<string>("Remarks", column => column.WithLength(255))
            //    );
            //Action<ContentPartDefinitionBuilder> houseMeterReadingPartAlteration =
            //    part => part
            //        .WithField("Year", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "年份")
            //            .WithSetting("EntityName", "HouseMeterReading")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "2")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "0")
            //        )
            //        .WithField("Month", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "月份")
            //            .WithSetting("EntityName", "HouseMeterReading")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "2")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "0")
            //        )
            //        .WithField("HouseMeterId", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "房间仪表")
            //            .WithSetting("EntityName", "HouseMeterReading")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "False")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "HouseMeter")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "HouseReading_HouseMeter")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "MeterType")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("MeterData", column => column
            //            .OfType("NumberField")
            //            .WithSetting("DisplayName", "抄表读数")
            //            .WithSetting("EntityName", "HouseMeterReading")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "18")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "")
            //        )
            //        .WithField("Amount", column => column
            //            .OfType("NumberField")
            //            .WithSetting("DisplayName", "用量")
            //            .WithSetting("EntityName", "HouseMeterReading")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("NumberFieldSettings.HelpText", "")
            //            .WithSetting("NumberFieldSettings.Required", "False")
            //            .WithSetting("NumberFieldSettings.ReadOnly", "False")
            //            .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("NumberFieldSettings.IsSystemField", "False")
            //            .WithSetting("NumberFieldSettings.IsAudit", "False")
            //            .WithSetting("NumberFieldSettings.Length", "18")
            //            .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
            //            .WithSetting("NumberFieldSettings.DefaultValue", "")
            //        )
            //        .WithField("Status", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "状态")
            //            .WithSetting("EntityName", "HouseMeterReading")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "False")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateHouseMeterReadingPartStatusOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("Remarks", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "备注")
            //            .WithSetting("EntityName", "HouseMeterReading")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "False")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("HouseMeterReading",
            //    type => type
            //        .DisplayedAs("房间仪表抄表")
            //        .WithPart(new ContentPartDefinition("HouseMeterReadingPart"), configuration => { },
            //            houseMeterReadingPartAlteration)
            //        .WithSetting("CollectionDisplayName", "HouseMeterReadings")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Year\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Month\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseMeterId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeterData\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Amount\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Status\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remarks\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            //SchemaBuilder.CreateTable("ContractHouse",
            //    table => table
            //        .Column<int>("Id", c => c.PrimaryKey().Identity())
            //        .Column<int>("ContractId", c => c.NotNull())
            //        .Column<int>("HouseId", c => c.NotNull())
            //    );

            //SchemaBuilder.CreateTable("ContractChargeItem",
            //    table => table
            //        .Column<int>("Id", c => c.PrimaryKey().Identity())
            //        .Column<int>("ContractId", c => c.NotNull())
            //        .Column<int>("ChargeItemId", c => c.NotNull())
            //        .Column<int>("ChargeItemSettingId", c => c.NotNull())
            //        .Column<DateTime>("BeginDate")
            //        .Column<DateTime>("EndDate")
            //        .Column<string>("Description")
            //    );

            //SchemaBuilder.CreateTable("Contract",
            //    table => table
            //        .ContentPartRecord()
            //        .Column<string>("Number", column => column.WithLength(255))
            //        .Column<string>("Name", column => column.WithLength(255))
            //        .Column<int>("OwnerId")
            //        .Column<int>("RenterId")
            //        .Column<string>("ContractStatus")
            //        .Column<DateTime>("BeginDate")
            //        .Column<DateTime>("EndDate")
            //        .Column<string>("Description", column => column.WithLength(5000))
            //    );

            //Action<ContentPartDefinitionBuilder> contractPartAlteration =
            //    part => part
            //        .WithField("Number", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "合同编号")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "True")
            //        )
            //        .WithField("Name", column => column
            //            .OfType("TextField")
            //            .WithSetting("DisplayName", "合同名称")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("TextFieldSettings.HelpText", "")
            //            .WithSetting("TextFieldSettings.Required", "True")
            //            .WithSetting("TextFieldSettings.ReadOnly", "False")
            //            .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("TextFieldSettings.IsSystemField", "False")
            //            .WithSetting("TextFieldSettings.IsAudit", "False")
            //            .WithSetting("TextFieldSettings.MaxLength", "255")
            //            .WithSetting("TextFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("OwnerId", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "业主")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "True")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "Contract_Customer_OwnerId")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("RenterId", column => column
            //            .OfType("ReferenceField")
            //            .WithSetting("DisplayName", "租户")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("ReferenceFieldSettings.HelpText", "")
            //            .WithSetting("ReferenceFieldSettings.Required", "True")
            //            .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
            //            .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
            //            .WithSetting("ReferenceFieldSettings.IsAudit", "False")
            //            .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
            //            .WithSetting("ReferenceFieldSettings.RelationshipName", "Contract_Customer_RenterId_Name")
            //            .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
            //            .WithSetting("ReferenceFieldSettings.QueryId", "1")
            //            .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
            //            .WithSetting("ReferenceFieldSettings.IsUnique", "False")
            //            .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
            //            .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
            //        )
            //        .WithField("ContractStatus", column => column
            //            .OfType("OptionSetField")
            //            .WithSetting("DisplayName", "状态")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("OptionSetFieldSettings.HelpText", "")
            //            .WithSetting("OptionSetFieldSettings.Required", "True")
            //            .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
            //            .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
            //            .WithSetting("OptionSetFieldSettings.IsAudit", "False")
            //            .WithSetting("OptionSetFieldSettings.OptionSetId",
            //                CreateContractPartContractStatusOptionSetPart())
            //            .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
            //            .WithSetting("OptionSetFieldSettings.IsUnique", "False")
            //        )
            //        .WithField("BeginDate", column => column
            //            .OfType("DateField")
            //            .WithSetting("DisplayName", "起租时间")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("DateFieldSettings.HelpText", "")
            //            .WithSetting("DateFieldSettings.Required", "False")
            //            .WithSetting("DateFieldSettings.ReadOnly", "False")
            //            .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("DateFieldSettings.IsSystemField", "False")
            //            .WithSetting("DateFieldSettings.IsAudit", "False")
            //            .WithSetting("DateFieldSettings.DefaultValue", "")
            //        )
            //        .WithField("EndDate", column => column
            //            .OfType("DateField")
            //            .WithSetting("DisplayName", "到期时间")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("DateFieldSettings.HelpText", "")
            //            .WithSetting("DateFieldSettings.Required", "False")
            //            .WithSetting("DateFieldSettings.ReadOnly", "False")
            //            .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("DateFieldSettings.IsSystemField", "False")
            //            .WithSetting("DateFieldSettings.IsAudit", "False")
            //            .WithSetting("DateFieldSettings.DefaultValue", "")
            //        )
            //        .WithField("Description", column => column
            //            .OfType("MultilineTextField")
            //            .WithSetting("DisplayName", "描述")
            //            .WithSetting("EntityName", "Contract")
            //            .WithSetting("Storage", "Part")
            //            .WithSetting("MultilineTextFieldSettings.HelpText", "")
            //            .WithSetting("MultilineTextFieldSettings.Required", "False")
            //            .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
            //            .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
            //            .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
            //            .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
            //            .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
            //            .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
            //        )
            //    ;

            //ContentDefinitionManager.AlterTypeDefinition("Contract",
            //    type => type
            //        .DisplayedAs("合同")
            //        .WithPart(new ContentPartDefinition("ContractPart"), configuration => { }, contractPartAlteration)
            //        .WithSetting("CollectionDisplayName", "Contracts")
            //        .WithSetting("ContentTypeSettings.Creatable", "True")
            //        .WithSetting("Layout",
            //            "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Number\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"OwnerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"RenterId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ContractStatus\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"BeginDate\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"EndDate\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
            //    );

            return 1;
        }

//        private string CreateCustomerPartCustomerTypeOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "CustomerType";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"业主", "租户"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateHousePartHouseStatusOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "HouseStatus";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"空置", "自营", "出租"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemSettingPartCalculationMethodOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "CalculationMethod";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"单价数量", "指定金额", "自定义公式"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemSettingPartChargingPeriodOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "ChargingPeriod";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[]
//            {
//                "每1个月收一次", "每2个月收一次", "每3个月收一次", "每4个月收一次", "每5个月收一次", "每6个月收一次", "每7个月收一次", "每8个月收一次", "每9个月收一次",
//                "每10个月收一次", "每11个月收一次", "每12个月收一次"
//            };
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemPartItemCategoryOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "ItemCategory";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"周期性收费项目", "抄表类收费项目", "临时性收费项目"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateHouseMeterReadingPartStatusOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "Status";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"已录入", "未录入"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateContractPartContractStatusOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "ContractStatus";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"签订", "正在执行", "合同变更", "终止"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemPartDelayChargeCalculationMethodOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "DelayChargeCalculationMethod";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"手动计算滞纳金", "自动计算滞纳金"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemPartStartCalculationDatetimeOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "StartCalculationDatetime";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"欠费开始时间", "欠费结束时间"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemPartChargingPeriodPrecisionOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "ChargingPeriodPrecision";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"按周期计算", "按实际天数计算"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemPartDefaultChargingPeriodOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "DefaultChargingPeriod";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"当期收当期", "当期收上期", "当期收下期"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        private string CreateChargeItemSettingPartMeteringModeOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "MeteringMode";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"建筑面积", "套内面积", "用量"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }


//        private string CreateVoucherPartOperationOptionSetPart()
//        {
//            var optionSetPart = _contentManager.New<OptionSetPart>("OptionSet");
//            optionSetPart.Name = "Operation";

//            _contentManager.Create(optionSetPart, VersionOptions.Published);

//            var options = new[] {"入库", "出库", "所有"};
//            foreach (var option in options)
//            {
//                var optionItem = _contentManager.New<OptionItemPart>("OptionItem");
//                optionItem.OptionSetId = optionSetPart.Id;
//                optionItem.Name = option;
//                _contentManager.Create(optionItem, VersionOptions.Published);
//            }
//            return optionSetPart.Id.ToString(CultureInfo.InvariantCulture);
//        }

//        public int UpdateFrom1()
//        {
//            return 2;
//        }

//        public int UpdateFrom2()
//        {
//            SchemaBuilder.AlterTable("HouseChargeItem",
//                table =>
//                    table.AddColumn<int>("ExpenserOptionId"));

//            SchemaBuilder.AlterTable("ContractChargeItem",
//                table =>
//                    table.AddColumn<int>("ExpenserOptionId"));
//            return 3;
//        }

//        public int UpdateFrom3()
//        {
//            SchemaBuilder.CreateTable("Bill",
//                table => table
//                    .Column<int>("Id", c => c.PrimaryKey().Identity())
//                    .Column<int>("CustomerId", c => c.NotNull())
//                    .Column<int>("ContractId", c => c.NotNull())
//                    .Column<int>("HouseId", c => c.NotNull())
//                    .Column<int>("ChargeItemSettingId", c => c.NotNull())
//                    .Column<string>("ChargeItemSettingDescription")
//                    .Column<decimal>("Amount")
//                    .Column<DateTime>("StartDate")
//                    .Column<DateTime>("EndDate")
//                    .Column<int>("Year")
//                    .Column<int>("Month")
//                    .Column<string>("Status", column => column.WithLength(255))
//                    .Column<string>("Notes")
//                );
//            return 4;
//        }

//        public int UpdateFrom4()
//        {
//            SchemaBuilder.CreateTable("Payment",
//                table => table
//                    .ContentPartRecord()
//                    .Column<int>("CustomerId")
//                    .Column<decimal>("Paid")
//                    .Column<DateTime>("PaidOn")
//                    .Column<int>("Operator")
//                );
//            SchemaBuilder.CreateTable("PaymentLineItem",
//                table => table
//                    .Column<int>("Id", c => c.PrimaryKey().Identity())
//                    .Column<int>("PaymentId")
//                    .Column<int>("BillId")
//                );
//            SchemaBuilder.AlterTable("Bill",
//                table => table.AddColumn<decimal>("Exempt"));

//            SchemaBuilder.AlterTable("Bill",
//                table => table.AddColumn<decimal>("DelayCharge"));
//            return 5;
//        }

//        public int UpdateFrom5()
//        {
//            Action<ContentPartDefinitionBuilder> paymentPartAlteration =
//                part => part
//                    .WithField("CustomerId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "用户")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Payment_Customer_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Operator", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "操作员")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Payment_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Paid", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "付款金额")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("PaidOn", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "付款时间")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Payment",
//                type => type
//                     .DisplayedAs("付款")
//                    .WithPart(new ContentPartDefinition("PaymentPart"), configuration => { }, paymentPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Payments")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Paid\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"PaidOn\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Operator\",\"IsValid\":false}}],\"IsMerged\":false}]}]"));
//            return 6;
//        }

//        public int UpdateFrom6()
//        {
//            const string cultureName = "zh-CN";
//            _cultureManager.AddCulture(cultureName);
//            var siteSettings = _siteService.GetSiteSettings().As<SiteSettingsPart>();
//            siteSettings.SiteCulture = cultureName;
//            return 7;
//        }

//        public int UpdateFrom7()
//        {
//            //SchemaBuilder.AlterTable("ContractChargeItem",
//            //    table => table.DropColumn("ExpenserId"));
//            return 8;
//        }

//        public int UpdateFrom8()
//        {
//            return 9;
//        }

//        public int UpdateFrom9()
//        {
//            SchemaBuilder.AlterTable("House", table => table.AddColumn<int>("ApartmentId"));
//            SchemaBuilder.AlterTable("House", table => table.AddColumn<int>("BuildingId"));
//            SchemaBuilder.ExecuteSql("update House set ApartmentId = Apartment");
//            SchemaBuilder.ExecuteSql("update House set BuildingId = Building");
//            SchemaBuilder.AlterTable("House", table => table.DropColumn("Apartment"));
//            SchemaBuilder.AlterTable("House", table => table.DropColumn("Building"));

//            ContentDefinitionManager.AlterTypeDefinition("ContractPart",
//                type => type
//                    .WithPart(new ContentPartDefinition("ContractPartPart"), configuration => { },
//                        part => part
//                            .RemoveField("Building")
//                            .RemoveField("Apartment")
//                            .WithField("BuildingId", column => column
//                                .OfType("ReferenceField")
//                                .WithSetting("DisplayName", "楼宇")
//                                .WithSetting("EntityName", "House")
//                                .WithSetting("Storage", "Part")
//                                .WithSetting("ReferenceFieldSettings.HelpText", "")
//                                .WithSetting("ReferenceFieldSettings.Required", "True")
//                                .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                                .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                                .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                                .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                                .WithSetting("ReferenceFieldSettings.ContentTypeName", "Building")
//                                .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Building_Name")
//                                .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                                .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                                .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                                .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                                .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                                .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                            )
//                            .WithField("ApartmentId", column => column
//                                .OfType("ReferenceField")
//                                .WithSetting("DisplayName", "楼盘")
//                                .WithSetting("EntityName", "House")
//                                .WithSetting("Storage", "Part")
//                                .WithSetting("ReferenceFieldSettings.HelpText", "")
//                                .WithSetting("ReferenceFieldSettings.Required", "True")
//                                .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                                .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                                .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                                .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                                .WithSetting("ReferenceFieldSettings.ContentTypeName", "Apartment")
//                                .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Apartment_Name")
//                                .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                                .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                                .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                                .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                                .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                                .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                            )
//                    ));
//            return 10;
//        }

//        public int UpdateFrom10()
//        {
//            Action<ContentPartDefinitionBuilder> chargeItemSettingPartAlteration =
//                part => part
//                    .RemoveField("MeteringMode")
//                    .WithField("MeteringMode", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "计量方式")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateChargeItemSettingPartMeteringModeOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("ChargeItemSetting",
//                type => type
//                    .WithPart(new ContentPartDefinition("ChargeItemSettingPart"), configuration => { },
//                        chargeItemSettingPartAlteration)
//                );
//            return 11;
//        }


//        public int UpdateFrom11()
//        {
//            Action<ContentPartDefinitionBuilder> buildingPartAlteration =
//                part => part
//                    .WithField("Apartment", column => column
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Building",
//                type => type
//                    .WithPart(new ContentPartDefinition("BuildingPart"), configuration => { }, buildingPartAlteration)
//                );
//            return 12;
//        }

//        public int UpdateFrom12()
//        {
//            return 13;
//        }

//        public int UpdateFrom13()
//        {
//            Action<ContentPartDefinitionBuilder> contractPartAlteration =
//                part => part
//                    .RemoveField("ContractStatus")
//                    .WithField("ContractStatus", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "状态")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateContractPartContractStatusOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Contract",
//                type => type
//                    .WithPart(new ContentPartDefinition("ContractPart"), configuration => { }, contractPartAlteration)
//                );

//            SchemaBuilder.ExecuteSql("UPDATE Contract SET ContractStatus = N'签订' WHERE ContractStatus = N'正常'");

//            return 14;
//        }


//        public int UpdateFrom14()
//        {
//            Action<ContentPartDefinitionBuilder> contractPartAlteration =
//                part => part
//                    .RemoveField("OwnerId");
//            ContentDefinitionManager.AlterTypeDefinition("Contract",
//                type => type
//                    .WithPart(new ContentPartDefinition("ContractPart"), configuration => { }, contractPartAlteration));
//            SchemaBuilder.AlterTable("Contract", table => table.DropColumn("OwnerId"));
//            return 15;
//        }

//        public int UpdateFrom15()
//        {
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<string>("ItemCategory"));
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<int>("MeterType"));
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<int>("DelayChargeDays"));
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<decimal>("DelayChargeRatio"));
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<string>("DelayChargeCalculationMethod"));
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<string>("StartCalculationDatetime"));
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<string>("ChargingPeriodPrecision"));
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.AddColumn<string>("DefaultChargingPeriod"));

//            Action<ContentPartDefinitionBuilder> chargeItemSettingPartAlteration =
//                part => part
//                    .RemoveField("ChargeItemId")
//                    .WithField("ItemCategory", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "项目类别")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateChargeItemPartItemCategoryOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("MeterType", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "仪表种类")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "MeterType")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "ChargeItemSetting_MeterType")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("DelayChargeDays", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "欠费XX天滞纳金")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "2")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "0")
//                    )
//                    .WithField("DelayChargeRatio", column => column
//                        .OfType("NumberField")
//                        .WithSetting("DisplayName", "每天收取比例")
//                        .WithSetting("EntityName", "ChargeItem")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "18")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("DelayChargeCalculationMethod", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "滞纳金计算方式")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateChargeItemPartDelayChargeCalculationMethodOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("StartCalculationDatetime", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "开始计算时间")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateChargeItemPartStartCalculationDatetimeOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("ChargingPeriodPrecision", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "不足一个收费周期")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateChargeItemPartChargingPeriodPrecisionOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("DefaultChargingPeriod", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "默认收费周期")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateChargeItemPartDefaultChargingPeriodOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("ChargeItemSetting",
//                type => type
//                    .WithPart(new ContentPartDefinition("ChargeItemSettingPart"), configuration => { },
//                        chargeItemSettingPartAlteration)
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CalculationMethod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"UnitPrice\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeteringMode\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ChargingPeriod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomFormula\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remark\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Money\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ItemCategory\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeterType\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeDays\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeRatio\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeCalculationMethod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"StartCalculationDatetime\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ChargingPeriodPrecision\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DefaultChargingPeriod\",\"IsValid\":false}}],\"IsMerged\":false}]}]"));
//            return 16;
//        }

//        public int UpdateFrom16()
//        {
//            SchemaBuilder.ExecuteSql(@"
//    UPDATE ChargeItemSetting 
//    SET ItemCategory = ci.ItemCategory,
//	    MeterType = ci.MeterType,
//	    DelayChargeDays = ci.DelayChargeDays,
//	    DelayChargeRatio = ci.DelayChargeRatio,
//	    DelayChargeCalculationMethod = ci.DelayChargeCalculationMethod,
//	    StartCalculationDatetime = ci.StartCalculationDatetime,
//	    ChargingPeriodPrecision = ci.ChargingPeriodPrecision,
//	    DefaultChargingPeriod = ci.DefaultChargingPeriod
//    FROM ChargeItemSetting cis
//	     INNER JOIN ChargeItem ci
//	     ON	ci.Id = cis.ChargeItemId");
//            SchemaBuilder.AlterTable("ChargeItemSetting",
//                table => table.DropColumn("ChargeItemId"));
//            return 17;
//        }

//        public int UpdateFrom17()
//        {
//            SchemaBuilder.AlterTable("HouseChargeItem", table =>
//            {
//                table.AddColumn<string>("Name");
//                table.AddColumn<string>("CalculationMethod");
//                table.AddColumn<decimal>("UnitPrice");
//                table.AddColumn<string>("MeteringMode");
//                table.AddColumn<string>("ChargingPeriod");
//                table.AddColumn<string>("CustomFormula");
//                table.AddColumn<string>("Remark");
//                table.AddColumn<decimal>("Money");
//                table.AddColumn<string>("ItemCategory");
//                table.AddColumn<int>("MeterType");
//                table.AddColumn<int>("DelayChargeDays");
//                table.AddColumn<decimal>("DelayChargeRatio");
//                table.AddColumn<string>("DelayChargeCalculationMethod");
//                table.AddColumn<string>("StartCalculationDatetime");
//                table.AddColumn<string>("ChargingPeriodPrecision");
//                table.AddColumn<string>("DefaultChargingPeriod");
//            });
//            SchemaBuilder.ExecuteSql(@"
//                UPDATE HouseChargeItem
//                SET Name = cis.Name,
//	                CalculationMethod = cis.CalculationMethod,
//	                UnitPrice	= cis.UnitPrice,
//	                MeteringMode = cis.MeteringMode,
//	                ChargingPeriod = cis.ChargingPeriod,
//	                CustomFormula = cis.CustomFormula,
//	                Remark = cis.Remark,
//	                Money = cis.Money,
//	                ItemCategory = ci.ItemCategory,
//	                MeterType = ci.MeterType,
//	                DelayChargeDays = ci.DelayChargeDays,
//	                DelayChargeRatio = ci.DelayChargeRatio,
//	                DelayChargeCalculationMethod = ci.DelayChargeCalculationMethod,
//	                StartCalculationDatetime = ci.StartCalculationDatetime,
//	                ChargingPeriodPrecision = ci.ChargingPeriodPrecision,
//	                DefaultChargingPeriod = ci.DefaultChargingPeriod
//                FROM HouseChargeItem hci
//	                    INNER JOIN ChargeItem ci
//	                    ON	ci.Id = hci.ChargeItemId
//		                INNER JOIN ChargeItemSetting cis
//		                ON	cis.Id = hci.ChargeItemSettingId");
//            SchemaBuilder.AlterTable("HouseChargeItem", table =>
//            {
//                table.DropColumn("ChargeItemId");
//                table.DropColumn("Unit");
//                table.DropColumn("Amount");
//            });
//            return 18;
//        }

//        public int UpdateFrom18()
//        {
//            SchemaBuilder.AlterTable("ContractChargeItem", table =>
//            {
//                table.AddColumn<string>("ChargeItemName");
//                table.AddColumn<string>("CalculationMethod");
//                table.AddColumn<decimal>("UnitPrice");
//                table.AddColumn<string>("MeteringMode");
//                table.AddColumn<string>("ChargingPeriod");
//                table.AddColumn<string>("CustomFormula");
//                table.AddColumn<decimal>("Money");
//                table.AddColumn<string>("ItemCategory");
//                table.AddColumn<int>("MeterTypeId");
//                table.AddColumn<string>("DelayChargeDays");
//                table.AddColumn<double>("DelayChargeRatio");
//                table.AddColumn<string>("DelayChargeCalculationMethod");
//                table.AddColumn<string>("StartCalculationDatetime");
//                table.AddColumn<string>("ChargingPeriodPrecision");
//                table.AddColumn<string>("DefaultChargingPeriod");
//            });
//            SchemaBuilder.ExecuteSql(@"
//                UPDATE ContractChargeItem
//                SET ChargeItemName = cis.Name,
//	                CalculationMethod = cis.CalculationMethod,
//	                UnitPrice	= cis.UnitPrice,
//	                MeteringMode = cis.MeteringMode,
//	                ChargingPeriod = cis.ChargingPeriod,
//	                CustomFormula = cis.CustomFormula,
//	                Money = cis.Money,
//					ItemCategory = ci.ItemCategory,
//	                MeterTypeId = ci.MeterType,
//	                DelayChargeDays = ci.DelayChargeDays,
//	                DelayChargeRatio = ci.DelayChargeRatio,
//	                DelayChargeCalculationMethod = ci.DelayChargeCalculationMethod,
//	                StartCalculationDatetime = ci.StartCalculationDatetime,
//	                ChargingPeriodPrecision = ci.ChargingPeriodPrecision,
//	                DefaultChargingPeriod = ci.DefaultChargingPeriod
//                FROM ContractChargeItem cci
//	                 INNER JOIN ChargeItem ci
//	                 ON	ci.Id = cci.ChargeItemId
//					 INNER JOIN ChargeItemSetting cis
//		             ON	cis.Id = cci.ChargeItemSettingId");
//            SchemaBuilder.AlterTable("ContractChargeItem", table => table.DropColumn("ChargeItemId"));
//            return 19;
//        }

//        public int UpdateFrom19()
//        {
//            SchemaBuilder.CreateTable("Inventory",
//                table => table
//                    .Column<int>("Id", c => c.PrimaryKey().Identity())
//                    .Column<int>("MaterialId", c => c.NotNull())
//                    .Column<int>("Number", c => c.NotNull())
//                    .Column<decimal>("CostPrice", c => c.NotNull())
//                );
//            SchemaBuilder.CreateTable("InventoryChange",
//                table => table
//                    .Column<int>("Id", c => c.PrimaryKey().Identity())
//                    .Column<string>("VoucherNo", c => c.WithLength(12))
//                    .Column<decimal>("CostPrice", c => c.WithDefault(0).NotNull())
//                    .Column<int>("MaterialId", c => c.NotNull())
//                    .Column<string>("Operation", c => c.WithLength(20).NotNull())
//                    .Column<int>("Number", c => c.NotNull())
//                    .Column<DateTime>("Date", c => c.NotNull())
//                    .Column<int>("OperatorId", c => c.NotNull())
//                );
//            SchemaBuilder.CreateTable("Material",
//                table => table
//                    .ContentPartRecord()
//                    .Column<string>("SerialNo", column => column.WithLength(255))
//                    .Column<string>("Name", column => column.WithLength(255))
//                    .Column<string>("Brand", column => column.WithLength(255))
//                    .Column<string>("Model", column => column.WithLength(255))
//                    .Column<decimal>("CostPrice")
//                    .Column<string>("Unit", column => column.WithLength(255))
//                    .Column<string>("Remark", column => column.WithLength(5000))
//                );

//            Action<ContentPartDefinitionBuilder> materialPartAlteration =
//                part => part
//                    .WithField("SerialNo", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "材料编号")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "材料名称")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Brand", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "品牌")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Model", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "规格型号")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("CostPrice", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "成本价")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("Unit", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "单位")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Remark", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                ;

//            ContentDefinitionManager.AlterTypeDefinition("Material",
//                type => type
//                    .DisplayedAs("材料")
//                    .WithPart(new ContentPartDefinition("MaterialPart"), configuration => { }, materialPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Materials")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"SerialNo\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Brand\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Model\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CostPrice\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Unit\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remark\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );
//            return 20;
//        }

//        public int UpdateFrom20()
//        {
//            SchemaBuilder.AlterTable("Inventory", table => table.AddColumn<decimal>("Amount"));
//            return 21;
//        }

//        public int UpdateFrom21()
//        {
//            Action<ContentPartDefinitionBuilder> contractPartAlteration =
//                part => part
//                    .WithField("HouseStatus", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "房屋状态")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateHousePartHouseStatusOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Contract",
//                type => type
//                    .WithPart(new ContentPartDefinition("ContractPart"), configuration => { }, contractPartAlteration)
//                );
//            SchemaBuilder.AlterTable("Contract",
//                table => table.AddColumn<string>("HouseStatus", c => c.WithDefault("出租")));
//            SchemaBuilder.ExecuteSql(@"UPDATE House SET HouseStatus = '自营' Where HouseStatus='经营'");
//            SchemaBuilder.ExecuteSql(@"UPDATE Contract SET HouseStatus = '出租'");
//            return 22;
//        }

//        public int UpdateFrom22()
//        {
//            SchemaBuilder.AlterTable("Bill", table => table.AddColumn<decimal>("Quantity"));
//            return 23;
//        }

//        public int UpdateFrom23()
//        {
//            Action<ContentPartDefinitionBuilder> chargeItemSettingPartAlteration =
//                part => part
//                    .RemoveField("ChargingPeriod")
//                    .WithField("ChargingPeriod", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "收费周期")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateChargeItemSettingPartChargingPeriodOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("ChargeItemSetting",
//                type => type
//                    .WithPart(new ContentPartDefinition("ChargeItemSettingPart"), configuration => { },
//                        chargeItemSettingPartAlteration)
//                );

//            SchemaBuilder.ExecuteSql(@"UPDATE ChargeItemSetting SET ChargingPeriod =replace(ChargingPeriod,'每隔','每')");
//            SchemaBuilder.ExecuteSql(@"UPDATE HouseChargeItem SET ChargingPeriod =replace(ChargingPeriod,'每隔','每')");
//            SchemaBuilder.ExecuteSql(@"UPDATE ContractChargeItem SET ChargingPeriod =replace(ChargingPeriod,'每隔','每')");
//            return 24;
//        }

//        public int UpdateFrom24()
//        {
//            SchemaBuilder.CreateTable("Supplier",
//                table => table
//                    .ContentPartRecord()
//                    .Column<string>("Name", column => column.WithLength(255))
//                    .Column<string>("Address", column => column.WithLength(255))
//                    .Column<string>("Contactor", column => column.WithLength(255))
//                    .Column<string>("Tel", column => column.WithLength(255))
//                    .Column<string>("MobilePhone", column => column.WithLength(255))
//                    .Column<string>("Email", column => column.WithLength(255))
//                    .Column<string>("QQ", column => column.WithLength(255))
//                    .Column<string>("Fax", column => column.WithLength(255))
//                    .Column<string>("Description", column => column.WithLength(255))
//                );
//            Action<ContentPartDefinitionBuilder> supplierPartAlteration =
//                part => part
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "名称")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Address", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "地址")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Contactor", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "联系人")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Tel", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "电话")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("MobilePhone", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "手机")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Email", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "邮件")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("QQ", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "QQ号")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Fax", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "传真")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Supplier",
//                type => type
//                    .DisplayedAs("供应商")
//                    .WithPart(new ContentPartDefinition("SupplierPart"), configuration => { }, supplierPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Suppliers")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Address\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Contactor\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Tel\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MobilePhone\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Email\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"QQ\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Fax\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );
//            return 25;
//        }

//        public int UpdateFrom25()
//        {
//            SchemaBuilder.CreateTable("Department",
//                table => table
//                    .ContentPartRecord()
//                    .Column<string>("Name", column => column.WithLength(255))
//                    .Column<string>("Description", column => column.WithLength(255))
//                );
//            Action<ContentPartDefinitionBuilder> departmentPartAlteration =
//                part => part
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "部门名称")
//                        .WithSetting("EntityName", "Department")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Department")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Department",
//                type => type
//                    .DisplayedAs("部门")
//                    .WithPart(new ContentPartDefinition("DepartmentPart"), configuration => { },
//                        departmentPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Departments")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            SchemaBuilder.CreateTable("Voucher",
//                table => table
//                    .ContentPartRecord()
//                    .Column<string>("VoucherNo", c => c.WithLength(12))
//                    .Column<string>("Operation")
//                    .Column<int>("OperatorId", c => c.NotNull())
//                    .Column<DateTime>("Date", c => c.NotNull())
//                    .Column<int>("SupplierId")
//                    .Column<int>("DepartmentId")
//                    .Column<string>("Remark", c => c.WithLength(255))
//                );


//            Action<ContentPartDefinitionBuilder> voucherPartAlteration =
//                part => part
//                    .WithField("VoucherNo", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "单号")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Operation", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "状态")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId",
//                            CreateVoucherPartOperationOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("OperatorId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "操作员")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Voucher_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Date", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "操作时间")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("SupplierId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "供应商")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Supplier")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Voucher_Supplier_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("DepartmentId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "部门")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Department")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Voucher_Department_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Remark", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Voucher",
//                type => type
//                     .DisplayedAs("物料管理")
//                    .WithPart(new ContentPartDefinition("VoucherPart"), configuration => { }, voucherPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Vouchers")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"VoucherNo\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Operation\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"OperatorId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Date\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"SupplierId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"DepartmentId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Remark\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );
//            return 26;
//        }

//        public int UpdateFrom26()
//        {
//            SchemaBuilder.AlterTable("MeterType",
//                table => table.AddColumn<string>("Unit", column => column.WithLength(255)));

//            Action<ContentPartDefinitionBuilder> meterTypePartAlteration =
//                part => part
//                    .WithField("Unit", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "计量单位")
//                        .WithSetting("EntityName", "MeterType")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    );

//            ContentDefinitionManager.AlterTypeDefinition("MeterType",
//                type => type
//                    .WithPart(new ContentPartDefinition("MeterTypePart"), configuration => { }, meterTypePartAlteration)
//                );
//            return 27;
//        }

//        public int UpdateFrom27()
//        {
//            SchemaBuilder.CreateTable("PaymentMethodItem",
//                table => table
//                    .Column<int>("Id", c => c.PrimaryKey().Identity())
//                    .Column<int>("PaymentId")
//                    .Column<decimal>("Amount")
//                    .Column<string>("PaymentMethod")
//                    .Column<string>("Description")
//                );
//            return 28;
//        }

//        public int UpdateFrom28()
//        {
//            SchemaBuilder.AlterTable("HouseChargeItem",
//                table => table
//                    .AddColumn<int>("MeterTypeId")
//                );
//            SchemaBuilder.ExecuteSql("update HouseChargeItem set MeterTypeId = MeterType");
//            SchemaBuilder.AlterTable("HouseChargeItem",
//                table => table
//                    .DropColumn("MeterType")
//                );
//            return 29;
//        }

//        public int UpdateFrom29()
//        {
//            SchemaBuilder.AlterTable("ContractChargeItem",
//                table =>
//                {
//                    table.AddColumn<string>("Name", column => column.WithLength(255));
//                    table.DropColumn("ChargeItemName");
//                }
//                );
//            SchemaBuilder.ExecuteSql(@"
//                    update  ContractChargeItem
//                    set		Name = cis.Name
//                    from	ContractChargeItem cci
//		                    INNER JOIN ChargeItemSetting cis
//		                    ON	cis.Id = cci.ChargeItemSettingId");
//            return 30;
//        }

//        public int UpdateFrom30()
//        {
//            SchemaBuilder.CreateTable("ChargeItemSettingCommon",
//                table => table
//                    .Column<int>("Id", column => column.PrimaryKey().Identity())
//                    .Column<string>("Type")
//                    .Column<string>("Name")
//                    .Column<int>("HouseId")
//                    .Column<int>("ContractId")
//                    .Column<int>("ChargeItemSettingId")
//                    .Column<DateTime>("BeginDate")
//                    .Column<DateTime>("EndDate")
//                    .Column<string>("Description", column => column.WithLength(5000))
//                    .Column<int>("ExpenserOption")
//                    .Column<string>("CalculationMethod")
//                    .Column<decimal>("UnitPrice")
//                    .Column<string>("MeteringMode")
//                    .Column<string>("ChargingPeriod")
//                    .Column<string>("CustomFormula")
//                    .Column<decimal>("Money")
//                    .Column<string>("ItemCategory")
//                    .Column<int>("MeterTypeId")
//                    .Column<int>("DelayChargeDays")
//                    .Column<decimal>("DelayChargeRatio")
//                    .Column<string>("DelayChargeCalculationMethod")
//                    .Column<string>("StartCalculationDatetime")
//                    .Column<string>("ChargingPeriodPrecision")
//                    .Column<string>("DefaultChargingPeriod")
//                );

//            SchemaBuilder.ExecuteSql(@"
//                    INSERT INTO ChargeItemSettingCommon
//                    SELECT 
//	                       [Type] = 'Contract'
//                          ,[Name]
//                          ,[HouseId] = NULL
//                          ,[ContractId]
//                          ,[ChargeItemSettingId]
//                          ,[BeginDate]
//                          ,[EndDate]
//                          ,[Description]
//                          ,[ExpenserOption] = [ExpenserOptionId]
//                          ,[CalculationMethod]
//                          ,[UnitPrice]
//                          ,[MeteringMode]
//                          ,[ChargingPeriod]
//                          ,[CustomFormula]
//                          ,[Money]
//                          ,[ItemCategory]
//                          ,[MeterTypeId]
//                          ,[DelayChargeDays]
//                          ,[DelayChargeRatio]
//                          ,[DelayChargeCalculationMethod]
//                          ,[StartCalculationDatetime]
//                          ,[ChargingPeriodPrecision]
//                          ,[DefaultChargingPeriod]
//                      FROM [ContractChargeItem]");
//            SchemaBuilder.ExecuteSql(@"
//                    INSERT INTO ChargeItemSettingCommon
//                    SELECT 
//	                       [Type] = 'House'
//                          ,[Name]
//                          ,[HouseId]
//                          ,[ContractId] = NULL
//                          ,[ChargeItemSettingId]
//                          ,[BeginDate]
//                          ,[EndDate]
//                          ,[Description]
//                          ,[ExpenserOption] = [ExpenserOptionId]
//                          ,[CalculationMethod]
//                          ,[UnitPrice]
//                          ,[MeteringMode]
//                          ,[ChargingPeriod]
//                          ,[CustomFormula]
//                          ,[Money]
//                          ,[ItemCategory]
//                          ,[MeterTypeId]
//                          ,[DelayChargeDays]
//                          ,[DelayChargeRatio]
//                          ,[DelayChargeCalculationMethod]
//                          ,[StartCalculationDatetime]
//                          ,[ChargingPeriodPrecision]
//                          ,[DefaultChargingPeriod]
//                      FROM [HouseChargeItem]");
//            SchemaBuilder.DropTable("HouseChargeItem");
//            SchemaBuilder.DropTable("ContractChargeItem");
//            return 31;
//        }

//        public int UpdateFrom31()
//        {
//            SchemaBuilder.AlterTable("Bill",
//                table =>
//                {
//                    table.AddColumn<int>("ChargeItemSettingCommonId");
//                    table.DropColumn("ChargeItemSettingId");
//                });
//            return 32;
//        }

//        public int UpdateFrom32()
//        {
//            SchemaBuilder.CreateTable("Service",
//                table => table
//                    .ContentPartRecord()
//                    .Column<int>("HouseId")
//                    .Column<int>("OwnerId")
//                    .Column<string>("Mobile", column => column.WithLength(255))
//                    .Column<string>("FaultDescription", column => column.WithLength(5000))
//                    .Column<DateTime>("ReceivedDate")
//                    .Column<int>("ServicePersonId")
//                    .Column<decimal>("ServiceCharge")
//                    .Column<string>("ServiceVoucherNo", c => c.WithLength(12))
//                    .Column<string>("StockOutVoucher", c => c.WithLength(12))
//                    .Column<string>("StockReturnVoucher", c => c.WithLength(12))
//                );
//            Action<ContentPartDefinitionBuilder> servicePartAlteration =
//                part => part
//                    .WithField("HouseId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "房间号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "House")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Service_House_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "HouseNumber")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("OwnerId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "业主")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Service_Customer_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Mobile", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "电话")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("FaultDescription", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "故障描述")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("ReceivedDate", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "接收时间")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("ServicePersonId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "维修人员")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Service_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("ServiceCharge", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "维修费用")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("ServiceVoucherNo", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "维修单号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("StockOutVoucher", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "维修出库单号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("StockReturnVoucher", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "维修反库单号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    );
//            ContentDefinitionManager.AlterTypeDefinition("Service",
//                type => type
//                    .DisplayedAs("业主报修服务")
//                    .WithPart(new ContentPartDefinition("ServicePart"), configuration => { }, servicePartAlteration)
//                    .WithSetting("CollectionDisplayName", "Services")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"OwnerId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Mobile\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"FaultDescription\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ReceivedDate\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ServicePersonId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ServiceCharge\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ServiceVoucherNo\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"StockOutVoucher\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"StockReturnVoucher\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            return 33;
//        }

//        public int UpdateFrom33()
//        {
//            SchemaBuilder.AlterTable("Customer", table => table.AddColumn<decimal>("CustomerBalance"));
//            Action<ContentPartDefinitionBuilder> customerPartAlteration =
//                part => part
//                    .WithField("CustomerBalance", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "客户账户余额")
//                        .WithSetting("EntityName", "Customer")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    );

//            ContentDefinitionManager.AlterTypeDefinition("Customer",
//                type => type
//                    .WithPart(new ContentPartDefinition("CustomerPart"), configuration => { }, customerPartAlteration)
//                );

//            SchemaBuilder.CreateTable("AdvancePayment",
//                table => table
//                    .ContentPartRecord()
//                    .Column<int>("CustomerId")
//                    .Column<decimal>("Paid")
//                    .Column<DateTime>("PaidOn")
//                    .Column<int>("Operator")
//                );
//            Action<ContentPartDefinitionBuilder> advancepaymentPartAlteration =
//                           part => part
//                               .WithField("CustomerId", column => column
//                                   .OfType("ReferenceField")
//                                   .WithSetting("DisplayName", "用户")
//                                   .WithSetting("EntityName", "AdvancePayment")
//                                   .WithSetting("Storage", "Part")
//                                   .WithSetting("ReferenceFieldSettings.HelpText", "")
//                                   .WithSetting("ReferenceFieldSettings.Required", "False")
//                                   .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                                   .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                                   .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                                   .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                                   .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                                   .WithSetting("ReferenceFieldSettings.RelationshipName", "AdvancePayment_Customer_Name")
//                                   .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                                   .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                                   .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                                   .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                                   .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                                   .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                               )
//                               .WithField("Operator", column => column
//                                   .OfType("ReferenceField")
//                                   .WithSetting("DisplayName", "操作员")
//                                   .WithSetting("EntityName", "AdvancePayment")
//                                   .WithSetting("Storage", "Part")
//                                   .WithSetting("ReferenceFieldSettings.HelpText", "")
//                                   .WithSetting("ReferenceFieldSettings.Required", "False")
//                                   .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                                   .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                                   .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                                   .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                                   .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                                   .WithSetting("ReferenceFieldSettings.RelationshipName", "AdvancePayment_User_Name")
//                                   .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                                   .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                                   .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                                   .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                                   .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                                   .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                               )
//                               .WithField("Paid", column => column
//                                   .OfType("CurrencyField")
//                                   .WithSetting("DisplayName", "预付款金额")
//                                   .WithSetting("EntityName", "AdvancePayment")
//                                   .WithSetting("Storage", "Part")
//                                   .WithSetting("CurrencyFieldSettings.HelpText", "")
//                                   .WithSetting("CurrencyFieldSettings.Required", "False")
//                                   .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                                   .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                                   .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                                   .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                                   .WithSetting("CurrencyFieldSettings.Length", "18")
//                                   .WithSetting("CurrencyFieldSettings.DecimalPlaces", "0")
//                                   .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                               )
//                               .WithField("PaidOn", column => column
//                                   .OfType("DateField")
//                                   .WithSetting("DisplayName", "付款时间")
//                                   .WithSetting("EntityName", "AdvancePayment")
//                                   .WithSetting("Storage", "Part")
//                                   .WithSetting("DateFieldSettings.HelpText", "")
//                                   .WithSetting("DateFieldSettings.Required", "False")
//                                   .WithSetting("DateFieldSettings.ReadOnly", "False")
//                                   .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                                   .WithSetting("DateFieldSettings.IsSystemField", "False")
//                                   .WithSetting("DateFieldSettings.IsAudit", "False")
//                                   .WithSetting("DateFieldSettings.DefaultValue", "")
//                               );
//            ContentDefinitionManager.AlterTypeDefinition("AdvancePayment",
//                type => type
//                     .DisplayedAs("预付款")
//                    .WithPart(new ContentPartDefinition("AdvancePaymentPart"), configuration => { }, advancepaymentPartAlteration)
//                    .WithSetting("CollectionDisplayName", "AdvancePayments")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout",
//                        "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Paid\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"PaidOn\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Operator\",\"IsValid\":false}}],\"IsMerged\":false}]}]"));

//            SchemaBuilder.CreateTable("AdvancePaymentItem",
//               table => table
//                   .Column<int>("Id", c => c.PrimaryKey().Identity())
//                   .Column<int>("AdvancePaymentId")
//                   .Column<decimal>("Amount")
//                   .Column<string>("AdvancePaymentMethod")
//                   .Column<string>("Description")
//               );
//            return 34;
//        }

//        public int UpdateFrom34()
//        {
//            return 35;
//        }

//        public int UpdateFrom35()
//        {


//            Action<ContentPartDefinitionBuilder> apartmentPartAlteration =
//                part => part
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "名称")
//                        .WithSetting("EntityName", "Apartment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Address", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "地址")
//                        .WithSetting("EntityName", "Apartment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Apartment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Apartment",
//                type => type
//                    .WithPart(new ContentPartDefinition("ApartmentPart"), configuration => { }, apartmentPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Apartments")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Address\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> housePartAlteration =
//                part => part
//                    .WithField("Building", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "楼宇")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Building")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Building_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Apartment", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "楼盘")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Apartment")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Apartment_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("OwnerId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "业主")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "House_Customer_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("HouseNumber", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "房号")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("OfficerId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "专管员")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "House_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("BuildingArea", column => column
//                        .OfType("NumberField")
//                        .WithSetting("DisplayName", "建筑面积")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "18")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "0")
//                    )
//                    .WithField("InsideArea", column => column
//                        .OfType("NumberField")
//                        .WithSetting("DisplayName", "套内面积")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "18")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "0")
//                    )
//                    .WithField("PoolArea", column => column
//                        .OfType("NumberField")
//                        .WithSetting("DisplayName", "公摊面积")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "18")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "0")
//                    )
//                    .WithField("HouseStatus", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "房屋状态")
//                        .WithSetting("EntityName", "House")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateHousePartHouseStatusOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("House",
//                type => type
//                    .WithPart(new ContentPartDefinition("HousePart"), configuration => { }, housePartAlteration)
//                    .WithSetting("CollectionDisplayName", "Houses")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Building\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Apartment\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"OwnerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseNumber\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"OfficerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"BuildingArea\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"InsideArea\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"PoolArea\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseStatus\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> houseMeterReadingPartAlteration =
//                part => part
//                    .WithField("Year", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "年份")
//                        .WithSetting("EntityName", "HouseMeterReading")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "2")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "0")
//                    )
//                    .WithField("Month", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "月份")
//                        .WithSetting("EntityName", "HouseMeterReading")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "2")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "0")
//                    )
//                    .WithField("HouseMeterId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "房间仪表")
//                        .WithSetting("EntityName", "HouseMeterReading")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "HouseMeter")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "HouseReading_HouseMeter")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "MeterType")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("MeterData", column => column
//                        .OfType("NumberField")
//                        .WithSetting("DisplayName", "抄表读数")
//                        .WithSetting("EntityName", "HouseMeterReading")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "18")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("Amount", column => column
//                        .OfType("NumberField")
//                        .WithSetting("DisplayName", "用量")
//                        .WithSetting("EntityName", "HouseMeterReading")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "18")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("Status", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "状态")
//                        .WithSetting("EntityName", "HouseMeterReading")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateHouseMeterReadingPartStatusOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Remarks", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "HouseMeterReading")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("HouseMeterReading",
//                type => type
//                    .WithPart(new ContentPartDefinition("HouseMeterReadingPart"), configuration => { }, houseMeterReadingPartAlteration)
//                    .WithSetting("CollectionDisplayName", "HouseMeterReadings")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Year\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Month\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseMeterId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeterData\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Amount\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Status\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remarks\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> paymentPartAlteration =
//                part => part
//                    .WithField("CustomerId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "用户")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Payment_Customer_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Operator", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "操作员")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Payment_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Paid", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "付款金额")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("PaidOn", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "付款时间")
//                        .WithSetting("EntityName", "Payment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Payment",
//                type => type
//                    .WithPart(new ContentPartDefinition("PaymentPart"), configuration => { }, paymentPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Payments")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Paid\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"PaidOn\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Operator\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> buildingPartAlteration =
//                part => part
//                    .WithField("Apartment", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "楼盘")
//                        .WithSetting("EntityName", "Building")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Apartment")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Building_Apartment_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "名称")
//                        .WithSetting("EntityName", "Building")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Building")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Building",
//                type => type
//                    .WithPart(new ContentPartDefinition("BuildingPart"), configuration => { }, buildingPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Buildings")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Apartment\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> materialPartAlteration =
//                part => part
//                    .WithField("SerialNo", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "材料编号")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "材料名称")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Brand", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "品牌")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Model", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "规格型号")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("CostPrice", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "成本价")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("Unit", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "单位")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Remark", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Material")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Material",
//                type => type
//                    .WithPart(new ContentPartDefinition("MaterialPart"), configuration => { }, materialPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Materials")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"SerialNo\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Brand\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Model\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CostPrice\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Unit\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remark\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> contractPartAlteration =
//                part => part
//                    .WithField("Number", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "合同编号")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "合同名称")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("RenterId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "租户")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Contract_Customer_RenterId_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("ContractStatus", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "状态")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateContractPartContractStatusOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("BeginDate", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "起租时间")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("EndDate", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "到期时间")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "描述")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("HouseStatus", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "房屋状态")
//                        .WithSetting("EntityName", "Contract")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateHousePartHouseStatusOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Contract",
//                type => type
//                    .WithPart(new ContentPartDefinition("ContractPart"), configuration => { }, contractPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Contracts")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Number\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"OwnerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"RenterId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ContractStatus\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"BeginDate\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"EndDate\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> chargeItemSettingPartAlteration =
//                part => part
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "名称")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("CalculationMethod", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "金额计算方式")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemSettingPartCalculationMethodOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("UnitPrice", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "单价")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("MeteringMode", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "计量方式")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemSettingPartMeteringModeOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("ChargingPeriod", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "收费周期")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemSettingPartChargingPeriodOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("CustomFormula", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "自定义公式")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "255")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Remark", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "255")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Money", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "指定金额")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("ItemCategory", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "项目类别")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemPartItemCategoryOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("MeterType", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "仪表种类")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "MeterType")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "ChargeItemSetting_MeterType")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("DelayChargeDays", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "欠费XX天滞纳金")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "2")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "0")
//                    )
//                    .WithField("DelayChargeRatio", column => column
//                        .OfType("NumberField")
//                        .WithSetting("DisplayName", "每天收取比例")
//                        .WithSetting("EntityName", "ChargeItem")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("NumberFieldSettings.HelpText", "")
//                        .WithSetting("NumberFieldSettings.Required", "False")
//                        .WithSetting("NumberFieldSettings.ReadOnly", "False")
//                        .WithSetting("NumberFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("NumberFieldSettings.IsSystemField", "False")
//                        .WithSetting("NumberFieldSettings.IsAudit", "False")
//                        .WithSetting("NumberFieldSettings.Length", "18")
//                        .WithSetting("NumberFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("NumberFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("DelayChargeCalculationMethod", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "滞纳金计算方式")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemPartDelayChargeCalculationMethodOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("StartCalculationDatetime", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "开始计算时间")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemPartStartCalculationDatetimeOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("ChargingPeriodPrecision", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "不足一个收费周期")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemPartChargingPeriodPrecisionOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("DefaultChargingPeriod", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "默认收费周期")
//                        .WithSetting("EntityName", "ChargeItemSetting")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateChargeItemPartDefaultChargingPeriodOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("ChargeItemSetting",
//                type => type
//                    .WithPart(new ContentPartDefinition("ChargeItemSettingPart"), configuration => { }, chargeItemSettingPartAlteration)
//                    .WithSetting("CollectionDisplayName", "ChargeItemSettings")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CalculationMethod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"UnitPrice\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeteringMode\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ChargingPeriod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomFormula\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Remark\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Money\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ItemCategory\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MeterType\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeDays\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeRatio\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DelayChargeCalculationMethod\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"StartCalculationDatetime\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"ChargingPeriodPrecision\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"DefaultChargingPeriod\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> supplierPartAlteration =
//                part => part
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "名称")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Address", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "地址")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Contactor", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "联系人")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Tel", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "电话")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("MobilePhone", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "手机")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Email", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "邮件")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("QQ", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "QQ号")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Fax", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "传真")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Supplier")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Supplier",
//                type => type
//                    .WithPart(new ContentPartDefinition("SupplierPart"), configuration => { }, supplierPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Suppliers")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Address\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Contactor\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Tel\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"MobilePhone\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Email\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"QQ\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Fax\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> departmentPartAlteration =
//                part => part
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "部门名称")
//                        .WithSetting("EntityName", "Department")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Department")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Department",
//                type => type
//                    .WithPart(new ContentPartDefinition("DepartmentPart"), configuration => { }, departmentPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Departments")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> voucherPartAlteration =
//                part => part
//                    .WithField("VoucherNo", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "单号")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("Operation", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "状态")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "False")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateVoucherPartOperationOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Radiobutton")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("OperatorId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "操作员")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Voucher_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Date", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "操作时间")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("SupplierId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "供应商")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Supplier")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Voucher_Supplier_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("DepartmentId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "部门")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Department")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Voucher_Department_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Remark", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "Voucher")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Voucher",
//                type => type
//                    .WithPart(new ContentPartDefinition("VoucherPart"), configuration => { }, voucherPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Vouchers")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"VoucherNo\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Operation\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"OperatorId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Date\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"SupplierId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"DepartmentId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Remark\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> meterTypePartAlteration =
//                part => part
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "名称")
//                        .WithSetting("EntityName", "MeterType")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "备注")
//                        .WithSetting("EntityName", "MeterType")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Unit", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "计量单位")
//                        .WithSetting("EntityName", "MeterType")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("MeterType",
//                type => type
//                    .WithPart(new ContentPartDefinition("MeterTypePart"), configuration => { }, meterTypePartAlteration)
//                    .WithSetting("CollectionDisplayName", "MeterTypes")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> servicePartAlteration =
//                part => part
//                    .WithField("HouseId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "房间号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "House")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Service_House_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "HouseNumber")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("OwnerId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "业主")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "True")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Service_Customer_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Mobile", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "电话")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("FaultDescription", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "故障描述")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("ReceivedDate", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "接收时间")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("ServicePersonId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "维修人员")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "Service_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("ServiceCharge", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "维修费用")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("ServiceVoucherNo", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "维修单号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("StockOutVoucher", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "维修出库单号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    )
//                    .WithField("StockReturnVoucher", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "维修反库单号")
//                        .WithSetting("EntityName", "Service")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "False")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "True")
//                    );

//            ContentDefinitionManager.AlterTypeDefinition("Service",
//                type => type
//                    .WithPart(new ContentPartDefinition("ServicePart"), configuration => { }, servicePartAlteration)
//                    .WithSetting("CollectionDisplayName", "Services")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"HouseId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"OwnerId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"Mobile\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"FaultDescription\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ReceivedDate\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ServicePersonId\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ServiceCharge\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"ServiceVoucherNo\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"StockOutVoucher\",\"IsValid\":false}}],\"IsMerged\":false}{\"Columns\":[{\"Field\":{\"FieldName\":\"StockReturnVoucher\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> customerPartAlteration =
//                part => part
//                    .WithField("Number", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "客户编号")
//                        .WithSetting("EntityName", "Customer")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Name", column => column
//                        .OfType("TextField")
//                        .WithSetting("DisplayName", "客户姓名")
//                        .WithSetting("EntityName", "Customer")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("TextFieldSettings.HelpText", "")
//                        .WithSetting("TextFieldSettings.Required", "True")
//                        .WithSetting("TextFieldSettings.ReadOnly", "False")
//                        .WithSetting("TextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("TextFieldSettings.IsSystemField", "False")
//                        .WithSetting("TextFieldSettings.IsAudit", "False")
//                        .WithSetting("TextFieldSettings.MaxLength", "255")
//                        .WithSetting("TextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("CustomerType", column => column
//                        .OfType("OptionSetField")
//                        .WithSetting("DisplayName", "客户类型")
//                        .WithSetting("EntityName", "Customer")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("OptionSetFieldSettings.HelpText", "")
//                        .WithSetting("OptionSetFieldSettings.Required", "True")
//                        .WithSetting("OptionSetFieldSettings.ReadOnly", "False")
//                        .WithSetting("OptionSetFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("OptionSetFieldSettings.IsSystemField", "False")
//                        .WithSetting("OptionSetFieldSettings.IsAudit", "False")
//                        .WithSetting("OptionSetFieldSettings.OptionSetId", CreateCustomerPartCustomerTypeOptionSetPart())
//                        .WithSetting("OptionSetFieldSettings.ListMode", "Dropdown")
//                        .WithSetting("OptionSetFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Phone", column => column
//                        .OfType("PhoneField")
//                        .WithSetting("DisplayName", "联系电话")
//                        .WithSetting("EntityName", "Customer")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("PhoneFieldSettings.HelpText", "")
//                        .WithSetting("PhoneFieldSettings.Required", "False")
//                        .WithSetting("PhoneFieldSettings.ReadOnly", "False")
//                        .WithSetting("PhoneFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("PhoneFieldSettings.IsSystemField", "False")
//                        .WithSetting("PhoneFieldSettings.IsAudit", "False")
//                        .WithSetting("PhoneFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("Description", column => column
//                        .OfType("MultilineTextField")
//                        .WithSetting("DisplayName", "描述")
//                        .WithSetting("EntityName", "Customer")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("MultilineTextFieldSettings.HelpText", "")
//                        .WithSetting("MultilineTextFieldSettings.Required", "False")
//                        .WithSetting("MultilineTextFieldSettings.ReadOnly", "False")
//                        .WithSetting("MultilineTextFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsSystemField", "False")
//                        .WithSetting("MultilineTextFieldSettings.IsAudit", "False")
//                        .WithSetting("MultilineTextFieldSettings.RowNumber", "3")
//                        .WithSetting("MultilineTextFieldSettings.MaxLength", "5000")
//                        .WithSetting("MultilineTextFieldSettings.IsUnique", "False")
//                    )
//                    .WithField("CustomerBalance", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "客户账户余额")
//                        .WithSetting("EntityName", "Customer")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "2")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("Customer",
//                type => type
//                    .WithPart(new ContentPartDefinition("CustomerPart"), configuration => { }, customerPartAlteration)
//                    .WithSetting("CollectionDisplayName", "Customers")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"Number\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Name\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomerType\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Phone\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Description\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );

//            Action<ContentPartDefinitionBuilder> advancePaymentPartAlteration =
//                part => part
//                    .WithField("CustomerId", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "用户")
//                        .WithSetting("EntityName", "AdvancePayment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "Customer")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "AdvancePayment_Customer_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "Name")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Operator", column => column
//                        .OfType("ReferenceField")
//                        .WithSetting("DisplayName", "操作员")
//                        .WithSetting("EntityName", "AdvancePayment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("ReferenceFieldSettings.HelpText", "")
//                        .WithSetting("ReferenceFieldSettings.Required", "False")
//                        .WithSetting("ReferenceFieldSettings.ReadOnly", "False")
//                        .WithSetting("ReferenceFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("ReferenceFieldSettings.IsSystemField", "False")
//                        .WithSetting("ReferenceFieldSettings.IsAudit", "False")
//                        .WithSetting("ReferenceFieldSettings.ContentTypeName", "User")
//                        .WithSetting("ReferenceFieldSettings.RelationshipName", "AdvancePayment_User_Name")
//                        .WithSetting("ReferenceFieldSettings.DisplayAsLink", "False")
//                        .WithSetting("ReferenceFieldSettings.QueryId", "1")
//                        .WithSetting("ReferenceFieldSettings.RelationshipId", "0")
//                        .WithSetting("ReferenceFieldSettings.IsUnique", "False")
//                        .WithSetting("ReferenceFieldSettings.DisplayFieldName", "UserName")
//                        .WithSetting("ReferenceFieldSettings.DeleteAction", "NotAllowed")
//                    )
//                    .WithField("Paid", column => column
//                        .OfType("CurrencyField")
//                        .WithSetting("DisplayName", "预付款金额")
//                        .WithSetting("EntityName", "AdvancePayment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("CurrencyFieldSettings.HelpText", "")
//                        .WithSetting("CurrencyFieldSettings.Required", "False")
//                        .WithSetting("CurrencyFieldSettings.ReadOnly", "False")
//                        .WithSetting("CurrencyFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("CurrencyFieldSettings.IsSystemField", "False")
//                        .WithSetting("CurrencyFieldSettings.IsAudit", "False")
//                        .WithSetting("CurrencyFieldSettings.Length", "18")
//                        .WithSetting("CurrencyFieldSettings.DecimalPlaces", "0")
//                        .WithSetting("CurrencyFieldSettings.DefaultValue", "")
//                    )
//                    .WithField("PaidOn", column => column
//                        .OfType("DateField")
//                        .WithSetting("DisplayName", "付款时间")
//                        .WithSetting("EntityName", "AdvancePayment")
//                        .WithSetting("Storage", "Part")
//                        .WithSetting("DateFieldSettings.HelpText", "")
//                        .WithSetting("DateFieldSettings.Required", "False")
//                        .WithSetting("DateFieldSettings.ReadOnly", "False")
//                        .WithSetting("DateFieldSettings.AlwaysInLayout", "False")
//                        .WithSetting("DateFieldSettings.IsSystemField", "False")
//                        .WithSetting("DateFieldSettings.IsAudit", "False")
//                        .WithSetting("DateFieldSettings.DefaultValue", "")
//                    )
//;

//            ContentDefinitionManager.AlterTypeDefinition("AdvancePayment",
//                type => type
//                    .WithPart(new ContentPartDefinition("AdvancePaymentPart"), configuration => { }, advancePaymentPartAlteration)
//                    .WithSetting("CollectionDisplayName", "AdvancePayments")
//                    .WithSetting("ContentTypeSettings.Creatable", "True")
//                    .WithSetting("Layout", "[{\"SectionColumns\":1,\"SectionColumnsWidth\":\"12\",\"SectionTitle\":\"General Information\",\"Rows\":[{\"Columns\":[{\"Field\":{\"FieldName\":\"CustomerId\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Paid\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"PaidOn\",\"IsValid\":false}}],\"IsMerged\":false},{\"Columns\":[{\"Field\":{\"FieldName\":\"Operator\",\"IsValid\":false}}],\"IsMerged\":false}]}]")
//                );
//            return 36;
//        }
    }
}