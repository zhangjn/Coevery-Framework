﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.Descriptors.Filter;
using Coevery.DeveloperTools.Projections.FilterEditors;
using Coevery.DeveloperTools.Projections.Services;
using Coevery.Localization;
using Coevery.Utility.Extensions;

namespace Coevery.DeveloperTools.Projections.Providers.Filters {
    public class MemberBindingFilter : IFilterProvider {
        private readonly IEnumerable<IMemberBindingProvider> _bindingProviders;
        private readonly IFilterCoordinator _filterCoordinator;

        public MemberBindingFilter(
            IEnumerable<IMemberBindingProvider> bindingProviders,
            IFilterCoordinator filterCoordinator) {
            _bindingProviders = bindingProviders;
            _filterCoordinator = filterCoordinator;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void Describe(DescribeFilterContext describe) {
            var builder = new BindingBuilder();

            foreach(var bindingProvider in _bindingProviders) {
                bindingProvider.GetMemberBindings(builder);
            }

            var groupedMembers = builder.Build().GroupBy(b => b.Property.DeclaringType).ToDictionary(b => b.Key, b => b);

            foreach (var typeMembers in groupedMembers.Keys) {
                var descriptor = describe.For(typeMembers.Name, new LocalizedString(typeMembers.Name.CamelFriendly()), T("Members for {0}", typeMembers.Name));
                foreach(var member in groupedMembers[typeMembers]) {
                    var closureMember = member;
                    string formName = _filterCoordinator.GetForm(closureMember.Property.PropertyType);
                    descriptor.Element(member.Property.Name, member.DisplayName, member.Description,
                        context => ApplyFilter(context, closureMember.Property),
                        context => _filterCoordinator.Display(closureMember.Property.PropertyType, closureMember.DisplayName.Text, context.State),
                        formName    
                    );
                }
            }
        }

        public void ApplyFilter(FilterContext context, PropertyInfo property) {
            var predicate = _filterCoordinator.Filter(property.PropertyType, property.Name, context.State);
            Action<IAliasFactory> alias = x => x.ContentPartRecord(property.DeclaringType);
            context.Query = context.Query.Where(alias, predicate);
        }
    }
}