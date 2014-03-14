using System.Collections.Generic;
using Coevery.ContentManagement;
using Coevery.Core.Projections.Descriptors;
using Coevery.Core.Projections.Descriptors.Filter;
using Coevery.Core.Projections.Descriptors.Layout;
using Coevery.Core.Projections.Descriptors.Property;
using Coevery.Core.Projections.Descriptors.SortCriterion;

namespace Coevery.Core.Projections.Services {
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