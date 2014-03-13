using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.DeveloperTools.Projections.Descriptors;
using Coevery.DeveloperTools.Projections.Descriptors.Filter;
using Coevery.DeveloperTools.Projections.Descriptors.Layout;
using Coevery.DeveloperTools.Projections.Descriptors.Property;
using Coevery.DeveloperTools.Projections.Descriptors.SortCriterion;

namespace Coevery.DeveloperTools.Projections.Services {
    public interface IProjectionManager : IDependency {
        IEnumerable<TypeDescriptor<FilterDescriptor>> DescribeFilters();
        IEnumerable<TypeDescriptor<SortCriterionDescriptor>> DescribeSortCriteria();
        IEnumerable<TypeDescriptor<LayoutDescriptor>> DescribeLayouts();
        IEnumerable<TypeDescriptor<PropertyDescriptor>> DescribeProperties();

        FilterDescriptor GetFilter(string category, string type);
        SortCriterionDescriptor GetSortCriterion(string category, string type);
        LayoutDescriptor GetLayout(string category, string type);
        PropertyDescriptor GetProperty(string category, string type);

        IEnumerable<ContentItem> GetContentItems(int queryId, int skip = 0, int count = 0, string contentTypeName = null);
        int GetCount(int queryId, string contentTypeName = null);
    }

}